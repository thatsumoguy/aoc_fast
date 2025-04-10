using System.Text;

namespace aoc_fast.Years._2016
{
    class Day6
    {
        public static string input
        {
            get;
            set;
        }

        private static List<int[]> Freqs = [];

        private static void Parse()
        {
            var width = input.Split("\n", StringSplitOptions.RemoveEmptyEntries)[0].Length;
            var freq = new List<int[]>();
            for(var i = 0; i < width; i++) freq.Add(new int[26]);

            foreach (var (b, i) in Encoding.ASCII.GetBytes(input)
                .Where(b => char.IsAsciiLetterLower((char)b))
                .Select((b, i) => (b, i))) freq[i % width][b - (byte)'a']++;

            Freqs = freq;
        }

        private static string Find(List<int[]> freq, Func<int[], (int, int)> ec) 
            => string.Join("", freq.Select(ec).Select(i => (char)((byte)(i.Item2) + (byte)'a')));

        public static string PartOne()
        {
            Parse();
            return Find(Freqs, (freq) => freq.Select((f, i) => (f, i)).Where(f => f.f > 0).MaxBy(f => f.f));
        }

        public static string PartTwo() => Find(Freqs, (freq) => freq.Select((f, i) => (f, i)).Where(f => f.f > 0).MinBy(f => f.f));
    }
}
