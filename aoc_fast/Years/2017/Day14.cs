using System.Text;
using aoc_fast.Extensions;

namespace aoc_fast.Years._2017
{
    internal class Day14
    {
        public static string input { get; set; }
        private static byte[] grid = new byte[0x4000];

        private static void Parse()
        {
            var prefix = input.Trim();
            grid = new byte[0x4000];

            Parallel.For(0, 128, index =>
            {
                var row = FillRow(prefix, index);
                Array.Copy(row, 0, grid, index * 128, row.Length);
            });
        }

        private static byte[] FillRow(string prefix, int index)
        {
            var s = $"{prefix}-{index}";
            var lengths = Encoding.ASCII.GetBytes(s).Select(b => (uint)b).ToList();
            lengths.AddRange([17, 31, 73, 47, 23]);

            var knot = KnotHash(lengths);
            var res = new byte[128];

            for (var i = 0; i < 16; i++)
            {
                byte reduced = (byte)(knot[i * 16] ^ knot[i * 16 + 1] ^ knot[i * 16 + 2] ^ knot[i * 16 + 3] ^
                                      knot[i * 16 + 4] ^ knot[i * 16 + 5] ^ knot[i * 16 + 6] ^ knot[i * 16 + 7] ^
                                      knot[i * 16 + 8] ^ knot[i * 16 + 9] ^ knot[i * 16 + 10] ^ knot[i * 16 + 11] ^
                                      knot[i * 16 + 12] ^ knot[i * 16 + 13] ^ knot[i * 16 + 14] ^ knot[i * 16 + 15]);

                for (var j = 0; j < 8; j++)
                {
                    res[i * 8 + j] = (byte)((reduced >> (7 - j)) & 1);
                }
            }

            return res;
        }

        private static byte[] KnotHash(List<uint> lengths)
        {
            byte[] knot = new byte[256];
            for (var i = 0; i < 256; i++) knot[i] = (byte)i;

            int pos = 0, skip = 0;

            for (var round = 0; round < 64; round++)
            {
                foreach (var length in lengths)
                {
                    ReverseSection(knot, pos, (int)length);
                    pos = (pos + (int)length + skip) % 256;
                    skip++;
                }
            }

            return knot;
        }

        private static void ReverseSection(byte[] knot, int pos, int length)
        {
            var end = (pos + length - 1) % 256;
            while (length > 1)
            {
                (knot[pos], knot[end]) = (knot[end], knot[pos]);
                pos = (pos + 1) % 256;
                end = (end - 1 + 256) % 256;
                length -= 2;
            }
        }

        private static int DFS(bool[] grid, int start)
        {
            var stack = new Stack<int>();
            stack.Push(start);
            grid[start] = false;
            var count = 0;

            while (stack.Count > 0)
            {
                var index = stack.Pop();
                count++;

                var x = index % 128;
                var y = index / 128;

                if (x > 0 && grid[index - 1]) { grid[index - 1] = false; stack.Push(index - 1); }
                if (x < 127 && grid[index + 1]) { grid[index + 1] = false; stack.Push(index + 1); }
                if (y > 0 && grid[index - 128]) { grid[index - 128] = false; stack.Push(index - 128); }
                if (y < 127 && grid[index + 128]) { grid[index + 128] = false; stack.Push(index + 128); }
            }

            return count;
        }

        public static int PartOne()
        {
            Parse();
            return grid.Sum(n => (int)n);
        }

        public static int PartTwo()
        {
            var partTwoGrid = grid.Select(n => n == 1).ToArray();
            int groups = 0;

            for (int i = 0; i < 0x4000; i++)
            {
                if (partTwoGrid[i])
                {
                    groups++;
                    DFS(partTwoGrid, i);
                }
            }

            return groups;
        }
    }
}


