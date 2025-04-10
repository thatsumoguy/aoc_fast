using System.Data;
using System.Text.RegularExpressions;

namespace aoc_fast.Years._2017
{
    partial class Day7
    {
        public static string input
        {
            get;
            set;
        }
        class Node()
        {
            public bool HasParent { get; set; }
            public uint Parent { get; set; }
            public uint Children { get; set; }
            public uint Processed { get; set; }
            public int weight { get; set; }
            public int total { get; set; }
            public int[] SubWeights { get; set; }
            public int[] SubTotals { get; set; }

            public static Node New()
            {
                var node = new Node
                {
                    HasParent = false,
                    Parent = 0,
                    Children = 0,
                    Processed = 0,
                    total = 0,
                    SubWeights = new int[2],
                    SubTotals = new int[2]
                };
                return node;
            }
        }

        private static (string partOne, int partTwo) answer;
        private static void Parse()
        {
            var pairs = input.Split("\n", StringSplitOptions.RemoveEmptyEntries).Select(l => l.Split(' ', 2) switch { var a => (a[0], a[1]) }).ToList();

            var indices = pairs.Select((p, i) => (p, i)).ToDictionary(p => p.p.Item1, p => p.i);

            var nodes = new Node[indices.Count];
            for (var i = 0; i < indices.Count; i++) nodes[i] = Node.New();

            var todo = new Queue<int>();

            foreach (var (pair, i) in pairs.Select((p, i) => (p, i)))
            {
                var suffix = pair.Item2;
                var iter = MyRegex().Split(suffix).Where(s => !string.IsNullOrEmpty(s)).GetEnumerator();
                iter.MoveNext();
                var weight = int.Parse(iter.Current);
                nodes[i].weight = weight;
                nodes[i].total = weight;

                while (iter.MoveNext())
                {
                    nodes[i].Children++;
                    var edge = iter.Current;
                    var child = indices[edge];
                    nodes[child].Parent = (uint)i;
                    nodes[child].HasParent = true;
                }

                if (nodes[i].Children == 0) todo.Enqueue(i);
            }

            var partOne = indices.First(i => !nodes[i.Value].HasParent).Key;
            var partTwo = 0;

            while (todo.TryDequeue(out var index))
            {
                var (parent, weight, total) = (nodes[index].Parent, nodes[index].weight, nodes[index].total);
                var node = nodes[parent];

                if (node.Processed < 2)
                {
                    node.SubWeights[node.Processed] = weight;
                    node.SubTotals[node.Processed] = total;
                }
                else
                {
                    if (node.SubTotals[0] == total)
                    {
                        (node.SubWeights[0], node.SubWeights[1]) = (node.SubWeights[1], node.SubWeights[0]);
                        (node.SubTotals[0], node.SubTotals[1]) = (node.SubTotals[1], node.SubTotals[0]);
                    }
                    else if (node.SubTotals[1] != total)
                    {
                        node.SubWeights[0] = weight;
                        node.SubTotals[0] = total;
                    }
                }
                node.total += total;
                node.Processed++;

                if (node.Processed == node.Children)
                {
                    todo.Enqueue((int)parent);

                    if (node.Children >= 3)
                    {
                        var subWeights = node.SubWeights;
                        var subTotals = node.SubTotals;
                        if (subTotals[0] != subTotals[1])
                        {
                            partTwo = subWeights[0] - subTotals[0] + subTotals[1];
                            break;
                        }
                    }
                }
            }
            answer = (partOne, partTwo);

        }

        public static string PartOne()
        {
            Parse();
            return answer.partOne;
        }

        public static int PartTwo() => answer.partTwo;
        [GeneratedRegex(@"\W+")]
        private static partial Regex MyRegex();
    }
}
