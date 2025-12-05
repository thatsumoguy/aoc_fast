using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            var ranges = split[0].Split("\n").Select(line => line.Split('-')).Select(parts => (start: long.Parse(parts[0]), end: long.Parse(parts[1]))).ToList();
            answers.partOne = split[1].Split("\n", StringSplitOptions.RemoveEmptyEntries).Select(freshIds =>
            {
                var id = long.Parse(freshIds);
                foreach(var (start,end) in ranges) if (id >= start && id <= end) return 1L;
                return 0L;
            }).Sum();

            ranges = [.. ranges.OrderBy(r => r.start)];

            var merged = new List<(long start, long end)>();
            var start = ranges[0].start;
            var end = ranges[0].end;

            foreach(var (s,e) in ranges)
            {
                if (s <= end + 1) end = end.Max(e);
                else
                {
                    merged.Add((start, end));
                    (start,end) = (s,e);
                }
            }
            merged.Add((start, end));

            answers.partTwo = merged.Sum(r => r.end - r.start + 1);
        }

        public static long PartOne()
        {
            Parse();
            return answers.partOne;
        }
        public static long PartTwo() => answers.partTwo;
    }
}
