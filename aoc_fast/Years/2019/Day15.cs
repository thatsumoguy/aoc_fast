using aoc_fast.Extensions;
using static aoc_fast.Extensions.Hash;
using Point = aoc_fast.Extensions.Point;

namespace aoc_fast.Years._2019
{
    internal class Day15
    {
        public static string input { get; set; }
        private static FastSet<Point> Maze = [];
        private static Point OxygenSystem;
        private static void Parse()
        {
            var code = input.ExtractNumbers<long>();
            var comp = new Computer(code);
            var first = true;
            var dir = Directions.UP;
            var pos = Directions.ORIGIN;
            var oxygenSystem = Directions.ORIGIN;
            var visited = new FastSet<Point>();
            
            while(true)
            {
                dir = first ? dir.Clockwise() : dir.CounterClockwise();

                switch((dir.X, dir.Y))
                {
                    case (0,-1):
                        comp.Input(1);
                        break;
                    case (0, 1):
                        comp.Input(2);
                        break;
                    case (-1, 0):
                        comp.Input(3);
                        break;
                    case (1, 0):
                        comp.Input(4);
                        break;
                }

                switch(comp.Run(out var res))
                {
                    case State.Output when res == 0: first = false; break;
                    case State.Output:
                        first = true;
                        pos += dir;
                        visited.Add(pos);

                        if (res == 2) oxygenSystem = pos;
                        if (pos == Directions.ORIGIN) goto outer;
                        break;
                }
            }
            outer:
            (Maze, OxygenSystem) = (visited, oxygenSystem);
        }

        public static int PartOne()
        {
            Parse();
            var maze = Maze.ToHashSet();
            var oxygenSystem = OxygenSystem;
            var todo = new Queue<(Point, int)>();
            todo.Enqueue((Directions.ORIGIN, 0));

            while(todo.TryDequeue(out var pair))
            {
                var point = pair.Item1;
                var cost = pair.Item2;
                maze.Remove(point);
                if (point == oxygenSystem) return cost;

                foreach(var nextPoint in Directions.ORTHOGONAL.Select(o =>  point + o))
                {
                    if (maze.Contains(nextPoint)) todo.Enqueue((nextPoint, cost + 1));
                }

            }
            return -1;
        }

        public static int PartTwo()
        {
            var maze = Maze.ToHashSet();
            var oxygenSystem = OxygenSystem;
            var todo = new Queue<(Point, int)>();
            todo.Enqueue((oxygenSystem, 0));
            var minutes = 0;
            while (todo.TryDequeue(out var pair))
            {
                var point = pair.Item1;
                var cost = pair.Item2;
                maze.Remove(point);
                minutes = Math.Max(minutes, cost);

                foreach (var nextPoint in Directions.ORTHOGONAL.Select(o => point + o))
                {
                    if (maze.Contains(nextPoint)) todo.Enqueue((nextPoint, cost + 1));
                }
            }
            return minutes;
        }
    }
}
