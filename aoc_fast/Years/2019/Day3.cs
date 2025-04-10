using System.Text;
using aoc_fast.Extensions;
using Point = aoc_fast.Extensions.Point;

namespace aoc_fast.Years._2019
{
    internal class Day3
    {
        public static string input { get; set; }
        private static (int partOne, int partTwo) answer;

        class Line(Point start, Point end, int distance)
        {
            public Point Start { get; set; } = start;
            public Point End { get; set; } = end;
            public int Distance { get; set; } = distance;
        }

        private static void Parse()
        {
            var lines = input.Split("\n");
            var steps = (int i) =>
            {
                var first = Encoding.UTF8.GetBytes(lines[i]).Where(b => char.IsAsciiLetter((char)b));
                var second = lines[i].ExtractNumbers<int>();
                return first.Zip(second);
            };

            var start = Directions.ORIGIN;
            var distance = 0;
            var vertical = new SortedDictionary<int, Line>();
            var horizontal = new SortedDictionary<int, Line>();

            foreach(var (dir, amount) in steps(0))
            {
                var delta = Point.FromByte(dir);
                var end = start + delta * amount;
                var line = new Line(start, end, distance);

                if(start.X == end.X) vertical.Add(start.X, line);
                else horizontal.Add(start.Y, line);

                start = end;
                distance += amount;
            }

            start = Directions.ORIGIN;
            distance = 0;
            var manhattan = int.MaxValue;
            var delay = int.MaxValue;

            foreach(var (dir, amount) in steps(1))
            {
                var delta = Point.FromByte(dir);
                var end = start + delta * amount;
                {
                    var update = (Line line, Point candidate) => 
                    {
                        if(candidate.Manhattan(line.Start) < line.End.Manhattan(line.Start) && candidate.Signum(line.Start) == line.End.Signum(line.Start)
                        && candidate.Manhattan(Directions.ORIGIN) > 0)
                        {
                            manhattan = Math.Min(manhattan, candidate.Manhattan(Directions.ORIGIN));
                            delay = Math.Min(delay, distance + candidate.Manhattan(start) + line.Distance + candidate.Manhattan(line.Start));
                        }
                    };

                    switch(dir)
                    {
                        case (byte)'U':
                            foreach(var (y, line) in horizontal.Where(kvp => kvp.Key >= end.Y && kvp.Key <= start.Y)) update(line, new Point(start.X, y));
                            break;
                        case (byte)'D':
                            foreach(var (y, line) in horizontal.Where(kvp => kvp.Key >= start.Y && kvp.Key <= end.Y)) update(line, new Point(start.X, y));
                            break;
                        case (byte)'L':
                            foreach (var (x, line) in vertical.Where(kvp => kvp.Key >= end.X && kvp.Key <= start.X)) update(line, new Point(x, start.Y));
                            break;
                        case (byte)'R':
                            foreach (var (x, line) in vertical.Where(kvp => kvp.Key >= start.X && kvp.Key <= end.X)) update(line, new Point(x, start.Y));
                            break;
                    }
                }

                start = end;
                distance += amount;
            }

            answer = (manhattan, delay);
        }

        public static int PartOne()
        {
            Parse();
            return answer.partOne;
        }
        public static int PartTwo() => answer.partTwo;
    }
}
