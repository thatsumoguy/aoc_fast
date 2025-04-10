using System.Text;

namespace aoc_fast.Years._2020
{
    internal class Day5
    {
        public static string input { get; set; }
        private static (uint min, uint max, uint xor) MinMaxXor;
        private static void Parse()
        {
            var min = uint.MaxValue;
            var max = uint.MinValue;
            var xor = 0u;

            foreach(var line in input.Split("\n", StringSplitOptions.RemoveEmptyEntries))
            {
                var id = Encoding.UTF8.GetBytes(line).Aggregate(0u, (acc, b) => (acc << 1) | ((b == 'B' || b == 'R') ? 1u : 0u));
                min = Math.Min(min, id);
                max = Math.Max(max, id);
                xor ^= id;
            }
            MinMaxXor = (min, max, xor);
        }
        public static uint PartOne()
        {
            Parse();
           return MinMaxXor.max;
        }
        public static uint PartTwo()
        {
            var rows = Enumerable.Range((int)MinMaxXor.min, (int)(MinMaxXor.max - MinMaxXor.min + 1)).Select(i => (uint)i).Aggregate(0u, (acc, b) => acc ^ b);
            return rows ^ MinMaxXor.xor;
        }
    }
}
