using System.Text;
using aoc_fast.Extensions;

namespace aoc_fast.Years._2018
{
    internal class Day20
    {
        public static string input { get; set; }
        private static (uint partOne, int partTwo) answer;

        private static void Parse()
        {
            var index = 6105u;
            var grid = new uint[12100];
            Array.Fill(grid, uint.MaxValue);
            var stack = new List<uint>(500);
            var partOne = 0u;

            grid[index] = 0;

            foreach(var b in Encoding.ASCII.GetBytes(input))
            {
                var dist = grid[index];

                switch(b)
                {
                    case (byte)'(':
                        stack.Add(index); 
                        break;
                    case (byte)'|':
                        index = stack[^1];
                        break;
                    case (byte)')':
                        index = stack.Pop();
                        break;
                    case (byte)'N':
                        index -= 110;
                        break;
                    case (byte)'S':
                        index += 110;
                        break;
                    case (byte)'W':
                        index--;
                        break;
                    case (byte)'E':
                        index++;
                        break;
                }

                grid[index] = Math.Min(grid[index], dist + 1);
                partOne = Math.Max(partOne, grid[index]);
            }

            answer= (partOne, grid.Where(d => d >= 1000 && d < uint.MaxValue).Count());
        }

        public static uint PartOne()
        {
            Parse();
            return answer.partOne;
        }
        public static int PartTwo() => answer.partTwo;
    }
}
