using System.Text;
using aoc_fast.Extensions;

namespace aoc_fast.Years._2023
{
    internal class Day1
    {
        public static string input { get; set; }
        private static readonly byte[][] DIGITS = [Encoding.ASCII.GetBytes("one"), Encoding.ASCII.GetBytes("two"), Encoding.ASCII.GetBytes("three"), Encoding.ASCII.GetBytes("four"), Encoding.ASCII.GetBytes("five"),
        Encoding.ASCII.GetBytes("six"), Encoding.ASCII.GetBytes("seven"), Encoding.ASCII.GetBytes("eight"), Encoding.ASCII.GetBytes("nine")];

        public static int PartOne() => input.Split("\n", StringSplitOptions.RemoveEmptyEntries).Select(line =>
        {
            var first = Encoding.ASCII.GetBytes(line).First(c => char.IsAsciiDigit((char)c)).SaturatingSub((byte)'0');
            var last = Encoding.ASCII.GetBytes(line).Last(c => char.IsAsciiDigit((char)c)).SaturatingSub((byte)'0');
            return 10 * first + last;
        }).Sum();
        public static int PartTwo() => input.Split("\n", StringSplitOptions.RemoveEmptyEntries).Select(line => 
        {
            var bytes = Encoding.ASCII.GetBytes(line).AsSpan();

            var first = 0;
        
            while (true)
            {
                if (bytes[0].IsAsciiDigit())
                {
                    first = bytes[0].SaturatingSub((byte)'0');
                    break;
                }
                foreach(var (value, digit) in DIGITS.Index())
                {
                    if(bytes.StartsWith(digit))
                    {
                        first = value + 1;
                        goto outerFirst;

                    }
                }
                bytes = bytes[1..];
            }
            outerFirst:
            var last = 0;
            while(true)
            {
                if (bytes[^1].IsAsciiDigit())
                {
                    last = bytes[^1].SaturatingSub((byte)'0');
                    break;
                }
                foreach(var (value, digit) in DIGITS.Index())
                {
                    if(bytes.EndsWith(digit))
                    {
                        last = value + 1;
                        goto outerLast;
                    }
                }
                bytes = bytes[..^1];
            }
        outerLast:
            return 10 * first + last;
        
        }).Sum();
    }
}
