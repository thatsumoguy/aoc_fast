using System.Runtime.CompilerServices;
using aoc_fast.Extensions;

namespace aoc_fast.Years._2022
{
    internal class Day9
    {
        public static string input { get; set; }
        private static (int[], List<(Point, int)> pairs) Input = ([], []);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool Apart(in Point a, in Point b) =>
            Math.Max(Math.Abs(a.X - b.X), Math.Abs(a.Y - b.Y)) > 1;

        private static uint Simulate((int[], List<(Point, int)> pairs) input, int N)
        {
            var (bounds, pairs) = input;
            int x1 = bounds[0], y1 = bounds[1], x2 = bounds[2], y2 = bounds[3];
            var width = x2 - x1 + 1;
            var height = y2 - y1 + 1;

            var rope = new Point[N];
            var start = new Point(-x1, -y1);
            for (var i = 0; i < N; i++) rope[i] = start;

            var visited = new HashSet<int>(width * height / 4)
            {
                width * start.Y + start.X
            };

            foreach (var (step, amount) in pairs)
            {
                for (var m = 0; m < amount; m++)
                {
                    rope[0] += step;

                    for (var i = 1; i < N; i++)
                    {
                        if (!Apart(rope[i - 1], rope[i])) break; 

                        var prev = rope[i - 1];
                        var curr = rope[i];

                        var dx = prev.X - curr.X;
                        var dy = prev.Y - curr.Y;

                        rope[i] = new Point(
                            curr.X + (dx > 0 ? 1 : (dx < 0 ? -1 : 0)),
                            curr.Y + (dy > 0 ? 1 : (dy < 0 ? -1 : 0))
                        );
                    }

                    var tail = rope[N - 1];
                    var index = width * tail.Y + tail.X;
                    visited.Add(index);
                }
            }

            return (uint)visited.Count;
        }

        private static void Parse()
        {
            var pairs = new List<(Point, int)>(input.Length / 3);
            var lines = input.AsSpan().Trim();
            var line = 0;
            var start = 0;
            int x1 = 0, y1 = 0, x2 = 0, y2 = 0;
            int px = 0, py = 0;

            while (line < lines.Length)
            {
                var end = line;
                while (end < lines.Length && lines[end] != '\n') end++;
                var direction = lines[line] switch
                {
                    'U' => new Point(0, 1),
                    'D' => new Point(0, -1),
                    'L' => new Point(-1, 0),
                    'R' => new Point(1, 0),
                    _ => throw new Exception("Invalid direction"),
                };

                var amount = 0;
                for (int i = line + 2; i < end; i++)
                {
                    if (char.IsDigit(lines[i])) amount = amount * 10 + (lines[i] - '0');
                }

                pairs.Add((direction, amount));

                px += direction.X * amount;
                py += direction.Y * amount;

                x1 = Math.Min(x1, px);
                y1 = Math.Min(y1, py);
                x2 = Math.Max(x2, px);
                y2 = Math.Max(y2, py);

                line = end + 1;
            }

            Input = ([x1, y1, x2, y2], pairs);
        }

        public static uint PartOne()
        {
            Parse();
            return Simulate(Input, 2);
        }
        public static uint PartTwo() => Simulate(Input, 10);
    }
}
