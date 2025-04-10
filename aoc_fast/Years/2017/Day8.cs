namespace aoc_fast.Years._2017
{
    class Day8
    {
        public static string input
        {
            get;
            set;
        }
        private static (int partOne, int partTwo) answer;

        private static void Parse()
        {
            var registers = new Dictionary<string, int>();
            var partTwo = 0;

            foreach(var l in input.Split([' ','\t','\n'], StringSplitOptions.RemoveEmptyEntries).Chunk(7))
            {
                var (a, b, c, e, f, g) = (l[0], l[1], l[2], l[4], l[5], l[6]);

                if(!registers.ContainsKey(e)) registers[e] = 0;
                var first = registers[e];
                var second = int.Parse(g);

                var predicate = f switch
                {
                    "==" => first == second,
                    "!=" => first != second,
                    ">=" => first >= second,
                    "<=" => first <= second,
                    ">" => first > second,
                    "<" => first < second,
                    _ => throw new Exception()
                };
                if(predicate)
                {
                    if(!registers.ContainsKey(a)) registers[a] = 0;
                    var third = registers[a];
                    var fourth = int.Parse(c);

                    switch(b)
                    {
                        case "inc":
                            third += fourth;
                            break;
                        case "dec":
                            third -= fourth;
                            break;
                    }

                    partTwo = Math.Max(partTwo, third);
                    registers[a] = third;
                }
            }
            var partOne = registers.Values.Max();
            answer = (partOne, partTwo);
        }

        public static int PartOne()
        {
            Parse();
            return answer.partOne;
        }
        public static int PartTwo() => answer.partTwo;
    }
}
