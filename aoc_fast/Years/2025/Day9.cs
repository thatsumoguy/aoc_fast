using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using aoc_fast.Extensions;
using EnvDTE;
using Microsoft.VisualStudio.Shell.Interop;

namespace aoc_fast.Years._2025
{
    internal class Day9
    {
        public static string input
        {
            get;
            set;
        }

        private static List<ulong[]> Points = [];

        private static void Parse()
        {
            Points = [.. input.ExtractNumbers<ulong>().Chunk(2)];
        }

        public static ulong PartOne()
        {
            Parse();
            var maxArea = 0ul;
            foreach(var (i, p) in Points.Index())
            {
                var (x1, y1) = (p[0],  p[1]);
                foreach(var p2 in Points.Skip(i + 1))
                {
                    var (x2, y2) = (p2[0], p2[1]);
                    var dx = x1 > x2 ? x1 - x2 : x2 - x1;
                    var dy = y1 > y2 ? y1 - y2 : y2 - y1;
                    maxArea = maxArea.Max((dx + 1) * (dy + 1));
                }
            }
            return maxArea;
        }

        public static ulong PartTwo()
        {
            var xs = new SortedSet<ulong>(Points.Select(p => p[0]))
            {
                0,
                ulong.MaxValue
            };
            var shrinkX = new SortedDictionary<ulong, int>();
            foreach(var (x,i) in xs.Index().Select(i => (i.Item, i.Index)))
            {
                shrinkX.Add(x, i);
            }
            var ys = new SortedSet<ulong>(Points.Select(p => p[1]))
            {
                0, 
                ulong.MaxValue
            };
            var shrinkY = new SortedDictionary<ulong, int>();
            foreach (var (y, i) in ys.Index().Select(i => (i.Item, i.Index)))
            {
                shrinkY.Add(y, i);
            }

            var grid = Grid<byte>.New(shrinkX.Count, shrinkY.Count, (byte)'X');

            for(var i = 0; i < Points.Count; i++)
            {
                var (x1, y1) = (Points[i][0],  Points[i][1]);
                var (x2, y2) = (Points[(i + 1) % Points.Count][0], Points[(i + 1) % Points.Count][1]);

                var subX1 = shrinkX[x1];
                var subX2 = shrinkX[x2];
                var subY1 = shrinkY[y1];
                var subY2 = shrinkY[y2];
                
                for(var x = subX1.Min(subX2); x <= subX1.Max(subX2); x++)
                {
                    for (var y = subY1.Min(subY2); y <= subY1.Max(subY2); y++) grid[x, y] = (byte)'#';
                }
            }

            var todo = new List<Point>
            {
                Directions.ORIGIN
            };

            while(todo.TryPopFront(out var p))
            {
                foreach(var o in Directions.ORTHOGONAL)
                {
                    var next = p + o;
                    if(grid.Contains(next) && grid[next] == (byte)'X')
                    {
                        grid[next] = (byte)'.';
                        todo.Add(next);
                    }
                }
            }

            var maxArea = 0ul;
            for(var i = 0; i < Points.Count; i++)
            {
                for (var j = i + 1;  j < Points.Count; j++)
                {
                    var (x1, y1) = (Points[i][0], Points[i][1]);
                    var (x2, y2) = (Points[j][0], Points[j][1]);

                    var subX1 = shrinkX[x1];
                    var subX2 = shrinkX[x2];
                    var subY1 = shrinkY[y1];
                    var subY2 = shrinkY[y2];

                    var continueOuter = false;
                    for (var x = subX1.Min(subX2); x <= subX1.Max(subX2); x++)
                    {
                        for (var y = subY1.Min(subY2); y <= subY1.Max(subY2); y++)
                        {
                            if (grid[x, y] == (byte)'.')
                            {
                                continueOuter = true;
                                break;
                            }
                        }
                        if (continueOuter) break;
                    }
                    if (continueOuter) continue;
                    var dx = x1 > x2 ? x1 - x2 : x2 - x1;
                    var dy = y1 > y2 ? y1 - y2 : y2 - y1;
                    maxArea = maxArea.Max((dx + 1) * (dy + 1));
                }
            
            }

            return maxArea;
        }
    }
}
