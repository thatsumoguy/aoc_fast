﻿using System.Buffers.Binary;
using System.Text;
using aoc_fast.Extensions;

namespace aoc_fast.Years._2016
{
    class Day14
    {
        public static string input
        {
            get;
            set;
        }
        private static object mutex = new();
        private static CancellationTokenSource cts = new();
        struct Shared()
        {
            public string Input;
            public bool PartTwo;
            public bool Done;
            public int Counter;
            public SortedDictionary<int, uint> Threes = [];
            public SortedDictionary<int, uint> Fives = [];
            public SortedSet<int> Found = [];
        }

        private static void Check(Shared shared, int n, (uint, uint, uint, uint) hash)
        {
            var (a, b, c, d) = hash;

            var prev = uint.MaxValue;
            var same = 1;
            var three = 0u;
            var five = 0u;
            var words = new uint[] { d, c, b, a };
            for (var i = 0; i < words.Length; i++)
            {
                var word = words[i];
                for (var _ = 0; _ < 8; _++)
                {
                    var next = word & 0xf;

                    if (next == prev) same++;
                    else same = 1;

                    if (same == 3) three = 1u << (int)next;
                    if (same == 5) five |= 1u << (int)next;

                    word >>= 4;
                    prev = next;
                }
            }
            if (three != 0 || five != 0)
            {
                lock (mutex)
                {
                    var candidates = new List<int>();

                    if (three != 0)
                    {
                        shared.Threes[n] = three;

                        foreach (var kvp in shared.Fives.Where(x => x.Key > n && x.Key <= n + 1000))
                        {
                            if ((three & kvp.Value) != 0)
                            {
                                candidates.Add(n);
                            }
                        }
                    }
                    if (five != 0)
                    {
                        shared.Fives[n] = five;

                        foreach (var kvp in shared.Threes.Where(x => x.Key >= n - 1000 && x.Key < n))
                        {
                            if ((five & kvp.Value) != 0)
                            {
                                candidates.Add(kvp.Key);
                            }
                        }
                    }

                    foreach (var candidate in candidates) shared.Found.Add(candidate);
                    if (shared.Found.Count >= 64)
                    {
                        shared.Done = true;
                        cts.Cancel();
                    }
                }
            }
        }

        private static byte[] ToAscii(uint i)
        {
            var n = (ulong)i;
            n = ((n << 16) & 0x0000ffff00000000) | (n & 0x000000000000ffff);
            n = ((n << 8) & 0x00ff000000ff0000) | (n & 0x000000ff000000ff);
            n = ((n << 4) & 0x0f000f000f000f00) | (n & 0x000f000f000f000f);

            var mask = ((n + 0x0606060606060606) >> 4) & 0x0101010101010101;
            n = n + 0x3030303030303030 + 0x27 * mask;

            return BitConverter.GetBytes(BinaryPrimitives.ReverseEndianness(n));
        }

        private static (byte[], int) FormatString(string prefix, int n)
        {
            var s = $"{prefix}{n}";
            var size = s.Length;
            return (Encoding.ASCII.GetBytes(s), size);
        }


        private static void Worker(Shared shared)
        {
            while (!cts.IsCancellationRequested)
            {
                var n = Interlocked.Add(ref shared.Counter, 1);
                var (buffer, size) = FormatString(shared.Input, n);
                if (buffer.Length < 64) Array.Resize(ref buffer, 64);
                var result = Md5.Hash(buffer, size);

                if (shared.PartTwo)
                {
                    for (var _ = 0; _ < 2016; _++)
                    {
                        Array.Copy(ToAscii(result.Item1), 0, buffer, 0, 8);
                        Array.Copy(ToAscii(result.Item2), 0, buffer, 8, 8);
                        Array.Copy(ToAscii(result.Item3), 0, buffer, 16, 8);
                        Array.Copy(ToAscii(result.Item4), 0, buffer, 24, 8);
                        result = Md5.Hash(buffer, 32);
                    }
                }
                Check(shared, n, result);
            }
        }

        private static int GeneratePad(string input, bool partTwo)
        {
            cts = new CancellationTokenSource();
            var shared = new Shared() { Input = input, PartTwo = partTwo, Done = false, Counter = 0, Fives = [], Found = [], Threes = [] };
            Threads.Spawn(() => Worker(shared));
            return shared.Found.ElementAt(63);
        }

        public static int PartOne() => GeneratePad(input.Trim(), false);

        //This part makes up 98% of the total runtime of the entire program...I tried everything, MD5 hashing using AVX2, custom MD5 hashing, parallel programming, everything
        //C# is just too slow with MD5 and even using C++ or C calls won't work, mainly just because I don't know enough C or C++ to work with them but also because they just don't mesh well.
        public static int PartTwo() => GeneratePad(input.Trim(), true);
    }
}