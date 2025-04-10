using System.Runtime.CompilerServices;
using System.Text;
using aoc_fast.Extensions;

namespace aoc_fast.Years._2022
{
    internal class Day23
    {
        public static string input { get; set; }
        private const uint HEIGHT = 210;

        struct U256(UInt128 left, UInt128 right)
        {
            public UInt128 Left { get; set; } = left;
            public UInt128 Right { get; set; } = right;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public void BitSet(int offset)
            {
                if (offset < 128) Left |= (UInt128)1 << (127 - offset);
                else Right |= (UInt128)1 << (255 - offset);
            }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public uint CountOnes() => (uint)(UInt128.PopCount(Left) + UInt128.PopCount(Right));
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public bool NonZero() => Left != 0 || Right != 0;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public uint? MinSet()
            {
                if (Left != 0) return (uint)UInt128.LeadingZeroCount(Left);
                else if(Right != 0) return (uint)UInt128.LeadingZeroCount(Right);
                else return null;
            }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public uint? MaxSet()
            {
                if (Right != 0) return (uint)(255 - UInt128.TrailingZeroCount(Right));
                else if(Left != 0) return (uint)(127  - UInt128.TrailingZeroCount(Left));
                else return null;
            }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public U256 LeftShift() => new(Left << 1 | Right >> 127, Right << 1);
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public U256 RightShift() => new(Left >> 1, Left << 127 | Right >> 1);
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static U256 operator &(U256 left, U256 right) => new(left.Left & right.Left, left.Right & right.Right);
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static U256 operator |(U256 left, U256 right) => new(left.Left | right.Left, left.Right | right.Right);
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static U256 operator ~(U256 a) => new(~a.Left, ~a.Right);
            public override string ToString() => $"{(Left, Right)}";
            
        }
        enum Direction
        {
            North,
            South,
            West,
            East
        }

        private static U256[] grid = new U256[HEIGHT];
        private static U256[] north = new U256[HEIGHT];
        private static U256[] south = new U256[HEIGHT];
        private static U256[] west = new U256[HEIGHT];
        private static U256[] east = new U256[HEIGHT];

        private static void Parse()
        {
            var offset = 70;
            var raw = input.Split("\n", StringSplitOptions.RemoveEmptyEntries).Select(Encoding.ASCII.GetBytes).ToArray();
            var defaultArray = new U256[HEIGHT];
            var subGrid = defaultArray.ToArray();

            foreach(var (y, row) in raw.Index())
            {
                foreach(var (x, col) in row.Index())
                {
                    if (col == (byte)'#') subGrid[offset + y].BitSet(offset + x);
                }
            }
            (grid, north, south, west, east) = (subGrid, defaultArray.ToArray(), defaultArray.ToArray(), defaultArray.ToArray(), defaultArray.ToArray());
        }
        private static bool Step(U256[] grid, U256[] north, U256[] south, U256[] west, U256[] east, Direction[] order)
        {

            var start = Array.FindIndex(grid, i => i.NonZero()) - 1;
            var end = Array.FindLastIndex(grid, i => i.NonZero()) + 2;
            var moved = false;
            U256 prev;

            var cur = ~(grid[0].RightShift() | grid[0] | grid[0].LeftShift());
            var next = ~(grid[1].RightShift() | grid[1] | grid[1].LeftShift());
            for (var i = start; i < end; i++)
            {
                prev = cur;
                cur = next;
                next = ~(grid[i + 1].RightShift() | grid[i + 1] | grid[i + 1].LeftShift());

                var up = prev;
                var down = next;

                var vertical = ~(grid[i - 1] | grid[i] | grid[i + 1]);
                var left = vertical.RightShift();
                var right = vertical.LeftShift();
                var remaining = grid[i] & ~(up & down & left & right);
                foreach (var direction in order)
                {
                    switch (direction)
                    {
                        case Direction.North:
                            up &= remaining;
                            remaining &= ~up;
                            break;
                        case Direction.South:
                            down &= remaining;
                            remaining &= ~down;
                            break;
                        case Direction.West:
                            left &= remaining;
                            remaining &= ~left;
                            break;
                        case Direction.East:
                            right &= remaining;
                            remaining &= ~right;
                            break;
                    }
                }
                north[i - 1] = up;
                south[i + 1] = down;
                west[i] = left.LeftShift();
                east[i] = right.RightShift();
            }

            for(var i = start; i < end; i++)
            {
                var up = north[i];
                var down = south[i];
                var left = west[i];
                var right = east[i];

                north[i] &= ~down;
                south[i] &= ~up;
                west[i] &= ~right;
                east[i] &= ~left;
            }

            for (var i = start; i < end; i++)
            {
                var same = grid[i] & ~(north[i - 1] | south[i + 1] | west[i].RightShift() | east[i].LeftShift());
                var change = north[i] | south[i] | west[i] | east[i];
                grid[i] = same | change;
                moved |= change.NonZero();
            }
            order.RotateLeft(1);
            return moved;
        }

        public static uint? PartOne()
        {
            Parse();
            Direction[] order = [Direction.North, Direction.South, Direction.West, Direction.East];
            for (var _ = 0; _ <= 10; _++) Step(grid, north, south, west, east, order);
            var elves = grid.Select(i => i.CountOnes()).Sum();
            var minX = grid.Select(i => i.MinSet()).Where(i => i != null).Min() + 1;
            var maxX = grid.Select(i => i.MaxSet()).Where(i => i != null).Max();
            var minY = (uint)Array.FindIndex(grid, i => i.NonZero());
            var maxY = (uint)Array.FindLastIndex(grid, i => i.NonZero());
            return (maxX - minX + 1u) * (maxY - minY + 1u) - elves;
        }
        public static uint? PartTwo()
        {
            Parse();
            Direction[] order = [Direction.North, Direction.South, Direction.West, Direction.East];
            var moved = true;
            var count = 0u;
            while (moved)
            {
                moved = Step(grid, north, south, west, east, order);
                count++;
            }
            return count;
        }
    }
}
