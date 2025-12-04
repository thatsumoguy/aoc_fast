using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Text;

namespace aoc_fast.Extensions
{
    public class Grid<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] T> : IEquatable<Grid<T>>
    {
        public int width { get; set; }
        public int height { get; set; }
        public T[] data { get; set; }
        
        public T this[int x, int y]
        {
            get => data[width * y + x];
            set => data[width * y + x] = value;
        }
        public T this[Point p]
        {
            get => data[width * p.Y + p.X];
            set => data[width * p.Y + p.X] = value;
        }
        public T this[Point? p]
        {
            get => data[width * p.Value.Y + p.Value.X];
            set => data[width * p.Value.Y + p.Value.X] = value;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Grid<byte> Parse(string input)
        {
            var raw = input.Split("\n", StringSplitOptions.RemoveEmptyEntries).Select(Encoding.ASCII.GetBytes).ToArray();
            var width = raw[0].Length;
            var height = raw.Length;
            return new Grid<byte> { width = width, height = height, data = [.. raw.SelectMany(r => r)] };
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public  Point? Find(T needle)
        {
            var pos = Array.IndexOf(data, needle);
            if(pos > -1) return ToPoint(pos);
            else return null;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private Point? ToPoint(int index)
        {
            var x = index % width;
            var y = index / width;
            return new Point(x, y);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Grid<T> New(int width, int height, T val)
        {
            if (typeof(T).IsValueType || typeof(T) == typeof(string))
            {
                // For value types or strings, repeat the same value
                return new Grid<T>
                {
                    width = width,
                    height = height,
                    data = Enumerable.Repeat(val, width * height).ToArray()
                };
            }
            else if (val == null || Activator.CreateInstance<T>() is not null)
            {
                // For reference types with parameterless constructors, create independent instances
                return new Grid<T>
                {
                    width = width,
                    height = height,
                    data = Slice.RepeatWith(() => Activator.CreateInstance<T>(), width * height).ToArray()
                };
            }
            else
            {
                throw new InvalidOperationException($"Type {typeof(T)} must either be a value type or have a parameterless constructor.");
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Grid<T> New(Grid<T> grid) => new() { width = grid.width, height = grid.height, data = [.. grid.data] };
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public  bool Contains(Point point) => point.X >= 0 && point.X < width && point.Y >= 0 && point.Y < height;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Contains(int x, int y) => x >= 0 && y >= 0 && x < width && y < height;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Contains(Point? point) => point.Value.X >= 0 && point.Value.X < width && point.Value.Y >= 0 && point.Value.Y < height;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(Grid<T> other)
        {
            return data.GetHashCode() == other.data.GetHashCode();
        }
    }

    public static class GridExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Grid<U?> DefaultCopy<T, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] U>(this Grid<T> g)
        {
            return new Grid<U?>
            {
                width = g.width,
                height = g.height,
                data = Enumerable.Repeat(default(U), g.width * g.height).ToArray()
            };
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Grid<U> NewWith<T, U>(this Grid<T> grid, U val)
        {
            var t = new U[grid.width * grid.height];
            Array.Fill(t, val);
            return new() { width = grid.width, height = grid.height, data = t };
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Grid<U> NewWith<T, U>(this Grid<T> grid, Func<U> valueFactory)
        {
            var data = new U[grid.width * grid.height];
            for (int i = 0; i < data.Length; i++)
            {
                data[i] = valueFactory();
            }
            return new Grid<U> { width = grid.width, height = grid.height, data = data };
        }
    }
}
