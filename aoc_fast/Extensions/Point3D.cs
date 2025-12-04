namespace aoc_fast.Extensions
{
    internal class Point3D(int x, int y, int z) : IEquatable<Point3D>, IComparable<Point3D>, ICloneable
    {
        public int X { get; set; } = x;
        public int Y { get; set; } = y;
        public int Z { get; set; } = z;

        public static Point3D Parse(int x, int y, int z) => new(x, y, z);

        public Point3D Transform(int index) => index switch
        {
            0 => new(x, y, z),
            1 => new(x, z, -y),
            2 => new(x, -z, y),
            3 => new(x, -y, -z),
            4 => new(-x, -z, -y),
            5 => new(-x, y, -z),
            6 => new(-x, -y, z),
            7 => new(-x, z, y),
            8 => new(y, z, x),
            9 => new(y, -x, z),
            10 => new(y, x, -z),
            11 => new(y, -z, -x),
            12 => new(-y, x, z),
            13 => new(-y, z, -x),
            14 => new(-y, -z, x),
            15 => new(-y, -x, -z),
            16 => new(z, x, y),
            17 => new(z, y, -x),
            18 => new(z, -y, x),
            19 => new(z, -x, -y),
            20 => new(-z, y, x),
            21 => new(-z, -x, y),
            22 => new(-z, x, -y),
            23 => new(-z, -y, -x),
        };

        public int Eucliden(Point3D other)
        {
            var delta = this - other;
            return delta.X * delta.X + delta.Y * delta.Y + delta.Z * delta.Z;
        }

        public int Manhattan(Point3D other)
        {
            var delta = this - other;
            return delta.X.Abs() + delta.Y.Abs() + delta.Z.Abs();
        }

        public static Point3D operator -(Point3D left, Point3D right) => new(left.X - right.X, left.Y - right.Y, left.Z - right.Z);
        public static Point3D operator +(Point3D left, Point3D right) => new(left.X + right.X, left.Y + right.Y, left.Z + right.Z);
        public Point3D Clone() => new(X,Y,Z);

        public int CompareTo(Point3D other)
        {
            if (other is null) return 1;

            int cmp = X.CompareTo(other.X);
            if (cmp != 0) return cmp;

            cmp = Y.CompareTo(other.Y);
            if (cmp != 0) return cmp;

            return Z.CompareTo(other.Z);
        }

        public bool Equals(Point3D other) => X == other.X && Y == other.Y && Z == other.Z;
        public override int GetHashCode() => HashCode.Combine(X, Y, Z);
        object ICloneable.Clone() => Clone();
        public override string ToString() => $"{X},{Y},{Z}";
    }
}
