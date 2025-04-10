using System.Text;
using aoc_fast.Extensions;

namespace aoc_fast.Years._2021
{
    internal class Day3
    {
        public static string input { get; set; }

        private static int Width;
        private static List<byte[]> Nums = [];
        private static void Filter(List<byte[]> numbers, int i, byte keep)
        {
            var j = 0;

            while (j < numbers.Count)
            {
                if (numbers[j][i] == keep) j++;
                else numbers.SwapRemove(j);
            }
        }

        private static int Fold(byte[] numbers, int width) => numbers.Take(width).Aggregate(0, (acc, b) => (acc << 1) | (int)(b & 1));
        private static long Sum(List<byte[]> numbers, int i)
        {
            var total = numbers.Select(b => (long)b[i]).Sum();
            return total - 48 * numbers.Count;
        }

        private static int Rating(int width, List<byte[]> nums, Func<long, long, bool> cmp)
        {
            var numbers = nums.ToList();

            for(var i = 0; i < width; i++)
            {
                var sum = Sum(numbers, i);
                var keep = cmp(sum, numbers.Count - sum) ? (byte)'1' : (byte)'0';
                Filter(numbers, i, keep);
                if(numbers.Count == 1) return Fold(numbers[0], width);
            }
            throw new Exception();
        }
        private static void Parse()
        {
            var numbers = input.TrimEnd().Split('\n').Select(Encoding.UTF8.GetBytes).ToList();
            (Width, Nums) = (numbers[0].Length, numbers);
        }

        public static int PartOne()
        {
            Parse();
            var gamma = 0;
            var epsilon = 0;

            for(var i = 0; i < Width; i++)
            {
                var sum = Sum(Nums, i);
                if(sum > Nums.Count - sum)
                {
                    gamma = (gamma << 1) | 1;
                    epsilon <<= 1;
                }
                else
                {
                    gamma <<= 1;
                    epsilon = (epsilon << 1) | 1;
                }
            }

            return gamma * epsilon;
        }
        public static int PartTwo()
        {
            var gamma = Rating(Width, Nums, (a, b) => a >= b);
            var epsilon = Rating(Width, Nums, (a, b) => a < b);
            return gamma * epsilon;
        }

    }
}
