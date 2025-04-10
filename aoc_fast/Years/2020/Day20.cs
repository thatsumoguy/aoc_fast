using System.Text;
using aoc_fast.Extensions;

namespace aoc_fast.Years._2020
{
    internal class Day20
    {
        public static string input { get; set; }

        class Tile(ulong id, ulong[] top, ulong[] left, ulong[] bottom, ulong[] right, byte[][] pixels)
        {
            public ulong Id { get; set; } = id;
            public ulong[] Top { get; set; } = top;
            public ulong[] Left { get; set; } = left;
            public ulong[] Right { get; set; } = right;
            public ulong[] Bottom { get; set; } = bottom;
            public byte[][] Pixels { get; set; } = pixels;
            private readonly int[][] COEFFICIENTS = 
            [
                [1, 0, 1, 0, 1, 1],
                [-1, 0, 8, 0, 1, 1],
                [1, 0, 1, 0, -1, 8],
                [-1, 0, 8, 0, -1, 8],
                [0, 1, 1, -1, 0, 8],
                [0, 1, 1, 1, 0, 1],
                [0, -1, 8, -1, 0, 8],
                [0, -1, 8, 1, 0, 1],
            ];
            public static Tile From(string[] chunk)
            {
                var id = ulong.Parse(chunk[0][5..9]);

                var pixels = new byte[10][];
                for (var i = 0; i < 10; i++) pixels[i] = Encoding.UTF8.GetBytes(chunk[i + 1]);

                var binary = (int row, int col) => (ulong)( pixels[row][col] & 1);
                var t = 0ul;
                var l = 0ul;
                var b = 0ul;
                var r = 0ul;

                for(var i = 0; i < 10; i++)
                {
                    t = (t << 1) | binary(0, i);
                    l = (l << 1) | binary(i, 0);
                    b = (b << 1) | binary(9, i);
                    r = (r << 1) | binary(i, 9);
                }

                var reverse = (ulong edge) => edge.ReverseElementBits() >> 54;
                var rt = reverse(t);
                var rl = reverse(l);
                var rb = reverse(b);
                var rr = reverse(r);

                ulong[] top = [t, rt, b, rb, rl, l, rr, r];
                ulong[] left = [l, r, rl, rr, b, t, rb, rt];
                ulong[] bottom = [b, rb, t, rt, rr, r, rl, l];
                ulong[] right = [r, l, rr, rl, t, b, rt, rb];
                return new Tile(id, top, left, bottom, right, pixels);
            }
            public void Transform(Span<UInt128> image, int permutation)
            {
                var coeff = COEFFICIENTS[permutation];
                var (a, b, c, d, e, f) = (coeff[0], coeff[1], coeff[2], coeff[3], coeff[4], coeff[5]);

                for (var row = 0; row < 8; row++)
                {
                    var acc = default(byte);
                    for(var col = 0; col < 8; col++)
                    {
                        var X = a * col + b * row + c;
                        var y = d * col + e * row + f;
                        var bit = Pixels[y][X];
                        acc = (byte)((acc << 1) | (bit & 1));
                    }
                    image[row] = (image[row] << 8) | (UInt128)(acc);
                }
            }
        }
        private static List<Tile> tiles = [];
        private static void Parse()
        {
            var lines = input.TrimEnd().Split("\n");
            tiles = lines.Chunk(12).Select(Tile.From).ToList();
        }
        public static ulong PartOne()
        {
            Parse();
            var freq = new int[1024];
            var res = 1ul;

            foreach(var tile in tiles)
            {
                foreach (var edge in tile.Top) freq[edge]++;
            }

            foreach(var tile in tiles)
            {
                var total = freq[tile.Top[0]] + freq[tile.Left[0]] + freq[tile.Bottom[0]] + freq[tile.Right[0]];
                if(total == 6) res *= tile.Id;
            }
            return res;
        }

