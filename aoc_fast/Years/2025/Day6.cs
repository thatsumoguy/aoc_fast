using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using aoc_fast.Extensions;
using Microsoft.Diagnostics.Tracing.Analysis.GC;
using Microsoft.VisualStudio.TextManager.Interop;

namespace aoc_fast.Years._2025
{
    internal class Day6
    {
        public static string input
        {
            get;
            set;
        }

        public static (ulong partOne, ulong partTwo) answers;

        private static void Parse()
        {
            var grid = Grid<byte>.Parse(input);
            var bottom = grid.height - 1;
            var right = grid.width;

            var block = ((ulong partOne, ulong partTwo) ans, int left) =>
            {
                var rows = Slice.Range(0, bottom).Select(y => Slice.Range(left, right).Aggregate(0ul, (num, x) => Acc(grid, num, x, y)));
                var cols = Slice.Range(left, right).Select(x => Slice.Range(0, bottom).Aggregate(0ul, (num, y) => Acc(grid, num, x, y)));
                var plus = grid[left, bottom] == (byte)'+';
                var first = plus ? rows.Sum() : rows.Aggregate(1ul, (acc, val) => acc * val);
                var second = plus ? cols.Sum() : cols.Aggregate(1ul, (acc, val) => acc * val);
                right = left - 1;
                return (ans.partOne + first, ans.partTwo + second);
            };

            answers = Slice.Range(0, grid.width).Reverse().Where(x => grid[x, bottom] != (byte)' ').Aggregate((0ul, 0ul), ((ulong partOne, ulong partTwo) ans, int left) => block(ans, left));
        }

        private static ulong Acc(Grid<byte> grid, ulong num, int x, int y)
        {
            var digit = grid[x, y];
            return digit == (byte)' ' ? num : 10 * num + (ulong)(digit - (byte)'0');
        }

        public static ulong PartOne()
        {
            Parse();
            return answers.partOne;
        }
        public static ulong PartTwo() => answers.partTwo;
    }
}
