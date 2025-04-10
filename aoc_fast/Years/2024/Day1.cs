namespace aoc_fast.Years._2024
{
    internal class Day1
    {
        public static string input
        {
            get;
            set;
        }

        private static List<int> Left = [];
        private static List<int> Right = [];

        private static void Parse()
        {
            var joined = input.Split(["\n", "   "], StringSplitOptions.RemoveEmptyEntries).Chunk(2).Select(c => (int.Parse(c[0]), int.Parse(c[1]))).ToList();
            foreach (var (l, r) in joined)
            {
                Left.Add(l);
                Right.Add(r);
            }
        }

        public static long PartOne()
        {
            Parse();
            var res = 0L;
            var tempL = Left.ToArray();
            var tempR = Right.ToArray();
            Array.Sort(tempL);
            Array.Sort(tempR);
            for (var i = 0; i < tempL.Length; i++)
            {
                res += Math.Abs(tempL[i] - tempR[i]);
            }
            return res;
        }

        public static long PartTwo()
        {
            var freqs = new int[100000];
            var res = 0L;
            for (var i = 0; i < 100000; ++i) freqs[i] = -1;
            foreach (var r in Right)
            {
                if (freqs[r] != -1) freqs[r]++;
                else freqs[r] = 1;
            }
            foreach (var l in Left)
            {
                if (freqs[l] != -1) res += l * freqs[l];

            }
            return res;
        }
    }
}
