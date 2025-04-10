using System.Text;
using aoc_fast.Extensions;
using Point = aoc_fast.Extensions.Point;

namespace aoc_fast.Years._2019
{
    internal class Day10
    {
        public static string input { get; set; }

        private static int Quadrant(Point p) => (p.X >= 0, p.Y >= 0) switch
        {
            (true, false) => 0,
            (true, true) => 1,
            (false, true) => 2,
            (false, false) => 3,
        };
        private static int Angle(Point p, Point other) => (other.X * p.Y).CompareTo(other.Y * p.X);
        private static int Distance(Point p) => p.X * p.X + p.Y * p.Y;
        private static int Clockwise(Point p, Point other)
        {
            var firstComp = Quadrant(p).CompareTo(Quadrant(other));
            if(firstComp != 0) return firstComp;
            var secondComp = Angle(p, other).CompareTo(0);
            if(secondComp != 0) return secondComp;
            return Distance(p).CompareTo(Distance(other));
        }

        private static (int partOne, int partTwo) answers;

        private static void Parse()
        {
            var raw = input.Split("\n").Select(Encoding.ASCII.GetBytes).ToList();
            var width = raw[0].Length;
            var height = raw.Count;

            var points = new List<Point>();

            foreach(var (y, row) in raw.Index())
            {
                foreach(var (X, col) in row.Index())
                {
                    if(col == '#') points.Add(new Point(X, y));
                }
            }

            var visible = new int[points.Count];
            var seen = new int[4 * width * height];
            var maxVis = 0;
            var maxIndex = 0;

            for(var i = 0; i < points.Count - 1;  i++)
            {
                for(var j = i + 1; j < points.Count; j++)
                {
                    var delta = points[j] - points[i];

                    var factor = Math.Abs(Numerics.gcd(delta.X, delta.Y));
                    delta.X /= factor;
                    delta.Y /= factor;

                    var index = (2 * width) * (height + delta.Y) + (width + delta.X);

                    if (seen[index] <= i)
                    {
                        seen[index] = i + 1;
                        visible[i]++;
                        visible[j]++;
                    }
                }
                if (visible[i] > maxVis)
                {
                    maxVis = visible[i];
                    maxIndex = i;
                }
            }

            var station = points.SwapRemove(maxIndex);
            for(var i = 0; i < points.Count; i++) points[i] -= station;
            points.Sort((a, b) => Clockwise(a, b));

            var groups = new List<(int, int, int)>(points.Count);
            var first = 0;
            var second = 0;

            groups.Add((first, second, 0));

            for(var i = 1; i < points.Count; i++)
            {
                if (Angle(points[i], points[i - 1])  == 1)
                {
                    first = 0;
                    second++;
                }
                else first++;
                groups.Add((first, second, i));
                
            }
            groups.Sort();
            var target = station + points[groups[199].Item3];
            var res = 100 * target.X + target.Y;

            answers = (maxVis, res);
        }
        public static int PartOne()
        {
            Parse();
            return answers.partOne;
        }
        public static int PartTwo() => answers.partTwo;
    }
}
