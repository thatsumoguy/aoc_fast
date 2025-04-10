using System.Text;
using aoc_fast.Extensions;

namespace aoc_fast.Years._2015
{
    class Day12
    {
        static byte[] RED = Encoding.ASCII.GetBytes("red");

        record Result(int next, bool ignore, int value);

        public static string input
        {
            get;
            set;
        }

        private static Result ParseArray(byte[] input, int start)
        {
            var index = start;
            var total = 0;

            while (input[index] != (byte)']')
            {
                var res = ParseJson(input, index + 1);
                index = res.next;
                total += res.value;
            }
            return new Result(index + 1, false, total);

        }

        private static Result ParseObject(byte[] input, int start) 
        {
            var index = start;
            var total = 0;
            var ignore = false;

            while (input[index] != (byte)'}')
            {
                var res1 = ParseJson(input, index + 1);
                var res2 = ParseJson(input, res1.next + 1);
                index = res2.next;
                total += res2.value;
                ignore |= res2.ignore;
            }

            return new Result(index + 1, false, ignore ? 0 : total);
        }

        private static Result ParseString(byte[] input, int start)
        {
            start++;
            var end = start;

            while (input[end] != (byte)'"') end++;

            return new Result(end + 1, RED.SequenceEqual(input[start..end]), 0);
        }

        private static Result ParseNumber(byte[] input, int start)
        {
            var end = start;
            var neg = false;
            var acc = 0;

            if (input[end] == (byte)'-')
            {
                neg = true;
                end++;
            }

            while (char.IsAsciiDigit((char)input[end]))
            {
                acc = 10 * acc + (input[end] - '0');
                end++;
            }
            return new Result(end, false, neg ? -acc : acc);

        }

        private static Result ParseJson(byte[] input, int start)
        {
            return input[start] switch
            {
                (byte)'[' => ParseArray(input, start),
                (byte)'{' => ParseObject(input, start),
                (byte)'"' => ParseString(input, start),
                _ => ParseNumber(input, start),
            };
        }
            

        public static int PartOne() => input.ExtractNumbers<int>().Sum();

        public static int PartTwo() => ParseJson(Encoding.ASCII.GetBytes(input), 0).value;
    }
}
