using System.Text;

namespace aoc_fast.Years._2017
{
    class Day9
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
            var groups = 0;
            var depth = 1;
            var characters = 0;

            while (iter.MoveNext())
            {
                var b = iter.Current;
                switch(b)
                {
                    case (byte)'<':
                        var continues = true;
                        while(continues && iter.MoveNext())
                        {
                            var c = iter.Current;
                            switch(c)
                            {
                                case (byte)'!':
                                    iter.MoveNext();
                                    break;
                                case (byte)'>':
                                    continues = false;
                                    break;
                                default:
                                    characters++;
                                    break;
                            }
                        }
                        break;
                    case (byte)'{':
                        groups += depth;
                        depth++;
                        break;
                    case (byte)'}':
                        depth--;
                        break;
                }
            }

            answer = (groups, characters);
        }

        public static int PartOne()
        {
            Parse();
            return answer.partOne;
        }
        public static int PartTwo() => answer.partTwo;
    }
}
