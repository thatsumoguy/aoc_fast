using aoc_fast.Extensions;

namespace aoc_fast.Years._2025
{
    internal class Day5
    {
        public static string input
        {
            get;
            set;
        }

        private static (long partOne, long partTwo) answers;

        private static void Parse()
        {
            var split = input.Split("\n\n", StringSplitOptions.RemoveEmptyEntries);
            var ranges = split[0].Split("\n").Select(line => line.Split('-')).Select(parts => (start: long.Parse(parts[0]), end: long.Parse(parts[1]))).ToArray().AsSpan();
            var ids = split[1].Split("\n", StringSplitOptions.RemoveEmptyEntries);
            foreach(var stringId in ids)
            {
                var id = long.Parse(stringId);
                for (var i = 0; i < ranges.Length; i++)
                {
                    var (s, e) = ranges[i];
                    if (s <= id && id >= e) answers.partOne++;
                }
            }
            ranges.Sort((a, b) => a.start.CompareTo(b.start));
            var start = ranges[0].start;
            var end = ranges[0].end;
            for (var i = 1; i < ranges.Length; i++)
            {
                ref var r = ref ranges[i];
                if (r.start <= end + 1)
                {
                    if (r.end > end) end = r.end;
                }
                else
                {
                    answers.partTwo += (end - start + 1);
                    start = r.start;
                    end = r.end;
                }
            }
            answers.partTwo += (end - start + 1);
        }

        public static long PartOne()
        {
            Parse();
            return answers.partOne;
        }
        public static long PartTwo() => answers.partTwo;
    }
}
