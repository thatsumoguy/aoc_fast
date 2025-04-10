using System.Runtime.CompilerServices;
using System.Text;
using aoc_fast.Extensions;
using Point = aoc_fast.Extensions.Point;

namespace aoc_fast.Years._2023
{
    internal class Day18
    {
        public static string input { get; set; }

        private static (List<(Point, int)>, List<(Point, int)>) moves = ([], []);
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        private static long Determinat(Point a, Point b) => (long)(a.X) * (long)(b.Y) - (long)(a.Y) * (long)(b.X);
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        private static long Lava(List<(Point, int)> moves)
        {
            Point prev;
            var pos = Directions.ORIGIN;
            var area = 0L;
            var perimeter = 0L;

            foreach(var (dir, amount) in moves)
            {
                prev = pos;
                pos += dir * amount;
                area += Determinat(prev, pos);
                perimeter += (long)amount;
            }

            return area / 2 + perimeter / 2 + 1;
        }

        private static void Parse()
        {
            var first = new List<(Point, int)>(1000);
            var second = new List<(Point, int)>(1000);

            foreach(var set in input.Split([' ', '\t', '\n'], StringSplitOptions.RemoveEmptyEntries).Chunk(3))
            {
                var (a, b, c) = (set[0], set[1], set[2]);
                var dir = Point.FromByte(Encoding.ASCII.GetBytes(a)[0]);
                var amount = int.Parse(b);

                first.Add((dir, amount));

                dir = Encoding.ASCII.GetBytes(c)[7] switch
                {
                    (byte)'0' => Directions.RIGHT,
                    (byte)'1' => Directions.DOWN,
                    (byte)'2' => Directions.LEFT,
                    (byte)'3' => Directions.UP,
                };
                var hex = c[2..(c.Length - 2)];
                amount = Convert.ToInt32(hex, 16);
                second.Add((dir, amount));
            }
            moves = (first, second);
        }

        public static long PartOne()
        {
            Parse();
            return Lava(moves.Item1);
        }
        public static long PartTwo() => Lava(moves.Item2);
    }
}
