using System.Text;
using aoc_fast.Extensions;

namespace aoc_fast.Years._2021
{
    internal class Day11
    {
        public static string input { get; set; }

        private static readonly byte[] NEIGHBORS = [1, 11, 12, 13, 243, 244, 245, 255];

        private static byte[] bytes = [];

        private static (ulong, ulong) Simulate(byte[] input, Func<ulong, ulong, bool> pred)
        {
            var grid = input.ToArray();
            var flashed = new bool[144];
            Array.Fill(flashed, true);
            var todo = new List<byte>(100);

            var flashes = 0ul;
            var steps = 0ul;
            var total = 0ul;

            while(pred(flashes, steps))
            {
                flashes = 0;

                for(var y = 0; y < 10; y++)
                {
                    for(var X = 0; X < 10; X++)
                    {
                        var index = 12 * (y + 1) + (X + 1);

                        if (grid[index] < 9)
                        {
                            grid[index]++;
                            flashed[index] = false;
                        }
                        else
                        {
                            grid[index] = 0;
                            flashed[index] = true;
                            todo.Add((byte)index);
                        }
                    }
                }

                while (todo.PopCheck(out var index))
                {
                    flashes++;
                    foreach (var offset in NEIGHBORS)
                    {
                        var next = (byte)(index + offset);
                        if (flashed[next]) continue;

                        if (grid[next] < 9) grid[next]++;
                        else
                        {
                            grid[next] = 0;
                            flashed[next] = true;
                            todo.Add((byte)next);
                        }
                    }
                }
                steps++;
                total += flashes;
            }
            return (total, steps);
        }

        private static void Parse()
        {
            var bs = input.TrimEnd().Split('\n').Select(Encoding.UTF8.GetBytes).ToArray();
            var grid = new byte[144];

            for(var y = 0; y < 10; y++)
            {
                for (var X = 0; X < 10; X++) grid[12 * (y + 1) + (X + 1)] = (byte)(bs[y][X] - (byte)'0');
            }
            bytes = grid;
        }

        public static ulong PartOne()
        {
            try
            {
                Parse();
                return Simulate(bytes, (_, steps) => steps < 100).Item1;
            }
            catch (Exception ex) { Console.WriteLine(ex); }
            return 0;
        }
        public static ulong PartTwo() => Simulate(bytes, (flashes, _) => flashes < 100).Item2;
    }
}
