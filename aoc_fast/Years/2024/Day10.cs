using aoc_fast.Extensions;

namespace aoc_fast.Years._2024
{
    internal class Day10
    {
        public static string input
        {
            get;
            set;
        }

        private static Grid<byte> grid;

        private static uint DFS(Grid<byte> grid, bool distinct, Grid<int> seen, int id, Point point)
        {
            var res = 0u;

            foreach (var next in Directions.ORTHOGONAL.Select(o => point + o))
            {
                if (grid.Contains(next) && grid[next] + 1 == grid[point] && (distinct || seen[next] != id))
                {
                    seen[next] = id;
                    if (grid[next] == '0') res++;
                    else res += DFS(grid, distinct, seen, id, next);
                }
            }

            return res;
        }

        private static uint Solve(Grid<byte> grid, bool distinct)
        {
            var res = 0u;
            var seen = grid.NewWith(-1);

            for (var y = 0; y < grid.height; y++)
            {
                for (var x = 0; x < grid.width; x++)
                {
                    var point = new Point(x, y);
                    if (grid[point] == '9')
                    {
                        var id = y * grid.height + x;
                        res += DFS(grid, distinct, seen, id, point);
                    }
                }
            }

            return res;
        }

        public static uint PartOne()
        {
            grid = Grid<byte>.Parse(input);
            return Solve(grid, false);
        }
        public static uint PartTwo() => Solve(grid, true);
    }
}
