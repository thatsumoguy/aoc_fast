using aoc_fast.Extensions;

namespace aoc_fast.Years._2018
{
    internal class Day6
    {
        public static string input
        {
            get;
            set;
        }
        record Input(int minY, int maxY, List<Point> points);

        private static Input inputobj;

        private static void Parse()
        {
            var minY = int.MaxValue;
            var maxY = int.MinValue;

            var points = input.ExtractNumbers<int>().Chunk(2).Select(p =>
            {
                minY = minY < p[1] ? minY : p[1];
                maxY = maxY > p[0] ? maxY : p[0];
                return new Point(p[0], p[1]);
            }).ToList();

            inputobj = new Input(minY, maxY, points);
        }

        public static int PartOne()
        {
            Parse();
            var points = inputobj.points.Clone().ToList();
            var area = new int[points.Count];
            var finite = Enumerable.Repeat(true, points.Count).ToArray();
            var candidate = new List<(ulong, int, int)>();

            var marker = ulong.MaxValue;

            points.Sort((p, o) => p.X.CompareTo(o.X));

            for (var row = inputobj.minY; row <= inputobj.maxY; row++)
            {
                foreach (var (p, j) in points.Select((p, j) => (p, j)))
                {
                    var m1 = Math.Abs(p.Y - row);
                    var x1 = p.X;

                    while (true)
                    {
                        if (candidate.Count > 0)
                        {
                            var (i, m0, x0) = candidate[^1];
                            candidate.RemoveAt(candidate.Count - 1);

                            var delta = m1 - m0;
                            var width = x1 - x0;

                            if (delta < -width) continue;
                            else if (delta == -width)
                            {
                                candidate.Add((marker, m0, x0));
                                candidate.Add(((ulong)j, m1, x1));
                            }
                            else if (delta == width)
                            {
                                candidate.Add((i, m0, x0));
                                candidate.Add((marker, m1, x1));
                            }
                            else if (delta > width) candidate.Add((i, m0, x0));
                            else
                            {
                                candidate.Add((i, m0, x0));
                                candidate.Add(((ulong)j, m1, x1));
                            }
                        }
                        else candidate.Add(((ulong)j, m1, x1));
                        break;
                    }
                }
                var left = candidate[0].Item1;
                if (left != marker) finite[left] = false;

                var right = candidate[candidate.Count - 1].Item1;
                if (right != marker) finite[right] = false;

                foreach (var w in candidate.Windows(3))
                {
                    var (_, m0, x0) = w[0];
                    var (i, wm1, wx1) = w[1];
                    var (_, m2, x2) = w[2];

                    if (i != marker)
                    {
                        if (row == inputobj.minY || row == inputobj.maxY) finite[i] = false;
                        else
                        {
                            var wleft = (wx1 - x0 + m0 - wm1 - 1) / 2;
                            var wright = (x2 - wx1 + m2 - wm1 - 1) / 2;
                            area[i] += wleft + 1 + wright;
                        }
                    }

                }

                candidate.Clear();
            }
            return Enumerable.Range(0, points.Count).Where(i => finite[i]).Select(i => area[i]).Max();
        }

        public static int PartTwo()
        {
            var xs = inputobj.points.Select(p => p.X).ToArray();
            Array.Sort(xs);
            var ys = inputobj.points.Select(p => p.Y).ToArray();
            Array.Sort(ys);

            var x = xs[xs.Length / 2];
            var y = ys[ys.Length / 2];

            var median = new Point(x, y);
            var yDist = inputobj.points.Select(o => o.Manhattan(median)).Sum();

            while(yDist + Prev(ys, y) < 10_000)
            {
                yDist += Prev(ys, y);
                y--;
            }

            var left = x;
            var leftDist = yDist;
            var right = x;
            var rightDist = yDist;
            var area = 0;

            while(yDist < 10_000)
            {
                while(leftDist < 10_000)
                { 
                    leftDist += Prev(xs, left);
                    left--;
                }

                while(leftDist >= 10_000)
                {
                    leftDist += Next(xs, left);
                    left++;
                }

                while(rightDist < 10_000)
                {
                    rightDist += Next(xs, right);
                    right++;
                }
                while(rightDist >= 10_000)
                {
                    rightDist += Prev(xs, right);
                    right--;
                }

                var next = Next(ys, y);
                yDist += next;
                leftDist += next;
                rightDist += next;
                y++;
                area += right - left + 1;
            }

            return area;
        }

        private static int Prev(int[] slice, int n)
        {
            var total = 0;
            
            foreach(var s in slice)
            {
                if (s >= n) total++;
                else total--;
            }

            return total;
        }

        private static int Next(int[] slice, int n)
        {
            var total = 0;

            foreach (var s in slice)
            {
                if (s <= n) total++;
                else total--;
            }
            return total;
        }

    }
}
