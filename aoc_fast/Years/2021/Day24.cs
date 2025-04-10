using System.Runtime.CompilerServices;
using aoc_fast.Extensions;

namespace aoc_fast.Years._2021
{
    internal class Day24
    {
        public static string input { get; set; }
        record Block
        {
            public record Push(int A) : Block;
            public record Pop(int A) : Block;
        }
        struct Constaint(int index, int value)
        {
            public int Index { get; set; } = index;
            public int Value { get; set; } = value;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public readonly int Min() => (1 + Value).Max(1);
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public readonly int Max() => (9 + Value).Min(9);
        }

        private static List<Constaint> Constaints = [];

        private static void Parse()
        {
            var lines = input.Split('\n', StringSplitOptions.RemoveEmptyEntries);
            var blocks = lines.Chunk(18).Select<string[], Block>(chunk => 
            {
                var helper = (int i) => int.Parse(chunk[i].Split([' ', '\t', '\n'])[^1]);
                if (helper(4) == 1) return new Block.Push(helper(15));
                return new Block.Pop(helper(5));
            }).ToList();

            var stack = new List<Constaint>();
            var constraints = new List<Constaint>();

            foreach(var (index, block) in blocks.Index())
            {
                switch(block)
                {
                    case Block.Push(var a):
                        stack.Add(new Constaint(index, a));
                        break;
                    case Block.Pop(var a):
                        var first = stack.Pop();
                        var delta = first.Value + a;
                        first.Value = -delta;
                        var second = new Constaint(index, delta);
                        constraints.Add(first);
                        constraints.Add(second);
                        break;

                }
            }

            constraints = [.. constraints.OrderBy(c => c.Index)];
            Constaints = constraints;
        }
        public static string PartOne()
        {
            Parse();
            return string.Join("", Constaints.Select(c => c.Max().ToString()));
        }
        public static string PartTwo() => string.Join("", Constaints.Select(c => c.Min().ToString()));
    }
}
