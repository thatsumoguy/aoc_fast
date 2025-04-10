using aoc_fast.Extensions;

namespace aoc_fast.Years._2017
{
    internal class Day24
    {
        public static string input
        {
            get;
            set;
        }

        record Component(ulong Left, ulong Right, ulong Weight, ulong Length);

        class State(ulong[] possible, ulong[] both, ulong[] weight, ulong[] length, ulong[] bridge)
        {
            public ulong[] Possible { get; set; } = possible;
            public ulong[] Both { get; set; } = both;
            public ulong[] Weight { get; set; } = weight;
            public ulong[] Length { get; set; } = length;
            public ulong[] Bridge { get; set; } = bridge;

            public override string ToString() => 
                $"State:" +
                $"\n\tPossible: ({string.Join(",", Possible)})" +
                $"\n\tBoth: ({string.Join(",", Both)})" +
                $"\n\tWeight: ({string.Join(",", Weight)})" +
                $"\n\tLength: ({string.Join(",", Length)})" +
                $"\n\tBridge: ({string.Join(",", Bridge)})\n\t";
        }

        private static void Build(State state, ulong cur, ulong used, ulong strength, ulong length)
        {
            var remaining = state.Possible[cur] & ~used;

            foreach(var index in remaining.Biterator())
            {
                var next = cur ^ state.Both[index];
                var subUsed  = used | (1ul << (int)index);
                var subStrength = strength + state.Weight[index];
                var subLength = length + state.Length[index];
                if ((state.Possible[next] & ~subUsed) == 0) state.Bridge[subLength] = state.Bridge[subLength] > subStrength ? state.Bridge[subLength] : subStrength;
                else
                {
                    Build(state, next, subUsed, subStrength, subLength);

                    if (cur == next) break;
                }
            }
        }

        private static ulong[] answer = [];

        private static void Parse()
        {
            var components = input.ExtractNumbers<ulong>().Chunk(2).Select(a => new Component(a[0], a[1], a[0] + a[1], 1)).ToList();

            var indices = new List<int>();

            for (ulong n = 1; n < 64; n++)
            {
                foreach (var (component, index) in components.Select((c, i) => (c, i)))
                {
                    if (component.Left == n || component.Right == n) indices.Add(index);
                }

                if (indices.Count == 2)
                {
                    var second = components[indices[1]];
                    (components[indices[1]], components[^1]) = (components[^1], components[indices[1]]);
                    components.RemoveAt(components.Count - 1);
                    var first = components[indices[0]];
                    (components[indices[0]], components[^1]) = (components[^1], components[indices[0]]);
                    components.RemoveAt(components.Count - 1);

                    var left = first.Left == n ? first.Right : first.Left;
                    var right = second.Left == n ? second.Right : second.Left;
                    var weight = first.Weight + second.Weight;
                    var length = first.Length + second.Length;

                    components.Add(new Component(left, right, weight, length));
                }

                indices.Clear();
            }

            components.Sort((a, b) =>
            (a.Left ^ a.Right)
            .CompareTo((b.Left & b.Right))
            .CompareTo(a.Left.CompareTo(b.Left)));

            var state = new State(new ulong[64], new ulong[64], new ulong[64], new ulong[64], new ulong[64]);

            foreach (var (component, index) in components.Select((c, i) => (c, i)))
            {
                var mask = 1ul << index;
                state.Possible[component.Left] |= mask;
                state.Possible[component.Right] |= mask;

                state.Both[index] = component.Left ^ component.Right;
                state.Weight[index] = component.Weight;
                state.Length[index] = component.Length;
            }

            Build(state, 0, 0, 0, 0);
            answer = state.Bridge;
        }

        public static ulong PartOne()
        {
            Parse();
            return answer.Max();
        }

        public static ulong PartTwo() => answer.Last(n => n > 0);
    }
}
