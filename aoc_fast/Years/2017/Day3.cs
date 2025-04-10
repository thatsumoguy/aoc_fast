using aoc_fast.Extensions;
using Point = aoc_fast.Extensions.Point;

namespace aoc_fast.Years._2017
{
    class Day3
    {
        public static string input
        {
            get;
            set;
        }
        private static int constant;

        public static int PartOne()
        {
            constant = int.Parse(input);
            var target = constant;
            var a = 3;

            while (a * a < target) a += 2;
            var b = a - 1;
            var c = a - 2;

            return (b / 2) + Math.Abs(c / 2 - (target - c * c - 1) % b);
        }

        public static int PartTwo()
        {
            var target = constant;

            var size = 2;

            var pos = new Point(1, 0);
            var dir = Directions.UP;
            var left = Directions.LEFT;

            var values = new Dictionary<Point, int>() { { Directions.ORIGIN, 1 } };
            while (true)
            {
                for(var edge = 0; edge < 4; edge++)
                {
                    for(var i = 0; i < size; i++)
                    {
                        var value = (Point point) => values.TryGetValue(point, out var i) ? i : 0;

                        var next = value(pos - dir) + value(pos + left + dir) + value(pos + left) + 
                            value(pos + left - dir);
                        if (next > target) return next;
                        values.Add(pos, next);

                        if (i == size - 1 && edge < 3) pos += left;
                        else pos += dir;

                    }
                    dir = left;
                    left = left.CounterClockwise();
                }
                size += 2;
            }
        }
    }
}
