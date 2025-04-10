using aoc_fast.Extensions;

namespace aoc_fast.Years._2017
{
    internal class Day18
    {
        public static string input
        {
            get;
            set;
        }
        private static List<ulong> nums = [];

        private static void Parse()
        {
            try
            {
                var p = input.Split("\n")[9].ExtractNumbers<ulong>()[0];
                var numbers = new List<ulong>(127);

                for (var _ = 0; _ < 127; _++)
                {
                    p = (p * 8505) % 0x7fffffff;
                    p = (p * 129749 + 12345) % 0x7fffffff;
                    numbers.Add(p % 10000);
                }

                nums = numbers;
            }
            catch (Exception e) { Console.WriteLine(e); }
        }

        public static ulong PartOne()
        {
            Parse();
            return nums[126];
        }

        public static ulong PartTwo()
        {
            var numbers = nums.ToList();

            var swapped = true;
            var count = 0ul;

            while(swapped)
            {
                swapped = false;

                for(var i = 1; i < 127 - (int)count; i++)
                {
                    if (numbers[i - 1] < numbers[i])
                    {
                        (numbers[i -1], numbers[i]) = (numbers[i], numbers[i - 1]);
                        swapped = true;
                    }
                }
                count++;
            }
            
            return 127 * (((count - 1) /2) + 1);
        }
    }
}
