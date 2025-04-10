using System.Text;
using aoc_fast.Extensions;

namespace aoc_fast.Years._2017
{
    internal class Day19
    {
        public static string input
        {
            get;
            set;
        }

        private static (string partOne, int partTwo) answer;

        private static void Parse()
        {
            var grid = Grid<byte>.Parse(input);

            var pos = grid.Find((byte)'|').Value;
            var dir = Directions.DOWN;

            var partOne = new StringBuilder();
            var partTwo = 0;
            var continueLoop = true;

            while(continueLoop)
            {
                var next = grid[pos];

                switch(next)
                {
                    case (byte)'+':
                        var left = dir.CounterClockwise();
                        var right = dir.Clockwise();
                        dir = grid[pos + right] == (byte)' ' ? left : right;
                        break;
                    case (byte)' ':
                        goto outer;
                    default:
                        if(char.IsAsciiLetter((char)next)) partOne.Append((char)next); break;
                }
                pos += dir;
                partTwo++;
            }
            outer:
            answer = (partOne.ToString(), partTwo);
        }

        public static string PartOne()
        {
            Parse();
            return answer.partOne;
        }

        public static int PartTwo() => answer.partTwo;
    }
}
