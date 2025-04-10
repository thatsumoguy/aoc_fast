using System.Text;
using aoc_fast.Extensions;

namespace aoc_fast.Years._2016
{
    class Day1
    {
        public static string input
        {
            get;
            set;
        }
        static List<(byte, int)> pairs = [];

        private static void Parse()
        {
            var first = Encoding.ASCII.GetBytes(input).Where(b => char.IsAsciiLetterUpper((char)b));
            var second = input.ExtractNumbers<int>();

            pairs = first.Zip(second).ToList();
        }

        public static int PartOne()
        {
            Parse();
            var pos = Directions.ORIGIN;
            var dir = Directions.UP;

            foreach(var (turn, amount)  in pairs)
            {
                dir = turn == (byte)'L' ? dir.CounterClockwise() : dir.Clockwise();
                pos += dir * amount;
            }

            return pos.Manhattan(Directions.ORIGIN);
        }

        public static int PartTwo()
        {
            var pos = Directions.ORIGIN;
            var dir = Directions.UP;
            var visited = new HashSet<Point>();

            foreach(var (turn, amount) in pairs)
            {
                dir = turn == (byte)'L' ? dir.CounterClockwise() : dir.Clockwise();

                for(var _ = 0; _ < amount; _++)
                {
                    pos += dir;
                    if(!visited.Add(pos)) return pos.Manhattan(Directions.ORIGIN);
                }
            }
            throw new Exception();
        }
    }
}
