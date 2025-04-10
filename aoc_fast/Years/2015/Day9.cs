using aoc_fast.Extensions;

namespace aoc_fast.Years._2015
{
    class Day9
    {
        public static string input
        {
            get;
            set;
        }

        public static (int partOne, int partTwo) answer = (0, 0);

        private static void Parse()
        {
            var tokens = input.Split([' ', '\t', '\r', '\n'], StringSplitOptions.RemoveEmptyEntries).Chunk(5).ToList();
            var indices = new Dictionary<string, int>();

            foreach (var t in tokens)
            {
                var size = indices.Count;
                indices.OrInsert(t[0], size);

                size = indices.Count;
                indices.OrInsert(t[2], size);
            }
            var stride = indices.Count;
            var distances = Enumerable.Repeat(0, stride * stride).ToList();

            foreach (var t in tokens)
            {
                var start = indices[t[0]];
                var end = indices[t[2]];
                var distance = int.Parse(t[4]);

                distances[stride * start + end] = distance;
                distances[stride * end + start] = distance;
            }

            var global_min = int.MaxValue;
            var global_max = int.MinValue;
            var newIndicies = Enumerable.Range(1, stride - 1).ToList();
            newIndicies.Permutations(slice =>
            {
                var sum = 0;
                var local_min = int.MaxValue;
                var local_max = int.MinValue;

                void trip(int from, int to)
                {
                    var distance = distances[stride * from + to];
                    sum += distance;
                    local_min = Math.Min(local_min, distance);
                    local_max = Math.Max(local_max, distance);
                }

                trip(0, slice[0]);
                trip(0, slice[slice.Count - 1]);

                for (var i = 1; i < slice.Count; i++) { trip(slice[i], slice[i - 1]); }
                global_min = Math.Min(sum - local_max, global_min);
                global_max = Math.Max(sum - local_min, global_max);
            });

            answer = (global_min, global_max);
        }

        public static int PartOne()
        {
            Parse();
            return answer.partOne;
        }
        public static int PartTwo() => answer.partTwo;
    }
}
