using System.Text;
using aoc_fast.Extensions;

namespace aoc_fast.Years._2021
{
    internal class Day10
    {
        public static string input { get; set; }

        private static ulong SyntaxScore(byte[] line, List<byte> stack)
        {
            foreach (var b in line)
            {
                switch (b)
                {
                    case (byte)'(' or (byte)'[' or (byte)'{' or (byte)'<':
                        stack.Add(b);
                        break;
                    case (byte)')':
                        if (stack.Pop() != (byte)'(') return 3;
                        break;
                    case (byte)']':
                        if (stack.Pop() != (byte)'[') return 57;
                        break;
                    case (byte)'}':
                        if (stack.Pop() != (byte)'{') return 1197;
                        break;
                    case (byte)'>':
                        if (stack.Pop() != (byte)'<') return 25137;
                        break;
                    default: throw new Exception();
                }
            }
            return 0;
        }

        private static ulong AutoCompleteScore(List<byte> stack)
        {
            var helper = (byte b) => b switch
            {
                (byte)'(' => 1ul,
                (byte)'[' => 2ul,
                (byte)'{' => 3ul,
                (byte)'<' => 4ul
            };

            return stack.Reverse<byte>().Aggregate(0ul, (acc, b) => 5 * acc + helper(b));
        }

        private static List<byte[]> bytesOfBytes = [];

        public static ulong PartOne()
        {
            bytesOfBytes = input.TrimEnd().Split('\n').Select(Encoding.ASCII.GetBytes).ToList();

            var stack = new List<byte>();
            var score = 0ul;

            foreach(var line in bytesOfBytes)
            {
                score += SyntaxScore(line, stack);
                stack.Clear();
            }
            return score;
        }

        public static ulong PartTwo()
        {
            var stack = new List<byte>();
            var scores = new List<ulong>();

            foreach(var line in bytesOfBytes)
            {
                if (SyntaxScore(line, stack) == 0) scores.Add(AutoCompleteScore(stack));
                stack.Clear();
            }
            scores.Sort();
            return scores[scores.Count / 2];
        }
    }
}
