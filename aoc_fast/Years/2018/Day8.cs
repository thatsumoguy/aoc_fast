using aoc_fast.Extensions;

namespace aoc_fast.Years._2018
{
    internal class Day8
    {
        public static string input
        {
            get;
            set;
        }

        private static (int partOne, int partTwo) answers;
        private static void Parse() => answers = ParseNode(input.ExtractNumbers<int>().GetEnumerator(), []);

        private static (int partOne, int PartTwo) ParseNode(IEnumerator<int> iter, List<int> stack)
        {
            iter.MoveNext();
            var childCount = iter.Current;
            iter.MoveNext();
            var metaDataCount = iter.Current;
            var metadata = 0;
            var score = 0;

            for(var _ =0; _ < childCount; _++)
            {
                var (first, second) = ParseNode(iter, stack);
                metadata += first;
                stack.Add(second);
            }


            for(var _= 0; _ < metaDataCount; _++)
            {
                iter.MoveNext();
                var n = iter.Current;
                metadata += n;
                if (childCount == 0) score += n;
                else if (n > 0 && n <= childCount) score += stack[stack.Count - childCount + (n - 1)];
            }

            stack.RemoveRange(stack.Count - childCount, childCount);
            return (metadata, score);
        }

        public static int PartOne()
        {
            Parse();
            return answers.partOne;
        }

        public static int PartTwo() => answers.partTwo;
    }
}
