using System.Text;

namespace aoc_fast.Years._2022
{
    internal class Day25
    {
        public static string input { get; set; }
        private static long FromSnafu(string snafu) => Encoding.ASCII.GetBytes(snafu).Aggregate(0L, (acc, c) => 
        {
            var digit = c switch
            {
                (byte)'=' => -2,
                (byte)'-' => -1,
                (byte)'0' => 0,
                (byte)'1' => 1,
                (byte)'2' => 2
            };
            return 5 * acc + digit;
        });
        private static string ToSnafu(long n)
        {
            var digits = new StringBuilder();

            while(n > 0)
            {
                var next = (n%5) switch
                {
                    0 => '0',
                    1 => '1',
                    2 => '2',
                    3 => '=',
                    4 => '-'
                };
                digits.Insert(0, next);
                n = (n + 2) / 5;
            }
            return digits.ToString();
        }

        public static string PartOne() => ToSnafu(input.Split("\n", StringSplitOptions.RemoveEmptyEntries).Select(FromSnafu).Sum());
        public static string PartTwo() => "Merry Christmas";
    }
}
