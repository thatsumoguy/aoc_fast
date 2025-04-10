using System.Text;
using aoc_fast.Extensions;

namespace aoc_fast.Years._2023
{
    internal class Day2
    {
        public static string input { get; set; }
        record Game(uint r, uint g, uint b);
        private static List<Game> games = [];

        private static void Parse()
        {
            games = input.Split("\n", StringSplitOptions.RemoveEmptyEntries).Select(line =>
            {
                return line.Split([' ', '\t', '\r']).Chunk(2).Skip(1).Aggregate(new Game(0, 0, 0), (g, a) =>
                {
                    var amount = uint.Parse(a[0]);
                    return Encoding.ASCII.GetBytes(a[1])[0] switch
                    {
                        (byte)'r' => new Game(g.r.Max(amount), g.g, g.b),
                        (byte)'g' => new Game(g.r, g.g.Max(amount), g.b),
                        (byte)'b' => new Game(g.r, g.g, g.b.Max(amount)),
                    };
                });
            }).ToList();
        }
        public static uint PartOne()
        {
            Parse();
            return games.Index().Select(i => (i.Item.r <= 12 && i.Item.g <= 13 && i.Item.b <= 14) ? (uint)i.Index + 1 : 0u).Sum();
        }
        public static uint PartTwo() => games.Select(g => g.r * g.g * g.b).Sum();
    }
}


