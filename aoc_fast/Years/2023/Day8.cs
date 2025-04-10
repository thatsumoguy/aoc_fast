using aoc_fast.Extensions;

namespace aoc_fast.Years._2023
{
    internal class Day8
    {
        public static string input { get; set; }
        private static (ulong partOne, ulong partTwo) answers;

        private static void Parse()
        {
            var lines = input.Split("\n", StringSplitOptions.RemoveEmptyEntries);
            var nodes = new Dictionary<string, string[]>(lines.Length);
            foreach (var line in lines[1..]) nodes.Add(line[..3], [line[7..10], line[12..15]]);
            var partOne = (ulong)lines[0].Length;
            var partTwo = (ulong)lines[0].Length;

            var todo = new Queue<(string, ulong)>();
            var seen = new HashSet<string>();
            foreach(var start in nodes.Keys.Where(k => k.EndsWith('A')))
            {
                todo.Enqueue((start, 0));
                seen.Add(start);

                while(todo.TryDequeue(out var i))
                {
                    var (node, cost) = i;
                    if (node.EndsWith('Z'))
                    {
                        if (start == "AAA") partOne = partOne.lcm(cost);
                        partTwo = partTwo.lcm(cost);
                        break;
                    }
                    foreach(var next in nodes[node])
                    {
                        if(seen.Add(next)) todo.Enqueue((next, cost + 1));  
                    }
                }
                todo.Clear();
                seen.Clear();
            }
            answers = (partOne, partTwo);
        }
        public static ulong PartOne()
        {
            Parse();
            return answers.partOne;
        }
        public static ulong PartTwo() => answers.partTwo;
    }
}
