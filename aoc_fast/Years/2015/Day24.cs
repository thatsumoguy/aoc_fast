using aoc_fast.Extensions;

namespace aoc_fast.Years._2015
{
    class Day24
    {
        public static string input
        {
            get;
            set;
        }
        private static ulong[] packages = [];
        private static ulong? Combinations(ulong[] packages, ulong target, int size)
        {
            var indices = Enumerable.Range(0, size).ToArray();

            var weight = packages.Take(size).Sum();

            while (true)
            {
                if (weight == target) return indices.Select(i => packages[i]).Aggregate(1ul, (c, a) => c * a);


                var depth = size - 1;

                while (indices[depth] == packages.Length - size + depth)
                {
                    if (depth == 0) return null;
                    depth--;
                }

                var from = indices[depth];
                var to = indices[depth] + 1;
                indices[depth] = to;
                weight = weight - packages[from] + packages[to];
                depth++;

                while (depth < size)
                {
                    from = indices[depth];
                    to = indices[depth - 1] + 1;
                    indices[depth] = to;
                    weight = weight - packages[from] + packages[to];
                    depth++;
                }
            }
        }

        private static void Parse()
        {
            packages = [.. input.ExtractNumbers<ulong>()];
            Array.Sort(packages);
        }

        public static ulong? PartOne()
        {
            Parse();
            var sum = packages.Sum();
            var target = sum / 3;
            for(var i = 1; i < packages.Length - 1; i++)
            {
                var res = Combinations(packages, target, i);
                if (res != null) return res;
            }
            return null;
        }
        public static ulong? PartTwo()
        {
            var sum = packages.Sum();
            var target = sum / 4;
            for (var i = 1; i < packages.Length - 1; i++)
            {
                var res = Combinations(packages, target, i);
                if (res != null) return res;
            }
            return null;
        }
    }
}
