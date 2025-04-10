using System.Text;

namespace aoc_fast.Years._2016
{
    class Day9
    {
        public static string input
        {
            get;
            set;
        }

        private static byte[] bytes = [];

        private static (byte[] bytes, int acc) Number(byte[] slice)
        {
            var index = 2;
            var acc = slice[1] - (byte)'0';

            while(char.IsAsciiDigit((char)slice[index]))
            {
                acc = 10 * acc + slice[index] - (byte)'0';
                index++;
            }
            return (slice[index..], acc);
        }

        private static long Decompress(byte[] slice, bool recurse = false)
        {
            var length = 0L;

            while(slice.Length > 0)
            {
                if (slice[0] == (byte)'(')
                {
                    var (next, amount) = Number(slice);
                    (next, var repeat) = Number(next);

                    var start = 1;

                    var end = start + amount;

                    var result = recurse ? Decompress(next[start..end], true) : amount;

                    slice = next[end..];
                    length += result * repeat;
                }
                else
                {
                    slice = slice[1..];
                    length++;
                }
            }

            return length;
        }

        public static long PartOne()
        {
            bytes = Encoding.ASCII.GetBytes(input.Trim());
            return Decompress(bytes);
        }

        public static long PartTwo() => Decompress(bytes, true);
    }
}
