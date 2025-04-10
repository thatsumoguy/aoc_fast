using aoc_fast.Extensions;

namespace aoc_fast.Years._2021
{
    internal class Day12
    {
        public static string input { get; set; }

        class State(ulong from, uint visited, bool twice)
        {
            public ulong From { get; set; } = from;
            public uint Visited { get; set; } = visited;
            public bool Twice { get; set; } = twice;
        }

        private static uint Paths((uint small, uint[] edges) input, State state, uint[] cache)
        {
            var (from, visited, twice) = (state.From, state.Visited, state.Twice);

            var index = (state.Twice ? 1ul : 0ul) + 2ul * (state.From) + ((ulong)input.edges.Length * ((ulong)visited/2));
            var total = cache[index];
            if(total > 0) return total;

            var caves = input.edges[from];
            var subTotal = 0u;
            var end = 1u << 1;

            if((caves & end) != 0)
            {
                caves ^= end;
                subTotal++;
            }

            foreach(var to in caves.Biterator())
            {
                var mask = 1u << to;
                var once = (input.small & mask) == 0 || (visited & mask) == 0;

                if(once || twice)
                {
                    var next = new State((ulong)to, visited | mask, once && twice);
                    subTotal += Paths(input, next, cache);
                }
            }

            cache[index] = subTotal;
            return subTotal;
        }

        private static uint Explore((uint small, uint[] edges) input, bool twice)
        {
            var size = 2 * input.edges.Length * (1 << (input.edges.Length - 2));
            var cache = new uint[size];

            var state = new State(0, 0, twice);
            return Paths(input, state, cache);
        }

        private static (uint small, uint[] edges) Edges = (0, []);

        private static void Parse()
        {
            var tokens = input.Split(['-', '\n', '\t']).Where(s => !string.IsNullOrWhiteSpace(s)).ToList();

            var indices = new Dictionary<string, uint> { {"start", 0 }, { "end", 1 } };
            foreach(var token in tokens)
            {
                if(!indices.ContainsKey(token)) indices[token] = (uint)indices.Count;
            }

            var edges = new uint[indices.Count];
            foreach(var pair in tokens.Chunk(2))
            {
                var (a, b) = (pair[0],  pair[1]);
                edges[indices[a]] |= 1u << (int)indices[b];
                edges[indices[b]] |= 1u << (int)indices[a];
            }
            var notStart = ~(1u << 0);
            for(var i = 0; i < edges.Length; i++) edges[i] &= notStart;

            var small = 0u;
            foreach(var (key, val) in indices)
            {
                if (char.IsAsciiLetterLower(key.ToCharArray()[0])) small |= 1u << (int)val;
            }

            Edges = (small, edges);
        }

        public static uint PartOne()
        {
            Parse();
            return Explore(Edges, false);
        }
        public static uint PartTwo() => Explore(Edges, true);
    }
}
