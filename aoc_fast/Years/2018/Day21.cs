using aoc_fast.Extensions;

namespace aoc_fast.Years._2018
{
    internal class Day21
    {
        public static string input { get; set; }
        private static ulong seed;

        private static void Parse() => seed = input.ExtractNumbers<ulong>()[22];

        private static ulong Step(ulong seed, ulong hash)
        {
            var c = seed;
            var d = hash | 0x10000;

            for(var _ = 0; _ < 3; _++)
            {
                c = (c + (d & 0xff)) & 0xffffff;
                c = (c * 65899) & 0xffffff;
                d >>= 8;
            }
            return c;
        }

        public static ulong PartOne()
        {
            Parse();
            return Step(seed, 0);
        }

        public static ulong PartTwo()
        {
            var prev = 0ul;
            var hash = 0ul;
            var seen = new HashSet<ulong>(20000);
            while(seen.Add(hash))
            {
                prev = hash;
                hash = Step(seed, hash);
            }
            return prev;
        }
    }
}
