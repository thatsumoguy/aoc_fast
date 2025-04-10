using aoc_fast.Extensions;
using Point = aoc_fast.Extensions.Point;

namespace aoc_fast.Years._2023
{
    internal class Day21
    {
        public static string input { get; set; }

        private static readonly Point CENTER = new(65, 65);
        private static readonly Point[] CORNERS = [new Point(0, 0), new Point(130, 0), new Point(0, 130), new Point(130, 130)];

        private static (ulong partOne, ulong partTwo) answers;

        private static (ulong, ulong, ulong, ulong) BFS(Grid<byte> Grid, List<Point> starts, uint limit)
        {
            var grid = Grid<byte>.New(Grid);
            var todo = new Queue<(Point, uint)>();

            var evenInner = 0uL;
            var evenOuter = 0uL;
            var oddInner = 0uL;
            var oddOuter = 0uL;

            foreach(var start in starts)
            {
                grid[start] = (byte)'#';
                todo.Enqueue((start, 0u));
            }

            while(todo.TryDequeue(out var next))
            {
                var (pos, cost) = next;
                if(cost % 2 == 1)
                {
                    if (pos.Manhattan(CENTER) <= 65) oddInner++;
                    else oddOuter++;
                }
                else if (cost <= 64) evenInner++;
                else evenOuter++;

                if(cost < limit)
                {
                    foreach(var nextPos in Directions.ORTHOGONAL.Select(o => pos + o))
                    {
                        if(grid.Contains(nextPos) && grid[nextPos] != (byte)'#')
                        {
                            grid[nextPos] = (byte)'#';
                            todo.Enqueue((nextPos, cost + 1));
                        }
                    }
                }
            }

            return (evenInner, evenOuter, oddInner, oddOuter);
        }

        private static void Parse()
        {
            var grid = Grid<byte>.Parse(input);

            var (evenInner, evenOuter, oddInner, oddOuter) = BFS(grid, [CENTER], 130);
            var partOne = evenInner;

            var evenFull = evenInner + evenOuter;
            var oddFull = oddInner + oddOuter;
            var removeCorners = oddOuter;

            var(nextEvenInner, _, _, _) = BFS(grid, CORNERS.ToList(), 64);
            var addCorners = nextEvenInner;

            var n = 202300uL;
            var first = n * n * evenFull;
            var second = (n + 1) * (n + 1) * oddFull;
            var third = n * addCorners;
            var fourth = (n + 1) * removeCorners;
            var partTwo = first + second + third - fourth;

            answers = (partOne, partTwo);
        }

        public static ulong PartOne()
        {
            Parse();
            return answers.partOne;
        }
        public static ulong PartTwo() => answers.partTwo;
    }
}
