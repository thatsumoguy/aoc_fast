using aoc_fast.Extensions;

namespace aoc_fast.Years._2024
{
    internal class Day18
    {
        public static string input
        {
            get;
            set;
        }

        private static Grid<bool> grid;
        private static List<int[]> bytes = [];

        private static void Parse()
        {
            grid = Grid<bool>.New(71, 71, false);
            bytes = input.ExtractNumbers<int>().Chunk(2).ToList();
        }

        private static int ShortestPath(Grid<bool> grid, Point start, Point end)
        {
            var q = new Queue<(Point pos, int steps)>();
            var visited = grid.NewWith(() => false);
            visited[start] = true;
            q.Enqueue((start, 0));
            while (q.TryDequeue(out var p))
            {
                if (p.pos == end) return p.steps;
                foreach (var dP in Directions.ORTHOGONAL)
                {
                    var next = p.pos + dP;
                    if (grid.Contains(next) && !grid[next] && !visited[next])
                    {
                        q.Enqueue((next, p.steps + 1));
                        visited[next] = true;
                    }
                }
            }
            return -1;
        }

        public static int PartOne()
        {
            Parse();
            for (var i = 0; i < 1024; i++) grid[bytes[i][0], bytes[i][1]] = true;
            return ShortestPath(grid, new Point(0, 0), new Point(70, 70));
        }

        public static string PartTwo()
        {
            var start = new Point(0, 0);
            var end = new Point(70, 70);
            for (var i = 1025; i < bytes.Count; i++)
            {
                grid[bytes[i][0], bytes[i][1]] = true;
                if (ShortestPath(grid, start, end) == -1) return $"{bytes[i][0]},{bytes[i][1]}";
            }
            return "";
        }

    }
}
