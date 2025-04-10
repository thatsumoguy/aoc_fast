using aoc_fast.Extensions;
using Point = aoc_fast.Extensions.Point;

namespace aoc_fast.Years._2019
{
    internal class Day11
    {
        public static string input { get; set; }

        private static Dictionary<Point, long> Paint(long[] input, long initial)
        {
            var comp = new Computer(input);
            var pos = Directions.ORIGIN;
            var dir = Directions.UP;
            var hull = new Dictionary<Point, long>(5000)
            {
                { pos, initial }
            };

            while(true)
            {
                var panel = hull.TryGetValue(pos, out var p) ? p : 0;
                comp.Input(panel);

                switch(comp.Run(out var output))
                {
                    case State.Output:
                        hull[pos] = output; break;
                    default:
                        goto outer;
                }
                switch(comp.Run(out var next))
                {
                    case State.Output:
                        dir = next == 0 ? dir.CounterClockwise() : dir.Clockwise();
                        pos += dir;
                        break;
                    default:
                        goto outer;
                }
            }
        outer:
            return hull;
        }

        private static long[] code = [];
        private static void Parse() => code = [.. input.ExtractNumbers<long>()];

        public static int PartOne()
        {
            try
            {
                Parse();
                return Paint(code, 0).Count;
            }
            catch (Exception e) { Console.WriteLine(e); }
            return 0;
        }
        public static string PartTwo()
        {
            var hull = Paint(code, 1);
            var panels = hull.Where(kvp => kvp.Value == 1).Select(kvp => kvp.Key).ToList();

            var x1 = int.MaxValue;
            var x2 = int.MinValue;
            var y1 = int.MaxValue;
            var y2 = int.MinValue;
            foreach(var point in panels)
            {
                x1 = Math.Min(x1, point.X);
                x2 = Math.Max(x2, point.X);
                y1 = Math.Min(y1, point.Y);
                y2 = Math.Max(y2, point.Y);
            }

            var width = x2 - x1 + 1;
            var height = y2 - y1 + 1;
            var offset = new Point(x1, y1);
            var image = new char[width * height];
            Array.Fill(image, '.');

            foreach(var point in panels)
            {
                var adjusted = point - offset;
                var index = width * adjusted.Y + adjusted.X;
                image[index] = '#';
            }
            var res = string.Join("\n\t\t\t ", image.Chunk(width).Select(row => new string(row)));
            res = res.Insert(0, "\n\t\t\t ");
            return res;
        }
    }
}
