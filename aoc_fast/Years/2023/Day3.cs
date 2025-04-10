using aoc_fast.Extensions;

namespace aoc_fast.Years._2023
{
    internal class Day3
    {
        public static string input { get; set; }
        private static Grid<byte> grid;
        private static Grid<int> seen;
        private static List<uint> parts = [];

        private static void Parse()
        {
            grid = Grid<byte>.Parse(input);
            seen = grid.NewWith(0);
            parts.Add(0);
            var num = 0u;
            for(var y = 0; y < grid.height; y++)
            {
                for(var x = 0; x < grid.width; x++)
                {
                    var p = new Point(x, y);
                    var b = grid[p];

                    if(b.IsAsciiDigit())
                    {
                        seen[p] = parts.Count;
                        num = 10 * num + (uint)b.SaturatingSub((byte)'0');
                    }
                    else if(num > 0)
                    {
                        parts.Add(num);
                        num = 0;
                    }
                }
                if(num > 0)
                {
                    parts.Add(num);
                    num = 0;
                }
            }
        }

        public static uint PartOne()
        {
            Parse();
            var subParts = parts.ToArray();
            var res = 0u;

            for(var y = 0; y < grid.height; ++y)
            {
                for(var x = 0; x < grid.width; ++x)
                {
                    var p = new Point(x, y);
                    var b = grid[p];
                    if(!b.IsAsciiDigit() && b != (byte)'.')
                    {
                        foreach(var next in Directions.DIAGONAL.Select(d => p + d))
                        {
                            var index = seen[next];
                            if(index != 0)
                            {
                                res += subParts[index];
                                subParts[index] = 0;
                            }
                        }
                    }
                }
            }
            return res;
        }
        public static uint PartTwo()
        {
            var res = 0u;

            for(var y = 0;y < grid.height; ++y)
            {
                for(var x = 0;x < grid.width; x++)
                {
                    var p = new Point(x, y);
                    if (grid[p] == (byte)'*')
                    {
                        var previous = 0;
                        var distinct = 0;
                        var subTotal = 1u;

                        foreach(var next in Directions.DIAGONAL.Select(d => p + d))
                        {
                            var index = seen[next];
                            if(index != 0 && index != previous)
                            {
                                previous = index;
                                distinct++;
                                subTotal *= parts[index];
                            }
                        }
                        if (distinct == 2) res += subTotal;
                    }
                }
            }
            return res;
        }
    }
}
