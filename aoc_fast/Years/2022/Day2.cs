using System.Text;
using aoc_fast.Extensions;

namespace aoc_fast.Years._2022
{
    internal class Day2
    {
        public static string input { get; set; }
        private static List<ulong> nums = [];
        private static void Parse() => nums = Encoding.UTF8.GetBytes(input.Trim()).Chunk(4).Select(c => (ulong)(3 * (c[0] - (byte)'A') + c[2] - (byte)'X')).ToList();
        public static uint PartOne()
        {
            Parse();
            uint[] score = [4, 8, 3, 1, 5, 9, 7, 2, 6];
            return nums.Select(i => score[i]).Sum();
        }
        public static uint PartTwo()
        {
            uint[] score = [3, 4, 8, 1, 5, 9, 2, 6, 7];
            return nums.Select((i) => score[i]).Sum();
        }
    }
}
