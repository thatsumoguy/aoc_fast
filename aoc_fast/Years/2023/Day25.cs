using System.Text;

namespace aoc_fast.Years._2023
{
    internal class Day25
    {
        public static string input { get; set; }

        private static Input inputObj;

        class Input(List<int> edges, List<(int, int)> nodes)
        {
            public List<int> Edges { get; set; } = edges;
            public List<(int, int)> Nodes { get; set; } = nodes;

            public IEnumerable<(int, int)> Neighbors(int node)
            {
                var (start, end) = Nodes[node];
                return Enumerable.Range(start, (end - start)).Select(edge => (edge, Edges[edge]));
            }
        }

        private static int PerfectMinimalHash(int[] lookup, List<List<int>> nodes, byte[] slice)
        {
            var hash = slice[..3].Aggregate(0, (acc, b) => 26 * acc + ((b - (byte)'a')));
            var index = lookup[hash];

            if(index == int.MaxValue)
            {
                index = nodes.Count;
                lookup[hash] = index;
                nodes.Add(new List<int>(10));
            }
            return index;
        }

        private static int Furthest(Input input, int start)
        {
            var todo = new Queue<int>();
            todo.Enqueue(start);

            var seen = new bool[input.Nodes.Count];
            seen[start] = true;

            var res = start;

            while(todo.TryDequeue(out var curr))
            {
                res = curr;

                foreach(var (_, next) in input.Neighbors(curr))
                {
                    if (!seen[next])
                    {
                        todo.Enqueue(next);
                        seen[next] = true;
                    }
                }
            }
            return res;
        }

        private static int Flow(Input input, int start, int end)
        {
            var todo = new Queue<(int, int)>();
            var path = new List<(int, int)>();

            var used = new bool[input.Edges.Count];
            var res = 0;

            for(var _ = 0; _ < 4;  _++)
            {
                todo.Enqueue((start, int.MaxValue));
                res = 0;

                var seen = new bool[input.Nodes.Count];
                seen[start] = true;

                while(todo.TryDequeue(out var curr))
                {
                    var (current, head) = curr;

                    res++;
                    if(current == end)
                    {
                        var index = head;
                        while(index != int.MaxValue)
                        {
                            var (edge, next) = path[index];
                            used[edge] = true;
                            index = next;
                        }
                        break;
                    }

                    foreach(var (edge, next) in input.Neighbors(current))
                    {
                        if (!used[edge] && !seen[next])
                        {
                            seen[next] = true;
                            todo.Enqueue((next, path.Count));
                            path.Add((edge, head));
                        }
                    }
                }

                todo.Clear();
                path.Clear();
            }
            return res;
        }

        private static void Parse()
        {
            var lookup = Enumerable.Repeat(int.MaxValue, 26 * 26* 26).ToArray();
            var neighbors = new List<List<int>>(2000);

            foreach(var line in input.Split("\n", StringSplitOptions.RemoveEmptyEntries).Select(Encoding.ASCII.GetBytes))
            {
                var first = PerfectMinimalHash(lookup, neighbors, line);

                foreach(var chunk in line[5..].Chunk(4))
                {
                    var second = PerfectMinimalHash(lookup, neighbors, chunk);

                    neighbors[first].Add(second);
                    neighbors[second].Add(first);
                }
            }

            var edges = new List<int>(5000);
            var nodes = new List<(int, int)>(neighbors.Count);

            foreach(var list in neighbors)
            {
                var start = edges.Count;
                var end = edges.Count + list.Count;
                edges.AddRange(list);
                nodes.Add((start, end));
            }
            inputObj = new Input(edges, nodes);
        }

        public static int PartOne()
        {
            try
            {
                Parse();
                var start = Furthest(inputObj, 0);
                var end = Furthest(inputObj, start);
                var size = Flow(inputObj, start, end);
                return size * (inputObj.Nodes.Count - size);
            }
            catch (Exception ex) { Console.WriteLine(ex); }
            return 0;
        }
        public static string PartTwo() => "Merry Christmas";
    }
}
