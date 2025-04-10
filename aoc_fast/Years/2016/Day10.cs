namespace aoc_fast.Years._2016
{
    class Day10
    {
        private static (int partOne, int partTwo) answer = (0, 0);
        public static string input
        {
            get;
            set;
        }

        class Bot
        {
            public (string kind, string index) Low { get; set; }
            public (string kind,string index) High { get; set; }
            public int[] Chips { get; set; }
            public int Amount { get; set; }
        }

        private static void Parse()
        {
            var tokens = input.Split([' ', '\n', '\t'], StringSplitOptions.RemoveEmptyEntries);
            var todo = new Queue<((string kind, string index), int val)>();

            var bots = new Dictionary<int, Bot>();

            var partOne = int.MaxValue;
            var partTwo = 1;

            while(tokens.Length > 0)
            {
                if(tokens[0] == "value")
                {
                    var val = int.Parse(tokens[1]);
                    var dest = (tokens[4], tokens[5]);

                    tokens = tokens[6..];
                    todo.Enqueue((dest, val));
                }
                else
                {
                    var key = int.Parse(tokens[1]);
                    var low = (tokens[5],tokens[6]);
                    var high = (tokens[10],tokens[11]);

                    tokens = tokens[12..];
                    bots.Add(key, new Bot { Low = low, High = high, Chips = new int[2], Amount = 0 });
                }
            }
            while(todo.TryDequeue(out var i))
            {
                var kind = i.Item1.kind;
                var index = int.Parse(i.Item1.index);
                var val = i.val;

                if (kind == "bot")
                {
                    if (bots.TryGetValue(index, out var bot))
                    {
                        bot.Chips[bot.Amount] = val;
                        bot.Amount++;

                        if (bot.Amount == 2)
                        {
                            var min = Math.Min(bot.Chips[0], bot.Chips[1]);
                            var max = Math.Max(bot.Chips[0], bot.Chips[1]);

                            todo.Enqueue((bot.Low, min));
                            todo.Enqueue((bot.High, max));

                            if (min == 17 && max == 61) partOne = index;
                        }
                    }
                }
                else if (index <= 2) partTwo *= val;
            }

            answer = (partOne,  partTwo);
        }

        public static int PartOne()
        {
            Parse();
            return answer.partOne;
        }
        public static int PartTwo() => answer.partTwo;
    }
}
