namespace aoc_fast.Years._2017
{
    internal class Day17
    {
        public static string input
        {
            get;
            set;
        }

        private static uint inputNum;

        public static ushort PartOne()
        {
            inputNum = uint.Parse(input);
            var step = inputNum + 1;
            var index = 0u;
            var buffer = new List<ushort>() { 0 };

            for(var n = 0; n < 2017; n++)
            {
                index = (uint)((index + step) % buffer.Count);
                buffer.Insert((int)index, (ushort)(n + 1));
            }

            return buffer[(int)((index + 1) % buffer.Count)];
        }

        public static int PartTwo()
        {
            var step = inputNum + 1;
            var n = 1;
            var index = 0;
            var res = 0;

            while(n <= 50_000_000)
            {
                if (index == 0) res = n;

                var skip = (((n-index) - 1) / step) + 1;
                n += (int)skip;
                index = (int)((index + skip * step) % n);
            }

            return res;
        }
    }
}
