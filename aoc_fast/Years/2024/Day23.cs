using System.Text;

namespace aoc_fast.Years._2024
{
    internal class Day23
    {
        public static string input
        {
            get;
            set;
        }
        private static Dictionary<int, List<int>> Nodes = new Dictionary<int, List<int>>(1000);
        private static List<bool[]> Edges = [];

        private static void Parse()
        {
            for (var i = 0; i < 676; i++)
            {
                Edges.Add(new bool[676]);
                for (var j = 0; j < 676; j++)
                    Edges[i][j] = false;
            }
            static int toInt(byte b) => b - (byte)'a';

            static int toIndex(byte[] b) => 26 * toInt(b[0]) + toInt(b[1]);
            foreach (var edge in Encoding.UTF8.GetBytes(input).Chunk(6))
            {
                var from = toIndex(edge[..2]);
                var to = toIndex(edge[3..5]);

                if (Nodes.TryGetValue(from, out var _)) Nodes[from].Add(to);
                else
                    Nodes[from] = new List<int>(16)
                    {
                        to
                    };
                if (Nodes.TryGetValue(to, out var _)) Nodes[to].Add(from);
                else
                    Nodes[to] = new List<int>(16)
                    {
                        from
                    };

                Edges[from][to] = true;
                Edges[to][from] = true;
            }
        }

        public static int PartOne()
        {
            Parse();
            var seen = Enumerable.Repeat(false, 676).ToArray();
            var triangle = 0;

            for (var n1 = 494; n1 < 520; n1++)
            {
                if (Nodes.TryGetValue(n1, out var neighbor))
                {
                    seen[n1] = true;

                    foreach (var (i, n2) in neighbor.Index())
                    {
                        foreach (var n3 in neighbor.Skip(i)) if (!seen[n2] && !seen[n3] && Edges[n2][n3]) triangle++;
                    }
                }
            }
            return triangle;
        }

        public static string PartTwo()
        {
            Parse();
            var seen = Enumerable.Repeat(false, 676).ToArray();
            var clique = new List<int>();
            var largest = new List<int>();
            static char toChar(int u) => (char)((byte)(u) + (byte)'a');

            foreach (var (n1, neighbor) in Nodes)
            {
                if (!seen[n1])
                {
                    clique.Clear();
                    clique.Add(n1);
                    foreach (var n2 in neighbor)
                    {
                        if (clique.All(c => Edges[n2][c]))
                        {
                            seen[n2] = true;
                            clique.Add(n2);
                        }
                    }
                    if (clique.Count > largest.Count) largest = new List<int>(clique);
                }
            }
            var res = new StringBuilder();
            largest.Sort();
            foreach (var n in largest)
            {
                res.Append(toChar(n / 26));
                res.Append(toChar(n % 26));
                res.Append(',');
            }
            res.Remove(res.Length - 1, 1);
            return res.ToString();
        }
    }
}
