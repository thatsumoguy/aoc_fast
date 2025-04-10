using aoc_fast.Extensions;

namespace aoc_fast.Years._2023
{
    internal class Day24
    {
        public static string input { get; set; }

        private static readonly (long Start, long End) RANGE = (200_000_000_000_000, 400_000_000_000_000);

        private static List<long[]> Nums = [];

        public static uint PartOne()
        {
            Nums = input.ExtractNumbers<long>().Chunk(6).ToList();

            var res = 0u;

            foreach(var (index, i) in Nums[1..].Index())
            {
                var (a, b, _, c, d, _) = (i[0], i[1], i[2], i[3], i[4], i[5]);
                foreach(var j in Nums[..(index + 1)])
                {
                    var (e, f, _, g, h, _) = (j[0], j[1], j[2], j[3], j[4], j[5]);

                    var determinant = d * g - c * h;
                    if (determinant == 0) continue;

                    var t = (g * (f -b) - h * (e -a)) / determinant;
                    var u = (c * (f -b) - d * (e -a)) / determinant;

                    var x = a + t * c;
                    var y = b + t * d;
                    if (t >= 0 && u >= 0 && x >= RANGE.Start && x <= RANGE.End && y >= RANGE.Start && y <= RANGE.End) res++;
                }
            }
            return res;
        }
        
        public static Int128 PartTwo()
        {
            var widen = (int i) =>
            {
                var n = Nums[i].Select(n => (Int128)n).ToList();
                var (px, py, pz, vx, vy, vz) = (n[0], n[1], n[2], n[3], n[4], n[5]);
                var p = new Vector(px, py, pz);
                var v = new Vector(vx, vy, vz);
                return (p, v);
            };

            var (p0, v0) = widen(0);
            var (p1, v1) = widen(1);
            var (p2, v2) = widen(2);

            var p3 = p1.Sub(p0);
            var p4 = p2.Sub(p0);    
            var v3 = v1.Sub(v0);
            var v4 = v2.Sub(v0);

            var q = v3.Cross(p3).GCD();
            var r = v4.Cross(p4).GCD();
            var s = q.Cross(r).GCD();

            var t = (p3.Y * s.X - p3.X * s.Y) / (v3.X * s.Y - v3.Y * s.X);
            var u = (p4.Y * s.X - p4.X * s.Y) / (v4.X * s.Y - v4.Y * s.X);


            var a = p0.Add(p3).Sum();
            var b = p0.Add(p4).Sum();
            var c = v3.Sub(v4).Sum();
            return (u * a - t * b + u * t * c) / (u - t);
        }
    }
    struct Vector(Int128 x, Int128 y, Int128 z)
    {
        public Int128 X { get; set; } = x;
        public Int128 Y { get; set; } = y;
        public Int128 Z { get; set; } = z;


    }

    static class VectorExtension
    {
        public static Vector Add(this Vector a, Vector b)
        {
            var x = a.X + b.X;
            var y = a.Y + b.Y;
            var z = a.Z + b.Z;
            return new Vector(x, y, z);
        }
        public static Vector Sub(this Vector a, Vector b)
        {
            var x = a.X - b.X;
            var y = a.Y - b.Y;
            var z = a.Z - b.Z;
            return new Vector(x, y, z);
        }

        public static Vector Cross(this Vector a, Vector b)
        {
            var x = a.Y * b.Z - a.Z * b.Y;
            var y = a.Z * b.X - a.X * b.Z;
            var z = a.X * b.Y - a.Y * b.X;
            return new Vector(x, y, z);
        }

        public static Vector GCD(this Vector a)
        {
            var gcd = Numerics.gcd(Numerics.gcd(a.X, a.Y), a.Z);
            var x = a.X / gcd;
            var y = a.Y / gcd;
            var z = a.Z / gcd;
            return new Vector(x, y, z);
        }
        public static Int128 Sum(this Vector a) => a.X + a.Y + a.Z;
    }
}
