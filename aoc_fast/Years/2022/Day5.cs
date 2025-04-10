using System.Text;

namespace aoc_fast.Years._2022
{
    internal class Day5
    {
        public static string input { get; set; }
        private static (List<List<char>> stack, List<int[]> move) Input = ([], []);

        private static string Play((List<List<char>> stack, List<int[]> move) input, bool reverse = true)
        {
            var stack = new List<List<char>>(input.stack.Count);
            foreach (var s in input.stack) stack.Add(new List<char>(s));

            var crates = new List<char>();

            foreach (var move in input.move)
            {
                var amount = move[0];
                var from = move[1];
                var to = move[2];

                if (from >= stack.Count || to >= stack.Count) continue; 

                var sourceStack = stack[from];
                var startIndex = sourceStack.Count - amount;

                if (startIndex < 0) continue;

                crates.Clear();
                for (int i = 0; i < amount; i++) crates.Add(sourceStack[startIndex + i]);

                sourceStack.RemoveRange(startIndex, amount);

                if (reverse)
                {
                    for (int i = amount - 1; i >= 0; i--) stack[to].Add(crates[i]);
                }
                else
                {
                    for (int i = 0; i < amount; i++) stack[to].Add(crates[i]);
                }
            }

            var result = new StringBuilder(stack.Count);
            foreach (var s in stack)
            {
                if (s.Count > 0) result.Append(s[^1]);
            }

            return result.ToString();
        }

        private static void Parse()
        {
            var parts = input.Split("\n\n");
            var prefix = parts[0];
            var suffix = parts[1];

            var lines = prefix.Split('\n');
            var width = (lines[0].Length + 1) / 4;

            var stack = new List<List<char>>(width);
            for (int i = 0; i < width; i++) stack.Add([]);

            for (int lineIdx = lines.Length - 2; lineIdx >= 0; lineIdx--)
            {
                var row = lines[lineIdx];

                for (int stackIdx = 0; stackIdx < width; stackIdx++)
                {
                    int charPos = 1 + (stackIdx * 4);
                    if (charPos < row.Length)
                    {
                        char c = row[charPos];
                        if (c >= 'A' && c <= 'Z')  stack[stackIdx].Add(c);
                    }
                }
            }

            var moves = new List<int[]>();
            var currentMove = new int[3];
            var moveIndex = 0;

            var number = 0;
            var inNumber = false;

            for (int i = 0; i < suffix.Length; i++)
            {
                var c = suffix[i];

                if (c >= '0' && c <= '9')
                {
                    if (!inNumber)
                    {
                        inNumber = true;
                        number = 0;
                    }
                    number = number * 10 + (c - '0');
                }
                else if (inNumber)
                {
                    inNumber = false;
                    currentMove[moveIndex++] = number;

                    if (moveIndex == 3)
                    {

                        currentMove[1]--;
                        currentMove[2]--;

                        moves.Add(currentMove);
                        currentMove = new int[3];
                        moveIndex = 0;
                    }
                }
            }

            if (inNumber && moveIndex < 3)
            {
                currentMove[moveIndex] = number;
                moveIndex++;

                if (moveIndex == 3)
                {
                    currentMove[1]--;
                    currentMove[2]--;
                    moves.Add(currentMove);
                }
            }

            Input = (stack, moves);
        }

        public static string PartOne()
        {
            Parse();
            return Play(Input);
        }
        public static string PartTwo() => Play(Input, false);
    }
}
