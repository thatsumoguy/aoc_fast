using aoc_fast.Extensions;

namespace aoc_fast.Years._2023
{
    internal class Day14
    {
        public static string input { get; set; }
        class ShortComp : IEqualityComparer<List<short>>
        {
            public bool Equals(List<short> x, List<short> y)
            {
                if(x == null && y == null) return true;
                if(x == null || y == null) return false;
                return x.SequenceEqual(y);
            }

            public int GetHashCode(List<short> obj)
            {
                if(obj == null) return 0;
                unchecked
                {
                    var hash = 17;
                    foreach(var item in obj) hash = hash * 31 + item.GetHashCode();
                    return hash;
                }
            }
        }

        record Input(int width, int height, 
            List<short> rounded, short[] fixedNorth, short[] fixedWest, short[] fixedSouth, short[] fixedEast, List<short> rollNorth, List<short> rollWest, List<short> rollSouth, List<short> rollEast);

        private static Input inputObj;

        private static List<short> Tilt(List<short> rounded, short[] fixedArray, List<short> roll, short direction)
        {
            var state = roll.ToList();
            for(var i = 0; i < rounded.Count; i++)
            {
                var rock = rounded[i];
                var index = fixedArray[(ushort)rock];
                state[index] += direction;
                rounded[i] = state[index];
            }
            return state;
        }


        private static void Parse()
        {
            var inner = Grid<byte>.Parse(input);
            var grid = Grid<byte>.New(inner.width + 2, inner.height + 2, (byte)'#');

            for(var y = 0; y < inner.width; y++)
            {
                for(var x = 0;  x < inner.width; x++)
                {
                    var src = new Point(x, y);
                    var dst = new Point(x + 1, y + 1);
                    grid[dst] = inner[src];
                }
            }

            var rounded = new List<short>();
            var north = grid.NewWith((short)0);
            var west = grid.NewWith((short)0);
            var south = grid.NewWith((short)0);
            var east = grid.NewWith((short)0);
            var rollNorth = new List<short>();
            var rollWest = new List<short>();
            var rollSouth = new List<short>();
            var rollEast = new List<short>();

            for(var y = 0; y < grid.height; y++)
            {
                for(var x = 0; x < grid.width; x++)
                {
                    var point = new Point(x, y);
                    if (grid[point] == (byte)'O') rounded.Add((short)(grid.width * point.Y + point.X));
                }
            }

            for (var x = 0; x < grid.width; x++)
            {
                for (var y = 0; y < grid.height; y++)
                {
                    var point = new Point(x, y);
                    if (grid[point] == (byte)'#') rollNorth.Add((short)(grid.width * point.Y + point.X));
                    north[point] = (short)(rollNorth.Count - 1);
                }
            }

            for (var y = 0; y < grid.height; y++)
            {
                for (var x = 0; x < grid.width; x++)
                {
                    var point = new Point(x, y);
                    if (grid[point] == (byte)'#') rollWest.Add((short)(grid.width * point.Y + point.X));
                    west[point] = (short)(rollWest.Count - 1);
                }
            }
            for (var x = 0; x < grid.width; x++)
            {
                for (var y = grid.height - 1; y >= 0; y--)
                {
                    var point = new Point(x, y);
                    if (grid[point] == (byte)'#') rollSouth.Add((short)(grid.width * point.Y + point.X));
                    south[point] = (short)(rollSouth.Count - 1);
                }
            }
            for (var y = 0; y < grid.height; y++)
            {
                for (var x = grid.width - 1; x >= 0; x--)
                {
                    var point = new Point(x, y);
                    if (grid[point] == (byte)'#') rollEast.Add((short)(grid.width * point.Y + point.X));
                    east[point] = (short)(rollEast.Count - 1);
                }
            }
            inputObj = new Input(grid.width, grid.height, rounded, north.data, west.data, south.data, east.data, rollNorth, rollWest, rollSouth, rollEast);
        }

        public static int PartOne()
        {
            Parse();
            var res = 0;
            var (width, height, fixedNorth, rollNorth) = (inputObj.width, inputObj.height, inputObj.fixedNorth, inputObj.rollNorth);

            var rounded = inputObj.rounded.ToList();
            var state = Tilt(rounded, fixedNorth, rollNorth, (short)width);

            foreach(var (a,b) in inputObj.rollNorth.Zip(state))
            {
                for(var index = a; index < b; index += (short)inputObj.width)
                {
                    var y = index / width;
                    res += height - 2 - y;
                }
            }
            return res;
        }
        public static int PartTwo()
        {
            try
            {
                Parse();
                var (width, height) = (inputObj.width, inputObj.height);

                var rounded = inputObj.rounded.ToList();
                var seen = new Dictionary<List<short>, int>(new ShortComp());
                var (start, end) = (0, 0);
                while (true)
                {
                    Tilt(rounded, inputObj.fixedNorth, inputObj.rollNorth, (short)width);
                    Tilt(rounded, inputObj.fixedWest, inputObj.rollWest, 1);
                    Tilt(rounded, inputObj.fixedSouth, inputObj.rollSouth, (short)-width);
                    var state = Tilt(rounded, inputObj.fixedEast, inputObj.rollEast, -1);

                    if (!seen.TryAdd(state, seen.Count))
                    {
                        (start, end) = (seen[state], seen.Count);
                        break;
                    }
                }
                 
                var offset = 1000000000 - 1 - start;
                var cycleWidth = end - start;
                var remainder = offset % cycleWidth;
                var target = start + remainder;

                var (targetState, _) = seen.First(i => i.Value == target);

                var res = 0;

                foreach (var (a, b) in inputObj.rollEast.Zip(targetState))
                {
                    var n = a - b;
                    var y = a / width;
                    res += n * (height - 1 - y);
                }
                return res;
            }
            catch (Exception ex) { Console.WriteLine(ex); }
            return 0;
        }
    }
}
