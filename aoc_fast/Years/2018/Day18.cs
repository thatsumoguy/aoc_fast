using System.Runtime.CompilerServices;
using System.Text;
using aoc_fast.Extensions;

namespace aoc_fast.Years._2018
{
    internal class Day18
    {
        public static string input { get; set; }
        const ulong OPEN = 0x00;
        const ulong TREE = 0x01;
        const ulong LUMBERYARD = 0x10;
        const ulong EDGE = 0xffff000000000000;
        const ulong LOWER = 0x0f0f0f0f0f0f0f0f;
        const ulong UPPER = 0xf0f0f0f0f0f0f0f0;
        const ulong THIRTEENS = 0x0d0d0d0d0d0d0d0d;
        const ulong FIFTEENS = 0x0f0f0f0f0f0f0f0f;

        public class Key 
        {
            public ulong[] Area { get; set; } = new ulong[350];
            
        }

        class KeyEquality : IEqualityComparer<Key>
        {

            public bool Equals(Key? prime, Key? other)
            {
                if (other is null) return false;
                if (ReferenceEquals(this, other)) return true;
                return Array.Equals(prime.Area, other.Area);
            }

            public int GetHashCode(Key key) => HashCode.Combine(key.Area[100], key.Area[200], key.Area[300]);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static ulong HorizontalSum(ulong left, ulong middle, ulong right) => (left << 56) + (middle >> 8) + middle + (middle << 8) + (right >> 56);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static ulong ResourceValue(ulong[] area)
        {
            var trees = area.Select(n => UInt64.PopCount(n & LOWER)).Sum();
            var lumberYards = area.Select(n => UInt64.PopCount(n & UPPER)).Sum();
            return trees * lumberYards;
        }

        private static void Step(ulong[] area, ulong[] rows)
        {
            for(var y = 0; y < 50; y++)
            {
                var areaOffset = area.AsSpan(7 * y);
                var rowsOffset = rows.AsSpan(7 * (y + 1));

                rowsOffset[0] = HorizontalSum(0, areaOffset[0], areaOffset[1]);
                rowsOffset[1] = HorizontalSum(areaOffset[0], areaOffset[1], areaOffset[2]);
                rowsOffset[2] = HorizontalSum(areaOffset[1], areaOffset[2], areaOffset[3]);
                rowsOffset[3] = HorizontalSum(areaOffset[2], areaOffset[3], areaOffset[4]);
                rowsOffset[4] = HorizontalSum(areaOffset[3], areaOffset[4], areaOffset[5]);
                rowsOffset[5] = HorizontalSum(areaOffset[4], areaOffset[5], areaOffset[6]);
                rowsOffset[6] = HorizontalSum(areaOffset[5], areaOffset[6], 0);

                rowsOffset[6] &= EDGE;
            }

            for (int i = 0; i < 350; i++)
            {
                var acre = area[i];
                var sum = rows[i] + rows[i + 7] + rows[i + 14] - acre;
                var toTree = (sum & LOWER) + THIRTEENS;
                toTree &= UPPER;
                toTree &= ~(acre | (acre << 4));
                toTree >>= 4;
                var toLumberyard = ((sum >> 4) & LOWER) + THIRTEENS;
                toLumberyard &= UPPER;
                toLumberyard &= acre << 4;
                toLumberyard |= toLumberyard >> 4;
                var toOpen = acre & UPPER;
                toOpen &= (sum & LOWER) + FIFTEENS;
                toOpen &= ((sum >> 4) & LOWER) + FIFTEENS;
                toOpen ^= acre & UPPER;
                area[i] = acre ^ (toTree | toLumberyard | toOpen);
            }
        }

        private static Key inputKey = new();

        private static void Parse()
        {
            var area = new ulong[350];

            foreach(var (y, line) in input.Split("\n").Select(s => Encoding.ASCII.GetBytes(s)).Index())
            {
                foreach(var (X, b) in line.Index())
                {
                    var acre = b switch
                    {
                        (byte)'|' => TREE,
                        (byte)'#' => LUMBERYARD,
                        _ => OPEN
                    };
                    var index = (y * 7) + (X / 8);
                    var offset = 56 - 8 * (X % 8);
                    area[index] |= acre << offset;
                }
            }
            inputKey = new Key { Area = area };
        }

        public static ulong PartOne()
        {
            Parse();

            var area = inputKey.Area;
            var rows = new ulong[364];
            for(var _ = 0; _ < 10; _++) Step(area, rows);
            return ResourceValue(area);
        }
        public static ulong PartTwo()
        {
            var area = inputKey.Area;
            var rows = new ulong[364];
            var keyComp = new KeyEquality();
            var seen = new Dictionary<Key, int>(keyComp);

            for (var minute = 1; ; minute++)
            {
                Step(area, rows);
                var key = new Key { Area = area };
                if (!seen.TryAdd(key, minute))
                {
                    var prev = seen[key];
                    var offset = 1_000_000_000 - prev;
                    var cycleWidth = minute - prev;
                    var remainder = offset % cycleWidth;
                    var target = prev + remainder;
                    var res = seen.Where(i => i.Value == target).First();
                    return ResourceValue(res.Key.Area);

                }
            }
        }
    }
}
