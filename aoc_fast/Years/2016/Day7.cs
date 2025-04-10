using System.Text;
using aoc_fast.Extensions;

namespace aoc_fast.Years._2016
{
    class Day7
    {
        public static string input
        {
            get;
            set;
        }
        static byte[] packets = [];

        public static int PartOne()
        {
            try
            {
                packets = Encoding.ASCII.GetBytes(input);
                var count = 0;
                var inside = false;
                var positive = false;
                var negative = false;

                foreach (var w in packets.Windows(4))
                {
                    if (char.IsAsciiLetterLower((char)w[0]))
                    {
                        if (w[0] == w[3] && w[1] == w[2] && w[0] != w[1])
                        {
                            if (inside) negative = true;
                            else positive = true;
                        }
                    }
                    else if (w[0] == (byte)'[') inside = true;
                    else if (w[0] == (byte)']') inside = false;
                    else
                    {
                        if (positive && !negative) count++;
                        positive = false;
                        negative = false;
                    }
                }

                if (positive && !negative) count++;
                return count;
            }
            catch (Exception e) { Console.WriteLine(e); }

            return -1;
        }

        public static int PartTwo()
        {
            try
            {
                
                var count = 0;
                var version = 0;
                var inside = false;
                var positive = false;
                var aba = Enumerable.Repeat(int.MaxValue, 676).ToArray();
                var bab = Enumerable.Repeat(int.MaxValue, 676).ToArray();

                foreach (var w in packets.Windows(3))
                {
                    if (char.IsAsciiLetterLower((char)w[1]))
                    {
                        if (w[0] == w[2] && w[0] != w[1])
                        {
                            var first = w[0] - (byte)'a';
                            var second = w[1] - (byte)'a';

                            if (inside)
                            {
                                var index = 26 * second + first;
                                bab[index] = version;
                                positive |= aba[index] == version;
                            }
                            else
                            {
                                var index = 26 * first + second;
                                aba[index] = version;
                                positive |= bab[index] == version;
                            }
                        }
                    }
                    else if (w[1] == (byte)'[') inside = true;
                    else if (w[1] == (byte)']') inside = false;
                    else
                    {
                        if (positive) count++;

                        version++;
                        positive = false;
                    }
                }

                if (positive) count++;
                return count;
            }
            catch (Exception e) { Console.WriteLine(e); }

            return -1;
        }
    }
}
