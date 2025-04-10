using aoc_fast.Extensions;

namespace aoc_fast.Years._2022
{
    internal class Day12
    {
        public static string input { get; set; }

        private static (Grid<byte> grid, Point start) Input;

        private static int Height(Grid<byte> grid, Point point) => grid[point] switch
        {
            (byte)'S' => (int)'a',
            (byte)'E' => (int)'z',
            var b => (int)b
        };

        private static uint BFS((Grid<byte> grid, Point start) input, byte end)
        {
            var (grid, start) = input;
            var todo = new Queue<(Point, uint)>();
            todo.Enqueue((start, 0u));
            var visited = grid.NewWith(false);

            while(todo.TryDequeue(out var a))
            {
                var (point, cost) = a;
                if (grid[point] == end) return cost;

                foreach(var next in Directions.ORTHOGONAL.Select(x => x + point))
                {
                    if(grid.Contains(next) && !visited[next] && Height(grid, point) - Height(grid, next) <= 1)
                    {
                        todo.Enqueue((next, cost + 1));
                        visited[next] = true;
                    }
                }
            }
            throw new InvalidOperationException();
        }

        private static void Parse()
        {
            var grid = Grid<byte>.Parse(input);
            var start = grid.Find((byte)'E');
            Input = (grid, start.Value);
        }

        public static uint PartOne()
        {
            Parse();
            return BFS(Input, (byte)'S');
        }
        public static uint PartTwo() => BFS(Input, (byte)'a');
    }
}
