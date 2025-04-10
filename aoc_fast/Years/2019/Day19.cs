using aoc_fast.Extensions;

namespace aoc_fast.Years._2019
{
    internal class Day19
    {
        public static string input { get; set; }

        class CodeStruct(long[] code, long lower, long upper)
        {
            public long[] Code { get; set; } = code;
            public long Lower { get; set; } = lower;
            public long Upper { get; set; } = upper;
        }
        private static bool TestCode(long[] code, long X, long y)
        {
            var comp = new Computer(code);
            comp.Input(X);
            comp.Input(y);
            if (comp.Run(out var res) != State.Output) throw new Exception();
            return res == 1;
        }
        private static bool PreCheck(CodeStruct input, long X, long y) => 50 * y > input.Upper * X && 50 * X > input.Lower * y;


        private static CodeStruct codeStruct;

        private static void Parse()
        {
            var code = input.ExtractNumbers<long>().ToArray();
            var lower = 0L;
            var upper = 0L;

            while (!TestCode(code, lower + 1, 50)) lower++;
            while (!TestCode(code, 50, upper + 1)) upper++;

            codeStruct = new CodeStruct(code, lower, upper);
        }
        public static long PartOne()
        {
            Parse();

            var res = TestCode(codeStruct.Code, 0, 0) ? 1L : 0L;
            for(var y = 0; y < 50;  y++)
            {
                var left = long.MaxValue;
                var right = long.MinValue;
                for(var X = 0; X < 50; X++)
                {
                    if (PreCheck(codeStruct, X, y) &&  TestCode(codeStruct.Code, X, y))
                    {
                        left = X; break;
                    }
                }
                for(var X = 50; X > 0;  X--)
                {
                    if(PreCheck(codeStruct, X, y) && TestCode(codeStruct.Code, X, y))
                    {
                        right = X; break;
                    }
                }
                if (left <= right) res += right - left + 1;
            }
            return res;
        }

        public static long PartTwo()
        {
            var code =  codeStruct.Code;
            var X = 0L;
            var y = 0L;
            var moved = true;

            while(moved)
            {
                moved = false;

                while(!PreCheck(codeStruct, X, y + 99) || !TestCode(code, X, y + 99))
                {
                    X++;
                    moved = true;
                }
                while (!PreCheck(codeStruct, X + 99, y) || !TestCode(code, X + 99, y))
                {
                    y++;
                    moved = true;
                }
            }
            return 10000 * X + y;
        }
    }
}
