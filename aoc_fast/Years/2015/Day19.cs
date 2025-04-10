using System.Text;
using System.Text.RegularExpressions;

namespace aoc_fast.Years._2015
{
    partial class Day19
    {
        public static string input
        {
            get;
            set;
        }

        private static (string, List<(string, string)>) Molecules = ("", []);

        private static void Parse()
        {
            var (replacements, molecule) = input.Split("\n\n") switch { var a => (a[0], a[1]) };
            Molecules = (molecule, replacements.Split("\n").Select(l => l.Split(" => ") switch { var a => (a[0], a[1]) }).ToList());
        }

        public static long PartOne()
        {
            Parse();

            var (molecule, replacements) = Molecules;

            var distinct = new HashSet<string>();

            foreach(var (from, to) in replacements)
            {
                var match = new Regex(from);
                foreach(Match m in match.Matches(molecule))
                {
                    var size = molecule.Length - from.Length + to.Length;
                    var end = m.Index + from.Length;

                    var s = new StringBuilder(size);
                    s.Append(molecule[..m.Index]);
                    s.Append(to);
                    s.Append(molecule[end..]);

                    distinct.Add(s.ToString());
                }
            }

            return distinct.Count;
        }

        public static int PartTwo()
        {
            var (molecule, _) = Molecules;

            var elements = molecule.ToCharArray().Where(char.IsAsciiLetterUpper).Count();
            var rn  = MyRegex().Matches(molecule).Count();
            var ar = MyRegex1().Matches(molecule).Count();
            var y = MyRegex2().Matches(molecule).Count();

            return elements - ar - rn - 2 * y - 1;
        }

        [GeneratedRegex("Rn")]
        private static partial Regex MyRegex();
        [GeneratedRegex("Ar")]
        private static partial Regex MyRegex1();
        [GeneratedRegex("Y")]
        private static partial Regex MyRegex2();
    }
}
