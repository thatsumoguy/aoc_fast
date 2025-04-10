using aoc_fast.Extensions;

namespace aoc_fast.Years._2019
{
    internal class Day5
    {
        public static string input { get; set; }
        private static long[] nums = [];
        private static void Parse() => nums = [.. input.ExtractNumbers<long>()];

        private static long Run(long[] nums, long val)
        {
            var comp = new Computer(nums);
            comp.Input(val);
            var res = 0l;
            while (comp.Run(out var next) == State.Output) res = next;
            return res;
        }
        public static long PartOne()
        {
            Parse();
            return Run(nums, 1);
        }
        public static long PartTwo() => Run(nums, 5);
    }
}
