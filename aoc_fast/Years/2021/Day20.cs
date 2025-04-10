using System.Text;

namespace aoc_fast.Years._2021
{
    internal class Day20
    {
        public static string input { get; set; }
        private static (int size, byte[] algorithm, byte[] pixels) Input;

        private static void Parse()
        {
            var bits = input.Split("\n", StringSplitOptions.RemoveEmptyEntries).Select(line => Encoding.ASCII.GetBytes(line).Select(b => (byte)(b & 1)).ToArray()).ToArray();
            var size = bits.Length - 1;
            var algorithm = bits[0];
            var pixels = new byte[40000];
            foreach(var (i, row) in bits[1..].Index())
            {
                var start = (i + 50) * 200 + 50;
                var end = start + size;
                Array.Copy(row, 0, pixels, start, size);
            }
            Input = (size, algorithm, pixels);
        }

        private static int Enhance((int size, byte[] algorithm, byte[] pixels) input, int steps)
        {
            var algorithm = input.algorithm;
            var pixels = input.pixels;
            var start = 50;
            var end = 50 + input.size;
            var defaultByte = (byte)0;

            for (var _ = 0; _ < steps; _++)
            {
                var next = new byte[40000];
                for (var y = start - 1; y < end + 1; y++)
                {
                    uint Helper(int sx, int sy, int shift)
                    {
                        var res = sx < end && sy >= start && sy < end ? (uint)pixels[sy * 200 + sx] : (uint)defaultByte;
                        return res << shift;
                    }

                    var index = defaultByte == 1 ? 0b11011011u : 0b00000000u;

                    for(var x = start - 1; x < end + 1; x++)
                    {
                        index = ((index << 1) & 0b110110110u) + Helper(x + 1, y - 1, 6) + Helper(x + 1, y, 3) + Helper(x + 1, y + 1, 0);
                        next[y * 200 + x] = algorithm[index];
                    }
                }
                pixels = next;
                start--;
                end++;

                if (defaultByte == 0) defaultByte = algorithm[0];
                else defaultByte = algorithm[511];
            }

            return pixels.Count(p => p == (byte)1);
        }

        public static int PartOne()
        {
            Parse();
            return Enhance(Input, 2);
        }
        public static int PartTwo() => Enhance(Input, 50);
    }
}
