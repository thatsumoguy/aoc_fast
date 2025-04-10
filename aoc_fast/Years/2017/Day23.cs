using aoc_fast.Extensions;

namespace aoc_fast.Years._2017
{
    internal class Day23
    {
        public static string input
        {
            get;
            set;
        }

        private static uint? Composite(uint n)
        {
            for(var f = 2; f < Math.Sqrt(n); f++)
            {
                if (n % f == 0) return n;
            }
            return null;
        }
        private static uint num;

        public static uint PartOne()
        {
            try
            {
                num = (uint)input.ExtractNumbers<int>()[0];
                return (num - 2) * (num - 2);
            }
            catch (Exception e) { Console.WriteLine(e); }
            return 0;
        }
        public static int PartTwo() => 
            Enumerable.Range(0, 1001)
            .Select(n => Composite(100000 + 100 * num + 17 * (uint)n))
            .Where(n => n.HasValue).Count();
    }
}
