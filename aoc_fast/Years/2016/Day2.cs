using System.Text;
using aoc_fast.Extensions;

namespace aoc_fast.Years._2016
{
    class Day2
    {
        public static string input
        {
            get;
            set;
        }

        public static string PartOne()
        {
            var digits = Grid<byte>.Parse("123\n456\n789");
            var pos = Directions.ORIGIN;
            var result = new StringBuilder();

            foreach (var line in input.Split("\n", StringSplitOptions.RemoveEmptyEntries))
            {
                foreach (var b in Encoding.ASCII.GetBytes(line))
                {
                    var next = pos + Point.FromByte(b);
                    if (Math.Abs(next.X) <= 1 && Math.Abs(next.Y) <= 1) pos = next;
                }
                result.Append((char)digits[pos + new Point(1,1)]);
            }

            return result.ToString();  
        }

        public static string PartTwo()
        {
            var digits = Grid<byte>.Parse("##1##\n#234#\n56789\n#ABC#\n##D##");
            var pos = new Point(-2, 0);
            var result = new StringBuilder();

            foreach(var line in input.Split("\n",StringSplitOptions.RemoveEmptyEntries))
            {
                foreach(var b in Encoding.ASCII.GetBytes(line))
                {
                    var next = pos + Point.FromByte(b);
                    if(next.Manhattan(Directions.ORIGIN) <= 2) pos = next;
                }
                result.Append((char)digits[pos + new Point(2,2)]);
            }

            return result.ToString();
        }
    }
}
