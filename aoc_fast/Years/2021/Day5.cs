using aoc_fast.Extensions;

namespace aoc_fast.Years._2021
{
    internal class Day5
    {
        public static string input { get; set; }

        private static long Vents(List<long[]> input, byte[] grid)
        {
            var res = 0L;

            foreach(var parts in input)
            {
                var (x1, y1, x2, y2) = (parts[0], parts[1], parts[2], parts[3]);
                var count = (y2 - y1).Abs().Max((x2 - x1).Abs());
                var delta = (y2 - y1).Sign() * 1000 + (x2 - x1).Sign();
                var index = y1 * 1000 + x1;

                for(var _ = 0; _ <= count; _++)
                {
                    if (grid[index] == 1) res++;
                    grid[index]++;
                    index += delta;
                }
            }
            return res;
        }
        private static (long partOne, long partTwo) answer;

        private static void Parse()
        {
            var all = input.ExtractNumbers<long>().Chunk(4);
            var (orthongonal, diagonal) = all.Partition(a => a[0] == a[2] || a[1] == a[3]);

            var grid = new byte[1_000_000];
            var first = Vents(orthongonal, grid);
            var second = Vents(diagonal, grid);
            answer = (first, second);
        }

        public static long PartOne()
        {
            Parse();
            return answer.partOne;
        }
        public static long PartTwo() => answer.partTwo + answer.partOne;
    }
}
