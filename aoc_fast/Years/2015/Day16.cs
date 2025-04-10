namespace aoc_fast.Years._2015
{
    class Day16
    {
        public static string input
        {
            get;
            set;
        }

        private static int Solve(string input, Func<string, string, bool> predicate)
        {
            var lines = input.Split("\n", StringSplitOptions.RemoveEmptyEntries);
            for (var index = 0; index < lines.Length; index++)
            {
            outer:
                var line = lines[index];
                var tokens = line.Split([' ', ':', ',']).Where(s => !string.IsNullOrEmpty(s)).ToList();
                foreach(var t in tokens.Chunk(2).Skip(1))
                {
                    if (!predicate(t[0], t[1]))
                    {
                        index++;
                        goto outer;
                    }
                }
                return index + 1;
            }

            throw new Exception();
        }

        public static int PartOne()
        {
            static bool predicate(string key, string value)
            {
                return key switch
                {
                    "akitas" or "vizslas" => value == "0",
                    "perfumes" => value == "1",
                    "samoyeds" or "cars" => value == "2",
                    "children" or "pomeranians" or "trees" => value == "3",
                    "goldfish" => value == "5",
                    "cats" => value == "7",
                };
            }

            return Solve(input, predicate);
        }

        public static int PartTwo()
        {
            static bool predicate(string key, string value)
            {
                return key switch
                {
                    "akitas" or "vizslas" => value == "0",
                    "perfumes" => value == "1",
                    "samoyeds" or "cars" => value == "2",
                    "children" => value == "3",
                    "pomeranians" => int.Parse(value) < 3,
                    "goldfish" => int.Parse(value) < 5,
                    "trees" => int.Parse(value) > 3,
                    "cats" => int.Parse(value) > 7,
                };
            }

            return Solve(input, predicate);
        }
    }
}
