using System.Text;

namespace aoc_fast.Years._2020
{
    internal class Day6
    {
        public static string input { get; set; }
        private static List<uint> nums = [];

        private static void Parse() => nums = input.TrimEnd().Split("\n").Select(line => Encoding.UTF8.GetBytes(line).Aggregate(0u, (acc, b) => acc | (1u << (b - (byte)'a')))).ToList();
        public static uint PartOne()
        {
            Parse();
            var total = 0u;
            var group = uint.MinValue;
            foreach(var passenger in nums)
            {
                if(passenger == 0)
                {
                    total += uint.PopCount(group);
                    group = uint.MinValue;
                }
                else group |= passenger;
            }
            return total + uint.PopCount(group);
        }
        public static uint PartTwo()
        {
            Parse();
            var total = 0u;
            var group = uint.MaxValue;
            foreach(var passenger in nums)
            {
                if(passenger == 0)
                {
                    total += uint.PopCount(group);
                    group = uint.MaxValue;
                }
                else group &= passenger;
            }
            
            return total + uint.PopCount(group);
        }
    }
}
