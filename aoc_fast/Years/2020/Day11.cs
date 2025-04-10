using aoc_fast.Extensions;
using Point = aoc_fast.Extensions.Point;

namespace aoc_fast.Years._2020
{
    internal class Day11
    {
        public static string input { get; set; }
        private static readonly Point[] DIRECTIONS = [new Point(-1, -1), new Point(0, -1), new Point(1, -1), new Point(-1, 0), new Point(1, 0), new Point(-1, 1), new Point(0, 1), new Point(1, 1)];

        class Seat(ushort index, byte size, ushort[] neighbors)
        {
            public ushort Index { get; set; } = index;
            public byte Size { get; set; } = size;
            public ushort[] Neighbors { get; set; } = neighbors;

            public void Push(ushort index)
            {
                Neighbors[Size] = index;
                Size++;
            }
        }

        private static uint Simulate(Grid<byte> input, byte limit, bool partOne = true)
        {
            var width = input.width;
            var height = input.height;
            var seats = new List<Seat>();

            for(var y  = 0; y < height; y++)
            {
                for(var X = 0; X < width; X++)
                {
                    var point = new Point(X, y);
                    if (input[point] == '.') continue;

                    var seat = new Seat((ushort)(width * y + X), 0, new ushort[8]);
                    foreach (var dir in DIRECTIONS)
                    {
                        if (partOne)
                        {
                            var next = point + dir;
                            if (input.Contains(next) && input[next] != '.') seat.Push( (ushort)(width * next.Y + next.X));
                        }
                        else
                        {
                            var next = point + dir;
                            while (input.Contains(next))
                            {
                                if (input[next] != '.')
                                {
                                    seat.Push((ushort)(width * next.Y + next.X));
                                    break;
                                }
                                next += dir;
                            }
                        }
                    }
                    seats.Add(seat);
                }
            }

            var currArr = new byte[width * height];
            var nextArr = new byte[width * height];
            var change = true;

            while (change)
            {
                change = false;
                foreach(var seat in seats)
                {
                    var index = seat.Index;
                    var total = (byte)0;

                    for (var i = 0; i < seat.Size; i++) total += currArr[seat.Neighbors[i]];
                    if (currArr[index] == 0 && total == 0)
                    {
                        nextArr[index] = 1;
                        change |= true;
                    }
                    else if(currArr[index] == 1 && total >= limit)
                    {
                        nextArr[index] = 0;
                        change |= true;
                    }
                    else nextArr[index] = currArr[index];
                }
                (currArr, nextArr) = (nextArr, currArr);
            }
            return currArr.Select(n => (uint)n).Sum();
        }
        private static Grid<byte> Grid;
        
        public static uint PartOne()
        {
            Grid = Grid<byte>.Parse(input);
            return Simulate(Grid, 4);
        }
        public static uint PartTwo() => Simulate(Grid, 5, false);
    }
}
