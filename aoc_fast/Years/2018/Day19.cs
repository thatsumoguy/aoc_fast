using aoc_fast.Extensions;

namespace aoc_fast.Years._2018
{
    internal class Day19
    {
        public static string input { get; set; }
        private static (uint partOne, uint partTwo) answer;
        private static uint DivisorSum(uint n)
        {
            var f = 2u;
            var sum = 1u;
            while(f * f <= n)
            {
                var g = sum;

                while(n % f == 0)
                {
                    n /= f;
                    g *= f;
                    sum += g;
                }

                f++;
            }
            return n == 1 ? sum : sum * (1 + n);
        }
        private static void Parse()
        {
            var tokens = input.ExtractNumbers<uint>();
            var baseNum = 22 * tokens[65] + tokens[71];
            answer = (DivisorSum(baseNum + 836), DivisorSum(baseNum + 10551236));
        }

        public static uint PartOne()
        {
            Parse();
            return answer.partOne;
        }
        public static uint PartTwo() => answer.partTwo;
    }
}
