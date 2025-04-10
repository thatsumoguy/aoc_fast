using System.Text;

namespace aoc_fast.Years._2017
{
    internal class Day11
    {
        public static string input
        {
            get;
            set;
        }

        private static (int partOne, int partTwo) answer;

        private static void Parse()
        {
            var iter = Encoding.ASCII.GetBytes(input).GetEnumerator();
            var q = 0;
            var r = 0;
            var partOne = 0;
            var partTwo = 0;

            while(iter.MoveNext())
            {
                var first = (byte)iter.Current;
                switch(first)
                {
                    case (byte)'n':
                        var i = iter.MoveNext() ? (byte)iter.Current : (byte)0;
                        switch (i)
                        {
                            case (byte)'e':
                                q++;
                                r--;
                                break;
                            case (byte)'w':
                                q--;
                                break;
                            default:
                                r--;
                                break;
                        }
                        break;
                    case (byte)'s':
                        var j = iter.MoveNext() ? (byte)iter.Current : (byte)0;
                        switch(j)
                        {
                            case (byte)'e':
                                q++;
                                break;
                            case (byte)'w':
                                q--;
                                r++;
                                break;
                            default:
                                r++;
                                break;
                        }
                        break;

                }
                var s = q + r;
                partOne = (Math.Abs(q) + Math.Abs(s) + Math.Abs(r)) / 2;
                partTwo = Math.Max(partTwo, partOne);
            }
            answer = (partOne,  partTwo);
        }

        public static int PartOne()
        {
            Parse();
            return answer.partOne;
        }
        public static int PartTwo() => answer.partTwo;
    }
}
