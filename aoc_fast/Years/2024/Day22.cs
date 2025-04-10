using aoc_fast.Extensions;

namespace aoc_fast.Years._2024
{
    internal class Day22
    {
        public static string input
        {
            get;
            set;
        }
        private static object Mutex = new();
        private static int ToIndex(ulong previous, ulong current) => (int)(9 + current % 10 - previous % 10);

        private static ulong NextNum(ulong num)
        {
            num = (num ^ (num << 6)) & 0xffffff;
            num = (num ^ (num >> 5)) & 0xffffff;
            return (num ^ (num << 11)) & 0xffffff;
        }

        private static List<ulong> nums = [];
        private static (ulong partOne, ushort partTwo) answers;

        private static void Parse()
        {
            nums = input.ExtractNumbers<ulong>();

            var partOneAnswer = 0uL;
            var partTwoAnswer = new List<ushort>(130321);
            for (var i = 0; i < 130321; i++) partTwoAnswer.Add(0);
            Threads.SpawnBatches(nums, (batch) => Worker(ref partOneAnswer, partTwoAnswer, batch));
            answers = (partOneAnswer, partTwoAnswer.Max());
        }

        public static ulong PartOne()
        {
            Parse();
            return answers.partOne;
        }
        public static ushort PartTwo()
        {
            return answers.partTwo;
        }

        private static void Worker(ref ulong partOneAnswer, List<ushort> partTwoAnswer, List<ulong> batch)
        {
            var partOne = 0uL;
            var partTwo = new List<ushort>(130321);
            var seen = new List<ushort>(130321);
            for (var i = 0; i < 130321; i++)
            {
                partTwo.Add(0);
                seen.Add(ushort.MaxValue);
            }

            foreach (var (id, num) in batch.Index())
            {
                var shortId = (ushort)id;

                var zero = num;
                var first = NextNum(zero);
                var second = NextNum(first);
                var third = NextNum(second);

                int a;
                var b = ToIndex(zero, first);
                var c = ToIndex(first, second);
                var d = ToIndex(second, third);
                var number = third;
                var prev = third % 10;

                for (var _ = 3; _ < 2000; _++)
                {
                    number = NextNum(number);
                    var price = number % 10;

                    (a, b, c, d) = (b, c, d, (int)(9 + price - prev));
                    var index = 6859 * a + 361 * b + 19 * c + d;

                    if (seen[index] != shortId)
                    {
                        partTwo[index] += (ushort)price;
                        seen[index] = shortId;
                    }
                    prev = price;
                }
                partOne += number;
            }
            Interlocked.Add(ref partOneAnswer, partOne);
            lock (Mutex)
            {
                for (var i = 0; i < partTwoAnswer.Count; i++)
                {
                    partTwoAnswer[i] += partTwo[i];
                }
            }
        }
    }
}
