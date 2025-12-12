using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using aoc_fast.Extensions;

namespace aoc_fast.Years._2025
{
    internal class Day8
    {
        public static string input
        {
            get;
            set;
        }

        private static List<ulong[]> Boxes = [];
        private static List<(int, int, ulong)> Pairs = [];

        private static void Parse()
        {
            var boxes = input.ExtractNumbers<ulong>().Chunk(3).ToList();
            var pairs = new List<(int, int, ulong)>(boxes.Count * (boxes.Count - 1));

            foreach(var (i, v1) in boxes.Index())
            {
                for(var j = i + 1; j < boxes.Count; j++)
                {
                    var v2 = boxes[j];
                    var dx = v1[0] > v2[0] ? v1[0] - v2[0] : v2[0] - v1[0];
                    var dy = v1[1] > v2[1] ? v1[1] - v2[1] : v2[1] - v1[1];
                    var dz = v1[2] > v2[2] ? v1[2] - v2[2] : v2[2] - v1[2];
                    var distance = dx * dx + dy * dy + dz * dz;
                    pairs.Add((i, j, distance));
                }
            }

            pairs = [.. pairs.OrderBy(x => x.Item3)];
            (Boxes, Pairs) = (boxes, pairs);
        }

        private static int Union(List<Node> set, int x, int y)
        {
            x = Find(set, x);
            y = Find(set, y);

            if(x != y)
            {
                if (set[x].Size < set[y].Size) (x, y) = (y, x);
                set[y].Parent = x;
                set[x].Size += set[y].Size;
            }
            return set[x].Size;
        }

        private static int Find(List<Node> set, int x)
        {
            while (set[x].Parent != x)
            {
                var parent = set[x].Parent;
                (x, set[x].Parent) = (parent, set[parent].Parent);
            }
            return x;
        }

        public static int PartOne()
        {
            Parse();
            var (boxes, pairs) = (Boxes, Pairs);
            var nodes = Enumerable.Range(0, boxes.Count).Select(i => new Node(i, 1)).ToList();

            foreach(var (i, j, _) in pairs.Take(1000)) Union(nodes, i, j);

            nodes = [.. nodes.OrderByDescending(node => node.Size)];
            return nodes.Take(3).Select(node => node.Size).Aggregate(1, (acc, val) => acc * val);
        }
        public static ulong PartTwo()
        {
            var (boxes, pairs) = (Boxes, Pairs);
            var nodes = Enumerable.Range(0, boxes.Count).Select(i => new Node(i, 1)).ToList();

            foreach (var (i, j, _) in pairs) if (Union(nodes, i, j) == boxes.Count) return boxes[i][0] * boxes[j][0];
            throw new Exception();
        }
    }

    record Node(int parent, int size)
    {
        public int Parent { get; set; } = parent;
        public int Size { get; set; } = size;
    }
}
