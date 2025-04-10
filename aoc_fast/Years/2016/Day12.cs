namespace aoc_fast.Years._2016
{
    class Day12
    {
        //After looking at the Rust code and examining my input, I can see what is happening
        //cpy 1 a
        //cpy 1 b
        //cpy 26 d
        //jnz c 2
        //jnz 1 5
        //cpy 7 c
        //inc d
        //dec c
        //jnz c -2
        //We are setting a and b to 1 and setting d to 26, in parttwo we set c to 1 which means d is actually set to 33
        //Then this is called
        //cpy a c
        //inc a
        //dec b
        //jnz b -2
        //cpy c b
        //dec d
        //jnz d -6
        //Which loops over c and b (starting at 0 and 1 and incrementing each iteration) to increment a to b + c each iteration, or the Fibonacci sequence
        //Then this is called
        //cpy 19 c
        //cpy 14 d
        //inc a
        //dec d
        //jnz d -2
        //dec c
        //jnz c -5
        //Which is where we get the constant from (19 * 14 in my case) to the Fibonacci number from the previous steps. I just did a quick Fib method to get the 28th and 35th number (respectically)
        //And then add that number to the constant.

        private static int constant = 0;

        public static string input
        {
            get;
            set;
        }

        private static int GetFibNum(int fibNum)
        {
            if (fibNum == 0) return 0;
            var fib = 0;
            var temp1 = 1;
            var temp2 = 0;

            for(var i = 0; i < fibNum; i++)
            {
                fib = temp1 + temp2;
                temp1 = temp2;
                temp2 = fib;
            }
            return fib;
        }

        private static void Parse()
        {
            var lines = input.Split('\n', StringSplitOptions.RemoveEmptyEntries);
            var first = int.Parse(lines[16][4..6]);
            var second = int.Parse(lines[17][4..6]);
            constant = first * second;
        }

        public static int PartOne()
        {
            Parse();
            return GetFibNum(28) + constant;
        }

        public static int PartTwo() => GetFibNum(35) + constant;
    }
}
