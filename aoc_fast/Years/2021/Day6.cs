using aoc_fast.Extensions;

namespace aoc_fast.Years._2021
{
    internal class Day6
    {
        public static string input { get; set; }

        private static long[] Fish = [];

        private static long Simulate(long[] input, int days)
        {
            var fish = input.ToArray();
            foreach (var day in Enumerable.Range(0, days))
            {
                fish[(day + 7) % 9] += fish[(day % 9)];
            }
            return fish.Sum();
        }
        private static void Parse()
        {
            var fish = new long[9];
            input.ExtractNumbers<long>().ForEach(i => fish[i]++);
            Fish = fish;
        }
        public static long PartOne()
        {
            Parse();
            return Simulate(Fish, 80);
        }
        public static long PartTwo() => Simulate(Fish, 256);
    }
}
