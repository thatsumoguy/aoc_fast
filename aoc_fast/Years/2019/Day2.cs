using aoc_fast.Extensions;

namespace aoc_fast.Years._2019
{
    internal class Day2
    {
        public static string input { get; set; }
        private static int[] Code = [];
        private static int Check(int[] input, int first, int second)
        {
            var code = input.ToList();
            code[1] = first;
            code[2] = second;
            return Execute(code);
        }

        private static int Execute(List<int> code)
        {
            var pc = 0;

            while(true)
            {
                switch (code[pc])
                {
                    case 1:
                        code[code[pc + 3]] = code[code[pc + 1]] + code[code[pc + 2]];
                        break;
                    case 2:
                        code[code[pc + 3]] = code[code[pc + 1]] * code[code[pc + 2]];
                        break;
                    default: return code[0];
                }
                pc += 4;
            }
        }

        private static int? Search(int[] input, int x1, int x2, int y1, int y2)
        {
            if(x1 > x2 || y1 > y2) return null;

            var X = (x1 + x2) / 2;
            var y = (y1 + y2) / 2;
            var (a, b, c) = (input[0], input[1], input[2]);
            var res = a * X + b * y + c;
            return res.CompareTo(19690720) switch
            {
                0 => 100 * X + y,
                -1 => Search(input, X + 1, x2, y1, y2) ?? Search(input, x1, x2, y + 1, y2),
                1 => Search(input, x1, X - 1, y1, y2) ?? Search(input, x1, x2, y1, y - 1)
            };
        }

        private static void Parse()
        {
            var code = input.ExtractNumbers<int>().ToArray();
            var c = Check(code, 0, 0);
            var a = Check(code, 1, 0);
            var b = Check(code, 0, 1);
            Code = [a - c, b - c, c];
        }

        public static int PartOne()
        {
            Parse();
            return Code[0] * 12 + Code[1] * 2 + Code[2];
        }
        public static int PartTwo() => Search(Code, 0, 99, 0, 99).Value;
    }
}
