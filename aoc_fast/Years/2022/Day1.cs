using System.Runtime.CompilerServices;

namespace aoc_fast.Years._2022
{
    internal class Day1
    {
        public static string input { get; set; }
        private static uint maxCalories;
        private static uint topThreeSum;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void Parse()
        {
            ReadOnlySpan<char> span = input;
            var currentElf = 0u;
            var top1 = 0u;
            var top2 = 0u;
            var top3 = 0u;

            var i = 0;
            while (i < span.Length)
            {
                var num = 0u;
                while (i < span.Length && char.IsDigit(span[i]))
                {
                    num = num * 10 + (uint)(span[i] - '0');
                    i++;
                }

                if (num > 0) currentElf += num;

                var isElfEnd = false;
                while (i < span.Length && char.IsWhiteSpace(span[i]))
                {
                    if (span[i] == '\n')
                    {
                        if (i + 1 >= span.Length || span[i + 1] == '\n')
                        {
                            isElfEnd = true;
                            i++;
                            break;
                        }
                    }
                    i++;
                }
                if (isElfEnd || i >= span.Length)
                {
                    if (currentElf > top1)
                    {
                        top3 = top2;
                        top2 = top1;
                        top1 = currentElf;
                    }
                    else if (currentElf > top2)
                    {
                        top3 = top2;
                        top2 = currentElf;
                    }
                    else if (currentElf > top3)
                    {
                        top3 = currentElf;
                    }

                    currentElf = 0;
                }
            }

            maxCalories = top1;
            topThreeSum = top1 + top2 + top3;
        }

        public static uint PartOne()
        {
            Parse();
            return maxCalories;
        }

        public static uint PartTwo() => topThreeSum;
    }
}