        public static uint PartTwo()
        {
            var edgeToTile = new int[1024][];
            for (var i = 0; i < edgeToTile.Length; i++) edgeToTile[i] = new int[2];

            var freq = new int[1024];
            var placed = new bool[1024];

            foreach (var (i, tile) in tiles.Index())
            {
                foreach (var edge in tile.Top)
                {
                    edgeToTile[edge][freq[edge]] = i;
                    freq[edge]++;
                }
            }

            var FindArbitaryCorner = () =>
            {
                foreach (var tile in tiles)
                {
                    for (var j = 0; j < 8; j++)
                    {
                        if (freq[tile.Top[j]] == 1 && freq[tile.Left[j]] == 1)
                        {
                            freq[tile.Top[j]]++;
                            return tile.Top[j];
                        }
                    }
                }
                throw new Exception();
            };

            var findMatchingTile = (ulong edge) =>
            {

                var edgeParts = edgeToTile[edge];
                var (first, second) = (edgeParts[0], edgeParts[1]);
                var next = placed[first] ? second : first;
                placed[next] = true;
                return tiles[next];

            };

            var nextTop = FindArbitaryCorner();
            var image = new UInt128[96];
            var index = 0;
            while (freq[nextTop] == 2)
            {
                var tile = findMatchingTile(nextTop);
                var permutation = Enumerable.Range(0, 8).ToList().FindIndex(X => tile.Top[X] == nextTop);
                tile.Transform(image.AsSpan()[index..], permutation);
                nextTop = tile.Bottom[permutation];

                var nextLeft = tile.Right[permutation];
                while (freq[nextLeft] == 2)
                {
                    var subTile = findMatchingTile(nextLeft);
                    var subPerm = Enumerable.Range(0, 8).ToList().FindIndex(X => subTile.Left[X] == nextLeft);
                    subTile.Transform(image.AsSpan()[index..], subPerm);
                    nextLeft = subTile.Right[subPerm];
                }
                index += 8;
            }
            var sea = image.Select(n => (uint)UInt128.PopCount(n)).Sum();
            var find = (UInt128[] monster, int width, int height) =>
            {
                var rough = sea;
                for (var _ = 0; _ < (96 - width + 1); _++)
                {
                    foreach (var window in image.Windows(height))
                    {
                        if (monster.Index().All(n => (n.Item & window[n.Index]) == n.Item)) rough -= 15;
                    }
                    for (var i = 0; i < monster.Length; i++) monster[i] <<= 1;
                }
                return rough < sea ? rough : 0;
            };

            UInt128[][] monsters =
            [
                [0b00000000000000000010, 0b10000110000110000111, 0b01001001001001001000],
                [0b01001001001001001000, 0b10000110000110000111, 0b00000000000000000010],
                [0b01000000000000000000, 0b11100001100001100001, 0b00010010010010010010],
                [0b00010010010010010010, 0b11100001100001100001, 0b01000000000000000000],
            ];

            for (var i = 0; i < monsters.Length; i++)
            {
                var rough = find(monsters[i], 20, 3);
                if (rough != 0) return rough;
            }
            monsters =
            [
               [2, 4, 0, 0, 4, 2, 2, 4, 0, 0, 4, 2, 2, 4, 0, 0, 4, 2, 3, 2],
               [2, 3, 2, 4, 0, 0, 4, 2, 2, 4, 0, 0, 4, 2, 2, 4, 0, 0, 4, 2],
               [2, 1, 0, 0, 1, 2, 2, 1, 0, 0, 1, 2, 2, 1, 0, 0, 1, 2, 6, 2],
               [2, 6, 2, 1, 0, 0, 1, 2, 2, 1, 0, 0, 1, 2, 2, 1, 0, 0, 1, 2],
            ];
            for (var i = 0; i < monsters.Length; i++)
            {
                var rough = find(monsters[i], 3, 20);
                if (rough != 0) return rough;
            }
            throw new Exception();
        }
    }
}
