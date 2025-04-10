using System.Text;

namespace aoc_fast.Years._2016
{
    class Day18
    {
        public static string input
        {
            get;
            set;
        }

        private static int Count(string input, int rows)
        {
            var width = input.Length;
            UInt128 mask = ((UInt128)1 << width) - 1;

            var total = 0;
            var row = Encoding.ASCII.GetBytes(input).Aggregate((UInt128)0, (acc, b) => (acc << 1) | (b == '^' ? 1UL : 0UL));
            
            for(var _ = 0; _ < rows; _++)
            {
                total += (int)UInt128.PopCount(row);
                row = (row << 1) ^ (row >> 1) & mask;
            }
            return rows * width - total;
        }

        public static int PartOne() => Count(input.Trim(), 40);
        public static int PartTwo() => Count(input.Trim(), 400000);
    }
}
