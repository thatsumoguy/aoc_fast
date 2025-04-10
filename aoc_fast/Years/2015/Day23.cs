using aoc_fast.Extensions;

namespace aoc_fast.Years._2015
{
    class Day23
    {
        public static string input
        {
            get;
            set;
        }

        public abstract record Op
        {
            public record Hlf : Op;
            public record Tpl : Op;
            public record IncA: Op;
            public record IncB: Op;
            public record Jmp(int jmpSize) : Op;
            public record Jie(int jmpSize) : Op;
            public record Jio(int jmpSize) : Op;
        }

        private static List<Op> ops = [];

        private static void Parse()
        {
            ops.Clear();
            foreach (var (i, s) in input.Split("\n", StringSplitOptions.RemoveEmptyEntries).Index())
            {
                switch (s)
                {
                    case "hlf a": ops.Add(new Op.Hlf()); break;
                    case "tpl a": ops.Add(new Op.Tpl()); break;
                    case "inc a": ops.Add(new Op.IncA()); break;
                    case "inc b": ops.Add(new Op.IncB()); break;
                    default:
                        {
                            var index = Int32.CreateTruncating(i + s.ExtractNumbers<int>()[0]);
                            switch (s[..3])
                            {
                                case "jmp": ops.Add(new Op.Jmp(index)); break;
                                case "jie": ops.Add(new Op.Jie(index)); break;
                                case "jio": ops.Add(new Op.Jio(index)); break;
                            }
                            break;
                        }
                }
            }
        }

        private static ulong Execute(List<Op> ops, ulong a)
        {
            var pc = 0;
            var b = 0ul;

            while(pc < ops.Count)
            {
                switch(ops[pc])
                {
                    case Op.Hlf:
                        {
                            a /= 2;
                            pc++;
                            break;
                        }
                    case Op.Tpl:
                        {
                            a *= 3;
                            pc++; 
                            break;
                        }
                    case Op.IncA:
                        {
                            a++;
                            pc++;
                            break;
                        }
                    case Op.IncB:
                        {
                            b++; 
                            pc++;
                            break;
                        }
                    case Op.Jmp(var index):
                        {
                            pc = index;
                            break;
                        }
                     case Op.Jie(var index):
                        {
                            pc = a % 2 == 0 ? index : pc + 1;
                            break;
                        }
                    case Op.Jio(var index):
                        {
                            pc = a == 1 ? index : pc + 1;
                            break;
                        }
                }
            }
            return b;
        }

        public static ulong PartOne()
        {
            Parse();
            return Execute(ops, 0);
        }
        public static ulong PartTwo()
        {
            Parse();
            return Execute(ops, 1);
        }
    }
}
