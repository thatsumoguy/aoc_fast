using aoc_fast.Extensions;

namespace aoc_fast.Years._2023
{
    internal class Day16
    {
        public static string input { get; set; }

        private const uint UP = 0;
        private const uint DOWN = 1;
        private const uint LEFT = 2;
        private const uint RIGHT = 3;

        record Input(Grid<byte> grid, Grid<int> up, Grid<int> down, Grid<int> left, Grid<int> right);

        private static Input inputObj;

        private static int Count(Input input, (Point, uint) start)
        {
            var (grid, up, down, left, right) = input;

            var todo = new Queue<(Point, uint)>(1000);
            var seen = grid.NewWith((byte)0);
            var energized = grid.NewWith(false);

            todo.Enqueue(start);

            while(todo.TryDequeue(out var nextPair))
            {
                var (pos, dir) = nextPair;
                var next = (uint direction) =>
                {
                    var mask = (byte)(1 << (int)direction);
                    if ((seen[pos] & mask) != 0) return;
                    seen[pos] |= mask;

                    switch(direction)
                    {
                        case UP:
                            var upX = pos.X;
                            var last = up[pos];

                            for (var y = last; y < pos.Y; y++) energized[upX, y + 1] = true;
                            if (last >= 0) todo.Enqueue((new Point(upX, last), UP));
                            break;
                        case DOWN:
                            var downX = pos.X;
                            var downLast = down[pos];

                            for (var y = pos.Y; y < downLast; y++) energized[downX, y] = true;
                            if(downLast < grid.height) todo.Enqueue((new Point(downX, downLast), DOWN));
                            break;
                        case LEFT:
                            var leftY = pos.Y;
                            var leftLast = left[pos];

                            for (var x = leftLast; x < pos.X; x++) energized[x + 1, leftY] = true;
                            if (leftLast >= 0) todo.Enqueue((new Point(leftLast, leftY), LEFT));
                            break;
                        case RIGHT:
                            var rightY = pos.Y;
                            var rightLast = right[pos];

                            for (var x = pos.X; x < rightLast; x++) energized[x, rightY] = true;
                            if(rightLast < grid.width) todo.Enqueue((new Point(rightLast, rightY), RIGHT));
                            break;

                    }
                };

                switch(grid[pos])
                {
                    case (byte)'.':
                        next(dir);
                        break;
                    case (byte)'/':
                        switch (dir)
                        {
                            case UP:
                                next(RIGHT);
                                break;
                            case DOWN:
                                next(LEFT);
                                break;
                            case LEFT:
                                next(DOWN);
                                break;
                            case RIGHT:
                                next(UP);
                                break;
                        }
                        break;
                    case (byte)'\\':
                        switch (dir)
                        {
                            case UP:
                                next(LEFT);
                                break;
                            case DOWN:
                                next(RIGHT);
                                break;
                            case LEFT:
                                next(UP);
                                break;
                            case RIGHT:
                                next(DOWN);
                                break;
                        }
                        break;
                    case (byte)'|':
                        switch(dir)
                        {
                            case UP or DOWN:
                                next(dir);
                                break;
                            case LEFT or RIGHT:
                                next(UP);
                                next(DOWN);
                                break;
                        }
                        break;
                    case (byte)'-':
                        switch(dir)
                        {
                            case LEFT or RIGHT:
                                next(dir);
                                break;
                            case UP or DOWN:
                                next(LEFT);
                                next(RIGHT);
                                break;
                        }
                        break;
                }
            }
            return energized.data.Where(b => b).Count();
        }

        private static void Parse()
        {
            var grid = Grid<byte>.Parse(input);

            var up = grid.NewWith(0);
            var down = grid.NewWith(0);
            var left = grid.NewWith(0);
            var right = grid.NewWith(0);

            for(var x = 0; x < grid.width; x++)
            {
                var last = -1;

                for(var y = 0; y < grid.height; y++)
                {
                    var point = new Point(x, y);
                    up[point] = last;

                    var cur = grid[point];
                    if (cur == (byte)'/' || cur == (byte)'\\' || cur == (byte)'-') last = y;
                }
            }

            for(var x = 0; x < grid.width; ++x)
            {
                var last = grid.height;
                for(var y = grid.height - 1; y >= 0; y--)
                {
                    var point = new Point(x, y);

                    down[point] = last;

                    var cur = grid[point];
                    if (cur == (byte)'/' || cur == (byte)'\\' || cur == (byte)'-') last = y;
                }
            }

            for(var y = 0; y < grid.height; y++)
            {
                var last = -1;
                for(var x = 0; x < grid.width; x++)
                {
                    var point = new Point(x, y);
                    left[point] = last;
                    var cur = grid[point];
                    if (cur == (byte)'/' || cur == (byte)'\\' || cur == (byte)'|') last = x;
                }
            }

            for(var y = 0; y < grid.height; y++)
            {
                var last = grid.width;
                for(var x = grid.width - 1; x >= 0; x--)
                {
                    var point = new Point(x, y);
                    right[point] = last;

                    var cur = grid[point];
                    if (cur == (byte)'/' || cur == (byte)'\\' || cur == (byte)'|') last = x;
                }
            }

            inputObj = new Input(grid, up, down, left, right);
        }

        public static int PartOne()
        {
            Parse();
            return Count(inputObj, (Directions.ORIGIN, RIGHT));
        }

        private static object mutex = new();
        public static int PartTwo()
        {
            var grid = inputObj.grid;
            var todo = new List<(Point, uint)>();

            for(var x = 0; x<grid.width; x++)
            {
                todo.Add((new Point(x, 0), DOWN));
                todo.Add((new Point(x, grid.height -1), UP));
            }
            for(var y = 0; y < grid.height; y++)
            {
                todo.Add((new Point(0, y), RIGHT));
                todo.Add((new Point(grid.width -1, y), LEFT));
            }
            var tiles = 0;
            Threads.Spawn(() => Worker(inputObj, ref tiles, todo));
            return tiles;
        }

        private static void Worker(Input input, ref int tiles, List<(Point, uint)> todo)
        {
            while(true)
            {
                (Point, uint) start;
                lock(mutex)
                {
                    if (!todo.TryPopFront(out start)) break;
                }
                var count = Count(input, start);
                lock(mutex)
                {
                    tiles = Math.Max(count, tiles);
                }
            }
        }
    }
}
