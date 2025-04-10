using System.Text;
using aoc_fast.Extensions;

namespace aoc_fast.Years._2021
{
    internal class Day18
    {
        public static string input { get; set; }
        private static readonly int[] INORDER = [1, 3, 7, 15, 16, 8, 17, 18, 4, 9, 19, 20, 10, 21, 22, 2, 5, 11, 23, 24, 12, 25, 26, 6, 13, 27,28, 14, 29, 30,];

        private static List<int[]> Snailfish = [];

        private static void Explode(int[] tree, int pair)
        {
            if(pair > 31)
            {
                var i = pair - 1;
                while(true)
                {
                    if (tree[i] >= 0)
                    {
                        tree[i] += tree[pair];
                        break;
                    }
                    i = (i - 1) / 2;
                }
            }

            if(pair < 61)
            {
                var i = pair + 2;
                while(true)
                {
                    if(tree[i] >= 0)
                    {
                        tree[i] += tree[pair + 1];
                        break;
                    }
                    i = (i - 1) / 2;
                }
            }

            tree[pair] = -1;
            tree[pair + 1] = -1;
            tree[(pair - 1) / 2] = 0;
        }
        private static bool Split(int[] tree)
        {
            foreach(var i in INORDER)
            {
                if (tree[i] >= 10)
                {
                    tree[2 * i + 1] = tree[i] / 2;
                    tree[2 * i + 2] = (tree[i] + 1) / 2;
                    tree[i] = -1;
                    if (i >= 15) Explode(tree, 2 * i + 1);
                    return true;
                }
            }
            return false;
        }
        private static int Magnitude(int[] tree)
        {
            for(var i = 30; i >= 0; i--)
            {
                if (tree[i] == -1) tree[i] = 3 * tree[2 * i + 1] + 2 * tree[2 * i + 2];
            }
            return tree[0];
        }

        private static int[] Add(int[] left, int[] right)
        {
            var tree = new int[63];
            Array.Fill(tree, -1);
            left.AsSpan(1, 2).CopyTo(tree.AsSpan(3, 2));
            left.AsSpan(3, 4).CopyTo(tree.AsSpan(7, 4));
            left.AsSpan(7, 8).CopyTo(tree.AsSpan(15, 8));
            left.AsSpan(15, 16).CopyTo(tree.AsSpan(31, 16));

            right.AsSpan(1, 2).CopyTo(tree.AsSpan(5, 2));
            right.AsSpan(3, 4).CopyTo(tree.AsSpan(11, 4));
            right.AsSpan(7, 8).CopyTo(tree.AsSpan(23, 8));
            right.AsSpan(15, 16).CopyTo(tree.AsSpan(47, 16));
            for(var pair = 31; pair < 63; pair += 2)
            {
                if (tree[pair] >= 0) Explode(tree, pair);
            }

            while (Split(tree)) { }
            return tree;
        }

        private static void Parse()
        {
            var helper = (byte[] bytes) =>
            {
                var tree = new int[63];
                Array.Fill(tree, -1);
                var i = 0;

                foreach (var b in bytes)
                {
                    switch ((char)b)
                    {
                        case '[':
                            i = 2 * i + 1;
                            break;
                        case ',':
                            i++;
                            break;
                        case ']':
                            i = (i - 1) / 2;
                            break;
                        default:
                            tree[i] = (byte)b - 48;
                            break;
                    }
                }
                return tree;
            };
            Snailfish = input.TrimEnd().Split("\n").Select(line => helper(Encoding.UTF8.GetBytes(line))).ToList();
        }
        private static int shared = 0;
        private static Object mutex = new();
        private static void Worker(List<(int[], int[])> iter)
        {
            var partial = 0;
            foreach(var (a,b) in iter) partial = partial.Max(Magnitude(Add(a,b)));
            lock(mutex)
            {
                shared = shared.Max(partial);
            }

        }

        public static int PartOne()
        {
            Parse();
            var sum = Snailfish.Aggregate((a, b) => Add(a, b));
            return Magnitude(sum);
        }

        public static int PartTwo()
        {
            var pairs = new List<(int[], int[])>();
            foreach(var (i, a) in Snailfish.Index())
            {
                foreach(var (j, b) in Snailfish.Index())
                {
                    if(i != j) pairs.Add((a, b));
                }
            }
            Threads.SpawnBatches(pairs, (iter) => Worker(iter));
            return shared;
        }
    }
}
