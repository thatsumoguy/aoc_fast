using System.Data;
using System.Text;
using aoc_fast.Extensions;

namespace aoc_fast.Years._2022
{
    internal class Day10
    {
        public static string input { get; set; }
        private static List<int> Xs = [];
        private static void Parse()
        {
            var x = 1;
            var xs = new List<int>() { 1 };

            foreach(var token in input.Trim().Split([' ', '\t', '\n']))
            {
                switch (token)
                {
                    case "noop" or "addx": break;
                    case var delta:
                        x += int.Parse(delta);
                        break;
                }
                xs.Add(x);
            }
            Xs = xs;
        }

        public static int PartOne()
        {
            Parse();
            return Xs.Index().Skip(19).StepBy(40).Select(x => (x.Index + 1) * x.Item).Sum();
        }

        public static string PartTwo()
        {
            var sb = new StringBuilder(240);
            sb.Append("\n\t\t\t");

            for (var i = 0; i < 240; i++)
            {
                if (i > 0 && i % 40 == 0) sb.Append("\n\t\t\t");

                var cycle = i % 40;
                var x = Xs[i];

                char c = Math.Abs(cycle - x) <= 1 ? '#' : '.';
                sb.Append(c);
            }
            return sb.ToString();
        }
    }
}
