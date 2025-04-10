using aoc_fast.Extensions;

namespace aoc_fast.Years._2021
{
    internal class Day4
    {
        public static string input { get; set; }
        private static List<(long turn, long score)> Scores = [];

        private static void Parse()
        {
            var toTurn = new long[100];
            var fromTurn = new long[100];

            var chunks = input.Split("\n\n").ToList();

            foreach(var (i, n) in chunks[0].ExtractNumbers<long>().Index())
            {
                toTurn[n] = i;
                fromTurn[i] = n;
            }

            var score = (string chunk) =>
            {
                var iter = chunk.ExtractNumbers<long>().GetEnumerator();
                var board = new long[25];
                var i = 0;
                while (iter.MoveNext())
                {
                    board[i] = (long)iter.Current;
                    i++;
                }
                var turns = new long[25];
                for(var j = 0; j < 25; j++) turns[j] = toTurn[board[j]];

                long[] rowsAndCols = 
                [
                    turns[0..5].Max(),
                    turns[5..10].Max(),
                    turns[10..15].Max(),
                    turns[15..20].Max(),
                    turns[20..25].Max(),
                    turns.StepBy(5).Max(),
                    turns.Skip(1).StepBy(5).Max(),
                    turns.Skip(2).StepBy(5).Max(),
                    turns.Skip(3).StepBy(5).Max(),
                    turns.Skip(4).StepBy(5).Max(),
                ];
                var winningTurn = rowsAndCols.Min();
                var unMarked = board.Where(n => toTurn[n] > winningTurn).Sum();
                var justCalled = fromTurn[winningTurn];

                return (winningTurn, unMarked * justCalled);
            };
            var scores = chunks[1..].Select(score).ToList();
            scores = [.. scores.OrderBy(s => s.winningTurn)];
            Scores = scores;
        }
        public static long PartOne()
        {
            Parse();
            return Scores[0].score;
        }
        public static long PartTwo() => Scores[^1].score;
    }
}
