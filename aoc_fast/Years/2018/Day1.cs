using aoc_fast.Extensions;

namespace aoc_fast.Years._2018
{
    internal class Day1
    {
        public static string input
        {
            get;
            set;
        }

        private static List<int> nums = [];

        public static int PartOne()
        {
            nums = input.ExtractNumbers<int>();
            return nums.Sum();
        }

        public static int PartTwo()
        {
            var total = nums.Sum();

            var freq = 0;
            var seen = new List<(int, int, int)>(nums.Count);

            foreach(var n in nums)
            {
                seen.Add(((freq % total + total) % total, freq, seen.Count));
                freq += n;
            }

            seen.Sort();

            var pairs = new List<(int, int, int)>();

            foreach(var w in seen.Windows(2))
            {
                var (remainder0, freq0, index0) = w[0];
                var (remainder1, freq1, _) = w[1];

                if (remainder0 == remainder1) pairs.Add((freq1 - freq0, index0, freq1));
            }
            pairs.Sort();

            return pairs[0].Item3;
        }
    }
}
