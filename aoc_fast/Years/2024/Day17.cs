using System.Text;
using aoc_fast.Extensions;

namespace aoc_fast.Years._2024
{
    internal class Day17
    {
        public static string input
        {
            get;
            set;
        }
        private static int RegA = 0;
        private static int RegB = 0;
        private static int RegC = 0;
        private static List<int> Program = [];
        private static (string partOne, long partTwo) answers;

        private static void Parse()
        {
            var getOut = (long a) =>
            {
                var partial = (a % 8) ^ 1;
                return ((partial ^ (a >> (int)partial)) ^ 5) % 8;
            };
            var getCombo = (int param) => param switch
            {
                0 or 1 or 2 or 3 => param,
                4 => RegA,
                5 => RegB,
                6 => RegC,
            };
            var nums = input.ExtractNumbers<int>();
            var output = new StringBuilder();
            RegA = nums[0];
            Program.AddRange(nums[3..]);
            while (RegA > 0)
            {
                output.Append(getOut(RegA));
                output.Append(',');
                RegA >>= 3;
            }
            var found = new HashSet<long>
            {
                0
            };
            foreach (var num in Program.Reverse<int>())
            {
                var innerFound = new HashSet<long>();
                foreach (var curr in found)
                {
                    for (var i = 0; i < 8; i++)
                    {
                        var newCurr = (curr << 3) + i;
                        if (getOut(newCurr) == num) innerFound.Add(newCurr);
                    }
                }
                found = innerFound;
            }
            answers = (output.Remove(output.Length - 1, 1).ToString(), found.Min());
        }
        public static string PartOne()
        {
            Parse();
            return answers.partOne;
        }
        public static long PartTwo() => answers.partTwo;
    }
}
