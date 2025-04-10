using aoc_fast.Extensions;

namespace aoc_fast.Years._2020
{
    internal class Day10
    {
        public static string input { get; set; }
        private static List<ulong> Adapters = [];

        private static void Parse()
        {
            var adapters = input.ExtractNumbers<ulong>();
            adapters.Sort();
            Adapters = adapters;
        }

        public static ulong PartOne()
        {
            Parse();
            ulong[] total = [0,0,0,1];
            total[Adapters[0]]++;
            foreach(var w in Adapters.Windows(2))
            {
                var diff = Math.Abs((decimal)w[0] - (decimal)w[1]);
                total[(ulong)diff]++;
            }
            return total[1] * total[3];
        }
        public static ulong PartTwo()
        {
            var last = Adapters.Last();
            var sum = new ulong[last + 1];
            sum[0] = 1;
            
            foreach(var i in Adapters)
            {
                sum[i] = i switch
                {
                    1 => sum[i - 1],
                    2 => sum[i - 1] + sum[i - 2],
                    _ => sum[i - 1] + (sum[i - 2] + sum[i - 3]),
                };
            }
            return sum.Last();
        }
    }
}
