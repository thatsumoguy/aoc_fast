namespace aoc_fast.Years._2024
{
    internal class Day21
    {
        public static string input
        {
            get;
            set;
        }
        static Dictionary<ulong, long> memo = [];

        static ulong Hash(int curr, int curc, int destr, int destc, int nrobots)
        {
            var result = (ulong)curr;
            result = result * 4 + (ulong)curc;
            result = result * 4 + (ulong)destr;
            result = result * 4 + (ulong)destc;
            result = result * 30 + (ulong)nrobots;
            return result;
        }

        static long CheapestDirPad(int curr, int curc, int destr, int destc, int nrobots)
        {
            var h = Hash(curr, curc, destr, destc, nrobots);
            if (memo.TryGetValue(h, out var value))
                return value;

            var answer = long.MaxValue;
            var queue = new Queue<(int r, int c, string presses)>();
            queue.Enqueue((curr, curc, ""));

            while (queue.Count > 0)
            {
                var (r, c, presses) = queue.Dequeue();

                if (r == destr && c == destc)
                {
                    var rec = CheapestRobot(presses + "A", nrobots - 1);
                    answer = Math.Min(answer, rec);
                    continue;
                }

                if (r == 0 && c == 0)
                    continue;

                if (r < destr)
                    queue.Enqueue((r + 1, c, presses + "v"));
                else if (r > destr)
                    queue.Enqueue((r - 1, c, presses + "^"));

                if (c < destc)
                    queue.Enqueue((r, c + 1, presses + ">"));
                else if (c > destc)
                    queue.Enqueue((r, c - 1, presses + "<"));
            }

            memo[h] = answer;
            return answer;
        }

        static long CheapestRobot(string presses, int nrobots)
        {
            if (nrobots == 1)
                return presses.Length;

            var result = 0L;
            var padConfig = "X^A<v>";

            var curr = 0;
            var curc = 2;

            foreach (var ch in presses)
            {
                for (var nextr = 0; nextr < 2; nextr++)
                {
                    for (var nextc = 0; nextc < 3; nextc++)
                    {
                        if (padConfig[nextr * 3 + nextc] == ch)
                        {
                            result += CheapestDirPad(curr, curc, nextr, nextc, nrobots);
                            curr = nextr;
                            curc = nextc;
                        }
                    }
                }
            }

            return result;
        }

        static long Cheapest(int curr, int curc, int destr, int destc, int robots)
        {
            var answer = long.MaxValue;
            var queue = new Queue<(int r, int c, string presses)>();
            queue.Enqueue((curr, curc, ""));

            while (queue.Count > 0)
            {
                var (r, c, presses) = queue.Dequeue();

                if (r == destr && c == destc)
                {
                    var rec = CheapestRobot(presses + "A", robots);
                    answer = Math.Min(answer, rec);
                    continue;
                }

                if (r == 3 && c == 0)
                    continue;

                if (r < destr)
                    queue.Enqueue((r + 1, c, presses + "v"));
                else if (r > destr)
                    queue.Enqueue((r - 1, c, presses + "^"));

                if (c < destc)
                    queue.Enqueue((r, c + 1, presses + ">"));
                else if (c > destc)
                    queue.Enqueue((r, c - 1, presses + "<"));
            }

            return answer;
        }

        private static (long partOne, long partTwo) answers;

        private static void Parse()
        {
            var codes = input.Split("\n", StringSplitOptions.RemoveEmptyEntries);

            foreach (var code in codes)
            {
                var result1 = 0L;
                var result2 = 0L;
                var padConfig = "789456123X0A";

                var curr = 3;
                var curc = 2;

                foreach (var ch in code)
                {
                    for (var nextr = 0; nextr < 4; nextr++)
                    {
                        for (var nextc = 0; nextc < 3; nextc++)
                        {
                            if (padConfig[nextr * 3 + nextc] == ch)
                            {
                                result1 += Cheapest(curr, curc, nextr, nextc, 3);
                                result2 += Cheapest(curr, curc, nextr, nextc, 26);
                                curr = nextr;
                                curc = nextc;
                            }
                        }
                    }
                }

                var codeValue = int.Parse(code[..3]);
                answers.partOne += result1 * codeValue;
                answers.partTwo += result2 * codeValue;
            }
        }

        public static long PartOne()
        {
            Parse();
            return answers.partOne;
        }
        public static long PartTwo() => answers.partTwo;
    }
}
