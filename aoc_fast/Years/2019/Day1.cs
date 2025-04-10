namespace aoc_fast.Years._2019
{
    internal class Day1
    {
        public static string input { get; set; }
        private static List<uint> nums = [];
        private static void Parse()
        {
            nums = input.TrimEnd().Split("\n").Select(uint.Parse).ToList();
        }

        public static uint PartOne()
        {
            Parse();
            var sum = 0u;
            foreach (var mass in nums) sum += mass / 3 - 2;
            return sum;
        }
        public static uint PartTwo()
        {
            var totalFuel = 0u;
            foreach(var mass in nums)
            {
                var fuel = 0u;
                var currMass = mass;
                while(currMass > 8)
                {
                    currMass = currMass / 3 - 2;
                    fuel += currMass;
                }
                totalFuel += fuel;
            }
            return totalFuel;
        }
    }
}
