using aoc_fast.Extensions;

namespace aoc_fast.Years._2021
{
    internal class Day21
    {
        public static string input { get; set; }
        private static ((ulong, ulong), (ulong, ulong)) State;
        private static readonly (ulong, ulong)[] DIRAC = [(3,1), (4,3), (5, 6), (6, 7), (7, 6), (8, 3), (9, 1)];

        private static (ulong, ulong) Dirac(((ulong, ulong), (ulong, ulong)) state, (ulong, ulong)?[] cache)
        {
            var ((playerPos, playerScore), (otherPos, otherScore)) = state;
            var index = playerPos + 10 * otherPos + 100 * playerScore + 2100 * otherScore;

            var res = cache[index];
            if(res.HasValue) return res.Value;

            var helper = ((ulong win, ulong lose) winLose, (ulong dice, ulong freq) diceFreq) =>
            {
                var nextPos = (playerPos + diceFreq.dice) % 10;
                var nextScore = playerScore + nextPos + 1;
                if(nextScore >= 21) return (winLose.win + diceFreq.freq, winLose.lose);
                else
                {
                    var nextState = ((otherPos, otherScore), (nextPos, nextScore));
                    var (nextLose, nextWin) = Dirac(nextState, cache);
                    return ((winLose.win + diceFreq.freq * nextWin, winLose.lose + diceFreq.freq * nextLose));
                }
            };

            var result = DIRAC.Aggregate((0ul, 0ul), helper);
            cache[index] = result;
            return result;
        }

        private static void Parse()
        {
            var parts = input.ExtractNumbers<uint>().Chunk(4).ToArray()[0];
            State = ((parts[1] - 1, 0u), (parts[3] - 1, 0));
        }
        
        public static ulong PartOne()
        {
            Parse();

            var state = State;
            var dice = 6ul;
            var rolls = 0ul;

            while(true)
            {
                var ((playerPos, playerScore), (otherPos, otherScore)) = state;
                var nextPos = (playerPos + dice) % 10;
                var nextScore = playerScore + nextPos + 1u;

                dice = (dice + 9) % 10;
                rolls += 3;

                if (nextScore >= 1000) return otherScore * rolls;

                state = ((otherPos, otherScore), (nextPos, nextScore));
            }
        }

        public static ulong PartTwo()
        {
            var cache = new (ulong, ulong)?[44100];
            for (var i = 0; i < 44100; i++) cache[i] = null;
            var (win, lose) = Dirac(State, cache);
            return win.Max(lose);
        }
    }
}
