using aoc_fast.Extensions;

namespace aoc_fast.Years._2018
{
    internal class Day23
    {
        public static string input { get; set; }


        class NanoBot(int X, int y, int z, int r)
        {
            public int X { get; set; } = X;
            public int Y { get; set; } = y;
            public int Z { get; set; } = z;
            public int R { get; set; } = r;

            public static NanoBot From(int[] a) => new(a[0], a[1], a[2], a[3]);
            public int Manhattan(NanoBot other) => Math.Abs(other.X - X) + Math.Abs(other.Y - Y) + Math.Abs(other.Z - Z);
        }

        class Cube(int x1, int x2, int y1, int y2, int z1, int z2)
        {
            public int X1 { get; set; } = x1;
            public int X2 { get; set; } = x2;
            public int Y1 { get; set; } = y1;
            public int Y2 { get; set; } = y2;
            public int Z1 { get; set; } = z1;
            public int Z2 { get; set; } = z2;

            public static Cube New(int x1, int x2, int y1, int y2, int z1, int z2) => new(x1, x2, y1, y2, z1, z2);

            public Cube[] Split()
            {
                var lx = (X1 + X2) / 2;
                var ly = (Y1 + Y2) / 2;
                var lz = (Z1 + Z2) / 2;
                var ux = lx + 1;
                var uy = ly + 1;
                var uz = lz + 1;
                return [Cube.New(x1, lx, y1, ly, z1, lz), Cube.New(ux, x2, y1, ly, z1, lz), Cube.New(x1, lx, uy, y2, z1, lz), Cube.New(ux, x2, uy, y2, z1, lz),
                Cube.New(x1, lx, y1, ly, uz, z2), Cube.New(ux, x2, y1, ly, uz, z2), Cube.New(x1, lx, uy, y2, uz, z2), Cube.New(ux, x2, uy, y2, uz, z2)];
            }

            public bool InRange(NanoBot nb)
            {
                var X = Math.Max(X1 - nb.X, 0) + Math.Max(nb.X - X2, 0);
                var y = Math.Max(Y1 - nb.Y, 0) + Math.Max(nb.Y - Y2, 0);
                var z = Math.Max(Z1 - nb.Z, 0) + Math.Max(nb.Z - Z2, 0);
                return X + y + z <= nb.R;
            }

            public int Closest()
            {
                var X = Math.Min(Math.Abs(X1), Math.Abs(X2));
                var y = Math.Min(Math.Abs(Y1), Math.Abs(Y2));
                var z = Math.Min(Math.Abs(Z1), Math.Abs(Z2));
                return X + y + z;
            }

            public int Size() => X2 - X1 + 1;
        }

        private static List<NanoBot> bots = [];

        private static void Parse() => bots = input.ExtractNumbers<int>().Chunk(4).Select(NanoBot.From).ToList();

        public static int PartOne()
        {
            Parse();
            var strongest = bots.MaxBy((nb) => nb.R);
            return bots.Where(nb => strongest.Manhattan(nb) <= strongest.R).Count();
        }
        public static int PartTwo()
        {
            const int SIZE = 1 << 29;
            var heap = new PriorityQueue<Cube, (int, int , int)>(1000);
            heap.Enqueue(Cube.New(-SIZE, SIZE - 1, -SIZE, SIZE - 1, -SIZE, SIZE - 1), (0, 0, 0));
            while(heap.TryDequeue(out var cube, out var p))
            {
                if (cube.Size() == 1) return cube.Closest();
                foreach(var next in cube.Split())
                {
                    var inRange = bots.Where(nb => next.InRange(nb)).Count();
                    var key = (bots.Count - inRange, next.Closest(), next.Size());
                    heap.Enqueue(next, key);
                }
            }

            throw new InvalidOperationException("Unreachable code");
        }
    }
}
