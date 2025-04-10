using aoc_fast.Extensions;

namespace aoc_fast.Years._2016
{
    class Day20
    {
        public static string input
        {
            get;
            set;
        }

        private static List<ulong[]> Ranges = [];

        private static void Parse()
        {
            var ranges = input.Replace('-', ' ').ExtractNumbers<ulong>().Chunk(2).ToList();
            ranges.Sort((X,y) => X[0].CompareTo(y[0]));
            Ranges = ranges;
        }

        public static ulong PartOne()
        {
            try
            {
                Parse();
                var index = 0UL;

                foreach (var r in Ranges)
                {
                    if (index < r[0]) return index;

                    index = Math.Max(index, r[1] + 1);
                }
            }
            catch(Exception ex) { Console.WriteLine(ex); }
            throw new Exception();
        }
        public static ulong PartTwo()
        {
            var index = 0UL;
            var total = 0UL;

            foreach(var r in Ranges)
            {
                if(index <  r[0]) total += r[0] - index;
                index = Math.Max(index, r[1] + 1);
            }

            return total;
        }
    }
}
