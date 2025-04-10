using System.Text;

namespace aoc_fast.Years._2024
{
    internal partial class Day3
    {
        public static string input
        {
            get;
            set;
        }
        private static void Parse()
        {
            static bool IsAsciiDigit(byte b)
            {
                return b >= 48 && b <= 57;
            }
            //Using span just to use the StartsWith, making my life easier since byte[] doesn't allow that with strings.
            Span<byte> mem = stackalloc byte[input.Length * 4];
            var dataWritten = Encoding.UTF8.GetBytes(input, mem);

            var mulBytes = Encoding.UTF8.GetBytes("mul(").AsSpan();
            var doBytes = Encoding.UTF8.GetBytes("do()").AsSpan();
            var dontBytes = Encoding.UTF8.GetBytes("don't()").AsSpan();
            var partOne = 0;
            var partTwo = 0;

            var index = 0;
            var enabled = true;
            while (index < mem.Length)
            {
                //Figure out the dos, donts, and muls, move the index past the bytes we don't care about
                var cur = mem[index..];
                switch (cur)
                {
                    case var _ when cur.StartsWith(mulBytes):
                        index += 4;
                        break;
                    case var _ when cur.StartsWith(doBytes):
                        index += 4;
                        enabled = true;
                        continue;
                    case var _ when cur.StartsWith(dontBytes):
                        index += 7;
                        enabled = false;
                        continue;
                    default:
                        index++;
                        continue;
                }

                var first = 0;
                var digits = 0;
                //Extract individual numbers from chars and then convert to number
                while (IsAsciiDigit(mem[index]))
                {
                    first = 10 * first + mem[index] - '0';
                    digits++;
                    index++;
                }
                //Skips over invalid entries
                if (digits == 0 || mem[index] != ',') continue;
                index++;

                var second = 0;
                digits = 0;
                //Extract the second number
                while (IsAsciiDigit(mem[index]))
                {
                    second = 10 * second + mem[index] - '0';
                    digits++;
                    index++;
                }
                if (digits == 0 || mem[index] != ')') continue;
                index++;
                //Get the product and totals for each parts
                var product = first * second;
                partOne += product;
                if (enabled) partTwo += product;
            }

            answer = (partOne, partTwo);
        }

        private static (int partOne, int partTwo) answer;
        public static int PartOne()
        {
            Parse();
            return answer.partOne;
        }
        public static int PartTwo() => answer.partTwo;
    }
}
