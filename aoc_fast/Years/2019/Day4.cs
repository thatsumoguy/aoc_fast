using aoc_fast.Extensions;

namespace aoc_fast.Years._2019
{
    internal class Day4
    {
        public static string input { get; set; }
        private static List<uint> nums = [];
        private static void Parse() => nums = input.ExtractNumbers<uint>();

        private static uint Passwords(List<uint> nums, Func<bool,bool,bool,bool,bool,bool> pred)
        {
            var start = nums[0];
            var end = nums[1];
            uint[] digits = 
             [
                start / 100000,
                (start / 10000) % 10,
                (start / 1000) % 10,
                (start / 100) % 10,
                (start / 10) % 10,
                start % 10,
             ];

            for(var i = 1; i < 6; i++)
            {
                if (digits[i] < digits[i - 1])
                {
                    for(var j = i; j < 6; j++) digits[j] = digits[i - 1];
                    break;
                }
            }

            var n = 0u;
            var count = 0u;

            while(n < end)
            {
                var first = digits[0] == digits[1];
                var second = digits[1] == digits[2];
                var third = digits[2] == digits[3];
                var fourth = digits[3] == digits[4];
                var fifth = digits[4] == digits[5];
                if (pred(first, second, third, fourth, fifth)) count++;

                var i = 5;
                while (digits[i] == 9) i--;
                var next = digits[i] + 1;
                while(i <= 5)
                {
                    digits[i] = next;
                    i++;
                }
                n = digits.FoldDecimal();
            }
            return count;
        }

        public static uint PartOne()
        {
            try { Parse(); }
            catch(Exception e) { Console.WriteLine(e); }
            static bool pred(bool first, bool second, bool third, bool fourth, bool fifth) => first || second || third || fourth || fifth;
            return Passwords(nums, pred);
        }
        public static uint PartTwo()
        {
            var pred = (bool first, bool second, bool third, bool fourth, bool fifth) => (first && !second) || (!first && second && !third)
                                                                                    || (!second && third && !fourth) || (!third && fourth && !fifth)
                                                                                    || (!fourth && fifth);
            return Passwords(nums, pred);
        }
    }
}
