using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using aoc_fast.Extensions;

namespace aoc_fast.Years._2025
{
    internal class Day7
    {
        public static string input
        {
            get;
            set;
        }

        private static (ulong partOne, ulong partTwo) answers;

        private static void Parse()
        {
            var lines = input.Split("\n",  StringSplitOptions.RemoveEmptyEntries).Select(Encoding.UTF8.GetBytes).ToArray();
            var width = lines[0].Length;
            var center = width / 2;

            var splits = 0ul;
            var timeLines = Enumerable.Repeat(0ul, width).ToArray();
            timeLines[center] = 1;

            foreach(var (y, row) in lines.Skip(2).StepBy(2).Index())
            {
                for(var x = (center - y); x < (center + y + 1); x += 2)
                {
                    var count = timeLines[x];

                    if(count > 0 && row[x] == (byte)'^')
                    {
                        splits++;
                        timeLines[x] = 0;
                        timeLines[x + 1] += count;
                        timeLines[x - 1] += count;
                    }
                }
            }

            answers = (splits, timeLines.Sum());
        }

        public static ulong PartOne()
        {
            Parse();
            return answers.partOne;
        }
        public static ulong PartTwo() => answers.partTwo;
    }
}
