using System.Text;
using aoc_fast.Extensions;

namespace aoc_fast.Years._2021
{
    internal class Day14
    {
        public static string input { get; set; }

        private static ulong Element(byte b) => (ulong)(b - (byte)'A');
        private static ulong Pair(byte first, byte second) => 26 * Element(first) + Element(second);

        class Rule(ulong from, ulong toLeft, ulong toRight, ulong element)
        {
            public ulong From { get; set; } = from;
            public ulong ToLeft { get; set; } = toLeft;
            public ulong ToRight { get; set; } = toRight;
            public ulong Element { get; set; } = element;

            public static Rule Parse(byte[] bytes)
            {
                var (a, b, c) = (bytes[0], bytes[1], bytes[2]);
                var from = Pair(a, b);
                var toLeft = Pair(a, c);
                var toRight = Pair(c, b);
                var element = Element(c);
                return new(from , toLeft, toRight, element);
            }
        }

        private static ulong Steps((ulong[] elements, ulong[] pairs, List<Rule> rules) input, int rounds)
        {
            var elements = input.elements;
            var pairs = input.pairs;
            var rules = input.rules;

            for(var _ = 0; _ < rounds; _++)
            {
                var next = new ulong[26 * 26];

                foreach(var rule in rules)
                {
                    var n = pairs[rule.From];
                    next[rule.ToLeft] += n;
                    next[rule.ToRight] += n;
                    elements[rule.Element] += n;
                }
                Array.Copy(next, pairs, next.Length);
            }

            var max = elements.Max();
            var min = elements.Where(n => n > 0).Min();
            return max - min;
        }

        private static (ulong[] elements, ulong[] pairs, List<Rule> rules) obj = (new ulong[26], new ulong[26 * 26], []);

        private static void Parse()
        {
            var parts = input.Split("\n\n");
            var (prefix, suffix) = (Encoding.UTF8.GetBytes(parts[0]), parts[1]);
            var elements = new ulong[26];
            foreach (var item in prefix) elements[Element(item)]++;

            var pairs = new ulong[26 * 26];
            foreach (var w in prefix.Windows(2)) pairs[Pair(w[0], w[1])]++;

            var rules = Encoding.UTF8.GetBytes(suffix.Trim()).Where(b => char.IsAsciiLetterUpper((char)b)).Chunk(3).Select(Rule.Parse).ToList();

            obj = (elements, pairs, rules);
        }

        public static ulong PartOne()
        {
            Parse();
            return Steps(obj, 10);
        }
        public static ulong PartTwo()
        {
            Parse();
            return Steps(obj, 40);
        }

    }
}
