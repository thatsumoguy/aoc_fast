using aoc_fast.Extensions;

namespace aoc_fast.Years._2015
{
    class Day14
    {
        public static string input
        {
            get;
            set;
        }
        private static List<int[]> reindeer = [];

        private static void Parse() => reindeer = input.ExtractNumbers<int>().Chunk(3).ToList();

        private static int Distance(int[] reindeer, int time)
        {
            var total = reindeer[1] + reindeer[2];
            var complete = time / total;
            var partial = Math.Min((time % total), reindeer[1]);

            return reindeer[0] * (reindeer[1] * complete + partial);
        }

        private static int NewScoreBoard(List<int[]> reindeer, int time)
        {
            var score = Enumerable.Repeat(0, reindeer.Count).ToArray();
            var distances = Enumerable.Repeat(0, reindeer.Count).ToArray();

            for (var min = 1; min < time; min++)
            {
                var lead = 0;

                foreach (var (r, index) in reindeer.Select((r, i) => (r, i)))
                {
                    var next = Distance(r, min);
                    distances[index] = next;
                    lead = Math.Max(lead, next);
                }

                foreach(var (d, index) in distances.Select((d, i) => (d, i)))
                {
                    if (d == lead) score[index]++;
                }
            }

            return score.Max();
        }

        public static int PartOne()
        {
            Parse();
            return reindeer.Select(r => Distance(r, 2503)).Max();
        }

        public static int PartTwo() => NewScoreBoard(reindeer, 2503);
    }
}
