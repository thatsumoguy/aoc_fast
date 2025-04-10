using aoc_fast.Extensions;

namespace aoc_fast.Years._2024
{
    internal class Day7
    {
        public static string input
        {
            get;
            set;
        }
        private static (long partOne, long partTwo) answer;
        private static long PowerOfTen(long n) => n < 10 ? 10 : n < 100 ? 100 : 1000;

        private static bool Equatable(long val, List<long> terms, int index, bool partTwo = false)
        {
            if (index == 1) return val == terms[1];
            else
            {
                return (partTwo && val % PowerOfTen(terms[index]) == terms[index]
                    && Equatable(val / PowerOfTen(terms[index]), terms, index - 1, partTwo)
                    || (val % terms[index] == 0 && Equatable(val / terms[index], terms, index - 1, partTwo))
                    || (val >= terms[index] && Equatable(val - terms[index], terms, index - 1, partTwo)));
            }
        }

        private static void Parse()
        {
            var equations = new List<long>();
            var partOne = 0L;
            var partTwo = 0L;

            foreach (var line in input.Split("\n"))
            {
                equations.AddRange(line.ExtractNumbers<long>());
                if (Equatable(equations[0], equations, equations.Count - 1))
                {
                    partOne += equations[0];
                    partTwo += equations[0];
                }
                else if (Equatable(equations[0], equations, equations.Count - 1, true)) partTwo += equations[0];

                equations.Clear();
            }

            answer = (partOne, partTwo);
        }

        public static long PartOne()
        {
            Parse();
            return answer.partOne;
        }

        public static long PartTwo() => answer.partTwo;
    }
}
