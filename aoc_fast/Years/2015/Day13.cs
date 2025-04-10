using aoc_fast.Extensions;

namespace aoc_fast.Years._2015
{
    class Day13
    {
        public static string input
        {
            get;
            set;
        }

        private static (int partOne, int partTwo) answer = (0,0);

        private static void Parse()
        {
            var lines = input.Split('\n', StringSplitOptions.RemoveEmptyEntries).Select(l => l.Split([' ', '.']).ToList()).ToList();
            var indices = new Dictionary<string, int>();

            foreach (var token in lines)
            {
                var size = indices.Count;
                indices.OrInsert(token[0], size);
                size = indices.Count;
                indices.OrInsert(token[10], size);
            }

            var stride = indices.Count;

            var happiness = Enumerable.Repeat(0, stride * stride).ToArray();

            foreach (var token in lines)
            {
                var start = indices[token[0]];
                var end = indices[token[10]];
                var sign = token[2] == "gain" ? 1 : -1;
                var value = int.Parse(token[3]);

                happiness[stride * start + end] += sign * value;
                happiness[stride * end + start] += sign * value;
            }

            var partOne = 0;
            var partTwo = 0;
            var newIndicies = Enumerable.Range(1, stride - 1).ToList();

            newIndicies.Permutations((slice) =>
            {
                var sum = 0;
                var weakestLink = int.MaxValue;

                var link = (int from, int to) =>
                {
                    var value = happiness[stride * from + to];
                    sum += value;
                    weakestLink = Math.Min(weakestLink, value);
                };

                link(0, slice[0]);
                link(0, slice[slice.Count - 1]);

                for (var i = 1; i < slice.Count; i++) link(slice[i], slice[i - 1]);

                partOne = Math.Max(partOne, sum);
                partTwo = Math.Max(partTwo, sum - weakestLink);
            });

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
