using aoc_fast.Extensions;

namespace aoc_fast.Years._2023
{
    internal class Day6
    {
        public static string input { get; set; }
        private static List<string> lines = [];
        private static string Merge(string line) => new(line.ToCharArray().Where(char.IsAsciiDigit).ToArray());

        private static UInt128 Race(string first, string second)
        {
            var times = first.ExtractNumbers<UInt128>();
            var distances = second.ExtractNumbers<UInt128>();
            var res = (UInt128)1;

            foreach(var (time, distance) in times.Zip(distances))
            {
                var root = Numerics.Sqrt(time * time - 4 * distance);
                var start = ((time - root) + 2 - 1) / 2;
                var end = (time + root) / 2;

                if (start * (time - start) > distance) start--;
                if (end * (time - end) > distance) end++;
                res *= end - start - 1;
            }
            return res;
        }

        public static UInt128 PartOne()
        {
            lines = [.. input.Split("\n", StringSplitOptions.RemoveEmptyEntries)];
            return Race(lines[0], lines[1]);
        }
        public static UInt128 PartTwo() => Race(Merge(lines[0]), Merge(lines[1]));
    }
}
