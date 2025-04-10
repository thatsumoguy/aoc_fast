using aoc_fast.Extensions;

namespace aoc_fast.Years._2023
{
    internal class Day5
    {
        public static string input { get; set; }
        private static List<ulong> seeds = [];
        private static List<List<ulong[]>> stages = [];

        private static void Parse()
        {
            var chunks = input.Split("\n\n", StringSplitOptions.RemoveEmptyEntries);
            seeds = chunks[0].ExtractNumbers<ulong>();
            stages = chunks[1..].Select(chunk =>
            {
                return chunk.ExtractNumbers<ulong>().Chunk(3).Select(triple => new ulong[] { triple[0], triple[1], triple[1] + triple[2] }).ToList();
            }).ToList();
        }

        public static ulong PartOne()
        {
            Parse();
            var subSeeds = seeds.ToList();
            foreach(var stage in stages)
            {
                for(var i = 0; i < subSeeds.Count; i++)
                {
                    var seed = subSeeds[i];
                    foreach(var triple in stage)
                    {
                        if (triple[1] <= seed && seed < triple[2])
                        {
                            subSeeds[i] = seed - triple[1] + triple[0];
                            break;
                        }
                    }
                }
            }
            return subSeeds.Min();
        }

        public static ulong PartTwo()
        {
            var current = new List<ulong[]>();
            var next = new List<ulong[]>();
            var nextStage = new List<ulong[]>();

            foreach(var pair in seeds.Chunk(2))
            {
                var (start, length) = (pair[0], pair[1]);
                current.Add([start, start + length]);
            }

            foreach(var stage in stages)
            {
                foreach(var triple in stage)
                {
                    var (dest, s2, e2) = (triple[0], triple[1], triple[2]);

                    while(current.PopCheck(out var pair))
                    {
                        var (s1, e1) = (pair[0], pair[1]);
                        var x1 = s1.Max(s2);
                        var x2 = e1.Min(e2);

                        if(x1 >= x2) next.Add([s1, e1]);
                        else
                        {
                            nextStage.Add([x1 - s2 + dest, x2 - s2 + dest]);

                            if(s1 < x1) next.Add([s1, x1]);
                            if(x2 < e1) next.Add([x2, e1]);
                        }
                    }
                    (current, next) = (next, current);
                }
                current.AddRange(nextStage);
                nextStage.Clear();
            }
            return current.Select(r => r[0]).Min();
        }
    }
}
