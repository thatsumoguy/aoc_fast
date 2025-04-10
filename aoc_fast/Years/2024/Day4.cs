using aoc_fast.Extensions;

namespace aoc_fast.Years._2024
{
    internal class Day4
    {
        public static string input
        {
            get;
            set;
        }

        private static (int partOne, int partTwo) answer;

        private static void Parse()
        {
            var grid = Grid<byte>.Parse(input);
            var partOne = 0;
            var partTwo = 0;
            for (var x = 0; x < grid.width; x++)
            {
                for (var y = 0; y < grid.height; y++)
                {
                    if (grid[x, y] == 'X')
                    {
                        foreach (var neighbor in Directions.DIAGONAL)
                        {
                            //Short circuit to save time
                            if (grid.Contains((x, y) + neighbor * 3) && grid[(x, y) + neighbor] == 'M' && grid[(x, y) + neighbor * 2] == 'A' && grid[(x, y) + neighbor * 3] == 'S')
                                partOne++;
                        }
                    }
                    else if (grid[x, y] == 'A' && x > 0 && y > 0 && x < grid.width - 1 && y < grid.height - 1)
                    {
                        var upLeft = grid[x - 1, y - 1];
                        var upRight = grid[x + 1, y - 1];
                        var downLeft = grid[x - 1, y + 1];
                        var downRight = grid[x + 1, y + 1];

                        //The two possible orientations
                        //M.S
                        //.A.
                        //M.S

                        //M.M
                        //.A.
                        //S.S
                        //Note that the upLeft and downRight are always either M-S or S-M meaning they are always 6 spaces apart
                        //This also applies to the upRight and downLeft
                        partTwo += Math.Abs(upLeft - downRight) == 6 && Math.Abs(upRight - downLeft) == 6 ? 1 : 0;
                    }
                }
            }
            answer = (partOne, partTwo);
        }

        public static int PartOne()
        {
            Parse();
            return answer.partOne;
        }
        public static int PartTwo() => answer.partTwo;
    }
}
