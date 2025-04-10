using aoc_fast.Extensions;

namespace aoc_fast.Years._2023
{
    internal class Day9
    {
        public static string input { get; set; }
        private static (long partOne, long partTwo) answers;
        private static void Parse()
        {
            var prefix = input.Split("\n")[0];
            var row = (long)prefix.ExtractNumbers<long>().Count();

            var coefficient = 1L;
            var triangle = new List<long>() { 1 };

            for(var i = 0; i < row; i++)
            {
                coefficient = (coefficient * (i - row)) / (i + 1);
                triangle.Add(coefficient);
            }

            var partOne = 0L;
            var partTwo = 0L;

            foreach(var line in input.Split("\n", StringSplitOptions.RemoveEmptyEntries))
            {
                foreach(var (k, value) in line.ExtractNumbers<long>().Index())
                {
                    partOne += value * triangle[k];
                    partTwo += value * triangle[k + 1];
                }
            }
            answers = (partOne, partTwo);
        }

        public static long PartOne()
        {
            Parse();
            return answers.partOne;
        }
        public static long PartTwo() => answers.partTwo.Abs();
    }
}
