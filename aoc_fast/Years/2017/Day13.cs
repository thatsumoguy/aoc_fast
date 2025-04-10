using aoc_fast.Extensions;

namespace aoc_fast.Years._2017
{
    internal class Day13
    {
        public static string input
        {
            get;
            set;
        }
        private static List<int[]> scanners = [];


        private static void Parse() => scanners = [.. input.ExtractNumbers<int>().Chunk(2).OrderBy(s => s[1])];

        public static int PartOne()
        {
            Parse();
            var res = 0;

            foreach(var scanner in scanners)
            {
                var period = 2 * (scanner[1] - 1);
                if (scanner[0] % period == 0) res += scanner[0] * scanner[1];
            }

            return res;
        }

        public static int PartTwo()
        {
            var lcm = 1;
            var cur = new List<int>();
            var next = new List<int>();

            cur.Add(1);

            foreach(var scanner in scanners)
            {
                var period = 2 * (scanner[1] - 1);
                var nextLcm = lcm.lcm(period);

                for(var extra = 0; extra < nextLcm; extra += lcm)
                {
                    foreach(var delay in cur)
                    {
                        if((delay + extra + scanner[0]) % period != 0) next.Add(delay + extra);
                    }
                }
                lcm = nextLcm;
                (cur, next) = (next, cur);
                next.Clear();
            }

            return cur.First();
        }
    }
}
