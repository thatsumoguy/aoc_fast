using aoc_fast.Extensions;

namespace aoc_fast.Years._2016
{
    class Day3
    {
        public static string input
        {
            get;
            set;
        }
        private static int Count(int[] iter) => iter.Chunk(3).Where(x => x[0] + x[1] > x[2] && x[0] + x[2] > x[1] && x[1] + x[2] > x[0]). Count();

        public static int PartOne() => Count([.. input.ExtractNumbers<int>()]);

        public static int PartTwo()
        {
            int[] start = [.. input.ExtractNumbers<int>()];
            var first = Count(start.Select((val, index) => (val, index)).Where(p => p.index % 3 == 0).Select(p => p.val).ToArray());
            var second = Count(start.Select((val, index) => (val, index)).Skip(1).Where(p => (p.index - 1) % 3 == 0).Select(p => p.val).ToArray());
            var third = Count(start.Select((val, index) => (val, index)).Skip(2).Where(p => (p.index - 2) % 3 == 0).Select(p => p.val).ToArray());

            return first + second + third;
        }
    }
}
