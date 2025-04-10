using System.Text;
using aoc_fast.Extensions;

namespace aoc_fast.Years._2016
{
    class Day16
    {
        public static string input
        {
            get;
            set;
        }
        private static List<int> inputs = [];

        private static int Count(List<int> ones, int length)
        {
            var half = ones.Count - 1;
            var full = 2 * half + 1;
            while(full < length)
            {
                half = full;
                full = 2 * half + 1;
            }

            var res = 0;

            while (length >= ones.Count)
            {
                while (length <= half)
                {
                    half /= 2;
                    full /= 2;
                }

                var next = full - length;
                res += half - next;
                length = next;
            }

            return res + ones[length];
        }
        private static string CheckSum(List<int> input, int stepSize)
        {
            return new string(Enumerable.Range(0, 19).Select(i => Count(input, i * stepSize))
                .ToArray().Windows(2)
                .Select(w => (w[1] - w[0]) % 2 == 0 ? '1' : '0').ToArray());
        }

        private static void Parse()
        {
            var sum = 0;
            List<int> ones = [0];

            foreach(var b in Encoding.ASCII.GetBytes(input.Trim()))
            {
                sum += b;
                ones.Add(sum);
            }
            inputs = ones;
        }

        public static string PartOne()
        {
            Parse();
            return CheckSum(inputs, 1 << 4);
        }
        public static string PartTwo() => CheckSum(inputs, 1 << 21);
    }
}
