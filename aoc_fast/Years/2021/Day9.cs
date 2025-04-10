using aoc_fast.Extensions;

namespace aoc_fast.Years._2021
{
    internal class Day9
    {
        public static string input { get; set; }

        private static Grid<byte> grid;

        private static int FloodFill(Grid<byte> grid, Point point)
        {
            grid[point] = (byte)'9';
            var size = 1;

            foreach(var next in Directions.ORTHOGONAL.Select(delta => point + delta))
            {
                if (grid.Contains(next) && grid[next] < (byte)'9') size += FloodFill(grid, next);
            }
            return size;
        }

        public static int PartOne()
        {
            grid = Grid<byte>.Parse(input);
            var riskLevels = 0;

            for(var x = 0; x < grid.width; x++)
            {
                for(var y = 0; y < grid.height; y++)
                {
                    var point = new Point(x, y);
                    var cur = grid[point];

                    var lowPoint = Directions.ORTHOGONAL.Select(n => point + n).Where(n => grid.Contains(n)).All(n => grid[n] > cur);
                    if (lowPoint) riskLevels += 1 + (int)(cur - (byte)'0');
                }
            }

            return riskLevels;
        }

        public static int PartTwo()
        {
            var grid2 = Grid<byte>.New(grid);
            var basins = new List<int>();

            for(var x = 0; x < grid2.width; x++)
            {
                for(var y = 0; y < grid2.height; y++)
                {
                    var next = new Point(x, y);
                    if (grid2[next] < (byte)'9') basins.Add(FloodFill(grid, next));
                }
            }
            basins.Sort();
            basins.Reverse();
            return basins.Take(3).Aggregate(1, (acc, b) => acc * b);
        }
    }
}
