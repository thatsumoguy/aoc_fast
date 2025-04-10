using aoc_fast.Extensions;

namespace aoc_fast.Years._2020
{
    internal class Day22
    {
        public static string input { get; set; }
        enum Winner
        {
            Player1,
            Player2
        }

        class Deck(ulong sum, ulong score, int start, int end, byte[] cards)
        {
            public ulong Sum { get; set; } = sum;
            public ulong Score { get; set; } = score;
            public int Start { get; set; } = start;
            public int End { get; set; } = end;
            public byte[] Cards { get; set;} = cards;

            public static Deck New() => new(0, 0, 0, 0, new byte[50]);

            public ulong PopFront()
            {
                var card = (ulong)Cards[Start % 50];
                Sum -= card;
                Score -= (ulong)Size() * card;
                Start++;
                return card;
            }
            public void PushBack(ulong card)
            {
                Cards[End % 50] = (byte)card;
                Sum += card;
                Score += Sum;
                End++;
            }

            public byte Max() => Enumerable.Range(Start, (End - Start)).Select(i => Cards[i % 50]).Max();

            public bool NonEmpty() => End > Start;

            public int Size() => End - Start;

            public Deck Copy(ulong amount)
            {
                var copy = new Deck(Sum, Score, Start, End, (byte[])Cards.Clone());
                copy.End = copy.Start + (int)amount;
                copy.Sum = 0;

                for(var i = 0; i < (int)amount; i++)
                {
                    var card = (ulong)copy.Cards[(copy.Start + i) % 50];
                    copy.Sum += card;
                }
                return copy;
            }
        }

        private static Winner Play(ref Deck deck1, ref Deck deck2, List<HashSet<(ulong, ulong)>> cache, int depth)
        {
            if (depth > 0 && deck1.Max() > deck2.Max()) return Winner.Player1;

            if (cache.Count == depth) cache.Add(new HashSet<(ulong, ulong)>(1000));
            else cache[depth].Clear();

            while(deck1.NonEmpty() && deck2.NonEmpty())
            {
                if (!cache[depth].Add((deck1.Score, deck2.Score)))
                {
                    return Winner.Player1;
                }

                var (card1, card2) = (deck1.PopFront(),  deck2.PopFront());
                if(deck1.Size() < (int)card1 || deck2.Size() < (int)card2)
                {
                    if(card1 >  card2)
                    {
                        deck1.PushBack(card1);
                        deck1.PushBack(card2);
                    }
                    else
                    {
                        deck2.PushBack(card2);
                        deck2.PushBack(card1);
                    }
                }
                else
                {
                    var copy1 = deck1.Copy(card1);
                    var copy2 = deck2.Copy(card2);
                    switch (Play(ref copy1, ref copy2, cache, depth + 1))
                    {
                        case Winner.Player1:
                            deck1.PushBack(card1);
                            deck1.PushBack(card2);
                            break;
                        case Winner.Player2:
                            deck2.PushBack(card2);
                            deck2.PushBack(card1);
                            break;
                    }
                }
            }
            return deck1.NonEmpty() ? Winner.Player1 : Winner.Player2;
        }
        private static (Deck deck1, Deck deck2) Decks;

        private static void Parse()
        {
            var (deck1, deck2) = (Deck.New(),  Deck.New());
            var parts = input.Split("\n\n");
            var (player1, player2) = (parts[0], parts[1]);

            foreach (var item in player1.ExtractNumbers<ulong>().Skip(1))
            {
                deck1.PushBack(item);
            }
            foreach (var item in player2.ExtractNumbers<ulong>().Skip(1))
            {
                deck2.PushBack(item);
            }
            Decks = (deck1, deck2);
        }

        public static ulong PartOne()
        {
            Parse();
            var (deck1, deck2) = Decks;

            while(deck1.NonEmpty() && deck2.NonEmpty())
            {
                var (card1, card2) = (deck1.PopFront(), deck2.PopFront());

                if(card1 > card2)
                {
                    deck1.PushBack(card1);
                    deck1.PushBack(card2);
                }
                else
                {
                    deck2.PushBack(card2);
                    deck2.PushBack(card1);
                }
            }
            return deck1.NonEmpty() ? deck1.Score : deck2.Score;
        }

        public static ulong PartTwo()
        {
            Parse();
            var (deck1, deck2) = Decks;
            return Play(ref deck1, ref deck2, [], 0) switch
            {
                Winner.Player1 => deck1.Score,
                Winner.Player2 => deck2.Score
            };
        }
    }
}
