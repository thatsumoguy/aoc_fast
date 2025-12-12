using System.Text;
using aoc_fast.Extensions;
using Iced.Intel;
namespace aoc_fast.Years._2025
{
    public class Day1
    {
        public static string input
        {
            get;
            set;
        }

        private static (int partOne, int partTwo) answers;
        private static void Parse()
        {
            var dirs = Encoding.UTF8.GetBytes(input).Where(b => b.IsAsciiUpperLetter()).ToArray();
            var amounts = input.ExtractNumbers<int>();
            var dial = 50;
            var partOne = 0;
            var partTwo = 0;

            foreach(var (dir, amount) in dirs.Zip(amounts))
            {
                if(dir == (byte)'R')
                {
                    partTwo += (dial + amount) / 100;
                    dial = (dial + amount) % 100;
                }
                else
                {
                    var reversed = (100 - dial) % 100;
                    partTwo += (reversed + amount) / 100;
                    var val = (dial - amount) % 100;
                    dial = val < 0 ? val + 100 : val;
                }
                partOne += dial == 0 ? 1 : 0;
            }
            answers = (partOne, partTwo);
        }

        public static int PartOne()
        {
            Parse();
            return answers.partOne;
        }
        public static int PartTwo() => answers.partTwo;
    }
}
