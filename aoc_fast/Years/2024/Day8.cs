using aoc_fast.Extensions;

namespace aoc_fast.Years._2024
{
    internal class Day8
    {
        public static string input
        {
            get;
            set;
        }

        private static Grid<byte> grid;

        private static HashSet<Point> antinodes = [];
        private static HashSet<Point> allAntiNodes = [];

        private static (int partOne, int partTwo) answers;
        private static void AntiNode(Point p1, Point p2, bool partTwo = false)
        {
            var newPoint = new Point(p2.X + (p2.X - p1.X), p2.Y + (p2.Y - p1.Y));
            if (partTwo)
            {
                allAntiNodes.Add(p2);
                while (grid.Contains(newPoint))
                {
                    allAntiNodes.Add(newPoint);
                    newPoint += new Point(p2.X - p1.X, p2.Y - p1.Y);
                }
            }
            else if (grid.Contains(newPoint)) antinodes.Add(newPoint);
        }

        private static void Parse()
        {
            grid = Grid<byte>.Parse(input);
            var maxX = grid.width;
            var maxY = grid.height;

            var nodes = new Dictionary<byte, List<Point>>();

            for (var y = 0; y < maxY; y++)
            {
                for (var x = 0; x < maxX; x++)
                {
                    if (grid[x, y] != '.')
                    {
                        if (nodes.TryGetValue(grid[x, y], out List<Point>? value)) value.Add(new Point(x, y));
                        else nodes[grid[x, y]] = [new Point(x, y)];
                    }
                }
            }

            foreach (var k in nodes)
            {
                var nodesList = nodes[k.Key];
                for (var i = 0; i < nodesList.Count; i++)
                {
                    for (var j = 0; j < i; j++)
                    {
                        var node1 = nodesList[i];
                        var node2 = nodesList[j];
                        AntiNode(node1, node2);
                        AntiNode(node2, node1);
                        answers.partOne = antinodes.Count;
                        AntiNode(node1, node2, true);
                        AntiNode(node2, node1, true);
                        answers.partTwo = allAntiNodes.Count;
                    }
                }
            }
        }

        public static int PartOne()
        {
            Parse();
            return answers.partOne;
        }
        public static int PartTwo() => answers.partTwo;
    }
}
