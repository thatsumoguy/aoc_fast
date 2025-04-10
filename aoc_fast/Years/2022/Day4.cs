using aoc_fast.Extensions;

namespace aoc_fast.Years._2022
{
    internal class Day4
    {
        public static string input { get; set; }
        private static List<uint[]> pairs = [];
        private static void Parse() => pairs = input.ExtractNumbers<uint>().Chunk(4).ToList();
        public static int PartOne()
        {
            Parse();
            return pairs.Where(a => (a[0] >= a[2] && a[1] <= a[3]) || (a[2] >= a[0] && a[3] <= a[1])).Count();
        }
        public static int PartTwo() => pairs.Where(a => a[0] <= a[3] && a[2] <= a[1]).Count();
    }
}
