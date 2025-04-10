using System.Text;
using aoc_fast.Extensions;
using static aoc_fast.Years._2015.Day6;

namespace aoc_fast.Years._2015
{
    public class Day6
    {
        public enum Command
        {
            On,
            Off,
            Toggle
        }
        public static string input
        {
            get;
            set;
        }
        public static (int part1, int part2) answer = (0, 0);

        private static void Parse()
        {
            var first = input.Split('\n').Select(Encoding.ASCII.GetBytes);
            var second = input.ExtractNumbers<int>().Chunk(4);
            var instructions = first.Zip(second).Select(x => Instrction.From(x.First, x.Second)).ToList();

            var xs = Enumerable.Repeat(0, 1000).ToList();
            var ys = Enumerable.Repeat(0, 1000).ToList();

            foreach (var ins in instructions)
            {
                var rect = ins.rect;
                xs.Add(rect.x1);
                xs.Add(rect.x2 + 1);
                ys.Add(rect.y1);
                ys.Add(rect.y2 + 1);
            }

            xs.Sort();
            xs = xs.Distinct().ToList();
            ys.Sort();
            ys = ys.Distinct().ToList();

            var xIndexFrom = Enumerable.Repeat(0, 1001).ToList();
            foreach (var (x, i) in xs.Select((x, i) => (x, i)))
            {
                xIndexFrom[x] = i;
            }
            var yIndexFrom = Enumerable.Repeat(0, 1001).ToList();
            foreach (var (y, i) in ys.Select((y, i) => (y, i)))
            {
                yIndexFrom[y] = i;
            }

            var stride = xs.Count;
            var capacity = stride * ys.Count;
            var up = Enumerable.Repeat(false, capacity).ToList();
            var left = Enumerable.Repeat(false, capacity).ToList();
            var prev = Enumerable.Repeat((false, 0), capacity).ToList();

            foreach (var ins in instructions)
            {
                var rect = ins.rect;
                var x1 = xIndexFrom[rect.x1];
                var x2 = xIndexFrom[rect.x2 + 1];
                var y1 = yIndexFrom[rect.y1];
                var y2 = yIndexFrom[rect.y2 + 1];
                foreach (var x in Enumerable.Range(x1, (x2 + 1) - x1))
                {
                    up[stride * y1 + x] = true;
                    up[stride * y2 + x] = true;
                }
                foreach (var y in Enumerable.Range(y1, (y2 + 1) - y1))
                {
                    left[stride * y + x1] = true;
                    left[stride * y + x2] = true;
                }
            }

            var res1 = 0;
            var res2 = 0;

            for (var j = 0; j < ys.Count - 1; j++)
            {
                var y1 = ys[j];
                var y2 = ys[j + 1];

                for (var i = 0; i < xs.Count - 1; i++)
                {
                    var x1 = xs[i];
                    var x2 = xs[i + 1];
                    var area = (x2 - x1) * (y2 - y1);
                    var index = stride * j + i;

                    var current = (false, 0);

                    if (i > 0 && !left[index]) current = prev[index - 1];
                    else if (j > 0 && !up[index]) current = prev[index - stride];
                    else
                    {
                        var light = false;
                        var brightness = (byte)0;

                        foreach (var ins in instructions)
                        {
                            if (ins.rect.Contains(x1, y1))
                            {
                                switch (ins.command)
                                {
                                    case Command.On:
                                        light = true;
                                        brightness += 1;
                                        break;
                                    case Command.Off:
                                        light = false;
                                        brightness = byte.CreateSaturating(brightness - 1);
                                        break;
                                    case Command.Toggle:
                                        light = !light;
                                        brightness += 2;
                                        break;
                                }
                            }
                        }

                        current = (light, brightness);
                    }

                    prev[index] = current;
                    if (current.Item1) res1 += area;
                    res2 += current.Item2 * area;
                }
            }
            answer = (res1, res2);
        }
        public static int PartOne()
        {
            Parse();
            return answer.part1;
        }
        public static int PartTwo() => answer.part2;

    }

    public static class CommandExtensions
    {
        public static Command From(this Command c, byte[] bytes) =>
           bytes[6] switch
           {
               (byte)'n' => Command.On,
               (byte)'f' => Command.Off,
               (byte)' ' => Command.Toggle,
               _ => throw new Exception()
           };
    }

    class Rect()
    {

        public int x1 { get; set; }
        public int x2 { get; set; }
        public int y1 { get; set; }
        public int y2 { get; set; }

        public static Rect From(int[] i) =>
            new()
            {
                x1 = i[0],
                x2 = i[2],
                y1 = i[1],
                y2 = i[3]
            };
        public bool Contains(int x, int y) => x1 <= x && x <= x2 && y1 <= y && y <= y2;
    }

    class Instrction
    {
        public Command command { get; set; }
        public Rect rect { get; set; }

        public static Instrction From(byte[] bytes, int[] points)
        {
            var command = Command.Off;
            command = command.From(bytes);
            var rect = Rect.From(points);
            return new() { command = command, rect = rect};
        }
    }
    
}
