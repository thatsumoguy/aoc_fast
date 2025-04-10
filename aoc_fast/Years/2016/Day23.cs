namespace aoc_fast.Years._2016
{
    class Day23
    {
        //Going over this input we can see this pattern
        //cpy a b
        //dec b
        //cpy a d
        //cpy 0 a
        //cpy b c
        //inc a
        //dec c
        //jnz c -2
        //dec d
        //jnz d -5
        //dec b
        //cpy b c
        //cpy c d
        //dec d
        //inc c
        //jnz d -2
        //This all basically is just finding a! (factorial) where the puzzle gives us a as 7 for partone and 12 for parttwo
        //tgl c
        //Eventually this tgl will set the jnz d and jnz c to a cpy which will allow the program to exit
        //cpy -16 c
        //jnz 1 c
        //cpy 96 c
        //jnz 91 d
        //These are the two constants that end up being added to a! so we can extract them
        //inc a
        //inc d
        //jnz d -2
        //inc c
        //jnz c -5
        public static string input
        {
            get;
            set;
        }

        private static int constant;

        private static int Factorial(int value)
        {
            if (value == 0) return 1;
            return value * Factorial(value - 1);
        }

        private static void Parse()
        {
            var lines = input.Split("\n", StringSplitOptions.RemoveEmptyEntries).ToArray();
            var first = int.Parse(lines[19][4..6]);
            var second = int.Parse(lines[20][4..6]);
            constant = first * second;
        }
        public static int PartOne()
        {
            Parse();
            return Factorial(7) + constant;
        }

        public static int PartTwo() => Factorial(12) + constant;
    }
}
