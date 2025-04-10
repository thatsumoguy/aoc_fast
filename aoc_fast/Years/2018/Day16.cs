using aoc_fast.Extensions;

namespace aoc_fast.Years._2018
{
    internal class Day16
    {
        public static string input { get; set; }

        private static (List<(int, int)> samples, List<int[]> program) InputObj;

        private static int CPU(int opCode, int a, int b, int[] regs) =>
            opCode switch
            {
                0 => regs[a] + regs[b],
                1 => regs[a] + b,
                2 => regs[a] * regs[b],
                3 => regs[a] * b,
                4 => regs[a] & regs[b],
                5 => regs[a] & b,
                6 => regs[a] | regs[b],
                7 => regs[a] | b,
                8 => regs[a],
                9 => a,
                10 => a > regs[b] ? 1 : 0,
                11 => regs[a] > b ? 1 : 0,
                12 => regs[a] > regs[b] ? 1 : 0,
                13 => a == regs[b] ? 1 : 0,
                14 => regs[a] == b ? 1 : 0,
                15 => regs[a] == regs[b] ? 1 : 0,
                _ => throw new NotImplementedException()
            };

        private static void Parse()
        {
            var split = input.Split("\n\n\n\n");
            var samples = split[0].ExtractNumbers<int>().Chunk(4).Chunk(3).Select(a => 
            {
                var instructions = a[1];
                var mask = 0;

                for(var opcode = 0; opcode < 16;  opcode++)
                {
                    if (CPU(opcode, instructions[1], instructions[2], a[0]) == a[2][instructions[3]]) mask |= 1 << opcode;
                }
                return (instructions[0], mask);
            }).ToList();
            InputObj = (samples, split[1].ExtractNumbers<int>().Chunk(4).ToList());
        }

        public static int PartOne()
        {
            Parse();
            return InputObj.samples.Where(a => int.PopCount(a.Item2) >= 3).Count();
        }
        public static int PartTwo()
        {
            var masks = Enumerable.Repeat(0xffff, 16).ToArray();

            foreach (var (unknown, mask) in InputObj.samples) masks[unknown] &= mask;

            var convert = Enumerable.Repeat(0, 16).ToArray();

            while(true)
            {
                var index = Array.FindIndex(masks, (m) => int.PopCount(m) == 1);
                if(index == -1) break;
                var mask = masks[index];
                for (var i = 0; i < masks.Length; i++) masks[i] &= ~mask;
                convert[index] = int.TrailingZeroCount(mask);
            }

            int[] reg = [0, 0, 0, 0];

            foreach(var prog in InputObj.program)
            {
                var opcode = convert[prog[0]];
                reg[prog[3]] = CPU(opcode, prog[1], prog[2], reg);
            }

            return reg[0];
        }
    }
}
