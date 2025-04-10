using aoc_fast.Extensions;

namespace aoc_fast.Years._2024
{
    internal class Day13
    {
        public static string input
        {
            get;
            set;
        }

        private static List<long[]> clawMachine = [];

        private static long Play(long[] clawMachine, bool partTwo = false)
        {
            var (aX, aY, bX, bY, pX, pY) = (clawMachine[0], clawMachine[1], clawMachine[2], clawMachine[3], clawMachine[4], clawMachine[5]);
            if (partTwo)
            {
                pX += 10_000_000_000_000;
                pY += 10_000_000_000_000;
            }

            var det = aX * bY - aY * bX;
            if (det == 0) return 0;

            var a = bY * pX - bX * pY;
            var b = aX * pY - aY * pX;

            if (a % det != 0 || b % det != 0) return 0;

            a /= det;
            b /= det;
            return (partTwo || (a <= 100 && b <= 100)) ? 3 * a + b : 0;
        }

        public static long PartOne()
        {
            clawMachine = input.ExtractNumbers<long>().Chunk(6).ToList();
            return clawMachine.Select(row => Play(row)).Sum();
        }
        public static long PartTwo() => clawMachine.Select(row => Play(row, true)).Sum();
    }
}
