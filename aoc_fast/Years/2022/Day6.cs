using System.Text;

namespace aoc_fast.Years._2022
{
    internal class Day6
    {
        public static string input { get; set; }

        private static int Find(string input, int marker)
        {
            var start = 0;
            var seen = new int[26];

            foreach(var (i,b) in Encoding.UTF8.GetBytes(input).Index())
            {
                var index = b - (byte)'a';
                var previous = seen[index];
                seen[index] = i + 1;
                if(previous > start) start = previous;
                if(i + 1 - start == marker) return i + 1;
            }
            throw new Exception();
        }

        public static int PartOne() => Find(input, 4);
        public static int PartTwo() => Find(input, 14);
    }
}
