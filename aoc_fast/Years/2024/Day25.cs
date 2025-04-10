using System.Text;

namespace aoc_fast.Years._2024
{
    internal class Day25
    {
        public static string input
        {
            get;
            set;
        }
        const ulong MASK = 0b_011111_011111_011111_011111_011111;

        public static int PartOne()
        {
            var slice = Encoding.Default.GetBytes(input);
            var locks = new List<ulong>(250);
            var keys = new List<ulong>(250);
            var res = 0;
            while (slice.Length > 0)
            {
                var bits = slice[6..35].Aggregate(0UL, (bits, n) => (bits << 1) | ((ulong)n & 1));

                if (slice[0] == '#') locks.Add(bits & MASK);
                else keys.Add(bits & MASK);
                slice = slice[(Math.Min(43, slice.Length))..];
            }

            foreach (var l in locks)
            {
                foreach (var k in keys)
                {
                    res += (l & k) == 0 ? 1 : 0;
                }
            }
            return res;
        }
        public static string PartTwo() => "Merry Christmas";
    }
}
