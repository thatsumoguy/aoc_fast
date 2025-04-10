using System.Text;
using aoc_fast.Extensions;

namespace aoc_fast.Years._2023
{
    internal class Day19
    {
        public static string input { get; set; }

        class Rule(uint start, uint end, ulong category, string next)
        {
            public uint Start { get; set; } = start;
            public uint End { get; set; } = end;
            public ulong Category { get; set; } = category;
            public string Next { get; set; } = next;
        }

        private static Dictionary<string, List<Rule>> Workflows = [];
        private static string Parts;

        private static void Parse()
        {
            var parts = input.Split("\n\n", StringSplitOptions.RemoveEmptyEntries);
            
            var (prefix, suffix) = (parts[0], parts[1]);
            var workFlows = new Dictionary<string, List<Rule>>(1000);
            foreach(var line in prefix.Split("\n"))
            {
                var rules = new List<Rule>(5);
                var iter = line.Split(['{', ':', ',', '}']);
                var key = iter[0];
                iter = iter[1..];

                foreach(var pair in iter.Chunk(2))
                {
                    var (first, second) = (pair[0], pair[1]);

                    Rule rule;
                    if (string.IsNullOrEmpty(second)) rule = new Rule(1, 4001, 0, first);
                    else
                    {
                        var category = Encoding.ASCII.GetBytes(first)[0] switch
                        {
                            (byte)'x' => 0ul,
                            (byte)'m' => 1ul,
                            (byte)'a' => 2ul,
                            (byte)'s' => 3ul
                         };

                        var value = uint.Parse(first[2..]);
                        var next = second;

                        rule = Encoding.ASCII.GetBytes(first)[1] switch
                        {
                            (byte)'<' => new Rule(1, value, category, next),
                            (byte)'>' => new Rule(value + 1, 4001, category, next)
                        };
                    }

                    rules.Add(rule);
                }

                workFlows[key] = rules;
            }

            (Workflows, Parts) = (workFlows, suffix);
        }

        public static uint PartOne()
        {
            Parse();
            var (workflows, parts) = (Workflows.ToDictionary(), Parts);

            var res = 0u;

            foreach (var part in parts.ExtractNumbers<uint>().Chunk(4))
            {
                var key = "in";

                while (key.Length > 1)
                {
                    foreach (var workflow in workflows[key])
                    {
                        var (start, end, category, next) = (workflow.Start, workflow.End, workflow.Category, workflow.Next);
                        if (start <= part[category] && part[category] < end)
                        {
                            key = next;
                            break;
                        }
                    }
                }
                if (key == "A") res += part.Sum();
            }
            return res;
        }

        public static ulong PartTwo()
        {
            Parse();
            var workFlows = Workflows.ToDictionary();

            var res = 0ul;
            var todo = new List<(string, ulong, (uint, uint)[])>() { ("in", 0, Enumerable.Repeat((1u, 4001u), 4).ToArray()) };

            while (todo.PopCheck(out var next))
            {
                var (key, index, part) = next;

                if (key.Length < 2)
                {
                    if (key == "A")
                    {
                        ulong product = 1;
                        foreach (var (start, end) in part)
                        {
                            product *= (ulong)(end - start);
                        }
                        res += product;
                    }
                    continue;
                }

                var rule = workFlows[key][(int)index];
                var (s2, e2, category, nextStr) = (rule.Start, rule.End, rule.Category, rule.Next);

                var (s1, e1) = part[category];

                var x1 = Math.Max(s1, s2);
                var x2 = Math.Min(e1, e2);

                if (x1 >= x2) todo.Add((key, index + 1, part.ToArray()));
                else
                {
                    var overlapPart = part.ToArray();
                    overlapPart[category] = (x1, x2);
                    todo.Add((nextStr, 0, overlapPart));

                    if (s1 < x1)
                    {
                        var beforePart = part.ToArray();
                        beforePart[category] = (s1, x1);
                        todo.Add((key, index + 1, beforePart));
                    }

                    if (x2 < e1)
                    {
                        var afterPart = part.ToArray();
                        afterPart[category] = (x2, e1);
                        todo.Add((key, index + 1, afterPart));
                    }
                }
            }
            return res;
        }
    }
}
