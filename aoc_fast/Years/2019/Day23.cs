using aoc_fast.Extensions;

namespace aoc_fast.Years._2019
{
    internal class Day23
    {
        public static string input { get; set; }
        private static (long partOne, long partTwo) answers;

        private static void Parse()
        {
            var code = input.ExtractNumbers<long>().ToArray();
            var network = Enumerable.Range(0, 50).Select(address =>
            {
                var comp = new Computer(code);
                comp.Input(address);
                return comp;
            }).ToList();

            var sent = new List<long>();
            var natX = 0L;
            var natY = 0L;
            long? firstY = null;
            long? idleY = null;
            while (true) 
            {
                var index = 0;
                var empty = 0;

                while(index < 50)
                {
                    var comp = network[index];
                    switch(comp.Run(out var val))
                    {
                        case State.Output:
                            sent.Add(val);
                            if (sent.Count < 3) continue;
                            else
                            {
                                var (address, X, y) = (sent[0], sent[1], sent[2]);
                                sent.Clear();
                                if(address == 255)
                                {
                                    firstY ??= y;
                                    natX = X;
                                    natY = y;
                                }
                                else
                                {
                                    var destination = network[(int)address];
                                    destination.Input(X);
                                    destination.Input(y);
                                    network[(int)address] = destination;
                                }
                            }
                            break;
                        case State.Input:
                            empty++;
                            comp.Input(-1);
                            break;
                        case State.Halted: throw new Exception();
                    }
                    index++;
                }
                if(empty == 50)
                {
                    if (idleY == natY) break;
                    idleY = natY;
                    var destination = network[0];
                    destination.Input(natX);
                    destination.Input(natY);
                    network[0] = destination;
                }
            }
            answers = (firstY.Value, idleY.Value);
        }
        public static long PartOne()
        {
            Parse();
            return answers.partOne;
        }
        public static long PartTwo() => answers.partTwo;
    }
}
