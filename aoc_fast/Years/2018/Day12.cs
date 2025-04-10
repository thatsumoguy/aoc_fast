using System.Text;
using aoc_fast.Extensions;

namespace aoc_fast.Years._2018
{
    internal class Day12
    {
        public static string input
        {
            get;
            set;
        }

        record Tunnel(List<ulong> plants, long start, long sum);

        record InputObj(List<ulong> rules, Tunnel state);

        private static InputObj inputObj;

        private static void Parse()
        {
            var lines = input.Split('\n', StringSplitOptions.RemoveEmptyEntries).Select(Encoding.ASCII.GetBytes).ToArray();

            var plants = lines[0][15..].Select(b => (ulong)(b & 1)).ToList();

            var rules = new ulong[32];

            foreach (var line in lines[1..])
            {
                var binary = line.Aggregate(0, (acc, b) => (acc << 1) | (b & 1));
                rules[binary >> 5] = (ulong)(binary & 1);
            }

            inputObj = new InputObj([.. rules], new Tunnel(plants, 0, 0));
        }

        private static Tunnel Step(List<ulong> rules, Tunnel tunnel)
        {
            var index = 0uL;
            var sum = 0L;
            var pos = tunnel.start - 2;
            var plants = new List<ulong>(1000);
            foreach (var plant in tunnel.plants.Concat(Enumerable.Repeat(0UL, 4)))
            {

                index = ((index << 1) | plant) & 0b11111ul;
                sum += pos * (long)rules[(int)index];

                pos++;

                plants.Add(rules[(int)index]);
            }

            return new Tunnel(plants, tunnel.start - 2, sum);
        }

        public static long PartOne()
        {
            Parse();

            var current = inputObj.state;

            for(var _ = 0;  _ < 20; _++)
            {
                current = Step(inputObj.rules, current);
            }
            return current.sum;
        }

        public static long PartTwo()
        {
            var current = inputObj.state;
            var delta = 0L;
            var generations = 0;

            while(true)
            {
                var next = Step(inputObj.rules, current);
                var nextDelta = next.sum - current.sum;

                if(delta == nextDelta) return current.sum + delta * (50_000_000_000 - generations);

                current = next;
                delta = nextDelta;
                generations++;
            }
        }
    }
}
