using System.Text;
using aoc_fast.Extensions;

namespace aoc_fast.Years._2022
{
    internal class Day3
    {
        public static string input { get; set; }
        private static List<string> strings = [];

        private static UInt128 Mask(string s) => Encoding.UTF8.GetBytes(s).Aggregate((UInt128)0, (acc, b) => acc | ((UInt128)1 << b));
        private static uint Priority(UInt128 mask)
        {
            var zeros = (uint)UInt128.TrailingZeroCount(mask);
            return zeros switch
            {
                (>= 65) and (<= 90) => zeros - 38,
                (>=97) and (<= 122) => zeros - 96
            };
        }

        public static uint PartOne()
        {
            strings = input.Split("\n", StringSplitOptions.RemoveEmptyEntries).ToList();
            return strings.Select(rucksack =>
            {
                var (a, b) = (rucksack[..(rucksack.Length / 2)], rucksack[(rucksack.Length / 2)..]);
                return Priority(Mask(a) & Mask(b));
            }).Sum();
        }
        public static uint PartTwo() => strings.Chunk(3).Select(a => Priority(Mask(a[0]) & Mask(a[1]) & Mask(a[2]))).Sum();
    }
}
