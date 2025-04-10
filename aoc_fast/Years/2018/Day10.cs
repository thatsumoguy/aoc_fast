using aoc_fast.Extensions;
using Point = aoc_fast.Extensions.Point;

namespace aoc_fast.Years._2018
{
    internal class Day10
    {
        public static string input
        {
            get;
            set;
        }

        private static void Tick(Point[] points, Point[] velocity, int time)
        {
            for (var i = 0; i < points.Length; i++)
            {
                var v = velocity[i];
                points[i] += v * time;
            }
        }

        private static (int, int, int, int) BoundingBox(Point[] points)
        {
            var minX = int.MaxValue;
            var maxX = int.MinValue;
            var minY = int.MaxValue;
            var maxY = int.MinValue;

            foreach(var point in points)
            {
                minX = minX < point.X ? minX : point.X;
                maxX = maxX > point.X ? maxX : point.X;
                minY = minY < point.Y ? minY : point.Y;
                maxY = maxY > point.Y ? maxY : point.Y;
            }
            return (minX, maxX, minY, maxY);
        }

        private static void Adjust(Point[] points)
        {
            var (minx, _, miny, _) = BoundingBox(points);
            var topLeft = new Point(minx, miny);
            for(var i = 0;i < points.Length;i++)
            {
                points[i] -= topLeft;
            }
        }

        private static int Size(Point[] points)
        {
            var (minX, maxX, minY, maxY) = BoundingBox(points);
            return (maxX - minX + 1) * (maxY - minY + 1);
        }

        private static (string partOne, int partTwo) answer;

        private static void Parse()
        {
            var inputPoints = input.ExtractNumbers<int>().Chunk(4);
            var (points, velocity) =
                (
                inputPoints
                     .Select(p => new Point(p[0], p[1]))
                     .ToArray(),
                inputPoints
                     .Select(p => new Point(p[2], p[3]))
                     .ToArray()
                );
            var up = Array.FindIndex(velocity, v => v.Y < 0);
            var down = Array.FindIndex(velocity, v => v.Y > 0);

            var p = Math.Abs(points[up].Y - points[down].Y);
            var v = Math.Abs(velocity[up].Y - velocity[down].Y);
            var time = (p - 10) / v;

            Tick(points, velocity, time);

            var area = Size(points);

            while (area > 620)
            {
                Tick(points, velocity, 1);
                area = Size(points);
                time++;
            }

            Adjust(points);

            var grid = Enumerable.Repeat('.', 620).ToArray();

            foreach (var point in points) grid[(62 * point.Y + point.X)] = '#';

            var message = String.Join("\n", grid.Chunk(62).Select(chars => new string(chars)));

            message = message.Insert(0, "\n");
            answer = (message, time);

        }

        public static string PartOne()
        {
            Parse();
            return answer.partOne;
        }
        public static int PartTwo() => answer.partTwo;
    }
}
