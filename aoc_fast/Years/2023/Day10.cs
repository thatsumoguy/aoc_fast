using aoc_fast.Extensions;

namespace aoc_fast.Years._2023
{
    internal class Day10
    {
        public static string input { get; set; }
        private static (int partOne, int partTwo) answers;

        private static void Parse()
        {
            var grid = Grid<byte>.Parse(input);
            var determinant = (Point a, Point b) => a.X * b.Y - a.Y * b.X;

            var corner = grid.Find((byte)'S').Value;
            var dirCorner = grid[corner + Directions.UP];
            var direction = dirCorner == (byte)'|' || dirCorner == (byte)'7' || dirCorner == (byte)'F' ? Directions.UP : Directions.DOWN;
            var pos = corner + direction;

            var steps = 1;
            var area = 0;

            while (true)
            {
                while (grid[pos] == (byte)'-' || grid[pos] == (byte)'|')
                {
                    pos += direction;
                    steps++;
                }

                switch (grid[pos])
                {
                    case (byte)'7' when direction == Directions.UP:
                        direction = Directions.LEFT;
                        break;
                    case (byte)'F' when direction == Directions.UP:
                        direction = Directions.RIGHT;
                        break;
                    case (byte)'J' when direction == Directions.DOWN:
                        direction = Directions.LEFT;
                        break;
                    case (byte)'L' when direction == Directions.DOWN:
                        direction = Directions.RIGHT;
                        break;
                    case (byte)'L' or (byte)'J':
                        direction = Directions.UP;
                        break;
                    case (byte)'F' or (byte)'7':
                        direction = Directions.DOWN;
                        break;
                    default:
                        area += determinant(corner, pos);
                        goto breakLoop;
                }

                area += determinant(corner, pos);
                corner = pos;
                pos += direction;
                steps++;
            };
        breakLoop:
            var partOne = steps / 2;
            var partTwo = area.Abs() / 2 - steps / 2 + 1;
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
