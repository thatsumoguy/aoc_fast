using System.Text;

namespace aoc_fast.Years._2020
{
    internal class Day2
    {
        public static string input { get; set; }

        class Rule(uint start, uint end, byte letter, byte[] password)
        {
            public uint Start = start;
            public uint End = end;
            public byte letter = letter;
            public byte[] Password = password;

            public static Rule From(string[] input)
            {
                var start = uint.Parse(input[0]);
                var end = uint.Parse(input[1]);
                var letter = Encoding.UTF8.GetBytes(input[2])[0];
                var password = Encoding.UTF8.GetBytes(input[3]);
                return new Rule(start, end, letter, password);
            }
        }
        private static List<Rule> rules = [];
        private static void Parse() => rules = input.Split(['-', ':', ' ', '\n']).Where(s => !string.IsNullOrEmpty(s)).Chunk(4).Select(Rule.From).ToList();

        public static int PartOne()
        {
            Parse();
            return rules.Where(rule =>
            {
                var count = rule.Password.Where(l => l == rule.letter).Count();
                return rule.Start <= count && count <= rule.End;
            }).Count();
        }
        public static int PartTwo() => rules.Where(rule =>
        {
            var first = rule.Password[rule.Start - 1] == rule.letter;
            var second = rule.Password[rule.End  - 1] == rule.letter;
            return first ^ second;
        }).Count();
    }
}
