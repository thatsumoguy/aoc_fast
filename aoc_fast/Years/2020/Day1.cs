using aoc_fast.Extensions;

namespace aoc_fast.Years._2020
{
    internal class Day1
    {
        public static string input { get; set; }
        private static List<uint> nums = [];
        private static void Parse() => nums = input.ExtractNumbers<uint>();
        private static uint? TwoSum(List<uint> nums, uint target, uint[] hash, uint round)
        {
            foreach(var i in nums)
            {
                if(i < target)
                {
                    if (hash[i] == round) return i * (target - i);
                    hash[target - i] = round;
                }
            }
            return null;
        }
        public static uint PartOne()
        {
            Parse();
            var hash = new uint[2020];
            return TwoSum(nums, 2020, hash, 1).Value;
        }
        public static uint PartTwo()
        {
            var hash = new uint[2020];
            for(var i = 0; i < nums.Count -2; i++)
            {
                var first = nums[i];
                var round = (uint)(i + 1);
                var slice = nums[(int)round..];
                var target = 2020 - first;

                var product = TwoSum(slice, target, hash, round);
                if (product != null)
                {
                    return first * product.Value;
                }
            }
            throw new Exception();
        }
    }
}
