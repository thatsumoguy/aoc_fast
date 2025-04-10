using System.Text;
using aoc_fast.Extensions;

namespace aoc_fast.Years._2017
{
    internal class Day25
    {
        public static string input
        {
            get;
            set;
        }
        record InputObj(ulong State, uint Steps, List<Rule[]> Rules);

        class Rule
        {
            public ulong NextState { get; set; }
            public ulong NextTape { get; set; }
            public bool Advance { get; set; }

            public static Rule Parse(byte[][] block)
            {
                var nextTape = (ulong)(block[0][22] - (byte)'0');
                var advance = block[1][27] == (byte)'r';
                var nextState = (ulong)(block[2][26] - (byte)'A');

                return new Rule {NextState = nextState, NextTape = nextTape, Advance = advance };
            }
        }

        record Skip(ulong NextState, ulong NextTape, uint Steps, int Ones, bool Advance);

        private static InputObj inputObj;

        private static void Parse()
        {
            var lines = input.Split("\n").Select(Encoding.ASCII.GetBytes).ToArray();

            var state = (ulong)(lines[0][15] - (byte)'A');
            var steps = input.ExtractNumbers<uint>()[0];
            var rules = lines[3..].Chunk(10).Select(chunk => new Rule[2] { Rule.Parse(chunk[2..5]), Rule.Parse(chunk[6..9]) }).ToList();

            inputObj = new InputObj(state, steps, rules);
        }

        private static Skip Turing(List<Rule[]> rules, ulong state, ulong tape, uint maxSteps)
        {
            var mask = 0b00001000u;
            var steps = 0u;
            var ones = 0;

            while(0 < mask && mask < 128 && steps < maxSteps)
            {
                var current = (tape & mask) != 0 ? (ulong)1 : (ulong)0;
                var rule = rules[(int)state][current];
                if (rule.NextTape == 1) tape |= mask;
                else tape &= ~mask;

                if (rule.Advance) mask >>= 1;
                else mask <<= 1;

                state = rule.NextState;
                steps++;

                ones += (int)rule.NextTape - (int)current;
            }

            return new Skip(state, tape, steps, ones, mask == 0);
        }

        public static int PartOne()
        {
            try
            {
                Parse();
                var table = Enumerable.Range(0, inputObj.Rules.Count).Select(state => Enumerable.Range(0, 256)
                .Select(tape => Turing(inputObj.Rules, (ulong)state, (ulong)tape, 100))
                .ToArray())
                .ToList();

                var state = inputObj.State;
                var remaining = inputObj.Steps;
                var tape = 0ul;
                var checksum = 0;
                var left = new List<ulong>();
                var right = new List<ulong>();


                while (true)
                {
                    var skip = table[(int)state][tape];
                    if (skip.Steps > remaining)
                    {
                        var s = Turing(inputObj.Rules, state, tape, remaining);
                        return checksum + s.Ones;
                    }

                    state = skip.NextState;
                    tape = skip.NextTape;
                    remaining -= skip.Steps;
                    checksum += skip.Ones;
                    if (skip.Advance)
                    {
                        left.Add(tape & 0xf0u);
                        tape = ((tape & 0xfu) << 4) | right.LastOrDefault(0ul);
                        if(right.Count > 0) right.RemoveAt(right.Count - 1);
                    }
                    else
                    {
                        right.Add(tape & 0xfu);
                        tape = (tape >> 4) | left.LastOrDefault(0ul);
                        if(left.Count > 0) left.RemoveAt(left.Count - 1);
                    }
                }
            }
            catch (Exception ex) { Console.WriteLine(ex); }
            return 0;
        }

        public static string PartTwo() => "Merry Christmas";
    }
}
