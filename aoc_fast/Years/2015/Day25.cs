using aoc_fast.Extensions;

namespace aoc_fast.Years._2015
{
    class Day25
    {
        public static string input
        {
            get;
            set;
        }

        public static ulong PartOne()
        {
            var (row, column) = input.ExtractNumbers<ulong>() switch { var a => (a[0], a[1]) };

            var n = column + row - 1;
            var triangle = (n * (n + 1)) / 2;
            var index = triangle - row;
            return (20151125 * 252533uL.ModPow<ulong>(index, 33554393)) % 33554393;
        }

        public static string PartTwo() => "Merry Christmas";

    }
}
