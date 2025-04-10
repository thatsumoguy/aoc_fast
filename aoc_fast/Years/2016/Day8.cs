using aoc_fast.Extensions;
using Point = aoc_fast.Extensions.Point;

namespace aoc_fast.Years._2016
{
    class Day8
    {
        public static string input
        {
            get;
            set;
        }
        private static Point[] Points = [];

        private static void Parse()
        {
            var amounts = input.ExtractNumbers<int>().Chunk(2);
            var points = new List<Point>();

            foreach(var (line, amts) in input.Split("\n",StringSplitOptions.RemoveEmptyEntries).Zip(amounts))
            {
                if(line.StartsWith("rect"))
                {
                    for(var x = 0; x < amts[0]; x++)
                    {
                        for(var y = 0; y < amts[1]; y++)
                        {
                            points.Add(new Point(x, y));
                        }
                    }
                }
                else if(line.StartsWith("rotate row"))
                {
                    for(var p = 0; p < points.Count; p++)
                    {
                        var point = points[p];
                        if (point.Y == amts[0]) point.X = (point.X + amts[1]) % 50;
                        points[p] = point;
                    }
                }
                else
                {
                    for (var p = 0; p < points.Count; p++)
                    {
                        var point = points[p];
                        if (point.X == amts[0]) point.Y = (point.Y + amts[1]) % 6;
                        points[p] = point;
                    }
                }
            }
            Points = [.. points];
        }

        public static int PartOne()
        {
            Parse();
            return Points.Length;
        }

        public static string PartTwo()
        {
            var width = Points.Max(p => p.X) + 1;
            var pixels = Enumerable.Repeat('.', width * 6).ToArray();

            foreach (var point in Points) pixels[(width * point.Y + point.X)] = '#';

            var result = string.Join('\n', pixels.Chunk(width).Select(row => new string(row)));
            result = result.Insert(0, "\n");
            return result;   
        }
    }
}
