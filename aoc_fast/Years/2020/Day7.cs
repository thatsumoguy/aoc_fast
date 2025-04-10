using System.Text;

namespace aoc_fast.Years._2020
{
    internal class Day7
    {
        public static string input { get; set; }
        private static readonly ulong[] FIRSTHASH = [43, 63, 78, 86, 92, 95, 98, 130, 294, 320, 332, 390, 401, 404, 475, 487, 554, 572];
        private static readonly ulong[] SECONDHASH = [16, 31, 37, 38, 43, 44, 59, 67, 70, 76, 151, 170, 173, 174, 221, 286, 294, 312, 313, 376, 381,401, 410, 447, 468, 476, 495, 498, 508, 515, 554, 580, 628];

        class Rule(uint amount, ulong next)
        {
            public uint Amount { get; set; } = amount;
            public ulong Next { get; set; } = next;
        }
        class Haversack(ulong shinyGold, Rule[][] bags)
        {
            public ulong ShinyGold = shinyGold;
            public Rule[][] Bags = bags;
        }
        private static Haversack haversack = new(0, []);
        private static void Parse()
        {
            var firstIndices = new ulong[676];
            var secondIndices = new ulong[676];
            var bags = new Rule[594][];
            for (var i = 0; i < 594; i++) bags[i] = new Rule[4];
            FIRSTHASH.ToList().Index().ToList().ForEach(h => firstIndices[h.Item] = (ulong)h.Index);
            SECONDHASH.ToList().Index().ToList().ForEach(h => secondIndices[h.Item] = (ulong)h.Index);

            var perfectMinimalHash = (string f, string s) =>
            {
                var first = Encoding.UTF8.GetBytes(f);
                var a = (ulong)first[0];
                var b = (ulong)first[1];

                var second = Encoding.UTF8.GetBytes(s);
                var c = (ulong)second[0];
                var d = (ulong)second[1] + (ulong)(second.Length % 2);
                return firstIndices[26 * a + b - 2619] + 18 * secondIndices[26 * c + d - 2619];
            };
            foreach(var line in input.TrimEnd().Split("\n"))
            {
                var tokens = line.Split([' ', '\n', '\t']).Chunk(4).ToArray();
                var (first, second) = (tokens[0][0], tokens[0][1]);
                var outer = perfectMinimalHash(first, second);

                foreach(var (index, chunk) in tokens[1..].Index())
                {
                    if(chunk.Length == 4)
                    {
                        var (a, f, s) = (chunk[0], chunk[1], chunk[2]);
                        var amount = uint.Parse(a);
                        var next = perfectMinimalHash(f, s);
                        bags[outer][index] = new Rule(amount, next);
                    }
                }
            }

            var shinyGold = perfectMinimalHash("shiny", "gold");
            haversack = new Haversack(shinyGold, bags);
        }
        public static int PartOne()
        {
            Parse();
            static bool helper(ulong key, Haversack haverSack, bool?[] cache)
            {
                if (cache[key] != null) return cache[key].Value;
                else
                {
                    var value = false;
                    var iter = haverSack.Bags[key].GetEnumerator();
                    while (iter.MoveNext())
                    {
                        if (iter.Current != null)
                        {
                            var rule = (Rule)iter.Current;
                            if (helper(rule.Next, haverSack, cache))
                            {
                                value = true;
                                break;
                            }
                        }
                    }
                    cache[key] = value;
                    return value;
                }
            }
            var cache = new bool?[haversack.Bags.Length];
            for (var i = 0; i < haversack.Bags.Length; i++) cache[i] = null;
            cache[haversack.ShinyGold] = true;
            return Enumerable.Range(0, haversack.Bags.Length).Where(key => helper((ulong)key, haversack, cache)).Count() - 1;
        }
        public static uint PartTwo()
        {
            static uint helper(ulong key, Haversack haversack, uint?[] cache)
            {
                if (cache[key] != null) return cache[key].Value;
                else
                {
                    var value = 1u;
                    var iter = haversack.Bags[key].GetEnumerator();

                    while (iter.MoveNext())
                    {
                        if (iter.Current != null)
                        {
                            var cur = (Rule)iter.Current;
                            value += cur.Amount * helper(cur.Next, haversack, cache);
                        }
                    }
                    cache[key] = value;
                    return value;
                }
            }
            var cache = new uint?[haversack.Bags.Length];
            for (var i = 0; i < haversack.Bags.Length; i++) cache[i] = null;
            return helper(haversack.ShinyGold, haversack, cache) - 1;
        }
    }
}
