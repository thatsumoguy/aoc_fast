namespace aoc_fast.Years._2021
{
    internal class Day2
    {
        public static string input { get; set; }
        record Sub()
        {
            public record Up(int a) : Sub;
            public record Down(int a) : Sub;
            public record Forward(int a) : Sub; 
        }

        private static List<Sub> Subs = [];
        private static void Parse()
        {
            static Sub helper(string[] parts)
            {
                var (a,b) = (parts[0], parts[1]);
                var amount = int.Parse(b);
                return a switch
                {
                    "up" => new Sub.Up(amount),
                    "down" => new Sub.Down(amount),
                    "forward" => new Sub.Forward(amount),
                };
            };
            Subs = input.TrimEnd().Split([' ', '\n', '\t']).Chunk(2).Select(helper).ToList();
        }

        public static int PartOne()
        {
            Parse();
            var helper = ((int pos, int depth) items, Sub next) => next switch
            {
                Sub.Up(var n) => (items.pos, items.depth - n),
                Sub.Down(var n) => (items.pos, items.depth + n),
                Sub.Forward(var n) => (items.pos + n, items.depth)
            };
            var (pos, depth) = Subs.Aggregate((0, 0), helper);
            return pos * depth;
        }
        public static int PartTwo()
        {
            var helper = ((int pos, int depth, int aim) items, Sub next) => next switch
            {
                Sub.Up(var n) => (items.pos, items.depth, items.aim - n),
                Sub.Down(var n) => (items.pos, items.depth, items.aim + n),
                Sub.Forward(var n) => (items.pos + n, items.depth + items.aim * n, items.aim),
            };
            var (pos, depth, _) = Subs.Aggregate((0, 0, 0), helper);
            return pos * depth;
        }
    }
}
