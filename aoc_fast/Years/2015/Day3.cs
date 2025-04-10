using System.Text;
using aoc_fast.Extensions;

namespace aoc_fast.Years._2015
{
    class Day3
    {
        public static string input
        {
            get;
            set;
        }
        private static List<Point> santaPoints = [];
        private static List<Point> Parse() => Encoding.ASCII.GetBytes(input).Select(Point.FromByte).ToList();

        private static long Deliver(List<Point> santaPoints, Func<long, bool> predicate)
        {
            var santa = Directions.ORIGIN;
            var robot = Directions.ORIGIN;
            var set = new HashSet<Point>(10000)
            {
                Directions.ORIGIN
            };

            foreach(var (point, index) in santaPoints.Select((x, i) => (x, i)))
            {
                if(predicate(index))
                {
                    santa += point;
                    set.Add(santa);
                }
                else
                {
                    robot += point;
                    set.Add(robot);
                }
            }

            return set.Count;
        }

        public static long PartOne()
        {
            santaPoints = Parse();
            return Deliver(santaPoints, (_) => true);
        }

        public static long PartTwo() => Deliver(santaPoints, (i) => i % 2 == 0);
    }
}
