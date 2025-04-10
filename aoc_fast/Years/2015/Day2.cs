using aoc_fast.Extensions;

namespace aoc_fast.Years._2015
{
    class Day2
    {
        public static string input
        {
            get;
            set;
        }

        private static List<int[]> gifts = [];

        private static List<int[]> Parse()
        {
            return input.ExtractNumbers<int>().Chunk(3).Select(c =>
            {
                var gift = c;
                Array.Sort(gift);
                return gift;
            }).ToList();
        }

        public static int PartOne()
        {
            gifts = Parse();
            return gifts.Select(X => 2 * (X[0] * X[1] + X[1] * X[2] + X[2] * X[0]) + X[0] * X[1]).Sum();
        }

        public static int PartTwo() => gifts.Select(X => 2 * (X[0] + X[1]) + (X[0] * X[1] * X[2])).Sum();
    }
}
