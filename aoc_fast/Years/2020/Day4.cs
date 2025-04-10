using System.Text;
using System.Text.RegularExpressions;

namespace aoc_fast.Years._2020
{
    internal partial class Day4
    {
        public static string input { get; set; }
        private static bool ValidatePassId(string pid) => pid.Length == 9 && Encoding.UTF8.GetBytes(pid).All(b => char.IsAsciiDigit((char)b));
        private static bool ValidateEyeColor(string ecl) => MyRegex().IsMatch(ecl);
        private static bool ValidateHairColor(string hcl)
        {
            var hclBytes = Encoding.UTF8.GetBytes(hcl);
            return hclBytes.Length == 7 && hclBytes[0] == '#' && hclBytes[1..].All(b => char.IsAsciiHexDigit((char)b));
        }
        private static bool ValidateRange(string s, List<int> range) => int.TryParse(s, out var n) && range.Contains(n);
        private static bool ValidateHeight(string hgt)
        {
            if (hgt.Length == 4 && hgt.EndsWith("in")) return ValidateRange(hgt[..2], Enumerable.Range(59, 18).ToList());
            else if (hgt.Length == 5 && hgt.EndsWith("cm")) return ValidateRange(hgt[..3], Enumerable.Range(150, 44).ToList());
            else return false;
        }
        private static bool ValidateField(string[] input) => input[0] switch
        {
            "byr" => ValidateRange(input[1], Enumerable.Range(1920, 103).ToList()),
            "iyr" => ValidateRange(input[1], Enumerable.Range(2010, 11).ToList()),
            "eyr" => ValidateRange(input[1], Enumerable.Range(2020, 11).ToList()),
            "hgt" => ValidateHeight(input[1]),
            "hcl" => ValidateHairColor(input[1]),
            "ecl" => ValidateEyeColor(input[1]),
            "pid" => ValidatePassId(input[1]),
        };
        private static List<string[]> ParseBlock(string block)
        {
            var field = new List<string[]>();
            foreach(var pair in block.Split([':', ' ', '\n']).Chunk(2))
            {
                if (pair[0] != "cid") field.Add(pair);
            }
            return field;
        }

        private static List<List<string[]>> fields = [];
        private static void Parse() => fields = input.Split("\n\n").Select(ParseBlock).ToList();
        public static int PartOne()
        {
            Parse();
            return fields.Where(passport => passport.Count == 7).Count();
        }
        public static int PartTwo() => fields.Where(passport => passport.Count == 7).Where(passport => passport.All(ValidateField)).Count();
        [GeneratedRegex("amb|blu|brn|gry|grn|hzl|oth")]
        private static partial Regex MyRegex();
    }
}
