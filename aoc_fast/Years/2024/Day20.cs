using aoc_fast.Extensions;

namespace aoc_fast.Years._2024
{
    internal class Day20
    {
        public static string input
        {
            get;
            set;
        }

        private static Grid<int> time;

        private static void Parse()
        {
            var grid = Grid<byte>.Parse(input);
            var start = grid.Find((byte)'S').Value;
            var end = grid.Find((byte)'E').Value;

            time = grid.NewWith(int.MaxValue);

            var elapsed = 0;

            var pos = start;

            var dir = Directions.ORTHOGONAL.First(o => grid[pos + o] != '#');

            while (pos != end)
            {
                time[pos] = elapsed;
                elapsed++;

                dir = new Point[] { dir, dir.Clockwise(), dir.CounterClockwise() }.First(o => grid[pos + o] != '#');
                pos += dir;
            }
            time[end] = elapsed;
        }
        private static int Check(Grid<int> time, Point first, Point delta)
        {
            var second = first + delta;

            return time.Contains(second) && time[second] != int.MaxValue && Math.Abs(time[first] - time[second]) - first.Manhattan(second) >= 100 ? 1 : 0;
        }
        private static void Worker(Grid<int> time, ref int total, List<Point> batch)
        {
            var cheats = 0;

            foreach (var point in batch)
            {
                for (var x = 2; x < 21; x++) cheats += Check(time, point, new Point(x, 0));
                for (var y = 1; y < 21; y++)
                {
                    for (var x = y - 20; x < (21 - y); x++) cheats += Check(time, point, new Point(x, y));
                }
            }
            Interlocked.Add(ref total, cheats);
        }
        public static int PartOne()
        {
            Parse();
            var cheats = 0;

            for (var y = 1; y < time.height - 1; y++)
            {
                for (var x = 1; x < time.height - 1; x++)
                {
                    var p = new Point(x, y);
                    if (time[p] != int.MaxValue)
                    {
                        cheats += Check(time, p, new Point(2, 0));
                        cheats += Check(time, p, new Point(0, 2));
                    }
                }
            }
            return cheats;
        }
        public static int PartTwo()
        {
            var items = new List<Point>(10000);
            for (var y = 1; y < time.height - 1; y++)
            {
                for (var x = 1; x < time.height - 1; x++)
                {
                    if (time[x, y] != int.MaxValue) items.Add(new Point(x, y));
                }
            }
            var total = 0;
            Threads.SpawnBatches(items, (batch) => Worker(time, ref total, batch));
            return total;
        }
    }
}
