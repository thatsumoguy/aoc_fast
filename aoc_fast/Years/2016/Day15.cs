using aoc_fast.Extensions;

namespace aoc_fast.Years._2016
{
    class Day15
    {
        public static string input
        {
            get;
            set;
        }

        private static List<(int size, int pos)> Discs = [];

        private static void Parse()
        {
            Discs = input.Split("\n", StringSplitOptions.RemoveEmptyEntries)
                .Select(l => l.ExtractNumbers<int>().Skip(1).ToList() 
                switch { var a => (a[0], a[2])}).ToList();
        }

        private static int Solve(List<(int size, int pos)> discs)
        {
            var time = 0;
            var step = 1;

            foreach(((int size, int pos), int offset) in discs.Select((c,i)=>(c,i)))
            {
                while((time + offset + 1 + pos) % size != 0) time += step;

                step *= size;
            }

            return time;
        }

        public static int PartOne()
        {
            Parse();
            return Solve(Discs);
        }
        public static int PartTwo()
        {
            var modified = Discs.Select(c => c).ToList();
            modified.Add((11, 0));
            return Solve(modified);
        }
    }
}
