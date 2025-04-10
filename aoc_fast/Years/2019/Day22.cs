using aoc_fast.Extensions;

namespace aoc_fast.Years._2019
{
    internal class Day22
    {
        public static string input { get; set; }
        class Technique(Int128 a, Int128 c, Int128 m)
        {
            private Int128 A { get; } = a;
            private Int128 C { get; } = c;
            private Int128 M { get; } = m;
            public Technique Compose(Technique other)
            {
                var m = M;
                var a = (A * other.A) % m;
                var c = (C * other.A + other.C) % m;
                return new Technique(a, c, m);
            }
            public Technique Inverse()
            {
                var m = M;
                var a = A.ModInv(m);
                var c = m - (a * C) % m;
                return new Technique(a, c, m);
            }
            public Technique Power(Int128 e)
            {
                var m = M;
                var a = A.ModPow(e, m);
                var c = (((a - 1) * (A - 1).ModInv(m) % m) * C) % m;
                return new Technique(a, c, m);
            }
            public Int128 Shuffle(Int128 index) => (A * index + C) % M;
        }

        private static Technique Deck(string input, Int128 m)
        {
            var techniques = input.Split('\n')
            .Where(line => !string.IsNullOrEmpty(line))
            .Select(line =>
            {
                var tokens = line.Split([' ' , '\n', '\t']);
                if (tokens[1] == "into")
                {
                    return new Technique(m - 1, m - 1, m);
                }
                else if (tokens[1] == "with")
                {
                    var n = Int128.Parse(tokens[3]);
                    var a = (m + n % m) % m;
                    return new Technique(a, 0, m);
                }
                else if (tokens[0] == "cut")
                {
                    var n = Int128.Parse(tokens[1]);
                    var c = (m - n % m) % m;
                    return new Technique(1, c, m);
                }
                else
                {
                    throw new InvalidOperationException("Unknown operation");
                }
            }).ToList();

            return techniques.Aggregate((a, b) => a.Compose(b));
        }

        public static Int128 PartOne() => Deck(input, 10007).Shuffle(2019);
        public static Int128 PartTwo() => Deck(input, 119315717514047).Inverse().Power(101741582076661).Shuffle(2020);

    }
}
