using aoc_fast.Extensions;

namespace aoc_fast.Years._2017
{
    class Day2
    {
        public static string input
        {
            get;
            set;
        }

        private static List<List<int>> Values = [];

        private static void Parse()
        {
            Values = input.Split("\n", StringSplitOptions.RemoveEmptyEntries).Select(l => 
            {
                var values = l.ExtractNumbers<int>();
                values.Sort();
                return values;
            }).ToList();
        }

        public static int PartOne()
        {
            Parse();
            return Values.Select(v => v[v.Count - 1] - v[0]).Sum();
        }

        public static int PartTwo() =>
            Values.Select(v => 
            {
                for(var i = 0; i < Values.Count; i++)
                {
                    for(var j = i + 1;  j < Values.Count; j++)
                    {
                        if (v[j] % v[i] == 0) return v[j] / v[i];
                    }
                }
                throw new Exception();
            }).Sum();
    }
}
