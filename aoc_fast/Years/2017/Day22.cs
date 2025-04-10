using aoc_fast.Extensions;

namespace aoc_fast.Years._2017
{
    internal class Day22
    {
        public static string input
        {
            get;
            set;
        }

        private static Grid<byte> grid;

        private static ulong Simulate(Grid<byte> input, ulong bursts, ulong delta)
        {
            try
            {
                var full = 512ul;
                var half = 256ul;

                ulong[] offsets = [1, full, unchecked(0ul - 1ul), unchecked(0ul - full)];

                var g = new byte[full * full];
                for (var i = 0; i < g.Length; i++) g[i] = 1;
                
                var offset = half - (ulong)(input.width / 2);

                for (var X = 0; X < input.width; X++)
                {
                    for (var y = 0; y < input.height; y++)
                    {
                        if (input[new Point(X, y)] == (byte)'#')
                        {
                            var i = full * (offset + (ulong)y) + (offset + (ulong)X);
                            g[i] = 3;
                        }
                    }
                }
                
                var index = full * half + half;
                var dir = 3ul;
                var res = 0ul;

                for (var _ = 0ul; _ < bursts; _++)
                {
                    var cur = (ulong)g[index];
                    var next = (cur + delta) & 0x3;
                    g[index] = (byte)next;
                    res += (next + 1) >> 2;

                    dir = (dir + cur + 2) & 0x3;
                    index = unchecked(index + offsets[dir]);
                }
                return res;
            }
            catch (Exception e) { Console.WriteLine(e); }

            return 0;
        }

        public static ulong PartOne()
        {
            grid = Grid<byte>.Parse(input);
            return Simulate(grid, 10000, 2);
        }
        public static ulong PartTwo() => Simulate(grid, 10000000, 1);
    }
}
