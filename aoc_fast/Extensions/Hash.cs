using System.Numerics;

namespace aoc_fast.Extensions
{
    public static class Hash
    {
        public class FastSet<T> : HashSet<T>
        {
            public FastSet() : base(new FxHasher<T>()) { }
            public FastSet(int capacity) : base(capacity, new FxHasher<T>()) { }
        }

        public class FastMap<K, V> : Dictionary<K, V>
        {
            public FastMap() : base(new FxHasher<K>()) { }
            public FastMap(int capacity) : base(capacity, new FxHasher<K>()) { }
        }

        private const ulong K = 0x517cc1b727220a95;

        public class FxHasher<T> : IEqualityComparer<T>
        {
            public bool Equals(T x, T y)
            {
                return EqualityComparer<T>.Default.Equals(x, y);
            }

            public int GetHashCode(T obj)
            {
                if (obj == null) return 0;
                var hash = 0ul;

                if (obj is string str)
                {
                    foreach (char c in str)
                        hash = BitOperations.RotateLeft(hash, 5) ^ c;
                }
                else if (obj is int i)
                {
                    hash = (ulong)i;
                }
                else if (obj is long l)
                {
                    hash = (ulong)l;
                }
                else if (obj is byte[] bytes)
                {
                    int index = 0;
                    while (index + 8 <= bytes.Length)
                    {
                        hash ^= BitConverter.ToUInt64(bytes, index);
                        index += 8;
                    }
                    if (index < bytes.Length)
                    {
                        hash ^= bytes[index];
                    }
                }
                else
                {
                    return obj.GetHashCode();
                }

                hash *= K;
                return (int)(hash & 0xFFFFFFFF);
            }
        }
    }
}