using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using aoc_fast.Extensions;

namespace aoc_fast.Years._2025
{
    internal class Day4
    {
        public static string input
        {
            get;
            set;
        }

        private static Grid<byte> Grid;

        private static void Parse() => Grid = Grid<byte>.Parse(input);

        public static int PartOne()
        {
            Parse();
            var res = 0;
            for(var y = 0; y < Grid.height; y++)
            {
                for(var x = 0; x < Grid.width; x++)
                {
                    if (Grid[x, y] != (byte)'@') continue;
                    var cur = new Point(x, y);
                    var neighborRolls = 0;
                    foreach(var p in Directions.DIAGONAL)
                    {
                        //Doing this versus a Select removes a ton of time
                        var neighbor = p + cur;
                        if (Grid.Contains(neighbor) && Grid[neighbor] == (byte)'@') neighborRolls++;
                    }
                    if (neighborRolls < 4) res++;
                }
            }
            return res;
        }
        public static int PartTwo()
        {
            var rollsLeft = true;
            var res = 0;

            while(rollsLeft)
            {
                rollsLeft = false;

                for(var y = 0;y < Grid.height; y++)
                {
                    for(var x = 0;x < Grid.width;x++)
                    {
                        if (Grid[x, y] != (byte)'@') continue;
                        var cur = new Point(x, y);
                        var neighborRolls = 0;
                        foreach (var p in Directions.DIAGONAL)
                        {
                            var neighbor = p + cur;
                            if (Grid.Contains(neighbor) && Grid[neighbor] == (byte)'@') neighborRolls++;
                        }
                        if (neighborRolls < 4)
                        {
                            Grid[cur] = (byte)'.';
                            rollsLeft = true;
                            res++;
                        }
                    }
                }
            }
            return res;
        }
    }
}
