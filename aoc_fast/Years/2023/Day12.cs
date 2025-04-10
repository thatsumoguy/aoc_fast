using System.Text;
using aoc_fast.Extensions;

namespace aoc_fast.Years._2023
{
    internal class Day12
    {
        public static string input { get; set; }
        private static List<(byte[], List<ulong>)> Springs = [];

        private static ulong Solve(List<(byte[], List<ulong>)> iter, int repeat)
        {
            var result = 0uL;
            var pattern = new List<byte>();
            var springs = new List<ulong>();
            var broken = new ulong[200];
            var table = new ulong[200 * 50];

            foreach (var (first, second) in iter)
            {
                pattern.Clear();
                springs.Clear();

                for (var _ = 1; _ < repeat; _++)
                {
                    pattern.AddRange(first);
                    pattern.Add((byte)'?');
                    springs.AddRange(second);
                }

                pattern.AddRange(first);
                pattern.Add((byte)'.');
                springs.AddRange(second);

                Array.Clear(broken, 0, broken.Length);
                var brokenSum = 0uL;

                for (var i = 0; i < pattern.Count; i++)
                {
                    if (pattern[i] != (byte)'.') brokenSum++;
                    broken[i + 1] = brokenSum;
                }

                var wiggle = pattern.Count - (int)springs.Sum() - springs.Count + 1;

                Array.Clear(table, 0, table.Length);

                var size = (int)springs[0];
                var sum = 0uL;
                var valid = true;

                for (var i = 0; i < wiggle; i++)
                {
                    var index = i + size;
                    if (pattern[index] == (byte)'#') sum = 0;
                    else if (valid && broken[index] - broken[i] == (ulong)size) sum++;

                    table[index] = sum;
                    valid &= pattern[i] != (byte)'#';
                }

                var start = size + 1;

                for (var row = 1; row < springs.Count; row++)
                {
                    var subSize = (int)springs[row];
                    var prev = (row - 1) * pattern.Count;
                    var cur = row * pattern.Count;

                    sum = 0;

                    for (var i = start; i < start + wiggle; i++)
                    {
                        var index = i + subSize;

                        if (index >= pattern.Count) continue;

                        if (pattern[index] == (byte)'#')
                        {
                            sum = 0;
                        }
                        else if (table[prev + i - 1] > 0 &&
                                 pattern[i - 1] != (byte)'#' &&
                                 broken[index] - broken[i] == (ulong)subSize)
                        {
                            sum += table[prev + i - 1];
                        }

                        table[cur + index] = sum;
                    }

                    start += subSize + 1;
                }

                var lastRow = (springs.Count - 1) * pattern.Count;
                var lastIndex = lastRow + pattern.Count - 1;

                result += sum;
            }

            return result;
        }
        private static void Parse()
        {
            Springs = input.Split("\n", StringSplitOptions.RemoveEmptyEntries).Select(line =>
            {
                var parts = line.Split(' ');
                var (prefix, suffix) = (parts[0], parts[1]);
                var first = Encoding.ASCII.GetBytes(prefix);
                var second = suffix.ExtractNumbers<ulong>();
                return (first, second);
            }).ToList();
        }

        public static ulong PartOne()
        {
            Parse();
            return Solve(Springs, 1);
        }

        private static object mutex = new object();
        public static ulong PartTwo()
        {
            var shared = 0UL;

            Threads.SpawnBatches(Springs, iter =>
            {
                var partial = Solve(iter, 5);
                lock (mutex) 
                {
                    shared += partial;
                }
            });
            return shared;
        }
    }
}
