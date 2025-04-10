using aoc_fast.Extensions;

namespace aoc_fast.Years._2022
{
    internal class Day18
    {
        public static string input { get; set; }
        private const int Size = 24;
        private static uint[] Cube = [];

        private static void FloodFill(uint[] cube, int i)
        {
            if (i < cube.Length && cube[i] == 0)
            {
                cube[i] = 8;

                FloodFill(cube, i - 1 < 0 ? 0 : i - 1);
                FloodFill(cube, i + 1);
                FloodFill(cube, i - Size < 0 ? 0 : i - Size);
                FloodFill(cube, i + Size);
                FloodFill(cube, i - Size * Size < 0 ? 0 : i - Size * Size);
                FloodFill(cube, i + Size * Size);
            }
        }

        private static uint Count(uint[] cube, Func<uint, uint> adjust)
        {
            var total = 0u;

            for(var i = 0; i < cube.Length; i++)
            {
                if (cube[i] == 1)
                {
                    total += adjust(cube[i - 1] + cube[i + 1] + cube[i - Size] + cube[i + Size] + cube[i - Size * Size] + cube[i + Size * Size]);
                }
            }
            return total;
        }

        private static void Parse()
        {
            var cube = new uint[Size * Size * Size];
            var nums = input.ExtractNumbers<uint>();
            foreach(var i in nums.Chunk(3))
            {
                var (x, y, z) = (i[0], i[1], i[2]);
                cube[(x + 1) * Size * Size + (y + 1) * Size + (z + 1)] = 1;
            }
            Cube = cube;
        }

        public static uint PartOne()
        {
            Parse();
            return Count(Cube, (x) => 6 - x);
        }
        public static uint PartTwo()
        {
            var cube = Cube.ToArray();
            FloodFill(cube, 0);
            return Count(cube, (x) => x >> 3);
        }
    }
}
