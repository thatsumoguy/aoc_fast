using System.Text;
using aoc_fast.Extensions;

namespace aoc_fast.Years._2015
{
    class Day4
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
            public bool Done;
            public uint First;
            public uint Second;
            public bool FirstDone;
        }
        private static object mutex = new object();
        private static void CheckHash(byte[] buffer, int size, uint n, Shared shared)
        {
            var (result, _, _, _) = Md5.Hash(buffer, size);

            if ((result & 0xffffff00) == 0)
            {
                lock (mutex)
                {
                    shared.Second = shared.Second.Min(n);
                    shared.Done = true;
                }
            }
            else if(!shared.FirstDone && (result & 0xfffff000) == 0)
            {
                lock(mutex)
                {
                    shared.FirstDone = true;
                    shared.First = shared.First.Min(n);
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
            while (!shared.Done)
            {
                var offset = (uint)Interlocked.Add(ref shared.Counter, 1000);
                var (buffer, size) = FormatString(shared.Prefix, offset);
                for (var n = 0u; n < 1000; n++)
                {
                    buffer[size - 3] = (byte)((byte)'0' + (n / 100));
                    buffer[size - 2] = (byte)((byte)'0' + ((n / 10) % 10));
                    buffer[size - 1] = (byte)((byte)'0' + (n % 10));
                    CheckHash(buffer, size, offset + n, shared);
                }
            }
        }
        private static Shared shared;
        private static void Parse()
        {
            shared = new Shared() { Prefix = input.Trim(), Done = false, Counter = 1000, First = uint.MaxValue, Second = uint.MaxValue, FirstDone = false};

            for(var n = 0u; n < 1000; n++)
            {
                var (buffer, size) = FormatString(shared.Prefix, n);
                CheckHash(buffer, size, n, shared);
            }

            Threads.Spawn(() => Worker(shared));
        }
        public static uint PartOne()
        {
            Parse();
            return shared.First;
        }
        public static uint PartTwo()
        {
            Parse();
            return shared.Second;
        }
    }
}
