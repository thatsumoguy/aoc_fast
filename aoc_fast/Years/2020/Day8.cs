namespace aoc_fast.Years._2020
{
    internal class Day8
    {
        public static string input { get; set; }
        enum InstructionType
        {
            Acc,
            Jmp,
            Nop,
        }
        class Instruction(InstructionType type, short amount)
        {
            public InstructionType Type { get; set; } = type;
            public short Amount { get; set; } = amount;
            public static Instruction From(string[] input)
            {
                var amount = short.Parse(input[1]);
                return input[0] switch
                {
                    "acc" => new Instruction(InstructionType.Acc, amount),
                    "jmp" => new Instruction(InstructionType.Jmp, amount),
                    "nop" => new Instruction(InstructionType.Nop, amount),
                    _ => throw new Exception()
                };
            }
            public override string ToString() => $"{Type},{Amount}";
        }

        enum StateType
        {
            Infinite,
            Halted
        }
        class State(StateType type, int amount)
        {
            public StateType Type { get; set; } = type;
            public int Amount { get; set; } = amount;
        }

        private static State Execute(List<Instruction> input, int pc, int acc, bool[] visited)
        {
            while(true)
            {
                if(pc >= input.Count) return new State(StateType.Halted, acc);
                else if (visited[pc]) return new State(StateType.Infinite,acc);
                visited[pc] = true;
                switch(input[pc].Type)
                {
                    case InstructionType.Acc:
                        acc += input[pc].Amount;
                        pc++;
                        break;
                    case InstructionType.Jmp:
                        pc += input[pc].Amount;
                        break;
                    case InstructionType.Nop:
                        pc++;
                        break;
                }
            }
        }

        private static List<Instruction> instructions = [];
        private static void Parse() => instructions = input.TrimEnd().Split([' ', '\n', '\t']).Chunk(2).Select(Instruction.From).ToList();

        public static int PartOne()
        {
            Parse();
            var visited = new bool[instructions.Count];
            var exec = Execute(instructions, 0, 0, visited);
            return exec.Type switch
            {
                StateType.Infinite => exec.Amount,
                StateType.Halted => throw new NotImplementedException(),
                _ => throw new NotImplementedException(),
            };
        }
        public static int PartTwo()
        {
            var pc = 0;
            var acc = 0;
            var visited = new bool[instructions.Count];

            while (true)
            {
                switch (instructions[pc].Type)
                {
                    case InstructionType.Acc:
                        acc += instructions[pc].Amount;
                        pc++;
                        break;
                    case InstructionType.Jmp:
                        var speculate = pc + 1;
                        var exec = Execute(instructions, speculate, acc, visited);
                        switch (exec.Type)
                        {
                            case StateType.Infinite:
                                pc += instructions[pc].Amount;
                                break;
                            case StateType.Halted: return exec.Amount;
                        }
                        break;
                    case InstructionType.Nop:
                        var speculative = pc + instructions[pc].Amount;
                        var execNop = Execute(instructions, speculative, acc, visited);
                        switch (execNop.Type)
                        {
                            case StateType.Infinite:
                                pc++;
                                break;
                            case StateType.Halted: return execNop.Amount;
                        }
                        break;

                }
            }
        }
    }
}
