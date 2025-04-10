using System.Text;
using aoc_fast.Extensions;

namespace aoc_fast.Years._2021
{
    internal class Day8
    {
        public static string input { get; set; }
        private static List<int[]> Descrambled = [];

        private static int ToDigit(byte total) => total switch
        {
            42 => 0,
            17 => 1,
            34 => 2,
            39 => 3,
            30 => 4,
            37 => 5,
            41 => 6,
            25 => 7,
            49 => 8,
            45 => 9,
            _ => throw new NotImplementedException()
        };
        private static int[] Descramble(string input)
        {
            var freq = new byte[104];
            var bytes = Encoding.UTF8.GetBytes(input);
            bytes[0..58].ToList().ForEach(b => freq[(int)b]++);
            return bytes[61..].Split(b => b == ' ').Select(scrambled => ToDigit(scrambled.Select(b => freq[(int)b]).Sum())).Chunk(4).ToList()[0];
        }

        private static void Parse() => Descrambled = input.TrimEnd().Split("\n").Select(Descramble).ToList();

        public static int PartOne()
        {
            Parse();
            return Descrambled.SelectMany(i => i).Where(d => d == 1 || d == 4 || d == 7 || d == 8).Count();
        }
        public static int PartTwo() => Descrambled.Select(digits => digits.FoldDecimal()).Sum();
    }
}
