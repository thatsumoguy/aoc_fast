using System.Text;
using aoc_fast.Extensions;

namespace aoc_fast.Years._2017
{
    class Day1
    {
        public static string input
        {
            get;
            set;
        }
        private static byte[] data = [];
        private static int Captcha(byte[] input, int offset)
        {
            var rotated = input.ToList();
            rotated.RotateLeft(offset);
            return input.Zip(rotated).Select(a => a.First == a.Second ? a.First - '0' : 0).Sum();
        }
        public static int PartOne()
        {
            data = Encoding.ASCII.GetBytes(input.Trim());
            return Captcha(data, 1);
        }
        public static int PartTwo() => Captcha(data, data.Length / 2);
    }
}
