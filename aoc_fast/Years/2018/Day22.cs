using aoc_fast.Extensions;

namespace aoc_fast.Years._2018
{
    internal class Day22
    {
        public static string input { get; set; }
        const int TORCH = 1;
        const int BUCKETS = 8;

        class Region
        {
            public int Erosion { get; set; }
            public int[] Mintues { get; set; }
            public Region()
            {
                Erosion = 0;
                Mintues = new int[3];
                Array.Fill(Mintues, int.MaxValue);
            }
            public int Update(int geologic)
            {
                var erosion = geologic % 20183;
                Erosion = erosion;

                Mintues[erosion % 3] = 0;
                return erosion;
            }
        }

        private static Grid<Region> ScanCave(int[] input, int width, int height)
        {
            var (depth, targetY, targetX) = (input[0], input[1], input[2]);
            var target = new Point(targetX, targetY);
            var region = new Region();
            var grid = Grid<Region>.New(width, height, region);

            grid[0, 0].Update(depth);

            for(var X = 1; X < width; X++) grid[X, 0].Update(48271 * X + depth);

            for(var y = 1; y < height; y++)
            {
                var prev = grid[0, y].Update(16807 * y + depth);

                for(var X = 1; X < width; X++)
                {
                    var point = new Point(X, y);
                    if(point == target) grid[point].Update(depth);
                    else
                    {
                        var up = grid[point + Directions.UP].Erosion;
                        prev = grid[point].Update(prev * up + depth);
                    }
                }
            }

            return grid;
        }

        private static int[] inputArray = [];

        private static void Parse() => inputArray = input.ExtractNumbers<int>().Chunk(3).First();

        public static int PartOne()
        {
            Parse();
            var cave = ScanCave(inputArray, inputArray[2] + 1, inputArray[1] + 1);
            return cave.data.Select(r => r.Erosion % 3).Sum();
        }
        public static int PartTwo()
        {
            var target = new Point(inputArray[2], inputArray[1]);
            var baseNum = 0;
            var todo = Slice.RepeatWith(() => new List<(Point, int)>(1000)).Take(BUCKETS).ToList();

            var cave = ScanCave(inputArray, inputArray[2] + 10, inputArray[1] + 140);

            todo[0].Add((Directions.ORIGIN, TORCH));
            cave[Directions.ORIGIN].Mintues[TORCH] = 0;

            while(true)
            {
                while (todo[baseNum % BUCKETS].PopCheck(out var item ))
                {
                    var (point, tool) = item;
                    var time = cave[point].Mintues[tool];
                    if (point == target && tool == TORCH) return time;
                    
                    foreach(var next in Directions.ORTHOGONAL.Select(o => point + o))
                    {
                        if(next.X >= 0 && next.Y >= 0 && time + 1 < cave[next].Mintues[tool])
                        {
                            var heuristic = next.Manhattan(target);
                            var index = time + 1 + heuristic;

                            cave[next].Mintues[tool] = time + 1;
                            todo[index % BUCKETS].Add((next, tool));
                        }
                    }
                    for(var other = 0; other < 3; other++)
                    {
                        if(time + 7 < cave[point].Mintues[other])
                        {
                            var heuristic = point.Manhattan(target);
                            var index = time + 7 + heuristic;

                            cave[point].Mintues[other] = time + 7;
                            todo[index % BUCKETS].Add((point, other));
                        }
                    }
                }
                baseNum++;
            }
        }
    }
}
