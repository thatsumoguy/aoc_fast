namespace aoc_fast.Years._2019
{
    public enum State
    {
        Input,
        Output,
        Halted
    }

    public class Computer
    {
        private const int EXTRA = 3000;
        private long pc; // Program counter
        private long baseAddress; // Relative base
        private long[] code;
        private Queue<long> input;

        public Computer(IEnumerable<long> inputCode)
        {
            var inputCodList = inputCode.ToList();
            code = new long[inputCodList.Count + EXTRA];
            for (var i = 0; i < inputCodList.Count; i++) code[i] = inputCodList[i];
            pc = 0;
            baseAddress = 0;
            input = new Queue<long>();
        }

        public void Input(long value)
        {
            input.Enqueue(value);
        }

        public void InputAscii(string ascii)
        {
            foreach (var ch in ascii)
            {
                input.Enqueue((long)ch);
            }
        }

        public void Reset()
        {
            pc = 0;
            baseAddress = 0;
            input.Clear();
        }

        public State Run(out long output, bool test = false)
        {
            output = 0;

            while (true)
            {
                var op = code[pc];
                //if(test)Console.WriteLine($"OP: {op % 100}");
                switch (op % 100)
                {
                    // Add
                    case 1:
                        {
                            var first = Address(op / 100, 1);
                            var second = Address(op / 1000, 2);
                            var third = Address(op / 10000, 3);
                            code[third] = code[first] + code[second];
                            pc += 4;
                            break;
                        }
                    // Multiply
                    case 2:
                        {
                            var first = Address(op / 100, 1);
                            var second = Address(op / 1000, 2);
                            var third = Address(op / 10000, 3);
                            code[third] = code[first] * code[second];
                            pc += 4;
                            break;
                        }
                    // Read input
                    case 3:
                        {
                            if (input.Count == 0)
                                return State.Input;
                            var first = Address(op / 100, 1);
                            code[first] = input.Dequeue();
                            pc += 2;
                            break;
                        }
                    // Write output
                    case 4:
                        {
                            var first = Address(op / 100, 1);
                            output = code[first];
                            pc += 2;
                            return State.Output;
                        }
                    // Jump if true
                    case 5:
                        {
                            var first = Address(op / 100, 1);
                            var second = Address(op / 1000, 2);
                            pc = code[first] != 0 ? code[second] : pc + 3;
                            break;
                        }
                    // Jump if false
                    case 6:
                        {
                            var first = Address(op / 100, 1);
                            var second = Address(op / 1000, 2);
                            pc = code[first] == 0 ? code[second] : pc + 3;
                            break;
                        }
                    // Less than
                    case 7:
                        {
                            var first = Address(op / 100, 1);
                            var second = Address(op / 1000, 2);
                            var third = Address(op / 10000, 3);
                            code[third] = code[first] < code[second] ? 1 : 0;
                            pc += 4;
                            break;
                        }
                    // Equals
                    case 8:
                        {
                            var first = Address(op / 100, 1);
                            var second = Address(op / 1000, 2);
                            var third = Address(op / 10000, 3);
                            code[third] = code[first] == code[second] ? 1 : 0;
                            pc += 4;
                            break;
                        }
                    // Adjust relative base
                    case 9:
                        {
                            var first = Address(op / 100, 1);
                            baseAddress += code[first];
                            pc += 2;
                            break;
                        }
                    // Halt
                    default:
                        return State.Halted;
                }
            }
        }

        private long Address(long mode, long offset)
        {
            return (mode % 10) switch
            {
                0 => code[pc + offset],
                1 => pc + offset,
                2 => baseAddress + code[pc + offset],
                _ => throw new InvalidOperationException("Unknown addressing mode"),
            };
        }
    }
}
