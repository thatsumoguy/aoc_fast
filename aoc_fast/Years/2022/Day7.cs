using System.Text;
using aoc_fast.Extensions;

namespace aoc_fast.Years._2022
{
    internal class Day7
    {
        public static string input { get; set; }

        private static List<uint> nums = [];

        private static void Parse()
        {
            var cd = false;

            var total = 0u;
            var stack = new List<uint>();
            var sizes = new List<uint>();

            foreach(var token in input.Split([' ', '\t', '\n'], StringSplitOptions.RemoveEmptyEntries))
            {
                if (cd)
                {
                    if (token == "..")
                    {
                        sizes.Add(total);
                        total += stack.Pop();
                    }
                    else
                    {
                        stack.Add(total);
                        total = 0;
                    }
                    cd = false;
                }
                else if (token == "cd") cd = true;
                
                else if (Encoding.UTF8.GetBytes(token)[0].IsAsciiDigit()) total += token.ExtractNumbers<uint>()[0];
            }
            while(stack.Count > 0)
            {
                sizes.Add(total);
                total += stack.Pop();
            }
            nums = sizes;
        }

        public static uint PartOne()
        {
            Parse();
            return nums.Where(x => x <= 100000).Sum();
        }
        public static uint PartTwo()
        {
            var root = nums.Last();
            var needed = 30000000 - (70000000 - root);
            return nums.Where(x => x >= needed).Min();
        }
    }
}
