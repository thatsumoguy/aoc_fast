using System.Collections.Concurrent;
using System.Text;
using aoc_fast.Extensions;

namespace aoc_fast.Years._2016
{
    class Day5
    {
        public static string input
        {
            get;
            set;
        }
        private class Shared
        {
            public int Counter;
            public string Prefix;
            public ConcurrentBag<(uint, uint)> Found = [];
            public int Mask;
            public bool Done;
        }
        private static object mutex = new();
        private static void CheckHash(byte[] buffer, int size, uint n, Shared shared)
        {
            var (result, _, _, _) = Md5.Hash(buffer, size);

            if((result & 0xfffff000) == 0)
            {
                  lock(mutex)
                  {
                    shared.Found.Add((n, result));
                    shared.Mask |= 1 << (int)(result >> 8);
                    if((shared.Mask & 0xff) == 0xff) shared.Done = true;
                  }
            }
        }
        private static (byte[], int) FormatString(string prefix, uint n)
        {
            var s = $"{prefix}{n}";
            var size = s.Length;
            var buffer = new byte[64];
            Array.Copy(Encoding.ASCII.GetBytes(s), buffer, size);
            return (buffer, size);
        }

        private static void Worker(Shared shared)
        {
            while(!shared.Done)
            {
                var offset = (uint)Interlocked.Add(ref shared.Counter, 1000);
                var (buffer, size) = FormatString(shared.Prefix, offset);
                for(var n = 0u; n < 1000; n++)
                {
                    buffer[size - 3] = (byte)('0' + (n / 100));
                    buffer[size - 2] = (byte)('0' + ((n / 10) % 10));
                    buffer[size - 1] = (byte)('0' + (n % 10));
                    CheckHash(buffer, size, offset + n, shared);
                }
            }
        }
        private static List<uint> Password = [];
        private static void Parse()
        {
            var shared = new Shared { Counter = 100, Done = false, Found = [], Mask = 0, Prefix = input.Trim() };

            for(var n = 1u; n < 1000; n++)
            {
                var (buffer, size) = FormatString(shared.Prefix, n);
                CheckHash(buffer, size, n, shared);
            }

            Threads.Spawn(() => Worker(shared));

            var found = shared.Found.ToList();
            found = [.. found.Order()];
            Password = found.Select(n => n.Item2).ToList();
        }
        public static string PartOne()
        {
            Parse();
            var password = Password.Take(8).Aggregate(0u, (acc, n) => (acc << 4) | (n >> 8));

            return $"{password:x8}";
        }

        public static string PartTwo()
        {
            var password = 0u;
            var mask = 0xffffffffu;
            foreach(var n in Password)
            {
                var sixth = n >> 8;
                if(sixth < 8)
                {
                    var shift = 4 * (7 - sixth);
                    var seventh = (n >> 4) & 0xfu;
                    password |= (seventh << (int)shift) & mask;
                    mask &= ~(0xfu << (int)shift);
                }
            }
            return $"{password:x8}";
        }
    }
}
