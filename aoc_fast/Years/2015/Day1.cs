using System.Text;

namespace aoc_fast.Years._2015
{
    class Day1
    {
        public static string input
        {
            get;
            set;
        }
        private static int[] inputArray;
        private static int[] Parse()
        {
            static int helper(byte b)
            {
                return b switch
                {
                    (byte)'(' => 1,
                    (byte)')' => -1,
                    _ => 0
                };
            }
            return Encoding.ASCII.GetBytes(input).Select(helper).ToArray();
        }

        public static int PartOne()
        {
            inputArray = Parse();
            return inputArray.Sum();
        }

        public static int PartTwo()
        {
            inputArray = Parse();
            var floor = 0;
            foreach(var (X, i) in inputArray.Select((X,i) => (X,i)))
            {
                floor += X;
                if (floor < 0) return i + 1;
            }

            return -1;
        }
    }
}
