using System.Text;

namespace aoc_fast.Years._2022
{
    internal class Day21
    {
        public static string input { get; set; }
        enum Operation
        {
            Add,
            Sub,
            Mul,
            Div
        }
        record Monkey
        {
            public record Number(long a) : Monkey;
            public record Result(int left, Operation op, int right) : Monkey;

            public static Monkey Parse(string str, Dictionary<string, int> indices)
            {
                if (str.Length < 11) return new Monkey.Number(long.Parse(str));
                var left = indices[str[0..4]];
                var right = indices[str[7..11]];
                var operation = Encoding.ASCII.GetBytes(str)[5] switch
                {
                    (byte)'+' => Operation.Add,
                    (byte)'-' => Operation.Sub,
                    (byte)'*' => Operation.Mul,
                    (byte)'/' => Operation.Div,
                };
                return new Monkey.Result(left, operation, right);
            }
        }

        private static (int root, List<Monkey> monkeys, List<long> yell, List<bool> unknown) Input = (default, [], [], []);

        private static long Inverse((int root, List<Monkey> monkeys, List<long> yell, List<bool> unknown) input, int index, long value)
        {
            var (root, monkeys, yell, unkown) = input;

            return monkeys[index] switch
            {
                Monkey.Number(var _) => value,
                Monkey.Result(var left, var _, var right) when index == root => unkown[left] ? Inverse(input, left, yell[right]) : Inverse(input, right, yell[left]),
                Monkey.Result(var left, var operation, var right) => unkown[left] ? operation switch
                {
                    Operation.Add => Inverse(input, left, value - yell[right]),
                    Operation.Sub => Inverse(input, left, value + yell[right]),
                    Operation.Mul => Inverse(input, left, value / yell[right]),
                    Operation.Div => Inverse(input, left, value * yell[right]),
                }: operation switch
                {
                    Operation.Add => Inverse(input, right, value -  yell[left]),
                    Operation.Sub => Inverse(input, right, yell[left] - value),
                    Operation.Mul => Inverse(input, right, value / yell[left]),
                    Operation.Div => Inverse(input, right, yell[left] / value),
                }
            };
        }
        private static long Compute((int root, List<Monkey> monkeys, List<long> yell, List<bool> unknown) input, int index)
        {
            var result = input.monkeys[index] switch
            {
                Monkey.Number(var n) => n,
                Monkey.Result(var left, var operation, var right) => operation switch
                {
                    Operation.Add => Compute(input, left) + Compute(input, right),
                    Operation.Sub => Compute(input, left) - Compute(input, right),
                    Operation.Mul => Compute(input, left) * Compute(input, right),
                    Operation.Div => Compute(input, left) / Compute(input, right),
                }
            };
            input.yell[index] = result;
            return result;
        }

        private static bool Find((int root, List<Monkey> monkeys, List<long> yell, List<bool> unknown) input, int humn, int index)
        {
            var res = input.monkeys[index] switch
            {
                Monkey.Number(var _) => humn == index,
                Monkey.Result(var left, var _, var right) => Find(input, humn, left) || Find(input, humn, right),
            };
            input.unknown[index] = res;
            return res;
        }

        private static void Parse()
        {
            var lines = input.Split("\n", StringSplitOptions.RemoveEmptyEntries);
            var indices = lines.Index().ToDictionary(i => i.Item[0..4], i => i.Index);

            var monkeys = lines.Index().Select(line => Monkey.Parse(line.Item[6..], indices)).ToList();

            var root = indices["root"];
            var humn = indices["humn"];
            var tInput = (root, monkeys, new List<long>(lines.Length), new List<bool>(lines.Length));
            for(var _ = 0; _ < lines.Length; _++)
            {
                tInput.Item3.Add(0);
                tInput.Item4.Add(false);
            }
            Compute(tInput, root);
            Find(tInput, humn, root);
            Input = tInput;
        }

        public static long PartOne()
        {
            Parse();
            return Input.yell[Input.root];
        }
        public static long PartTwo() => Inverse(Input, Input.root, -1);
    }
}
