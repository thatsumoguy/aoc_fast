using System.Text;

namespace aoc_fast.Years._2023
{
    internal class Day7
    {
        public static string input { get; set; }

        struct Hand
        {
            public byte[] Cards { get; }
            public int Bid { get; }

            public Hand(byte[] cards, int bid)
            {
                Cards = cards;
                Bid = bid;
            }
        }

        private static List<Hand> hands = [];

        private static int Sort(List<Hand> input, int j)
        {
            var hands = input.Select(h =>
            {
                var (cards, bid) = (h.Cards, h.Bid);

                var rank = cards.Select(b => b switch
                {
                    (byte)'A' => 14,
                    (byte)'K' => 13,
                    (byte)'Q' => 12,
                    (byte)'J' => j,
                    (byte)'T' => 10,
                    _ => b - (byte)'0'
                });

                var freq = new int[15];
                foreach (var r in rank) freq[r]++;

                var jokers = freq[1];
                freq[1] = 0;
                Array.Sort(freq);
                Array.Reverse(freq);
                freq[0] += jokers;

                var key = 0uL;
                foreach (var f in freq.Take(5)) key = (key << 4) | (uint)f;
                foreach (var r in rank) key = (key << 4) | (uint)r;

                return (key, bid);
            }).ToList();

            hands.Sort((a, b) => a.key.CompareTo(b.key));

            return hands.Select((h, i) => (i + 1) * h.bid).Sum();
        }

        private static void Parse()
        {
            hands = input.Split("\n", StringSplitOptions.RemoveEmptyEntries).Select(line =>
            {
                var prefix = line[..5];
                var suffix = line[5..];
                var cards = Encoding.ASCII.GetBytes(prefix);
                var bid = int.Parse(suffix);
                return new Hand(cards, bid);
            }).ToList();
        }

        public static int PartOne()
        {
            Parse();
            return Sort(hands, 11);
        }

        public static int PartTwo() => Sort(hands, 1);
        
    }
}
