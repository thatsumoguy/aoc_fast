using aoc_fast.Extensions;

namespace aoc_fast.Years._2020
{
    internal class Day17
    {
        public static string input { get; set; }
        private const int SIZEX = 22;
        private const int SIZEY = 22;
        private const int SIZEZ = 15;
        private const int SIZEW = 15;
        private const int STRIDEX = 1;
        private const int STRIDEY = SIZEX * STRIDEX;
        private const int STRIDEZ = SIZEY * STRIDEY;
        private const int STRIDEW = SIZEZ * STRIDEZ;

        private static Grid<byte> grid;

        private static int BootProcess(Grid<byte> input, int size, int baseNum, int[] fourthDimension)
        {
            int[] dimension = [-1, 0, 1];
            var neighbors = new List<uint>();

            foreach (var X in dimension)
            {
                foreach (var y in dimension)
                {
                    foreach (var z in dimension)
                    {
                        foreach (var w in fourthDimension)
                        {
                            var offset = X * STRIDEX + y * STRIDEY + z * STRIDEZ + w * STRIDEW;
                            if (offset != 0) neighbors.Add((uint)offset);
                        }
                    }
                }
            }

            var active = new List<uint>(5000);
            var candidate = new List<uint>(5000);
            var nextActive = new List<uint>(5000);
            for (var X = 0; X < input.width; X++)
            {
                for (var y = 0; y < input.height; y++)
                {
                    if (input[X, y] == '#')
                    {
                        var index = 7 * baseNum + X + y * STRIDEY;
                        active.Add((uint)index);
                    }
                }
            }
            for (var _ = 0; _ < 6; _++)
            {
                var state = new byte[size];

                foreach (var cube in active)
                {
                    foreach (var offset in neighbors)
                    {
                        var index = cube + offset;
                        state[index]++;
                        if (state[index] == 3) candidate.Add(index);
                    }
                }
                foreach (var cube in active)
                {
                    if (state[cube] == 2) nextActive.Add(cube);
                }
                foreach (var cube in candidate)
                {
                    if (state[cube] == 3) nextActive.Add(cube);
                }

                (active, nextActive) = (nextActive, active);
                candidate.Clear();
                nextActive.Clear();
            }

            return active.Count;
        }

        public static int PartOne()
        {
            grid = Grid<byte>.Parse(input);
            var size = SIZEX * SIZEY * SIZEZ;
            var baseNum = STRIDEX + STRIDEY + STRIDEZ;
            return BootProcess(grid, size, baseNum, [0]);
        }
        public static int PartTwo()
        {
            var size = SIZEX * SIZEY * SIZEZ * SIZEW;
            var baseNum = STRIDEX + STRIDEY + STRIDEZ + STRIDEW;
            return BootProcess(grid, size, baseNum, [-1, 0, 1]);
        }
    }
}
