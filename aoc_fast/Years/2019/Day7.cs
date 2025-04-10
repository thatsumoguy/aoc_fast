using aoc_fast.Extensions;

namespace aoc_fast.Years._2019
{
    internal class Day7
    {
        public static string input { get; set; }

        private static List<long> nums = [];
        private static void Parse() => nums = input.ExtractNumbers<long>();

        public static long PartOne()
        {
            Parse();
            var res = 0l;
            var computer = new Computer(nums);
            var seq = (List<long> slice) =>
            {
                var signal = 0l;

                foreach (var phase in slice)
                {
                    computer.Reset();
                    computer.Input(phase);
                    computer.Input(signal);

                    switch (computer.Run(out var next))
                    {
                        case State.Output:
                            signal = next;
                            break;
                    }
                }
                res = Math.Max(res, signal);
            };
            long[] n = [0, 1, 2, 3, 4];
            n.Permutations<long>(seq);
            return res;
        }

        public static long PartTwo()
        {
            var res = 0l;
            Computer[] computers = [new Computer(nums), new Computer(nums), new Computer(nums), new Computer(nums), new Computer(nums)];
            var feedback = (List<long> slice) =>
            {
                foreach(var comp in computers) comp.Reset();
                foreach(var (comp, phase) in computers.Zip(slice)) comp.Input(phase);
                var signal = 0l;

                while(true)
                {
                    foreach(var comp in computers)
                    {
                        comp.Input(signal);
                        switch (comp.Run(out var next))
                        {
                            case State.Output:
                                signal = next; break;
                            default: goto outer;
                        }
                    }
                }
                outer:
                res = Math.Max(res, signal);

            };
            long[] n = [5, 6, 7, 8, 9];
            n.Permutations(feedback);
            return res;
        }
    }
}
