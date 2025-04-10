using aoc_fast.Extensions;

namespace aoc_fast.Years._2023
{
    internal class Day13
    {
        public static string input { get; set; }

        private static List<(List<uint>, List<uint>)> Input = [];
        private static int? ReflextAxis(List<uint> axis, uint target)
        {
            var size = axis.Count;

            for(var i = 1; i < size; i++)
            {
                var smudges = 0u;

                for (var j = 0; j < i.Min(size - i); j++) smudges += uint.PopCount(axis[i - j - 1] ^ axis[i + j]);
                if(smudges == target) return i;
            }
            return null;
        }

        private static int? Reflect(List<(List<uint>, List<uint>)> input, uint target) => input.Select(pair =>
        {
            var x = ReflextAxis(pair.Item2, target);
            if (x != null) return x;
            var y = ReflextAxis(pair.Item1, target);
            if (y != null) return 100 * y;
            else throw new Exception();
        }).Sum();

        private static void Parse()
        {
            Input = input.Split("\n\n", StringSplitOptions.RemoveEmptyEntries).Select(block =>
            {
                var grid = Grid<byte>.Parse(block);
                var rows = new List<uint>(grid.height);
                var columns = new List<uint>(grid.width);

                for(var y = 0; y < grid.height; y++)
                {
                    var n = 0u;

                    for (var x = 0; x < grid.width; x++) n = (n << 1) | (grid[x, y] == (byte)'#' ? 1u : 0u);
                    rows.Add(n);
                }

                for(var x = 0; x < grid.width; x++)
                {
                    var n = 0u;

                    for (var y = 0; y < grid.height; y++) n = (n << 1) | (grid[x, y] == (byte)'#' ? 1u : 0u);
                    columns.Add(n);
                }
                return (rows,columns);
            }).ToList();
        }

        public static int PartOne()
        {
            Parse();
            return Reflect(Input, 0).Value;
        }
        public static int PartTwo() => Reflect(Input, 1).Value;
    }
}
