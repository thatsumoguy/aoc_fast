using aoc_fast.Extensions;

namespace aoc_fast.Years._2021
{
    internal class Day7
    {
        public static string input { get; set; }

        private static List<int> Nums = [];

        private static int Median(List<int> input)
        {
            var crabs = input.ToList();

            crabs.Sort();

            var half = input.Count / 2;
            var odd = crabs.Count % 2 == 1;
            return odd ? crabs[half] : (crabs[half - 1] + crabs[half]) / 2;
        }

        public static int Mean(List<int> input) => input.Sum() / input.Count;

        public static int PartOne()
        {
            Nums = input.ExtractNumbers<int>();
            var median = Median(Nums);
            return Nums.Select(n => (n - median).Abs()).Sum();
        }
        public static int PartTwo()
        {
            var mean = Mean(Nums);
            var triangle = (int x, int mean) =>
            {
                var n = (x - mean).Abs();
                return (n * (n + 1)) / 2;
            };

            var first = Nums.Select(x => triangle(x, mean)).Sum();
            var second = Nums.Select(x => triangle(x, mean + 1)).Sum();
            var third = Nums.Select(x => triangle(x, mean - 1)).Sum();
            return first.Min(second).Min(third);
        }
    }
}
