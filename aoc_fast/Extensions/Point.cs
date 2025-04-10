using System.Runtime.CompilerServices;

namespace aoc_fast.Extensions
{
    public static class Directions
    {
        public static readonly Point ORIGIN = new(0, 0);
        public static readonly Point UP = new(0, -1);
        public static readonly Point DOWN = new(0, 1);
        public static readonly Point LEFT = new(-1, 0);
        public static readonly Point RIGHT = new(1, 0);
        public static readonly Point[] ORTHOGONAL = [UP, DOWN, LEFT, RIGHT];
        public static readonly Point[] DIAGONAL = 
        [
            new(-1, -1), UP, new(1, -1),
            LEFT, RIGHT, new(-1, 1),
            DOWN, new(1, 1)
        ];
    }

    public struct Point(int x, int y) : IEquatable<Point>, IComparable<Point>, ICloneable
    {
        public int X { get; set; } = x;
        public int Y { get; set; } = y;

        public Point Clone()
        {
            return new Point(X, Y);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Point Clockwise()
        {
            return new Point(-Y, X);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Point CounterClockwise()
        {
            return new Point(Y, -X);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int Manhattan(Point other)
        {
            return Math.Abs(X - other.X) + Math.Abs(Y - other.Y);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Point Signum(Point other)
        {
            return new Point(Math.Sign(X - other.X), Math.Sign(Y - other.Y));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point FromByte(byte value)
        {
            return value switch
            {
                (byte)'^' or (byte)'U' => Directions.UP,
                (byte)'v' or (byte)'D' => Directions.DOWN,
                (byte)'<' or (byte)'L' => Directions.LEFT,
                (byte)'>' or (byte)'R' => Directions.RIGHT,
                _ => throw new ArgumentException("Invalid character for Point conversion.")
            };
        }

        public override bool Equals(object obj)
        {
            return obj is Point point && Equals(point);
        }

        public bool Equals(Point other)
        {
            return X == other.X && Y == other.Y;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point operator +(Point a, Point b) => new(a.X + b.X, a.Y + b.Y);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point operator +(Point a, (int x, int y) b) => new(a.X + b.x, a.Y + b.y);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point operator +((int x, int y) b, Point a) => new(a.X + b.x, a.Y + b.y);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point operator *(Point a, int k) => new(a.X * k, a.Y * k);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point operator -(Point a, Point b) => new(a.X - b.X, a.Y - b.Y);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(Point left, Point right) => left.X == right.X && left.Y == right.Y;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(Point left, Point right) => !(left == right);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string ToString() => $"{(X,Y)}";

        public int CompareTo(Point other)
        {
            return Y.CompareTo(other.Y).CompareTo(other.X);
        }

        object ICloneable.Clone()
        {
            return Clone();
        }
    }
}
