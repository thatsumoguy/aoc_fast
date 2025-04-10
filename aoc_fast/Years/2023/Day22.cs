using aoc_fast.Extensions;

namespace aoc_fast.Years._2023
{
    internal class Day22
    {
        public static string input { get; set; }
        private static (uint partOne, uint partTwo) answers;

        private static void Parse()
        {
            var bricks = input.ExtractNumbers<uint>().Chunk(6).ToList();

            var heights = new uint[100];
            var indicies = new uint[100];
            for (var i = 0; i < 100; i++) indicies[i] = uint.MaxValue;

            var safe = new bool[bricks.Count];
            for(var i = 0; i < safe.Length; i++) safe[i] = true;

            var dominator = new List<(uint, uint)>(bricks.Count);

            bricks = [.. bricks.OrderBy(x => x[2])];

            foreach(var (i, brick) in bricks.Index())
            {
                var(x1, y1, z1, x2, y2, z2) = (brick[0],  brick[1], brick[2], brick[3], brick[4], brick[5]);

                var start = 10 * y1 + x1;
                var end = 10 * y2 + x2;
                var step = y2 > y1 ? 10u : 1u;
                var height = z2 - z1 + 1;

                var top = 0u;
                var prev = uint.MaxValue;
                var underneath = 0;
                var parent = 0u;
                var depth = 0u;

                for (var j = start; j <= end; j += step) top = top.Max(heights[j]);

                for(var j = start; j <= end; j += step)
                {
                    if (heights[j] == top)
                    {
                        var index = indicies[j];
                        if(index != prev)
                        {
                            prev = index;
                            underneath++;
                            if (underneath == 1) (parent, depth) = dominator[(int)prev];
                            else
                            {
                                var (a, b) = (parent, depth);
                                var (x, y) = dominator[(int)prev];

                                while (b > y) (a, b) = dominator[(int)a];
                                while(y > b) (x,y) = dominator[(int)x];

                                while(a != x)
                                {
                                    (a, b) = dominator[(int)a];
                                    (x, _) = dominator[(int)x];
                                }

                                (parent, depth) = (a, b);
                            }
                        }
                    }

                    heights[j] = top + height;
                    indicies[j] = (uint)i;
                }

                if(underneath == 1)
                {
                    safe[prev] = false;
                    parent = prev;
                    depth = dominator[(int)prev].Item2 + 1;
                }

                dominator.Add((parent, depth));
            }

            answers = ((uint)safe.Where(b => b).Count(), dominator.Select(i => i.Item2).Sum());
        }

        public static uint PartOne()
        {
            Parse();
            return answers.partOne;
        }
        public static uint PartTwo() => answers.partTwo;
    }
}
