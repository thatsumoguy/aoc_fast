using aoc_fast.Extensions;

namespace aoc_fast.Years._2018
{
    internal class Day11
    {
        public static string input
        {
            get;
            set;
        }

        public class Result
        {
            public int X { get; set; }
            public int Y { get; set; }
            public int Size { get; set; }
            public int Power { get; set; }
        }
        private static List<Result> answer = [];
        public class Shared(int[,] sat)
        {
            public int[,] Sat { get; } = sat;
            public List<Result> Results { get; } = [];
        }

        private static (int power, int X, int y) Square(int[,] sat, int size)
        {
            int maxPower = int.MinValue, maxX = 0, maxY = 0;

            for (int y = size; y < 301; y++)
            {
                for (int X = size; X < 301; X++)
                {
                    int power = sat[X, y] - sat[X - size, y] - sat[X, y - size] + sat[X - size, y - size];
                    if (power > maxPower)
                    {
                        maxPower = power;
                        maxX = X - size + 1;
                        maxY = y - size + 1;
                    }
                }
            }

            return (maxPower, maxX, maxY);
        }

        private static void Worker(Shared shared, List<int> batch)
        {
            var results = new List<Result>();
            foreach (var size in batch)
            {
                var (power, X, y) = Square(shared.Sat, size);
                results.Add(new Result { X = X, Y = y, Size = size, Power = power });
            }

            lock (shared.Results)
            {
                shared.Results.AddRange(results);
            }
        }

        private static void Parse()
        {
            var gridSerialNumber = int.Parse(input.Trim());

            // Build Summed-area table (SAT)
            var sat = new int[301, 301];
            for (int y = 1; y < 301; y++)
            {
                for (int X = 1; X < 301; X++)
                {
                    int rackId = X + 10;
                    int powerLevel = ((rackId * y + gridSerialNumber) * rackId / 100 % 10) - 5;
                    sat[X, y] = powerLevel + sat[X - 1, y] + sat[X, y - 1] - sat[X - 1, y - 1];
                }
            }

            // Shared state with a mutex for thread safety
            var shared = new Shared(sat);
            var sizes = Enumerable.Range(1, 300).ToList();

            // Spawn batches of work
            Threads.SpawnBatches(sizes, batch => Worker(shared, batch));
            answer = shared.Results;
        }

        public static string PartOne()
        {
            Parse();
            var res = answer.First(r => r.Size == 3);
            return $"{res.X},{res.Y}";
        }

        public static string PartTwo()
        {
            var res = answer.MaxBy(r => r.Power);
            return $"{res.X},{res.Y},{res.Size}";
        }
    }
}
