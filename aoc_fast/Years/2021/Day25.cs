using System.Text;

namespace aoc_fast.Years._2021
{
    internal class Day25
    {
        public static string input { get; set; }

        struct U256(UInt128 left, UInt128 right)
        {
            public UInt128 Left { get; set; } = left;
            public UInt128 Right { get; set; } = right;

            public void BitSet(int offset)
            {
                if (offset < 128) Right |= (UInt128)1 << offset;
                else Left |= (UInt128)1 << (offset - 128);
            }
            public bool NonZero() => Left != 0 || Right != 0;
            public U256 LeftRoll(int width)
            {
                if(width < 128)
                {
                    var mask = ~((UInt128)1 << width);
                    var right = (((Right << 1) & mask) | (Right >> (width -1)));
                    return new U256(Left, right);
                }
                else
                {
                    var mask = ~((UInt128)1 << (width - 128));
                    var left = ((Left << 1) & mask) | (Right >> 127);
                    var right = (Right << 1) | (Left >> (width - 129));
                    return new U256(left, right);
                }
            }
            public U256 RightRoll(int width)
            {
                if(width < 128)
                {
                    var right = (Right >> 1) | ((Right & 1) << (width - 1));
                    return new U256(Left, right);
                }
                else
                {
                    var left = (Left >> 1) | ((Right & 1) << (width - 129));
                    var right = (Right >> 1) | ((Left & 1) << 127);
                    return new U256(left, right);
                }
            }
            public static U256 operator &(U256 left, U256 right) => new(left.Left & right.Left , left.Right & right.Right);
            public static U256 operator |(U256 left, U256 right) => new(left.Left | right.Left , left.Right | right.Right);
            public static U256 operator ~(U256 a) => new(~a.Left, ~a.Right);
        }

        private static (int width, int height, List<U256> across, List<U256> down) state = (default, default, [], []);

        private static void Parse()
        {
            var bytes = input.Split("\n", StringSplitOptions.RemoveEmptyEntries).Select(Encoding.UTF8.GetBytes).ToList();
            var width = bytes[0].Length;
            var height = bytes.Count;
            var across = new List<U256>();
            var down = new List<U256>();

            foreach(var row in bytes)
            {
                var nextAcross = new U256(default, default);
                var nextDown = new U256(default, default);

                foreach(var (offset, col) in row.Index())
                {
                    switch(col)
                    {
                        case (byte)'>':
                            nextAcross.BitSet(offset);
                            break;
                        case (byte)'v':
                            nextDown.BitSet(offset);
                            break;
                    }
                }
                across.Add(nextAcross);
                down.Add(nextDown);
            }
            state = (width, height, across, down);
        }

        public static int PartOne()
        {
            Parse();
            var (width, height, across, down) = (state.width, state.height, new List<U256>(state.across), new List<U256>(state.down));

            var changed = true;
            var count = 0;

            while(changed)
            {
                changed = false;
                count++;

                for(var i = 0; i < height ; i++)
                {
                    var canidates = across[i].LeftRoll(width);
                    var move = canidates & ~(across[i] | down[i]);
                    changed |= move.NonZero();
                    var stay = across[i] & ~move.RightRoll(width);
                    across[i] = move | stay;
                }

                var lastMask = across[0] | down[0];
                var moved = down[height - 1] & ~lastMask;

                for(var i = 0;i < height - 1 ; i++)
                {
                    changed |= moved.NonZero();
                    var mask = across[i + 1] | down[i + 1];
                    var stay = down[i] & mask;
                    var nextMoved = down[i] & ~mask;
                    down[i] = moved | stay;
                    moved = nextMoved;
                }
                changed |= moved.NonZero();
                var stayed = down[height - 1] & lastMask;
                down[height - 1] = moved | stayed;
            }
            return count;
        }
        public static string PartTwo() => "Merry Christmas";
    }
}
