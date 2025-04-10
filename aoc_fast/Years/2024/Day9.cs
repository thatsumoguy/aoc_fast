using System.Text;
using aoc_fast.Extensions;

namespace aoc_fast.Years._2024
{
    internal class Day9
    {
        public static string input
        {
            get;
            set;
        }

        private static readonly int[] TRIANGLE = [0, 0, 1, 3, 6, 10, 15, 21, 28, 36];
        private static int[] disks = [];

        private static (long, long) Update(long checksum, long block, int index, int size)
        {
            var id = index / 2;
            var extra = block * size + TRIANGLE[size];
            return (checksum + id * extra, block + size);
        }

        private static void Parse() => disks = Encoding.ASCII.GetBytes(input.Trim()).Select(b => b - '0').ToArray();

        public static long PartOne()
        {
            Parse();
            var left = 0;
            var right = disks.Length - 2 + disks.Length % 2;
            var needed = disks[right];
            var block = 0L;
            var checksum = 0L;

            while (left < right)
            {
                (checksum, block) = Update(checksum, block, left, disks[left]);
                var available = disks[left + 1];
                left += 2;


                while (available > 0)
                {
                    if (needed == 0)
                    {
                        if (left == right) break;

                        right -= 2;
                        needed = disks[right];
                    }

                    var size = needed < available ? needed : available;
                    (checksum, block) = Update(checksum, block, right, size);
                    available -= size;
                    needed -= size;
                }
            }

            (checksum, _) = Update(checksum, block, right, needed);
            return checksum;
        }

        public static long PartTwo()
        {
            var block = 0L;
            var checksum = 0L;
            var free = Enumerable.Range(0, 10).Select(_ => new List<long>(1100)).ToList();

            foreach (var (size, index) in disks.Select((s, i) => (s, i)))
            {
                if (index % 2 == 1 && size > 0) free[size].Add(block);
                block += size;
            }

            for (var i = 0; i < 10; i++)
            {
                free[i].Add(block);
                free[i].Reverse();
            }

            foreach (var (size, index) in disks.Select((s, i) => (s, i)).Reverse())
            {
                block -= size;

                if (index % 2 == 1) continue;

                var nextBlock = block;
                var nextIndex = int.MaxValue;

                for (var i = size; i < free.Count; i++)
                {
                    var top = free[i].Count - 1;
                    var first = free[i][top];

                    if (first < nextBlock)
                    {
                        nextBlock = first;
                        nextIndex = i;
                    }
                }

                if (free.Count > 0)
                {
                    var biggest = free.Count - 1;
                    var top = free[biggest].Count - 1;

                    if (free[biggest][top] > block) free.Pop();
                }

                var id = index / 2;
                var extra = nextBlock * size + TRIANGLE[size];
                checksum += id * extra;

                if (nextIndex != int.MaxValue)
                {
                    free[nextIndex].Pop();

                    var to = nextIndex - size;

                    if (to > 0)
                    {
                        var i = free[to].Count;
                        var value = nextBlock + size;

                        while (free[to][i - 1] < value) i -= 1;

                        free[to].Insert(i, value);
                    }
                }
            }

            return checksum;
        }
    }
}
