using System.Text;

namespace aoc_fast.Years._2015
{
    class Day5
    {
        public static string input
        {
            get;
            set;
        }
        private static List<byte[]> bytes = [];

        private static List<byte[]> Parse() => input.Split('\n').Select(Encoding.ASCII.GetBytes).ToList();

        public static long PartOne()
        {
            bytes = Parse();
            static bool nice(byte[] line)
            {
                var vowels = 0;
                var pairs = 0;
                var prev = 0;

                foreach (var b in line)
                {
                    var current = 1 << (b - (byte)'a');
                    if ((0x101000a & current & (prev << 1)) != 0) return false;
                    if ((0x0104111 & current) != 0) vowels += 1;
                    if (prev == current) pairs += 1;
                    else prev = current;
                }
                return vowels >= 3 && pairs >= 1;
            }

            return bytes.Where(nice).Count();
        }

        public static long PartTwo()
        {
            var pairs = Enumerable.Repeat(0l, 729).ToArray();

            static bool nice(byte[] line, long baseNum, long[]? pairs)
            {
                var first = 0l;
                var second = 0l;
                var twoPair = false;
                var splitPair = false;
                foreach (var (b, offset) in line.Select((b, i) => (b, i)))
                {
                    var third = (long)(b - (byte)'a' + 1);
                    var index = 27 * second + third;
                    var pos = baseNum * 1000 + offset;
                    var delta = pos - pairs[index];

                    if (delta > offset) pairs[index] = pos;
                    else if (delta > 1) twoPair = true;
                    if (first == third) splitPair = true;

                    first = second;
                    second = third;

                }
                return twoPair && splitPair;
                
            }

            return bytes.Select((b, i) => (b, i)).Where(x => nice(x.b, x.i, pairs)).Count();
        }
    }
}
