using aoc_fast.Extensions;

namespace aoc_fast.Years._2020
{
    internal class Day3
    {
        public static string input { get; set; }
        private static Grid<byte> grid;
        private static void Parse() => grid = Grid<byte>.Parse(input);
        private static ulong Toboggan(Grid<byte> grid, int dx, int dy)
        {
            var point = Directions.ORIGIN;
            var trees = 0ul;

            while(point.Y < grid.height)
            {
                if (grid[point] == '#') trees++;
                point.X = (point.X + dx) % grid.width;
                point.Y += dy;
            }
            return trees;
        }
        public static ulong PartOne()
        {
            Parse();
            return Toboggan(grid, 3, 1);
        }
        public static ulong PartTwo() => Toboggan(grid, 1, 1) * Toboggan(grid, 3, 1) * Toboggan(grid, 5, 1) * Toboggan(grid, 7, 1) * Toboggan(grid, 1, 2);
    }
}
