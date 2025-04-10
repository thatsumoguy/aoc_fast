using aoc_fast.Extensions;

namespace aoc_fast.Years._2023
{
    internal class Day4
    {
        public static string input { get; set; }
        private static List<int> nums = [];

        private static void Parse()
        {
            nums = input.Split("\n", StringSplitOptions.RemoveEmptyEntries).Select(line =>
            {
                var found = new bool[100];
                var parts = line.Split('|');
                var (win, have) = (parts[0], parts[1]);
                foreach (var i in win.ExtractNumbers<int>().Skip(1)) found[i] = true;
                return have.ExtractNumbers<int>().Where(i => found[i]).Count();
            }).ToList();
        }

        public static int PartOne()
        {
            Parse();
            return nums.Select(n => (1 << n) >> 1).Sum();
        }
        public static int PartTwo()
        {
            var copies = new int[nums.Count];
            for (var i = 0; i < copies.Length; i++) copies[i] = 1;
            foreach(var (i, n) in nums.Index())
            {
                for (var j = 1; j <= n; j++) copies[i + j] += copies[i];
            }
            return copies.Sum();
        }
    }
}
