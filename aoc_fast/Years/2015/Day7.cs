namespace aoc_fast.Years._2015
{
    class Day7
    {
        public static string input
        {
            get;
            set;
        }
        private static (UInt16 partOne, UInt16 partTwo) res = (0, 0);
        public abstract record Gate
        {
            public record Wire(string Name) : Gate;
            public record Not(string Name) : Gate;
            public record And(string Left, string Right) : Gate;
            public record Or(string Left, string Right) : Gate;
            public record LeftShift(string Name, ushort ShiftAmount) : Gate;
            public record RightShift(string Name, ushort ShiftAmount) : Gate;
        }

        private static void Parse()
        {
            var tokens = input.Split([' ', '\t', '\n', '\r'], StringSplitOptions.RemoveEmptyEntries).GetEnumerator();
            var circuit = new Dictionary<string, Gate>();
            while (tokens.MoveNext())
            {
                var first = tokens.Current!.ToString();
                tokens.MoveNext();
                var second = tokens.Current!.ToString();
                Gate gate = null;
                if (first == "NOT")
                {
                    tokens.MoveNext();
                    gate = new Gate.Not(second);
                }
                else if (second == "->") gate = new Gate.Wire(first);
                else
                {
                    tokens.MoveNext();
                    var third = tokens.Current!.ToString();
                    tokens.MoveNext();

                    switch (second)
                    {
                        case "AND":
                            gate = new Gate.And(first, third);
                            break;
                        case "OR":
                            gate = new Gate.Or(first, third);
                            break;
                        case "LSHIFT":
                            gate = new Gate.LeftShift(first, ushort.Parse(third));
                            break;
                        case "RSHIFT":
                            gate = new Gate.RightShift(first, ushort.Parse(third));
                            break;
                    }
                }

                tokens.MoveNext();
                var wire = tokens.Current!.ToString();
                circuit.Add(wire, gate);
                
            }

            var cache = new Dictionary<string, ushort>();
            var res1 = Signal("a", circuit, cache);
            cache.Clear();
            cache.Add("b", res1);
            var res2 = Signal("a", circuit, cache);

            res = (res1, res2);
        }

        private static ushort Signal(string key, Dictionary<string, Gate> circuit, Dictionary<string, ushort> cache)
        {
            if (cache.TryGetValue(key, out ushort value)) return value;

            if (ushort.TryParse(key, out ushort constant)) return constant;
            var res = circuit[key] switch
            {
                Gate.Wire wire => Signal(wire.Name, circuit, cache),
                Gate.Not not => (ushort)~Signal(not.Name, circuit, cache),
                Gate.And and => (ushort)(Signal(and.Left, circuit, cache) & Signal(and.Right, circuit, cache)),
                Gate.Or or => (ushort)(Signal(or.Left, circuit, cache) | Signal(or.Right, circuit, cache)),
                Gate.LeftShift leftShift => (ushort)(Signal(leftShift.Name, circuit, cache) << leftShift.ShiftAmount),
                Gate.RightShift rightShift => (ushort)(Signal(rightShift.Name, circuit, cache) >> rightShift.ShiftAmount),
                _ => throw new InvalidOperationException("Unexpected gate type")
            };
            cache.Add(key, res);
            return res;
        }


        public static ushort PartOne()
        {
            Parse();
            return res.partOne;
        }
        public static ushort PartTwo() => res.partTwo;
    }
}
