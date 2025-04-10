using aoc_fast.Extensions;

namespace aoc_fast.Years._2019
{
    internal class Day9
    {
        public static string input { get; set; }
        private static List<long> nums = [];
        private static void Parse() => nums = input.ExtractNumbers<long>();
        private static long Run(long val)
        {
            var comp = new Computer(nums);
            comp.Input(val);

            return comp.Run(out var res) switch
            {
                State.Output => res,
                _ => throw new Exception()
            };
        }
        public static long PartOne()
        {
            Parse();
            return Run(1);
        }
        public static long PartTwo() => Run(2);
    }
}
