using aoc_fast.Extensions;

namespace aoc_fast.Years._2021
{
    internal class Day1
    {
        public static string input { get; set; }

        private static List<int> nums = [];

        public static int PartOne()
        {
            nums = input.ExtractNumbers<int>();
            return nums.Windows(2).Where(w => w[0] < w[1]).Count();
        }
        public static int PartTwo() => nums.Windows(4).Where(w => w[0] < w[3]).Count();
    }
}
