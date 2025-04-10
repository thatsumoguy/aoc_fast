namespace aoc_fast.Years._2016
{
    class Day25
    {
        //So we are meant to supply a here to find a int value that outputs (via out b) the signal 0,1,0,1,0,1...
        //cpy a d
        //cpy 7 c
        //cpy 365 b
        //These two numbers are the constants and extracted
        //inc d
        //dec b
        //jnz b -2
        //dec c
        //jnz c -5
        //cpy d a
        //jnz 0 0
        //cpy a b
        //cpy 0 a
        //cpy 2 c
        //jnz b 2
        //jnz 1 6
        //dec b
        //dec c
        //jnz c -4
        //inc a
        //jnz 1 -7
        //cpy 2 b
        //jnz c 2
        //jnz 1 4
        //dec b
        //dec c
        //jnz 1 -4
        //jnz 0 0
        //out b
        //jnz a -19
        //jnz 1 -21

        //All this is doing is basically taking the constant (the two values times each other plus the input), dividing by 2 and then outputing the remainder, in reverse order
        //so what we need is a number who as you decrease it will output even, odd, even, odd, etc.
        //We can do that with the loop in partone.
        public static string input
        {
            get;
            set;
        }

        private static int constant;

        private static void Parse()
        {
            var lines = input.Split("\n", StringSplitOptions.RemoveEmptyEntries);
            var first = int.Parse(lines[1][4..6]);
            var second = int.Parse(lines[2][4..7]);

            constant = first * second;
        }

        public static int PartOne()
        {
            Parse();

            var offset = constant;

            var res = 0;

            while (res < constant) res = (res << 2) | 2;

            return res - offset;
        }
        public static string PartTwo() => "Merry Christmas";
    }
}
