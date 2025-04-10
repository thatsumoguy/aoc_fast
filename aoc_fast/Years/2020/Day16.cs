using aoc_fast.Extensions;

namespace aoc_fast.Years._2020
{
    internal class Day16
    {
        public static string input { get; set; }

        class Rule(bool departure, uint a, uint b, uint c, uint d) : IEquatable<Rule>
        {
            public bool Depature { get; set; } = departure;
            public uint A { get; set; } = a;
            public uint B { get; set; } = b;
            public uint C { get; set; } = c;
            public uint D { get; set; } = d;

            public static Rule From(string line)
            {
                var dep = line.StartsWith("departure");
                var chunks = line.ExtractNumbers<uint>().Chunk(4).ToArray()[0];
                var (a, b, c, d) = (chunks[0], chunks[1], chunks[2], chunks[3]);
                return new(dep, a, b, c, d);
            }
            
            public bool Check(uint n) => (A <= n && n <= B) || (C <= n && n <= d);

            public bool Equals(Rule? other)
            {
                if (other != null)
                    return (other.Depature == Depature && other.A == A && other.B == B && other.C == C & other.D == D);
                return false;
            }
        }
        private static (uint, List<bool>) SolveP1(List<Rule> rules, List<List<uint>> tickets)
        {
            var range = new Range(26, (976 -25));
            var total = 0u;
            var valid = new bool[tickets[0].Count];
            Array.Fill(valid, true);

            foreach (var column in tickets)
            {
                foreach (var (index, n) in column.Index())
                {
                    if (!(range.Start.Value <= n && range.End.Value >= n))
                    {
                        total += n;
                        valid[index] = false;
                    }
                }
            }
            return (total, valid.ToList());
        }
        private static ulong SolveP2(List<Rule> rules, List<uint> yourTicket, List<List<uint>> nearbyTickets, List<bool> valid)
        {
            var rulesByColumn = new List<List<Rule>>();
            var product = 1ul;
            foreach(var ticket in nearbyTickets)
            {
                var remaining = rules.ToList();
                foreach(var (validKey, n) in valid.Zip(ticket))
                {
                    if(validKey)
                    {
                        remaining = remaining.Where(rule => rule.Check(n)).ToList();
                    }
                }
                rulesByColumn.Add(remaining);
            }
            while(rulesByColumn.Any(rules => rules.Count > 1))
            {
                for(var i = 0; i < rulesByColumn.Count; i++)
                {
                    if (rulesByColumn[i].Count == 1)
                    {
                        var found = rulesByColumn[i].Pop();
                        if (found.Depature) product *= (ulong)yourTicket[i];

                        for (var j = 0; j < rulesByColumn.Count; j++) rulesByColumn[j] = rulesByColumn[j].Where(rule => rule != found).ToList();
                    }
                }
            }
            return product;
        }
        private static (uint partOne, ulong partTwo) answer;

        private static void Parse()
        {
            var parts = input.TrimEnd().Split("\n\n");
            var rules = parts[0].Split("\n").Select(Rule.From).ToList();
            var yourTicket = parts[1].ExtractNumbers<uint>();
            var nearbyTickets = new List<List<uint>>(rules.Count + 1);
            for (var n = 0; n < rules.Count; n++) nearbyTickets.Add([]);
            foreach (var (i, n) in parts[2].ExtractNumbers<uint>().Index()) nearbyTickets[i % rules.Count].Add(n);

            var (errorRate, validOnes) = SolveP1(rules, nearbyTickets);
            var product = SolveP2(rules, yourTicket, nearbyTickets, validOnes);
            answer = (errorRate, product);
        }
        public static uint PartOne()
        {
            Parse();
            return answer.partOne;
        }
        public static ulong PartTwo() => answer.partTwo;
    }
}
