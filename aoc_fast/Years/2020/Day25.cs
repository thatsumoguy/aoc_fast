using aoc_fast.Extensions;

namespace aoc_fast.Years._2020
{
    internal class Day25
    {
        public static string input { get; set; }

        private static ulong DiscreteLogarithm(ulong pubKey)
        {
            var m = 4495ul;
            var map = new Dictionary<ulong, ulong>((int)m);

            var a = 1ul;
            for(var j = 0ul; j < m; j++)
            {
                map.Add(a, j);
                a = (a * 7) % 20201227;
            }

            var b = pubKey;

            for(var i = 0ul;  i < m; i++)
            {
                if (map.TryGetValue(b, out var j)) return i * m + j;
                b = (b * 680915) % 20201227;
            }
            throw new Exception();
        }

        public static ulong PartOne()
        {
            var nums = input.ExtractNumbers<ulong>().Chunk(2).ToArray()[0];
            var(cardPubKey, doorPubKey) = (nums[0],  nums[1]);
            var cardLoopCount = DiscreteLogarithm(cardPubKey);
            return doorPubKey.ModPow(cardLoopCount, 20201227ul);
        }
        public static string PartTwo() => "Merry Christmas";
    }
}
