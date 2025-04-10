using aoc_fast.Extensions;

namespace aoc_fast.Years._2015
{
    class Day17
    {
        static int[][] NCR = [[1, 0, 0, 0, 0], [1, 1, 0, 0, 0], [1, 2, 1, 0, 0], [1, 3, 3, 1, 0], [1, 4, 6, 4, 1]];

        record State(List<int> size, List<long> freq, List<int> result);

        public static string input
        {
            get;
            set;
        }

        private static List<int> answer = [];

        private static void Parse()
        {
            var containers = new SortedDictionary<int, long>();

            foreach(var size in input.ExtractNumbers<int>())
            {
                if(containers.TryGetValue(size, out long value)) containers[size] = ++value;
                else containers[size] = 1L;
            }

            var state = 
                new State([.. containers.Keys],
                [.. containers.Values], 
                Enumerable.Repeat(0, containers.Count).ToList());
            state.size.Reverse();
            state.freq.Reverse();

            Combinations(state, 0, 0, 0, 1);
            answer = state.result;
        }

        private static void Combinations(State state, int index, long containers, int liters, int factor)
        {
            var n = state.freq[index];
            var next = liters;

            for (var r = 0; r < n + 1; r++)
            {
                if(next < 150)
                {
                    if(index < state.size.Count - 1)
                    {
                        Combinations(state, index + 1, containers + r, next, factor * NCR[n][r]);
                    }
                }
                else
                {
                    if(next == 150) state.result[(int)containers + r] += factor * NCR[n][r];
                    break;
                }
                next += state.size[index];
            }
        }

        public static int PartOne()
        {
            Parse();
            return answer.Sum();
        }

        public static int PartTwo() => answer.Find(X => X > 0);
    }
}
