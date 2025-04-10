using aoc_fast.Extensions;
using static aoc_fast.Extensions.Hash;

namespace aoc_fast.Years._2024
{
    internal class Day6
    {
        public static string input
        {
            get;
            set;
        }

        private static Grid<byte> OrigGrid;
        private static (int partOne, int partTwo) answer;

        private static bool IsLoop(ShortCuts sc, HashSet<(Point, Point)> seen, Point pos, Point dir)
        {
            var obstacle = pos + dir;

            while (sc.Up.Contains(pos))
            {
                if (!seen.Add((pos, dir))) return true;

                switch ((dir.X, dir.Y))
                {
                    case (0, -1):
                        var next = sc.Up[pos];
                        if (pos.X == obstacle.X && pos.Y > obstacle.Y && obstacle.Y >= next.Y) pos = obstacle - Directions.UP;
                        else pos = next;
                        break;
                    case (0, 1):
                        next = sc.Down[pos];
                        if (pos.X == obstacle.X && pos.Y < obstacle.Y && obstacle.Y <= next.Y) pos = obstacle - Directions.DOWN;
                        else pos = next;
                        break;
                    case (-1, 0):
                        next = sc.Left[pos];
                        if (pos.Y == obstacle.Y && pos.X > obstacle.X && obstacle.X >= next.X) pos = obstacle - Directions.LEFT;
                        else pos = next;
                        break;
                    case (1, 0):
                        next = sc.Right[pos];
                        if (pos.Y == obstacle.Y && pos.X < obstacle.X && obstacle.X <= next.X) pos = obstacle - Directions.RIGHT;
                        else pos = next;
                        break;

                }

                dir = dir.Clockwise();
            }

            return false;
        }

        private static void Parse() => OrigGrid = Grid<byte>.Parse(input);




        public static int PartOne()
        {
            Parse();
            var grid = Grid<byte>.New(OrigGrid);
            var start = grid.Find((byte)'^');
            var curPos = start;
            var dir = Directions.UP;
            var res = 1;

            while (grid.Contains(curPos + dir))
            {
                if (grid[curPos + dir] == '#')
                {
                    dir = dir.Clockwise();
                    continue;
                }
                var next = curPos + dir;

                if (grid[next] == '.')
                {
                    res++;
                    grid[next] = (byte)'^';
                }
                curPos = next;
            }
            return res;
        }
        public static int PartTwo()
        {

            var grid = Grid<byte>.New(OrigGrid);
            var start = grid.Find((byte)'^').Value;
            var curPos = start;
            var dir = Directions.UP;
            var path = new List<(Point, Point)>(5000);

            while (grid.Contains(curPos + dir))
            {
                if (grid[curPos + dir] == '#')
                {
                    dir = dir.Clockwise();
                    continue;
                }
                var next = curPos + dir;

                if (grid[next] == '.')
                {
                    path.Add((curPos, dir));
                    grid[next] = (byte)'^';
                }
                curPos = next;
            }

            var shortCut = ShortCuts.New(grid);
            var total = 0;

            Threads.SpawnBatches(path, batch => Worker(shortCut, batch, ref total));

            return total;
        }


        private static void Worker(ShortCuts sc, List<(Point, Point)> batch, ref int total)
        {
            var seen = new FastSet<(Point, Point)>();
            var res = batch.Where(p =>
            {
                seen.Clear();
                return IsLoop(sc, seen, p.Item1, p.Item2);
            }).Count();
            Interlocked.Add(ref total, res);
        }
    }

    class ShortCuts
    {
        public Grid<Point> Up { get; set; }
        public Grid<Point> Down { get; set; }
        public Grid<Point> Left { get; set; }
        public Grid<Point> Right { get; set; }


        public static ShortCuts New(Grid<byte> grid)
        {

            var up = grid.NewWith(Directions.ORIGIN);
            var down = grid.NewWith(Directions.ORIGIN);
            var left = grid.NewWith(Directions.ORIGIN);
            var right = grid.NewWith(Directions.ORIGIN);

            for (var x = 0; x < grid.width; x++)
            {
                var last = new Point(x, -1);
                for (var y = 0; y < grid.height; y++)
                {
                    var p = new Point(x, y);
                    if (grid[p] == '#') last = new Point(x, y + 1);
                    up[p] = last;
                }
            }
            for (var x = 0; x < grid.width; x++)
            {
                var last = new Point(x, grid.height);
                for (var y = grid.height - 1; y > 0; y--)
                {
                    var p = new Point(x, y);
                    if (grid[p] == '#') last = new Point(x, y - 1);
                    down[p] = last;
                }
            }

            for (var y = 0; y < grid.height; y++)
            {
                var last = new Point(-1, y);
                for (var x = 0; x < grid.width; x++)
                {
                    var p = new Point(x, y);
                    if (grid[p] == '#') last = new Point(x + 1, y);
                    left[p] = last;
                }
            }
            for (var y = 0; y < grid.height; y++)
            {
                var last = new Point(grid.width, y);
                for (var x = grid.width - 1; x > 0; x--)
                {
                    var p = new Point(x, y);
                    if (grid[p] == '#') last = new Point(x - 1, y);
                    right[p] = last;
                }
            }
            return new() { Up = up, Down = down, Left = left, Right = right };

            throw new Exception();
        }
    }
}
