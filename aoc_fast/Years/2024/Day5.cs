using aoc_fast.Extensions;

namespace aoc_fast.Years._2024
{
    internal class Day5
    {
        public static string input
        {
            get;
            set;
        }


        private static (int partOne, int partTwo) answer;
        private static Dictionary<int, HashSet<int>> updatePairs = [];
        private static void Parse()
        {
            var split = input.Split("\n\n");
            var partOne = 0;
            var partTwo = 0;
            var comparer = new int[100][];
            for (var i = 0; i < 100; i++)
            {
                comparer[i] = new int[100];
                for (var j = 0; j < 100; j++) comparer[i][j] = -1;
            }
            foreach (var line in split[0].ExtractNumbers<int>().Chunk(2))
            {
                var (b, a) = line switch { var p => (p[0], p[1]) };
                if (!updatePairs.ContainsKey(b)) updatePairs.Add(b, []);
                updatePairs[b].Add(a);
            }

            foreach (var line in split[1].Split("\n", StringSplitOptions.RemoveEmptyEntries))
            {
                var updates = line.Split(",").Select(int.Parse).ToList();
                var valid = true;
                for (var i = 1; i < updates.Count - 1; i++)
                {
                    var first = updates[i - 1];
                    if (updatePairs.TryGetValue(first, out HashSet<int>? up)) valid = valid = updates[(i + 1)..].All(a => up.Contains(a));
                    if (!valid) break;
                }
                if (valid) partOne += updates[updates.Count / 2];
                else
                {
                    var last = 0;
                    var midPoint = (updates.Count / 2) + 1;
                    for (var j = 0; j < midPoint; j++)
                    {
                        for (var i = 0; i < updates.Count; i++)
                        {
                            var current = updates[i];
                            var remaining = new List<int>(updates);
                            remaining.SwapRemove(i);

                            if (updatePairs.TryGetValue(current, out var successors) && remaining.All(a => successors.Contains(a)))
                            {
                                last = current;
                                updates = remaining;
                                break;
                            }
                        }
                    }
                    partTwo += last;
                }
            }

            answer = (partOne, partTwo);
        }

        public static int PartOne()
        {
            Parse();
            return answer.partOne;
        }
        public static int PartTwo() => answer.partTwo;
    }
}
