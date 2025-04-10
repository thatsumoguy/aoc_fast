using System.Text;

namespace aoc_fast.Years._2024
{
    internal class Day19
    {
        public static string input
        {
            get;
            set;
        }
        class Node
        {
            public long[] Next { get; set; }
            public static Node New() => new() { Next = new long[6] };

            public void SetTowel() => Next[3] = 1;
            public long Towels() => Next[3];
        }

        private static void Parse()
        {
            var split = input.Split("\n\n", StringSplitOptions.RemoveEmptyEntries);

            var trie = new List<Node>
            {
                Node.New()
            };

            foreach (var towel in split[0].Split(", "))
            {
                var i = 0L;

                foreach (var j in Encoding.UTF8.GetBytes(towel).Select(PerfectHash))
                {
                    if (trie[(int)i].Next[j] == 0)
                    {
                        trie[(int)i].Next[j] = trie.Count;
                        i = trie.Count;
                        trie.Add(Node.New());
                    }
                    else i = trie[(int)i].Next[j];
                }
                trie[(int)i].SetTowel();
            }

            var partOne = 0L;
            var partTwo = 0L;
            var ways = new long[100];

            foreach (var design in split[1].Split("\n").Select(Encoding.UTF8.GetBytes))
            {
                var size = design.Length;
                Array.Clear(ways);
                Array.Resize(ref ways, size + 1);
                Array.Fill(ways, 0);

                ways[0] = 1;

                for (var start = 0; start < size; start++)
                {
                    if (ways[start] > 0)
                    {
                        var i = 0L;
                        for (var end = start; end < size; end++)
                        {
                            i = trie[(int)i].Next[PerfectHash(design[end])];
                            if (i == 0) break;
                            ways[end + 1] += trie[(int)i].Towels() * ways[start];
                        }
                    }
                }

                var total = ways[size];
                partOne += total > 0 ? 1 : 0;
                partTwo += total;
            }
            answers = (partOne, partTwo);
        }

        private static (long partOne, long partTwo) answers;

        private static int PerfectHash(byte b) => ((int)b ^ ((int)b >> 4)) % 8;

        public static long PartOne()
        {
            Parse();
            return answers.partOne - 1;
        }
        public static long PartTwo() => answers.partTwo - 1;
    }
}
