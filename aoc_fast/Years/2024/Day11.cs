using aoc_fast.Extensions;
using static aoc_fast.Extensions.FastMath;
using static aoc_fast.Extensions.Hash;

namespace aoc_fast.Years._2024
{
    internal class Day11
    {
        public static string input
        {
            get;
            set;
        }

        private static List<long> StartNums = [];

        private static long Blinking(List<long> nums, int blinks)
        {
            var stones = new List<(long, long)>(5000);
            var indices = new FastMap<long, long>(5000);

            var todo = new List<long>();

            var current = new List<long>();

            foreach (var num in nums)
            {
                if (indices.TryGetValue(num, out var index)) current[(int)index]++;
                else
                {
                    indices.Add(num, indices.Count);
                    todo.Add(num);
                    current.Add(1);
                }
            }

            for (var _ = 0; _ < blinks; _++)
            {
                var numbers = todo;

                todo = new List<long>(200);

                long indexOf(long num)
                {
                    var size = indices.Count;

                    if (!indices.TryGetValue(num, out var s))
                    {
                        todo.Add(num);
                        indices.Add(num, size);
                        return size;
                    }
                    return s;
                }

                foreach (var number in numbers)
                {
                    var (first, second) = (0L, 0L);
                    if (number == 0) (first, second) = (indexOf(1), long.MaxValue);
                    else
                    {
                        var digits = ILog10(number) + 1;
                        if (digits % 2 == 0)
                        {
                            var power = Pow(10L, digits / 2);
                            (first, second) = (indexOf(number / power), indexOf(number % power));
                        }
                        else (first, second) = (indexOf(number * 2024), long.MaxValue);
                    }

                    stones.Add((first, second));
                }

                var next = Enumerable.Repeat(0L, indices.Count).ToList();


                foreach (var ((first, second), amount) in stones.Zip(current))
                {
                    next[(int)first] += amount;
                    if (second != long.MaxValue) next[(int)second] += amount;
                }

                current = next;
            }

            return current.Sum();
        }

        public static long PartOne()
        {
            StartNums = input.ExtractNumbers<long>();
            return Blinking(StartNums, 25);
        }

        public static long PartTwo() => Blinking(StartNums, 75);

    }
}
