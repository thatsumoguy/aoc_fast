using System.Text;
using aoc_fast.Extensions;

namespace aoc_fast.Years._2017
{
    internal class Day21
    {
        public static string input
        {
            get;
            set;
        }

        record Pattern(uint three, uint four, uint six, ulong[] nine);

        private static ulong ToIndex(byte[] a) => a.Aggregate(0uL, (acc, n) => (acc << 1) | (ulong)n);

        private static ulong[] TwoByTwoPerms(byte[] a)
        {
            Span<ulong> indices = stackalloc ulong[8];
            for(var i = 0; i < indices.Length; i++)
            {
                var index = indices[i];
                index = ToIndex(a);
                indices[i] = index;
                a = [a[2], a[0], a[3], a[1]];
                if (i == 3) a = [a[2], a[3], a[0], a[1]];
            }
            return indices.ToArray();
        }

        private static ulong[] ThreeByThreePerms(byte[] a)
        {
            Span<ulong> indices = stackalloc ulong[8];
            for (var i = 0; i < indices.Length; i++)
            {
                var index = indices[i];
                index = ToIndex(a);
                indices[i] = index;
                a = [a[6],a[3],a[0],a[7],a[4],a[1],a[8],a[5],a[2]];
                if (i == 3) a = [a[6],a[7],a[8],a[3],a[4],a[5],a[0],a[1],a[2]];
            } 
            return indices.ToArray();
        }

        private static List<uint> res = [];

        private static void Parse()
        {
            var patternLookup = new ulong[16];
            var twoToThree = new byte[16][];
            var threeToFour = new byte[512][];
            for(var i = 0; i < 16; i++) twoToThree[i] = new byte[9];
            for (var i = 0; i < 512; i++) threeToFour[i] = new byte[16];

            var todo = new List<ulong>() { 143 };

            foreach(var line in input.Split("\n",StringSplitOptions.RemoveEmptyEntries).Select(Encoding.ASCII.GetBytes))
            {
                var bit = (ulong i) =>  (byte)(line[i] & 1);

                if(line.Length == 20)
                {
                    List<ulong> indices = [0, 1, 3, 4];
                    var from = indices.Select(bit).ToArray();

                    indices = [9, 10, 11, 13, 14, 15, 17, 18, 19];
                    var value = indices.Select(bit).ToArray();

                    var pattern = todo.Count;
                    todo.Add(ToIndex(value));

                    foreach(var key in TwoByTwoPerms(from))
                    {
                        twoToThree[key] = value;
                        patternLookup[key] = (ulong)pattern;
                    }
                }
                else
                {
                    List<ulong> indices = [0, 1, 2, 4, 5, 6, 8, 9, 10];
                    var from = indices.Select(bit).ToArray();

                    indices = [15, 16, 17, 18, 20, 21, 22, 23, 25, 26, 27, 28, 30, 31, 32, 33];
                    var value = indices.Select(bit).ToArray();
                    
                    foreach(var key in ThreeByThreePerms(from)) threeToFour[key] = value;
                }
            }

            var patterns = todo.Select(index => 
            {
                var four = threeToFour[index];
                var six = new byte[36];

                foreach (var (src, dst) in new (ulong,ulong)[] { (0,0),(2,3),(8,18),(10,21)})
                {
                    var tIndex = ToIndex([four[src], four[src + 1], four[src + 4], four[src + 5]]);
                    var replacement = twoToThree[tIndex];
                    Array.Copy(replacement, 0, six, (int)dst, 3);
                    Array.Copy(replacement, 3, six, (int)dst + 6, 3);
                    Array.Copy(replacement, 6, six, (int)dst + 12, 3);
                }

                var nine = new ulong[] { 0, 2, 4, 12, 14, 16, 24, 26, 28 }.Select(i => 
                {
                    var jIndex = ToIndex([six[i], six[i + 1], six[i + 6], six[i + 7]]);
                    return patternLookup[jIndex];
                });
                var three = (uint)ulong.PopCount(index);
                var f = (uint)four.Sum();
                var s = (uint)six.Sum();
                return new Pattern(three, f, s, nine.ToArray());
            }).ToList();

            var cur = new uint[patterns.Count];
            res = [];

            cur[0] = 1;

            for(var _ = 0; _ < 7; _++)
            {
                var three = 0u;
                var four = 0u;
                var six = 0u;
                var next = new uint[patterns.Count];

                foreach(var (count, pattern) in cur.Zip(patterns))
                {
                    three += count * pattern.three;
                    four += count * pattern.four;
                    six += count * pattern.six;
                    foreach (var item in pattern.nine) next[item] += count;
                }

                res.Add(three);
                res.Add(four);
                res.Add(six);

                cur = next;
            }
        }

        public static uint PartOne()
        {
            Parse();
            return res[5];
        }
        public static uint PartTwo() => res[18];
    }
}
