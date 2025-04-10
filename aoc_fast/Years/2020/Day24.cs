using System.Text;

namespace aoc_fast.Years._2020
{
    internal class Day24
    {
        public static string input { get; set; }
        class Hex(int q, int r) : IEquatable<Hex>
        {
            public int Q { get; set; } = q;
            public int R { get; set; } = r;

            public bool Equals(Hex? other) => other.Q == Q && other.R == R;

            public override int GetHashCode() => HashCode.Combine(Q, R);
        }

        private static HashSet<Hex> Tiles = [];
        private static void Parse()
        {
            var tiles = new HashSet<Hex>();

            foreach (var line in input.TrimEnd().Split("\n"))
            {
                var iter = Encoding.ASCII.GetBytes(line).GetEnumerator();
                var q = 0;
                var r = 0;
                while(iter.MoveNext())
                {
                    var b = (byte)iter.Current;
                    switch(b)
                    {
                        case (byte)'e':
                            q++;
                            break;
                        case (byte)'w':
                            q--;
                            break;
                        case (byte)'n':
                            iter.MoveNext();
                            if ((byte)iter.Current == 'e') q++;
                            r--;
                            break;
                        case (byte)'s':
                            iter.MoveNext();
                            if ((byte)iter.Current != 'e') q--;
                            r++;
                            break;
                    }
                }
                var tile = new Hex(q, r);
                if (!tiles.Remove(tile)) tiles.Add(tile);
            }
            Tiles = tiles;
        }
        public static int PartOne()
        {
            Parse();
            return Tiles.Count;
        }
        public static int PartTwo()
        {
            var q1 = int.MaxValue;
            var q2 = int.MinValue;
            var r1 = int.MaxValue;
            var r2 = int.MinValue;

            foreach (var hex in Tiles)
            {
                q1 = Math.Min(q1, hex.Q);
                q2 = Math.Max(q2, hex.Q);
                r1 = Math.Min(r1, hex.R);
                r2 = Math.Max(r2, hex.R);
            }

            var width = q2 - q1 + 203;
            var height = r2 - r1 + 203;

            var neighbors = new int[] {-1, 1, -width, width, 1 - width, width - 1}.Select(i => (ulong)i).ToArray();

            var active = new List<ulong>();
            var candidates = new List<ulong>();
            var nextActive = new List<ulong>();
            foreach(var hex in Tiles)
            {
                var index = width * (hex.R - r1 + 101) + (hex.Q - q1 + 101);
                active.Add((ulong)index);
            }
            for(var _ = 0; _ < 100;  _++)
            {
                var state = new byte[width * height];

                foreach(var tile in active)
                {
                    foreach(var offset in neighbors)
                    {
                        var index = tile + offset;

                        state[index]++;
                        if (state[index] == 2) candidates.Add(index);
                    }
                }

                foreach(var tile in active)
                {
                    if (state[tile] == 1) nextActive.Add(tile);
                }

                foreach(var tile in candidates)
                {
                    if(state[tile] == 2) nextActive.Add(tile);
                }

                (active, nextActive) = (nextActive, active);
                candidates.Clear();
                nextActive.Clear();
            }

            return active.Count;
        }
    }
}
