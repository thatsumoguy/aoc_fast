using aoc_fast.Extensions;

namespace aoc_fast.Years._2020
{
    internal class Day9
    {
        public static string input { get; set; }
        private static (ulong partOne, ulong partTwo) answers;
        private static (ulong, ulong) Decrypt(string input, int window)
        {
            var nums = input.ExtractNumbers<ulong>();
            var index = nums.Windows(window + 1).First(w =>
            {
                for (var i = 0; i < window - 1; i++)
                {
                    for (var j = i + 1; j < window; j++)
                    {
                        if (w[i] + w[j] == w[window]) return false;
                    }
                }
                return true;
            });
            var invalid = index[window];
            var start = 0;
            var end = 2;
            var sum = nums[0] + nums[1];

            while (sum != invalid)
            {
                if(sum < invalid)
                {
                    sum += nums[end];
                    end++;
                }
                else
                {
                    sum -= nums[start];
                    start++;
                }
            }
            var slice = nums[start..end];
            var min = slice.Min();
            var max = slice.Max();
            var weakness = min + max;
            return (invalid, weakness);
        }
        public static ulong PartOne()
        {
            try
            {
                answers = Decrypt(input, 25);
                return answers.partOne;
            }
            catch (Exception e) { Console.WriteLine(e); }
            return 0;
        }
        public static ulong PartTwo() => answers.partTwo;
    }
}
