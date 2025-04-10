using aoc_fast.Extensions;

namespace aoc_fast.Years._2017
{
    class Day5
    {
        public static string input
        {
            get;
            set;
        }
        const int WIDTH = 16;
        const int LENGTH = 1 << WIDTH;

        private static List<int> Jumps = [];

        public static int PartOne()
        {
            Jumps = input.ExtractNumbers<int>();
            var jump = Jumps.ToList();
            var total = 0;
            var index = 0;

            while(index < jump.Count)
            {
                var next = index + jump[index];
                jump[index]++;
                total++;
                index = next;
            }

            return total;
        }
        public static int PartTwo()
        {
            var jump = Jumps.ToArray();
            var total = 0;
            var index = 0;
            var fine = 0;
            var coarse = 0;
            var compact = new List<ushort>();
            var Cache = GenerateCache();

            static (ushort, byte, byte)[,] GenerateCache()
            {
                const int WIDTH = 16;
                const int LENGTH = 1 << WIDTH;
                var cache = new (ushort, byte, byte)[WIDTH, LENGTH];

                Parallel.For(0, WIDTH, i =>
                {
                    for (var j = 0; j < LENGTH; j++)
                    {
                        var offset = (ushort)i;
                        var value = (ushort)j;
                        var steps = (byte)0;
                        while (offset < 16)
                        {
                            value ^= (ushort)(1 << offset);
                            steps += 1;
                            offset += (ushort)(3 - ((value >> offset) & (ushort)1));
                        }
                        cache[i, j] = (value, steps, (byte)(offset - i));
                    }
                });
                return cache;
            }

            while (index < jump.Length)
            {
                if (index < coarse)
                {
                    var based = index / 16;
                    var offset = index % 16;
                    var value = compact[based];
                    var (next, steps, delta) = Cache[offset, value];
                    compact[based] = next;
                    total += steps;
                    index += delta;
                }
                else
                {
                    var next = index + jump[index];
                    jump[index] += jump[index] == 3 ? -1 : 1;
                    total++;

                    if (jump[index] == 2 && index == fine)
                    {
                        fine++;
                        if (fine % 16 == 0)
                        {
                            var value = 0;
                            for (var i = fine - 1; i >= coarse; i--)
                            {
                                value = (value << 1) | (jump[i] & 1);
                            }
                            coarse = fine;
                            compact.Add((ushort)value);
                        }
                    }

                    index = next;
                }
            }

            return total;
        }
    }
}
