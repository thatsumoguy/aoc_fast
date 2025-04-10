using System.Text.RegularExpressions;
using aoc_fast.Extensions;

namespace aoc_fast.Years._2023
{
    internal partial class Day20
    {
        public static string input { get; set; }

        private static uint[] Nums = [];

        private static void Parse()
        {
            var nodes = new Dictionary<string, List<string>>();
            var kind = new Dictionary<string, bool>();

            foreach(var line in input.Split("\n", StringSplitOptions.RemoveEmptyEntries))
            {
                var tokens = MyRegex().Split(line).Where(s => !string.IsNullOrEmpty(s)).ToList();
                
                var key = tokens.PopFront();
                var children = tokens.ToList();
                nodes[key] = children;
                kind[key] = !line.StartsWith('&');
            }

            var todo = new List<(string, uint, uint)>();
            var numbers = new List<uint>();

            foreach (var start in nodes["broadcaster"]) todo.Add((start, 0, 1));

            while(todo.Count > 0)
            {
                var (key, value, bit) = todo.Pop();
                var children = nodes[key];
                var next = children.Find(k => kind[k]);
                if(next != null)
                {
                    if (children.Count == 2) value |= bit; 
                    todo.Add((next, value, bit << 1));
                }
                else numbers.Add(value | bit);
            }

            Nums = [.. numbers];
        }

        public static uint PartOne()
        {
            Parse();

            var pairs = Nums.Select(n => (n, 13 - uint.PopCount(n))).ToList();

            var low = 5000u;
            var high = 0u;

            for (var n = 0u; n < 1000; n++)
            {
                var rising = ~n & (n + 1);
                high += 4 * uint.PopCount(rising);

                var falling = n & ~(n + 1);
                low += 4 * uint.PopCount(falling);

                foreach (var (num, feedback) in pairs)
                {
                    var factor = uint.PopCount(rising & num);
                    high += factor * (feedback + 3);
                    low += factor;

                    factor = uint.PopCount(falling & num);
                    high += factor * (feedback + 2);
                    low += 2 * factor;
                }
            }
            return low * high;
        }

        public static ulong PartTwo() => Nums.Select(n => (ulong)n).Aggregate(1ul, (acc, i) => acc * i);

        [GeneratedRegex("[^a-z]")]
        private static partial Regex MyRegex();
    }
}
