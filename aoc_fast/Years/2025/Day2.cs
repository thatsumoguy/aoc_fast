using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using aoc_fast.Extensions;

namespace aoc_fast.Years._2025
{
    internal class Day2
    {
        public static string input
        {
            get;
            set;
        }

        private static ulong[] answers = [];

        private static ulong[][] first = [[2, 1], [4, 2], [6, 3], [8, 4], [10, 5]];
        private static ulong[][] second = [[3, 1], [5, 1], [6, 2], [7, 1], [9, 3], [10, 2]];
        private static ulong[][] third = [[6, 1], [10, 1]];

        private static void Parse()
        {
            var ranges = input.ExtractNumbers<ulong>().Chunk(2).ToArray();
            var partOne = Sum(first, ranges);
            var partTwo = partOne + Sum(second, ranges) - Sum(third, ranges);
            answers = [partOne, partTwo];
        }

        private static ulong Sum(ulong[][] ranges, ulong[][] ids)
        {
            ulong res = 0;

            foreach(var range in ranges)
            {
                var digits = range[0];
                var size = range[1];

                var repeat = digits / size;
                var power = FastMath.Pow(10ul, (int)size);
                var step = Enumerable.Range(0, (int)repeat).Aggregate(0ul, (acc, _) => acc * power + 1);
                var start = step * (power / 10);
                var end = step * (power - 1);

                foreach(var id in ids)
                {
                    var from = id[0];
                    var to = id[1];

                    var lower = from.NextMultipleOf(step).Max(start);
                    var upper = to.Min(end);

                    if(lower <= upper)
                    {
                        var n = (upper - lower) / step;
                        var trianglar = n * (n + 1) / 2;
                        res += lower * (n + 1) + step * trianglar;
                    }
                }
            }
            return res;
        }

        public static ulong PartOne()
        {
            Parse();
            return answers[0];
        }
        public static ulong PartTwo() => answers[1];
    }
}
