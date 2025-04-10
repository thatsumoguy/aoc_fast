using aoc_fast.Extensions;

namespace aoc_fast.Years._2022
{
    internal class Day11
    {
        public static string input { get; set; }
        private static List<Monkey> monkeys = [];

        private class Monkey
        {
            public List<ulong> items = [];
            public Operation operation;
            public ulong test;
            public int yes;
            public int no;
        }

        record Operation
        {
            public record Square() : Operation;
            public record Multiply(ulong Value) : Operation;
            public record Add(ulong Value) : Operation;
        }

        private static ulong[] Solve(List<Monkey> monkeys, Func<List<Monkey>, List<(int, ulong)>, ulong[]> play)
        {
            var pairs = new List<(int, ulong)>();

            for (var from = 0; from < monkeys.Count; from++)
            {
                foreach (var item in monkeys[from].items)
                {
                    pairs.Add((from, item));
                }
            }

            var business = play(monkeys, pairs);
            Array.Sort(business);
            Array.Reverse(business);
            return business;
        }

        private static ulong[] Sequential(List<Monkey> monkeys, List<(int, ulong)> pairs)
        {
            var business = new ulong[8];

            foreach (var pair in pairs)
            {
                var extra = Play(monkeys, 20, x => x / 3, pair);
                for (var i = 0; i < extra.Length; i++)
                {
                    business[i] += extra[i];
                }
            }

            return business;
        }

        private static ulong[] ParallelExec(List<Monkey> monkeys, List<(int, ulong)> pairs)
        {
            var product = monkeys.Select(m => m.test).Aggregate((a, b) => a * b);
            var business = new ulong[8];

            var chunkSize = Math.Max(1, pairs.Count / Environment.ProcessorCount);
            var partitions = new List<List<(int, ulong)>>();

            for (var i = 0; i < pairs.Count; i += chunkSize)
            {
                partitions.Add(pairs.GetRange(i, Math.Min(chunkSize, pairs.Count - i)));
            }

            var results = new ulong[partitions.Count][];

            Parallel.For(0, partitions.Count, i =>
            {
                var localBusiness = new ulong[8];
                foreach (var pair in partitions[i])
                {
                    var extra = Play(monkeys, 10000, x => x % product, pair);
                    for (var j = 0; j < 8; j++)
                    {
                        localBusiness[j] += extra[j];
                    }
                }
                results[i] = localBusiness;
            });

            foreach (var result in results)
            {
                for (int i = 0; i < 8; i++)
                {
                    business[i] += result[i];
                }
            }

            return business;
        }

        private static ulong[] Play(List<Monkey> monkeys, int maxRounds, Func<ulong, ulong> adjust, (int, ulong) pair)
        {
            var (from, item) = pair;
            var rounds = 0;
            var business = new ulong[8];

            while (rounds < maxRounds)
            {

                var worry = monkeys[from].operation switch
                {
                    Operation.Square => item * item,
                    Operation.Multiply(var mult) => item * mult,
                    Operation.Add(var add) => item + add,
                    _ => 0uL
                };
                item = adjust(worry);

                var to = (item % monkeys[from].test == 0) ? monkeys[from].yes : monkeys[from].no;

                rounds += (to < from) ? 1 : 0;
                business[from]++;
                from = to;
            }

            return business;
        }

        private static void Parse()
        {
            var chunks = input.Split("\n\n");

            foreach (var chunk in chunks)
            {
                var lines = chunk.Split('\n');
                var monkey = new Monkey();

                var itemsLine = lines[1].Split(':')[1];
                monkey.items = [.. itemsLine.ExtractNumbers<ulong>()];

                var opLine = lines[2].Trim();
                var parts = opLine.Split('=')[1].Trim().Split(' ');

                if (parts[2] == "old")
                {
                    monkey.operation = new Operation.Square();
                }
                else if (parts[1] == "*")
                {
                    monkey.operation = new Operation.Multiply(ulong.Parse(parts[2]));
                }
                else if (parts[1] == "+")
                {
                    monkey.operation = new Operation.Add(ulong.Parse(parts[2]));
                }

                monkey.test = lines[3].ExtractNumbers<ulong>().First();
                monkey.yes = lines[4].ExtractNumbers<int>().First();
                monkey.no = lines[5].ExtractNumbers<int>().First();

                monkeys.Add(monkey);
            }
        }

        public static ulong PartOne()
        {
            Parse();
            var business = Solve(monkeys, Sequential);
            return business[0] * business[1];
        }

        public static ulong PartTwo()
        {
            var business = Solve(monkeys, ParallelExec);
            return business[0] * business[1];
        }
    }
}