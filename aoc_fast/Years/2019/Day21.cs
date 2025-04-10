using aoc_fast.Extensions;

namespace aoc_fast.Years._2019
{
    internal class Day21
    {
        public static string input { get; set; }
        const string SLOW = "OR A J\nAND B J\nAND C J\nNOT J J\nAND D J";
        const string FAST = "OR E T\nOR H T\nAND T J";
        private static long[] code = [];
        private static long Survey(long[] code, string springScript)
        {
            var comp = new Computer(code);
            comp.InputAscii(springScript);

            var res = 0L;
            while (comp.Run(out var next) == State.Output) res = next;
            return res;
        }

        public static long PartOne()
        {
            code = [.. input.ExtractNumbers<long>()];
            return Survey(code, $"{SLOW}\nWALK\n");
        }
        public static long PartTwo() => Survey(code, $"{SLOW}\n{FAST}\nRUN\n");

    }
}
