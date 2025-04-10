using System.Text;

namespace aoc_fast.Years._2020
{
    internal class Day14
    {
        public static string input { get; set; }
        enum InstructionType
        {
            Mask,
            Mem
        }
        class Instruction(InstructionType type, ulong a, ulong b)
        {
            public InstructionType Type { get;set; } = type;
            public ulong A { get; set; } = a;
            public ulong B { get; set; } = b;
            public static Instruction Mask(string pattern)
            {
                var ones = 0ul;
                var xs = 0ul;

                foreach (var b in Encoding.ASCII.GetBytes(pattern))
                {
                    ones <<= 1;
                    xs <<= 1;
                    switch (b)
                    {
                        case (byte)'1':
                            ones |= 1;
                            break;
                        case (byte)'X':
                            xs |= 1;
                            break;
                    }
                }
                return new Instruction(InstructionType.Mask, ones, xs);
            }
        }
        class Set(ulong ones, ulong floating, ulong weight)
        {
            public ulong Ones { get; set; } = ones;
            public ulong Floating { get; set; } = floating;
            public ulong Weight { get; set; } = weight;

            public static Set From(ulong address, ulong value, ulong ones, ulong floating) => new((address | ones) & ~floating, floating, value);
            public bool InterSect(Set other, out Set set)
            {
                var disjoint = (Ones ^ other.Ones) & ~(Floating | other.Floating);
                if(disjoint == 0)
                {
                    set = new(Ones | other.Ones, Floating & other.Floating, 0);
                    return true;
                }
                set = this;
                return false;
            }
            public long Size() => 1 << (int)ulong.PopCount(Floating);
        }
        
        private static long SubSets(Set cube, long sign, Set[] candidates)
        {
            var total = 0L;
            foreach(var (i, other) in candidates.Index())
            {
                if (cube.InterSect(other, out var next)) total += sign * next.Size() + SubSets(next, -sign, candidates[(i + 1)..]);
            }
            return total;
        }


        private static List<Instruction> Instructions = [];

        private static void Parse()
        {
            var instructions = new List<Instruction>();

            foreach(var line in input.TrimEnd().Split('\n'))
            {
                Instruction instruction;
                if (line.Length == 43) instruction = Instruction.Mask(line[7..]);
                else
                {
                    var split = line[4..].Split("] = ");
                    var (address, value) = (split[0], split[1]);
                    var add = ulong.Parse(address);
                    var val = ulong.Parse(value);
                    instruction = new Instruction(InstructionType.Mem, add, val);
                }
                instructions.Add(instruction);
            }
            Instructions = instructions;
        }
        public static ulong PartOne()
        {
            Parse();
            var set = 0ul;
            var keep = 0ul;
            var memory = new Dictionary<ulong, ulong>();

            foreach(var instruction in  Instructions)
            {
                switch(instruction.Type)
                {
                    case InstructionType.Mask:
                        set = instruction.A;
                        keep = instruction.A | instruction.B;
                        break;
                    case InstructionType.Mem:
                        memory[instruction.A] = (instruction.B | set) & keep;
                        break;
                }
            }
            return memory.Values.Aggregate(0ul,  (acc, i) => acc + i);
        }

        public static ulong PartTwo()
        {
            var ones = 0ul;
            var floating = 0ul;
            var sets = new List<Set>();

            foreach(var instruction in Instructions)
            {
                switch (instruction.Type)
                {
                    case InstructionType.Mask:
                        ones = instruction.A;
                        floating = instruction.B;
                        break;
                    case InstructionType.Mem:
                        sets.Add(Set.From(instruction.A, instruction.B, ones, floating));
                        break;
                }
            }

            var total = 0ul;
            var candidates = new List<Set>();

            foreach (var (i, set) in sets.Index())
            {
                sets[(i + 1)..].Select(other =>
                {
                    if (set.InterSect(other, out var s)) return s;
                    return null;
                }).Where(s => s != null)
                .ToList()
                .ForEach(next => candidates.Add(next));
                var size = set.Size() + SubSets(set, -1, [.. candidates]);

                total += (ulong)size * set.Weight;
                candidates.Clear();
            }
            return total;
        }
    }
}
