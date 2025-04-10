using System.Text;

namespace aoc_fast.Years._2015
{
    class Day18
    {
        public static string input
        {
            get;
            set;
        }

        private static ulong[][] Lights = [];
        private static ulong GameofLife(ulong[][] lights, bool partTwo)
        {
            var grid = Lights.Select(row => row.ToArray()).ToArray();
            var temp = new ulong[100][];
            var next = new ulong[100][];
            for(var y = 0; y < 100; y++)
            {
                temp[y] = new ulong[7];
                next[y] = new ulong[7];
            }

            for (var _ = 0; _ < 100; _++)
            {
                for (var y = 0; y < 100; y++)
                {
                    for (var X = 0; X < 7; X++)
                    {
                        var sum = grid[y][X] + (grid[y][X] >> 4) + (grid[y][X] << 4);

                        if (X > 0) sum += grid[y][X - 1] << 60;
                        if (X < 6) sum += grid[y][X + 1] >> 60;

                        temp[y][X] = sum;
                    }
                }

                for (var y = 0; y < 100; y++)
                {
                    for (var X = 0; X < 7; X++)
                    {
                        var sum = temp[y][X] - grid[y][X];
                        if (y > 0) sum += temp[y - 1][X];
                        if (y < 99) sum += temp[y + 1][X];

                        var a = sum >> 3;
                        var b = sum >> 2;
                        var c = sum >> 1;
                        var d = sum  | grid[y][X];

                        next[y][X] = (~a & ~b & c & d) & 0x1111111111111111;
                    }

                    next[y][6] &= 0x1111000000000000;
                }

                if(partTwo)
                {
                    next[0][0] |= 1ul << 60;
                    next[0][6] |= 1ul << 48;
                    next[99][0] |= 1ul << 60;
                    next[99][6] |= 1ul << 48;
                }

                (grid, next) = (next, grid);
            }

            return grid.Select(row => row.Select(n => UInt64.PopCount(n)).Aggregate((a, c) => a + c)).Aggregate((a, c) => a + c);
        }

        private static void Parse()
        {
            var grid = new ulong[100][];
            for (var y = 0; y < 100; y++) grid[y] = new ulong[7];

            foreach(var (row, y) in input.Split("\n",StringSplitOptions.RemoveEmptyEntries).Select((s, i) => (s,i)))
            {
                foreach(var (col, X) in Encoding.ASCII.GetBytes(row).Select((b, i) => (b,i)))
                {
                    var index = X / 16;
                    var offset = 4 * (15 - (X % 16));
                    var bit = (ulong)(col & 1);
                    grid[y][index] |= bit << offset;
                }    
            }

            Lights = grid;
        }

        public static ulong PartOne()
        {
            Parse();
            return GameofLife(Lights, false);
        }

        public static ulong PartTwo() => GameofLife(Lights, true);
    }
}
