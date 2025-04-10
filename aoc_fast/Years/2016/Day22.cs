using aoc_fast.Extensions;

namespace aoc_fast.Years._2016
{
    class Day22
    {
        record Node(int X, int y, int used, int available);

        public static string input
        {
            get;
            set;
        }

        private static List<Node> Nodes = [];

        private static void Parse() => 
            Nodes = input.ExtractNumbers<int>().Chunk(6)
            .Select(c => 
            c switch { var a => new Node(a[0], a[1], a[3], a[4]) }).ToList();
        public static int PartOne()
        {
            Parse();
            var used = Nodes.Select(n => n.used).Where(n => n > 0).ToList();
            used.Sort();

            var avail = Nodes.Select(n => n.available).ToList();
            avail.Sort();

            var i = 0;

            var viable = 0;

            foreach(var next in used)
            {
                while (i < avail.Count && avail[i] < next) i++;
                viable += avail.Count - i;
            }

            return viable;
        }

        public static int PartTwo()
        {
            var width = 0;
            var emptyX = 0;
            var emptyY = 0;
            var wallX = int.MaxValue;

            foreach(var (X, y, used, _) in Nodes)
            {
                width = Math.Max(width, X + 1);
                if(used == 0)
                {
                    emptyX = X;
                    emptyY = y;
                }

                if (used >= 100) wallX = Math.Min(wallX, X - 1);
            }

            var a = emptyX - wallX;
            var b = emptyY;
            var c = width - 2 - wallX;
            var d = 1;
            var e = 5 * (width - 2);


            return a + b + c + d + e;
        }
    }
}
