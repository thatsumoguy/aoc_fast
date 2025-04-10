using System.Text;
using aoc_fast.Extensions;
using Point = aoc_fast.Extensions.Point;

namespace aoc_fast.Years._2020
{
    internal class Day12
    {
        public static string input { get; set; }
        private static Point Rotate(Point point, int amount) => amount switch
        {
            90 or -270 => point.Clockwise(),
            180 or -180 => point * -1,
            270 or -90 => point.CounterClockwise(),
            _ => throw new NotImplementedException()
        };

        private static List<(byte, int)> commands = [];
        public static int PartOne()
        {
            foreach(var line in input.TrimEnd().Split("\n")) commands.Add((Encoding.UTF8.GetBytes(line)[0], line[1..].ExtractNumbers<int>()[0]));
            var pos = Directions.ORIGIN;
            var dir = new Point(1, 0);
            foreach(var (command, amount) in commands)
            {
                switch (command)
                {
                    case (byte)'N':
                        pos.Y -= amount;
                        break;
                    case (byte)'S':
                        pos.Y += amount;
                        break;
                    case (byte)'E':
                        pos.X += amount;
                        break;
                    case (byte)'W':
                        pos.X -= amount;
                        break;
                    case (byte)'L':
                        dir = Rotate(dir, -amount);
                        break;
                    case (byte)'R':
                        dir = Rotate(dir, amount);
                        break;
                    case (byte)'F':
                        pos += dir * amount;
                        break;
                    default: throw new NotImplementedException();
                }
            }
            return pos.Manhattan(Directions.ORIGIN);
        }
        public static int PartTwo()
        {
            var pos = Directions.ORIGIN;
            var wayPoint = new Point(10, -1);
            foreach (var (command, amount) in commands)
            {
                switch (command)
                {
                    case (byte)'N':
                        wayPoint.Y -= amount;
                        break;
                    case (byte)'S':
                        wayPoint.Y += amount;
                        break;
                    case (byte)'E':
                        wayPoint.X += amount;
                        break;
                    case (byte)'W':
                        wayPoint.X -= amount;
                        break;
                    case (byte)'L':
                        wayPoint = Rotate(wayPoint, -amount);
                        break;
                    case (byte)'R':
                        wayPoint = Rotate(wayPoint, amount);
                        break;
                    case (byte)'F':
                        pos += wayPoint * amount;
                        break;
                    default: throw new NotImplementedException();
                }
            }
            return pos.Manhattan(Directions.ORIGIN);
        }
    }
}
