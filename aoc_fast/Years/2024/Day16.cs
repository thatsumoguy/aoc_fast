using aoc_fast.Extensions;

namespace aoc_fast.Years._2024
{
    internal class Day16
    {
        public static string input
        {
            get;
            set;
        }

        private static Grid<byte> grid;
        private static Point[] Direction = [Directions.RIGHT, Directions.DOWN, Directions.LEFT, Directions.UP];
        private static (int partOne, int partTwo) answers;

        private static void Parse()
        {
            grid = Grid<byte>.Parse(input);
            var start = grid.Find((byte)'S').Value;
            var end = grid.Find((byte)'E').Value;

            var buckets = new List<(Point, int)>[1001];
            for (int i = 0; i < 1001; i++) buckets[i] = [];

            var seen = grid.NewWith(() => new int[4] { int.MaxValue, int.MaxValue, int.MaxValue, int.MaxValue });
            var cost = 0;
            var lowestCost = int.MaxValue;

            buckets[0].Add((start, 0));
            seen[start][0] = 0;

            while (lowestCost == int.MaxValue)
            {
                var index = cost % 1001;
                while (buckets[index].PopCheck(out var item))
                {
                    var pos = item.Item1;
                    var dir = item.Item2;
                    if (pos == end)
                    {
                        lowestCost = cost;
                        break;
                    }
                    var left = (dir + 3) % 4;
                    var right = (dir + 1) % 4;

                    (Point, int, int)[] next = [(pos + Direction[dir], dir, cost + 1), (pos, left, cost + 1000), (pos, right, cost + 1000)];

                    for (var x = 0; x < 3; x++)
                    {

                        var (nextPos, nextDir, nextCost) = next[x];
                        if (grid[nextPos] != (byte)'#' && nextCost < seen[nextPos][nextDir])
                        {
                            var innerIndex = nextCost % 1001;
                            buckets[innerIndex].Add((nextPos, nextDir));
                            seen[nextPos][nextDir] = nextCost;
                        }
                    }
                }
                cost++;
            }
            var todo = new Queue<(Point, int, int)>();
            var path = grid.NewWith(() => false);

            for (var dir = 0; dir < 4; dir++)
            {
                if (seen[end][dir] == lowestCost) todo.Enqueue((end, dir, lowestCost));
            }
            while (todo.TryDequeue(out var next))
            {
                var pos = next.Item1;
                var dir = next.Item2;
                var backwardCost = next.Item3;
                path[pos] = true;
                if (pos == start) continue;

                var left = (dir + 3) % 4;
                var right = (dir + 1) % 4;
                (Point, int, int)[] nextitem = [(pos - Direction[dir], dir, backwardCost - 1), (pos, left, backwardCost - 1000), (pos, right, backwardCost - 1000)];

                foreach (var (nextPos, nextDir, nextCost) in nextitem)
                {
                    if (nextCost == seen[nextPos][nextDir])
                    {
                        todo.Enqueue((nextPos, nextDir, nextCost));
                        seen[nextPos][nextDir] = int.MaxValue;
                    }
                }
            }

            answers = (lowestCost, path.data.Where(b => b).Count());
        }
        public static int PartOne()
        {
            Parse();
            return answers.partOne;
        }
        public static int PartTwo() => answers.partTwo;

    }
}
