namespace aoc_fast.Years._2016
{
    class Day13
    {
        public static string input
        {
            get;
            set;
        }

        private static (int partOne, int partTwo) answer = (0, 0);

        private static void Parse()
        {
            var favorite = uint.Parse(input);

            var maze = new bool[52][];
            for (var i = 0; i < 52; i++) maze[i] = new bool[52];

            for(var x = 0u; x < 52; x++)
            {
                for(var y = 0u; y < 52; y++)
                {
                    uint n = x * x + 3 * x + 2 * x * y + y + y * y + favorite;
                    var ones = uint.PopCount(n);
                    maze[x][y] = ones % 2 == 0;
                }
            }
            var partOne = 0;
            var partTwo = 0;
            var todo = new Queue<(int X, int y, int cost)>();

            todo.Enqueue((1, 1, 0));
            maze[1][1] = false;

            while(todo.TryDequeue(out var i))
            {
                if (i.X == 31 && i.y == 39) partOne = i.cost;
                if (i.cost <= 50) partTwo++;
                if(i.X > 0 && maze[i.X - 1][i.y])
                {
                    todo.Enqueue((i.X -1, i.y, i.cost + 1));
                    maze[i.X - 1][i.y] = false;
                }
                if (i.y > 0 && maze[i.X][i.y - 1])
                {
                    todo.Enqueue((i.X, i.y - 1, i.cost + 1));
                    maze[i.X][i.y - 1] = false;
                }
                if (i.X < 51 && maze[i.X + 1][i.y])
                {
                    todo.Enqueue((i.X + 1, i.y, i.cost + 1));
                    maze[i.X + 1][i.y] = false;
                }
                if (i.y < 51 && maze[i.X][i.y + 1])
                {
                    todo.Enqueue((i.X, i.y + 1, i.cost + 1));
                    maze[i.X][i.y + 1] = false;
                }
            }

            answer = (partOne, partTwo);
        }

        public static int PartOne()
        {
            Parse();
            return answer.partOne;
        }
        public static int PartTwo() => answer.partTwo;
    }
}
