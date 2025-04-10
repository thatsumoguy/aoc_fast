using System.Text;
using aoc_fast.Extensions;

namespace aoc_fast.Years._2024
{
    internal class Day15
    {
        public static string input
        {
            get;
            set;
        }

        private static Grid<byte> Map;
        private static string instructions = "";

        private static void Shrink(Grid<byte> grid, ref Point start, Point dir)
        {
            var pos = start + dir;
            var size = 1;

            while (grid[pos] != '.' && grid[pos] != '#')
            {
                pos += dir;
                size++;
            }

            if (grid[pos] == '.')
            {
                var prev = (byte)'.';
                var innerPos = start + dir;
                for (var _ = 0; _ < size; _++)
                {
                    (grid[innerPos], prev) = (prev, grid[innerPos]);
                    innerPos += dir;
                }

                start += dir;
            }
        }

        private static void Grow(Grid<byte> grid, ref Point start, Point dir, List<Point> todo)
        {
            if (grid[start + dir] == '.')
            {
                start += dir;
                return;
            }

            todo.Clear();
            todo.Add(Directions.ORIGIN);
            todo.Add(start);
            var index = 1;

            while (index < todo.Count)
            {
                var next = todo[index] + dir;
                index++;

                Point first;
                Point second;
                switch (grid[next])
                {
                    case (byte)'[':
                        (first, second) = (next, next + Directions.RIGHT); break;
                    case (byte)']':
                        (first, second) = (next + Directions.LEFT, next); break;
                    case (byte)'#':
                        return;
                    default: continue;
                }

                if (first != todo[^2])
                {
                    todo.Add(first);
                    todo.Add(second);
                }
            }

            foreach (var point in todo[2..].Reverse<Point>())
            {
                grid[point + dir] = grid[point];
                grid[point] = (byte)'.';
            }

            start += dir;
        }

        private static Grid<byte> Stretch(Grid<byte> grid)
        {
            var next = Grid<byte>.New(grid.width * 2, grid.height, (byte)'.');

            for (var y = 0; y < grid.height; y++)
            {
                for (var x = 0; x < grid.width; x++)
                {
                    var b = grid[new Point(x, y)];
                    if (!"#O@"u8.ToArray().Contains(b)) continue;
                    var (left, right) = b switch
                    {
                        (byte)'#' => ((byte)'#', (byte)'#'),
                        (byte)'O' => ((byte)'[', (byte)']'),
                        (byte)'@' => ((byte)'@', (byte)'.'),
                    };

                    next[new Point(2 * x, y)] = left;
                    next[new Point(2 * x + 1, y)] = right;
                }
            }

            return next;
        }

        private static int GPS(Grid<byte> grid, byte needle)
        {
            var res = 0;

            for (var y = 0; y < grid.height; y++)
            {
                for (var x = 0; x < grid.width; x++)
                {
                    var point = new Point(x, y);
                    if (grid[point] == needle) res += 100 * point.Y + point.X;
                }
            }

            return res;
        }
        private static void Parse()
        {
            var split = input.Split("\n\n", StringSplitOptions.RemoveEmptyEntries);
            (Map, instructions) = (Grid<byte>.Parse(split[0]), split[1]);
        }

        public static int PartOne()
        {
            Parse();
            var grid = Grid<byte>.New(Map);
            var pos = grid.Find((byte)'@').Value;
            grid[pos] = (byte)'.';
            foreach (var b in Encoding.UTF8.GetBytes(input))
            {
                switch (b)
                {
                    case (byte)'<': Shrink(grid, ref pos, Directions.LEFT); break;
                    case (byte)'>': Shrink(grid, ref pos, Directions.RIGHT); break;
                    case (byte)'^': Shrink(grid, ref pos, Directions.UP); break;
                    case (byte)'v': Shrink(grid, ref pos, Directions.DOWN); break;
                }
            }

            return GPS(grid, (byte)'O');
        }

        public static int PartTwo()
        {
            var grid = Stretch(Map);
            var pos = grid.Find((byte)'@').Value;
            grid[pos] = (byte)'.';

            var todo = new List<Point>(50);

            foreach (var b in Encoding.UTF8.GetBytes(input))
            {
                switch (b)
                {
                    case (byte)'<': Shrink(grid, ref pos, Directions.LEFT); break;
                    case (byte)'>': Shrink(grid, ref pos, Directions.RIGHT); break;
                    case (byte)'^': Grow(grid, ref pos, Directions.UP, todo); break;
                    case (byte)'v': Grow(grid, ref pos, Directions.DOWN, todo); break;
                }
            }

            return GPS(grid, (byte)'[');
        }
    }
}
