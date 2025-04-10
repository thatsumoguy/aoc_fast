using System.Runtime.CompilerServices;
using System.Text;
using aoc_fast.Extensions;

namespace aoc_fast.Years._2019
{
    internal class Day16
    {
        public static string input { get; set; }
        private static readonly int[][] PASCALS_TRIANGLE = [[1, 0, 0, 0, 0], [1, 1, 0, 0, 0], [1, 2, 1, 0, 0], [1, 3, 3, 1, 0], [1, 4, 6, 4, 1]];
        private static byte[] Bytes = [];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int BinomialMod2(int n, int k) => (k & ~n) == 0 ? 1 : 0;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int BinomialMod5(int n, int k)
        {
            var r = 1;
            while (k > 0 && r > 0)
            {
                r *= PASCALS_TRIANGLE[n % 5][k % 5];
                n /= 5;
                k /= 5;
            }
            return r % 5;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int BinomialMod10(int n, int k) => 5 * BinomialMod2(n,k) + 6 * BinomialMod5(n,k);

        private static int Compute(int[] digits, int size, int start, int upper)
        {
            var coefficients = new int[8];
            var res = new int[8];

            foreach(var (k, index) in Enumerable.Range(start, upper - start).Index())
            {
                coefficients.RotateRight(1);
                coefficients[0] = BinomialMod10(k + 99, k);
                var next = digits[index % size];

                for(var i = 0; i < res.Length; i++)
                {
                    res[i] += next * coefficients[i];
                }
            }

            for (var i = 0; i < res.Length; i++) res[i] %= 10;
            return res.Aggregate(0, (acc, b) => 10 * acc + b);
        }
        private static void Parse() => Bytes = Encoding.ASCII.GetBytes(input.TrimEnd()).Select(b => (byte)(b - (byte)'0')).ToArray();

        public static int PartOne()
        {
            Parse();
            var size = Bytes.Length;
            var mid = size / 2;
            var end = size - 1;


            var curr = Bytes.Select(b => (int)b).ToArray();
            var next = new int[size];
            for (var _ = 0; _ < 100; _++)
            {
                for (var i = 0; i < mid; i++)
                {
                    var phase = i + 1;
                    var skip = 2 * phase;
                    var remaining = curr[i..];
                    var total = 0;

                    while (remaining.Length != 0)
                    {
                        var take = Math.Min(phase, remaining.Length); ;
                        total += remaining[..take].Sum();
                        if (remaining.Length <= skip) break;
                        remaining = remaining[skip..];
                        var subtake = Math.Min(phase, remaining.Length);
                        total -= remaining[..subtake].Sum();
                        if (remaining.Length <= skip) break;
                        remaining = remaining[skip..];
                    }
                    next[i] = Math.Abs(total) % 10;
                }
                next[end] = curr[end];
                for (var i = end - 1; i >= mid; i--) next[i] = (curr[i] + next[i + 1]) % 10;

                (curr, next) = (next, curr);
            }
            return curr[..8].Aggregate(0, (acc, b) => 10 * acc + b);
        }

        public static int PartTwo()
        {
            var digits = Bytes.Select(b => (int)b).ToArray();
            var start = digits[..7].Aggregate(0, (acc, b) => 10 * acc + b);
            var size = digits.Length;
            var lower = size * 5000;
            var upper = size * 10000;

            return Compute(digits, size, start, upper);
        }
    }
}
