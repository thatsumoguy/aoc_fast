using aoc_fast.Extensions;

namespace aoc_fast.Years._2024
{
    internal class Day12
    {
        public static string input
        {
            get;
            set;
        }
        private static (int partOne, int partTwo) answers;

        private static void Parse()
        {
            var grid = Grid<byte>.Parse(input);
            var todo = new List<Point>();
            var edge = new List<(Point, Point)>();

            var seen = Grid<bool>.New(grid.width, grid.height, false);

            var partOne = 0;
            var partTwo = 0;

            for (var y = 0; y < grid.height; y++)
            {
                for (var x = 0; x < grid.width; x++)
                {
                    var point = new Point(x, y);
                    if (seen[point]) continue;

                    var kind = grid[point];
                    var check = (Point p) => grid.Contains(p) && grid[p] == kind;

                    var area = 0;
                    var perimeter = 0;
                    var sides = 0;

                    todo.Add(point);
                    seen[point] = true;

                    while (area < todo.Count)
                    {
                        var p = todo[area];
                        area++;

                        foreach (var dir in Directions.ORTHOGONAL)
                        {
                            var next = p + dir;

                            if (check(next))
                            {
                                if (!seen[next])
                                {
                                    todo.Add(next);
                                    seen[next] = true;
                                }
                            }
                            else
                            {
                                edge.Add((p, dir));
                                perimeter++;
                            }
                        }
                    }

                    foreach (var (p, d) in edge)
                    {
                        var r = d.Clockwise();
                        var l = d.CounterClockwise();

                        sides += (!check(p + l) || check(p + l + d) ? 1 : 0);
                        sides += (!check(p + r) || check(p + r + d) ? 1 : 0);
                    }
                    todo.Clear();
                    edge.Clear();

                    partOne += area * perimeter;
                    partTwo += area * (sides / 2);
                }
            }

            answers = (partOne, partTwo);
        }
        public static int PartOne()
        {
            Parse();
            return answers.partOne;
        }
        public static int PartTwo() => answers.partTwo;
    }
}
