using System.Text;

namespace aoc_fast.Years._2023
{
    internal class Day11
    {
        public static string input { get; set; }
        private static ulong[] xs = [];
        private static ulong[] ys = [];

        private static void Parse()
        {
            xs = new ulong[140];
            ys = new ulong[140];
            foreach(var (y, row) in input.Split("\n", StringSplitOptions.RemoveEmptyEntries).Index())
            {
                foreach(var (x, b) in Encoding.ASCII.GetBytes(row).Index())
                {
                    if(b == (byte)'#')
                    {
                        xs[x]++;
                        ys[y]++;
                    }
                }
            }
        }

        private static ulong Axis(ulong[] counts, ulong factor)
        {
            var gaps = 0UL;
            var result = 0UL;
            var prefixSum = 0UL;
            var prefixItems = 0UL;

            foreach(var (i, count) in counts.Index())
            {
                if(count > 0)
                {
                    var expand = (ulong)i + factor * gaps;
                    var extra = prefixItems * expand - prefixSum;
                    result += count * extra;
                    prefixSum += count * expand;
                    prefixItems += count;
                }
                else gaps++;
            }
            return result;
        }

        public static ulong PartOne()
        {
            Parse();
            return Axis(xs, 1) + Axis(ys, 1);
        }
        public static ulong PartTwo() => Axis(xs, 999999) + Axis(ys, 999999);
    }
}
