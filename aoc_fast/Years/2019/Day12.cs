using aoc_fast.Extensions;

namespace aoc_fast.Years._2019
{
    internal class Day12
    {
        public static string input { get; set; }
        private static int[][] axises = [];

        private static int[] Step(int[] axis)
        {
            var (p0, p1, p2, p3, v0, v1, v2, v3) = (axis[0], axis[1], axis[2], axis[3], axis[4], axis[5], axis[6], axis[7]);
            var n0 = v0 + int.Sign(p1 - p0) + int.Sign(p2 - p0) + int.Sign(p3 - p0);
            var n1 = v1 + int.Sign(p0 - p1) + int.Sign(p2 - p1) + int.Sign(p3 - p1);
            var n2 = v2 + int.Sign(p0 - p2) + int.Sign(p1 - p2) + int.Sign(p3 - p2);
            var n3 = v3 + int.Sign(p0 - p3) + int.Sign(p1 - p3) + int.Sign(p2 - p3);
            return [p0 + n0, p1 + n1, p2 + n2, p3 + n3, n0, n1, n2, n3];
        }

        private static bool Stop(int[] axis) => axis[4] == 0 && axis[5] == 0 && axis[6] == 0 && axis[7] == 0; 
        private static void Parse()
        {
            var n = input.ExtractNumbers<int>();
            axises = [[n[0], n[3], n[6], n[9], 0, 0, 0, 0], [n[1], n[4], n[7], n[10], 0, 0, 0, 0], 
            [ n[2], n[5], n[8], n[11], 0, 0, 0, 0]];
        }

        public static int PartOne()
        {
            Parse();
            var (X, y, z) = (axises[0], axises[1], axises[2]);
            for(var _ = 0; _ < 1000; _++)
            {
                X = Step(X);
                y = Step(y);
                z = Step(z);
            }
            var e = Enumerable.Range(0, 8).Select(i => Math.Abs(X[i]) + Math.Abs(y[i]) + Math.Abs(z[i])).ToArray();
            return e[0] * e[4] + e[1] * e[5] + e[2] * e[6] + e[3] * e[7];
        }

        public static long PartTwo()
        {
            var (X, y, z) = (axises[0], axises[1], axises[2]);
            var (a, b, c) = (0l, 0l, 0l);
            var count = 0l;
            while(a == 0 || b == 0 || c == 0)
            {
                count++;
                if(a == 0)
                {
                    X = Step(X);
                    if (Stop(X)) a = count;
                }
                if(b == 0)
                {
                    y = Step(y);
                    if (Stop(y)) b = count;
                }
                if(c == 0)
                {
                    z = Step(z);
                    if (Stop(z)) c = count;
                }
            }

            return 2 * a.lcm(b.lcm(c));
        }
    }
}
