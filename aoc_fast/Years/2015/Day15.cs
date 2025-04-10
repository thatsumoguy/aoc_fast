using System.Data;
using aoc_fast.Extensions;

namespace aoc_fast.Years._2015
{
    class Day15
    {
        public static string input
        {
            get;
            set;
        }

        private static (int partOne, int partTwo) answer = (0, 0);

        private static void Parse()
        {
            var recipe = input.ExtractNumbers<int>().Chunk(5).ToList();
            var partOne = 0;
            var partTwo = 0;

            for(var a = 0; a <= 100; a ++)
            {
                var first = Enumerable.Range(0, 5).Select(i => a * recipe[0][i]).ToArray();

                for(var b = 0; b < (101 - a); b ++)
                {
                    var second = Enumerable.Range(0, 5).Select(i => first[i] + b * recipe[1][i]).ToArray();

                    var check = Enumerable.Range(0, 5).Select(i =>
                    second[i] + Math.Max(recipe[2][i], recipe[3][i]) * (100 - a - b)).ToArray();
                    if (check.Any(n => n <= 0)) continue;

                    for(var c = 0; c < (101 - a - b); c ++)
                    {
                        var d = 100 - a - b - c;
                        var third = Enumerable.Range(0, 5).Select(i => second[i] + c * recipe[2][i]).ToArray();
                        var fourth = Enumerable.Range(0, 5).Select(i => third[i] + d * recipe[3][i]).ToArray();

                        var score = fourth.Take(4).Select(n => Math.Max(n, 0)).Aggregate(1, (acc, X) => acc * X);
                        var calories = fourth[4];

                        partOne = Math.Max(partOne, score);

                        if(calories == 500) partTwo = Math.Max(partTwo, score);
                    }
                }
            }
            answer = (partOne, partTwo);
        }

        public static int PartOne()
        {
            Parse();
            return answer.partOne;
        }

        public static int PartTwo() => answer.partTwo;
    }
}
