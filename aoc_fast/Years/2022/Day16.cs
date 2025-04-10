using aoc_fast.Extensions;

namespace aoc_fast.Years._2022
{
    internal class Day16
    {
        public static string input { get; set; }
        private static (ulong size, ulong aa, ulong allValves, uint[] flow, uint[] distance, uint[] closest) Input = (default, default, default, [], [], []);

        class State(ulong todo, ulong from, uint time, uint pressure)
        {
            public ulong Todo { get; set; } = todo;
            public ulong From { get; set; } = from;
            public uint Time { get; set; } = time;
            public uint Pressure { get; set; } = pressure;
        }

        struct Valve(string name, uint flow, List<string> edges)
        {
            public string Name { get; set; } = name;
            public uint Flow { get; set; } = flow;
            public List<string> Edges { get; set; } = edges;

            public static Valve Parse(string input)
            {
                var tokens = input.ToCharArray().Split(c => !char.IsAsciiLetterUpper(c) && !char.IsAsciiDigit(c)).Select(c => new string(c))
                    .Where(s => !string.IsNullOrEmpty(s)).ToList();
                var name = tokens[1];
                var flow = uint.Parse(tokens[2]);
                tokens.RemoveRange(0, Math.Min(3, tokens.Count));
                return new Valve(name, flow, tokens);
            }
            public override readonly string ToString()
            {
                return $"Name {Name}, Flow {Flow}, Edges: {string.Join(",", Edges)}";
            }
        }

        class ValveCmp : IComparer<Valve>
        {
            public int Compare(Valve x, Valve y)
            {
                var valeCmp = y.Flow.CompareTo(x.Flow);
                if(valeCmp == 0) return x.Name.CompareTo(y.Name);
                return valeCmp;
            }
        }

        private static void Parse()
        {
            var valves = input.Split("\n", StringSplitOptions.RemoveEmptyEntries).Select(Valve.Parse).ToArray();
            Array.Sort(valves, new ValveCmp());
            var size = valves.Where(v => v.Flow > 0).Count() + 1;
            var distance = new uint[size * size];
            Array.Fill(distance, uint.MaxValue);
            var indices = valves.Index().ToDictionary(v => v.Item.Name,  v => v.Index);

            foreach(var (from, valve) in valves.Index().Take(size))
            {
                distance[from * size + from] = 0;

                foreach(var edge in valve.Edges)
                {
                    var prev = valve.Name;
                    var cur = edge;
                    var to = indices[cur];
                    var total = 1u;
                    while(to >= size)
                    {
                        var next = valves[to].Edges.Find(e => e != prev);
                        prev = cur;
                        cur = next;
                        to = indices[cur];
                        total++;
                    }
                    distance[from * size + to] = total;
                }
            }
            for (var k = 0; k < size; k++)
            {
                for(var i = 0; i < size; i++)
                {
                    for(var  j = 0; j < size; j++)
                    {
                        var candidate = distance[i * size + k].SaturatingAdd(distance[k * size + j]);
                        if(candidate < distance[i * size + j]) distance[i * size + j] = candidate;
                    }
                }
            }

            for (var i = 0; i < distance.Length; i++) distance[i]++;

            var aa = size - 1;
            var allValves = (1ul << aa) - 1;
            var flow = valves.Take(size).Select(v => v.Flow).ToArray();
            var closest = distance.ChunkExact(size).Chunks.Select(chunk => chunk.Where(d => d > 1).Min()).ToArray();
            Input = ((ulong)size, (ulong)aa, allValves, flow, distance, closest);
        }

        private static void Explore((ulong size, ulong aa, ulong allValves, uint[] flow, uint[] distance, uint[] closest) input, State state, Func<ulong, uint, uint> highScore)
        {
            var (todo, from, time, pressure) = (state.Todo, state.From, state.Time, state.Pressure);

            var score = highScore(todo, pressure);

            foreach (var to in todo.Biterator())
            {
                var needed = input.distance[from * input.size + to];
                if (needed >= time) continue;

                var subTodo = todo ^ (1ul << (int)to);
                var subTime = time - needed;
                var subPressure = pressure + subTime * input.flow[to];

                uint heuristic()
                {
                    var valves = subTodo;
                    var time = subTime;
                    var pressure = subPressure;

                    while (valves > 0 && time > 3)
                    {
                        var to = ulong.TrailingZeroCount(valves);
                        valves ^= 1ul << (int)to;
                        time -= input.closest[to];
                        pressure += time * input.flow[to];
                    }
                    return pressure;
                }
                if (heuristic() > score)
                {
                    var next = new State(subTodo, (ulong)to, subTime, subPressure);
                    Explore(input, next, highScore);
                }
            }
        }

        public static uint PartOne()
        {
            Parse();
            var score = 0u;

            var highScore = (ulong _, uint pressure) =>
            {
                score = Math.Max(score, pressure);
                return score;
            };

            var start = new State(Input.allValves, Input.aa, 30, 0);
            Explore(Input, start, highScore);
            return score;
        }

        public static uint PartTwo()
        {
            var you = 0u;
            var remianing = 0uL;

            uint highScore(ulong todo, uint pressure)
            {
                if (pressure > you)
                {
                    you = pressure;
                    remianing = todo;
                }
                return you;
            }

            var first = new State(Input.allValves, Input.aa, 26, 0);
            Explore(Input, first, highScore);

            var elephant = 0u;
            uint highScore2(ulong _, uint pressure)
            {
                elephant = Math.Max(elephant, pressure);
                return elephant;
            }
            var second = new State(remianing, Input.aa, 26, 0);
            Explore(Input, second, highScore2);

            var score = new uint[Input.allValves + 1];
            uint highScore3(ulong todo, uint pressure)
            {
                var done = Input.allValves ^ todo;
                score[done] = Math.Max(score[done], pressure);
                return elephant;
            }
            var third = new State(Input.allValves, Input.aa, 26, 0);
            Explore(Input, third, highScore3);

            var res = you + elephant;

            var candidates = score.Index().Where(s => s.Item > 0);
            candidates = [.. candidates.OrderBy(t => t.Item)];
            var en = candidates.GetEnumerator();
            for(var i = candidates.Count() - 1; i >= 1; i--)
            {
                var (mask1, subYou) = candidates.ElementAt(i);

                if (subYou * 2 <= res) break;

                for(var j = i; j >= 0; j--)
                {
                    var (mask2, subElephant) = candidates.ElementAt(j);

                    if((mask1 & mask2) == 0)
                    {
                        res = Math.Max(res, subYou + subElephant);
                        break;
                    }
                }
            }
            return res;
        }
    }
}
