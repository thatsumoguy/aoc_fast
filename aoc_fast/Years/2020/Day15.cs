using aoc_fast.Extensions;
using static aoc_fast.Extensions.Hash;

namespace aoc_fast.Years._2020
{
    internal class Day15
    {
        public static string input { get; set; }
        private const int THRESHOLD = 1_000_000;
        private static List<ulong> Nums = [];
        private static ulong Play(List<ulong> input, int rounds)
        {
            var size = input.Count - 1;
            var last = input[size];

            var spokenLow = new uint[Math.Min(rounds, THRESHOLD)];
            var spokenHigh = new FastMap<uint, uint>(rounds /5);
            for (var i = 0; i < size; i++) spokenLow[input[i]] = (uint)(i + 1);

            for(var i = input.Count; i < rounds; i++)
            {
                if(last < THRESHOLD)
                {
                    var prev = (ulong)spokenLow[last];
                    spokenLow[last] = (uint)i;
                    last = prev == 0 ? 0 : (ulong)i - prev;
                }
                else
                {
                    if(spokenHigh.TryGetValue((uint)last, out var previous))
                    {
                        spokenHigh[(uint)last] = (uint)i;
                        last = (ulong)i - previous;                    }
                    else
                    {
                        spokenHigh[(uint)last] = (uint)i;
                        last = 0ul;
                    }
                }
            }
            return last;
        }

        public static ulong PartOne()
        {
            Nums = input.ExtractNumbers<ulong>();
            return Play(Nums, 2020);
        }
        public static ulong PartTwo() => Play(Nums, 30_000_000);
    }
}
