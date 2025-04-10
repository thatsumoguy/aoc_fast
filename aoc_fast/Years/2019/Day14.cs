using System.Text.RegularExpressions;
using aoc_fast.Extensions;

namespace aoc_fast.Years._2019
{
    internal partial class Day14
    {
        public static string input { get; set; }
        private static List<Reaction> Reactions = [];
        class Ingrident(ulong amount, int chemical)
        {
            public ulong Amount { get; set; } = amount;
            public int Chemical { get; set; } = chemical;
            public override string ToString() => $"{Amount},{Chemical}";
        }

        class Reaction(ulong amount, int chemical, List<Ingrident> ingridents)
        {
            public ulong Amount { get; set; } = amount;
            public int Chemical { get; set;} = chemical;
            public List<Ingrident> Ingridents { get; set; } = ingridents; 
        }

        private static void Topological(List<Reaction> reactions, List<int> order, int chemical, int depth)
        {
            order[chemical] = Math.Max(order[chemical], depth);

            foreach(var ingredient in reactions[chemical].Ingridents)
            {
                Topological(reactions, order, ingredient.Chemical, depth + 1);
            }
        }

        private static ulong Ore(List<Reaction> reactions, ulong amount)
        {
            var total = new ulong[reactions.Count];
            total[0] = amount;
            foreach(var reaction in reactions[..(reactions.Count - 1)])
            {
                var multiplier = FastMath.CeilingDivide(total[reaction.Chemical] , reaction.Amount);
                foreach (var ingridient in reaction.Ingridents)
                {
                    total[ingridient.Chemical] += multiplier * ingridient.Amount;
                }
            }
            return total[1];
        }

        private static void Parse()
        {
            var lines = input.Split("\n");

            var reactions = Slice.RepeatWith(() => { return new Reaction(0, 1, []); }).Take(lines.Length +1).ToList();

            var indices = new Dictionary<string, int>
            {
                ["FUEL"] = 0,
                ["ORE"] = 1
            };

            foreach( var line in lines)
            {
                var tokens = MyRegex().Split(line).Where(s => !string.IsNullOrWhiteSpace(s)).Reverse().Chunk(2).ToList();
                var (kind, amount) = (tokens[0][0], tokens[0][1]);
                var size = indices.Count;
                if (!indices.TryGetValue(kind, out int chemical))
                {
                    indices[kind] = size;
                    chemical = size;
                }
                var reaction = reactions[chemical];
                reaction.Amount = ulong.Parse(amount);
                reaction.Chemical = chemical;

                foreach(var set in tokens[1..])
                {
                    var (subKind, subAmount) = (set[0], set[1]);
                    var am = ulong.Parse(subAmount);
                    var subSize = indices.Count;
                    if (!indices.TryGetValue(subKind, out int subChem))
                    {
                        indices[subKind] = subSize;
                        subChem = subSize;
                    }
                    reaction.Ingridents.Add(new Ingrident(am, subChem));
                }
                reactions[chemical] = reaction;;
            }
            
            var order = Enumerable.Repeat(0, reactions.Count).ToList();
            Topological(reactions, order, 0, 0);
            reactions = [.. reactions.OrderBy(r => order[r.Chemical])];
            Reactions = reactions;
        }

        public static ulong PartOne()
        {
            try
            {
                Parse();
                return Ore(Reactions, 1);
            }
            catch (Exception e) { Console.WriteLine(e); }
            return 0;
        }

        public static ulong PartTwo()
        {
            var threshold = 1_000_000_000_000u;
            var start = 1ul;
            var end = threshold;
            while (start < end)
            {
                var middle = (start + end) / 2;
                switch(Ore(Reactions, middle).CompareTo(threshold))
                {
                    case -1:
                        start = middle + 1;
                        break;
                    case 0:
                        return middle;
                    case 1:
                        end = middle - 1;
                        break;
                }
            }
            return start;
        }

        [GeneratedRegex(@"[^a-zA-Z0-9]+")]
        private static partial Regex MyRegex();
    }
}
