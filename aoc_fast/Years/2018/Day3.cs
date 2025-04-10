using aoc_fast.Extensions;

namespace aoc_fast.Years._2018
{
    internal class Day3
    {
        public static string input
        {
            get;
            set;
        }

        private static (ulong partOne,int partTwo) answer;

        private static void Parse()
        {
            var claims = input.ExtractNumbers<ulong>().Chunk(5).Select(x =>
            {
                var start = 16 * x[2] + (x[1] / 64);
                var end = start + 16 * x[4];

                var mask = (UInt128)((1 << (int)x[3]) - 1) << ((int)x[1] % 64);
                var lower = (ulong)mask;
                var upper = (ulong)(mask >> 64);
                return (start, end, lower, upper);
            }).ToList();

            var fabric = new ulong[16 * 1000];
            var overlap = new ulong[16 * 1000];
            foreach(var (start, end, lower, upper) in claims)
            {
                for(var index = start; index < end; index += 16)
                {
                    overlap[index] |= fabric[index] & lower;
                    fabric[index] |= lower;

                    if(upper > 0)
                    {
                        overlap[index + 1] |= fabric[index + 1] & upper;
                        fabric[index + 1] |= upper;
                    }
                }
            }

            var pos = claims.FindIndex((a) =>
            {
                for(var index = a.start; index < a.end; index += 16)
                {
                    if (!(((overlap[index] & a.lower) == 0) && (a.upper == 0 || (overlap[index + 1] & a.upper) == 0))) return false;
                }
                return true;
            });
            answer = (overlap.Select(n => ulong.PopCount(n)).Sum(), pos + 1);
        }

        public static ulong PartOne()
        {
            Parse();
            return answer.partOne;
        }
        public static int PartTwo() => answer.partTwo;
    }
}
