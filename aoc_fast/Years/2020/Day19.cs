using System.Text;
using aoc_fast.Extensions;

namespace aoc_fast.Years._2020
{
    internal class Day19
    {
        public static string input { get; set; }
        record Rule
        {
            public record Letter(byte B) : Rule;
            public record Follow(int A) : Rule;
            public record Choice (int A, int B) : Rule;
            public record Sequence(int A, int B) : Rule;
            public record Compound(int A, int B, int C, int D) : Rule;
        }
        private static (Rule[] rules, List<byte[]> messages) RulesAndMessages = ([], []);

        private static bool Check(Rule[] rules, int rule, byte[] message, int index, out int answer)
        {
            var ans = 0;
            var apply = (int a) => Check(rules, a, message, index, out ans);
            var sequence = (int a, int b) => apply(a) ? Check(rules, b, message, ans, out ans) : false;
            switch(rules[rule])
            {
                case Rule.Letter(var b):
                    if (index < message.Length && message[index] == b)
                    {
                        answer = index + 1;
                        return true;
                    }
                    answer = default;
                    return false;
                case Rule.Follow(var a):
                    if(apply(a))
                    {
                        answer = ans;
                        return true;
                    }
                    answer = default; 
                    return false;
                case Rule.Choice(var a, var  b):
                    if(apply(a))
                    {
                        answer = ans;
                        return true;
                    }
                    else if(apply(b))
                    {
                        answer = ans;
                        return true;
                    }
                    answer = default; 
                    return false;
                case Rule.Sequence(var a, var b):
                    if(sequence(a,b))
                    {
                        answer = ans;
                        return true;
                    }
                    answer = default;
                    return false;
                case Rule.Compound(var a, var b, var c, var d):
                    if(sequence(a,b))
                    {
                        answer = ans;
                        return true;
                    }
                    else if(sequence(c,d))
                    {
                        answer = ans;
                        return true;
                    }
                    answer = default;
                    return false;
            }
            throw new Exception();
        }
        private static void Parse()
        {
            var parts = input.TrimEnd().Split("\n\n");
            var (prefix, suffix) = (parts[0], parts[1]);
            var tokens = new List<int>();
            var rules = new Rule[640];
            Array.Fill(rules, new Rule.Letter(0));
            foreach(var line in prefix.Split("\n"))
            {
                tokens.AddRange(line.ExtractNumbers<int>());
                rules[tokens[0]] = tokens[1..] switch
                {
                    [] when line.Contains('a') => new Rule.Letter((byte)'a'),
                    [] => new Rule.Letter((byte)'b'),
                    [var a] => new Rule.Follow(a),
                    [var a, var b] when line.Contains('|') => new Rule.Choice(a, b),
                    [var a, var b] => new Rule.Sequence(a, b),
                    [var a, var b, var c, var d] => new Rule.Compound(a, b, c, d),
                    _ => throw new Exception()
                };
                tokens.Clear();
            }
            var messages = suffix.Split("\n").Select(Encoding.UTF8.GetBytes).ToList();
            RulesAndMessages = (rules, messages);
        }

        public static int PartOne()
        {
            Parse();
            var (rules, messages) = RulesAndMessages;
            return messages.Where(message => Check(rules, 0, message, 0, out var answer) && answer == message.Length).Count();
        }
        public static int PartTwo()
        {
            var (rules, messages) = RulesAndMessages;
            var predicate = (byte[] message) =>
            {
                var index = 0;
                var first = 0;
                var second = 0;
                while(Check(rules, 42, message, index, out var next))
                {
                    index = next;
                    first++;
                }
                if(first >= 2)
                {
                    while(Check(rules, 31, message, index, out var next))
                    {
                        index = next;
                        second++;
                    }
                }
                return index == message.Length && second >= 1 && (first > second);
            };
            return messages.Where(predicate).Count();
        }
    }
}
