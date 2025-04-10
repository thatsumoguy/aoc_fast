using aoc_fast.Extensions;

namespace aoc_fast.Years._2022
{
    internal class Day14
    {
        public static string input { get; set; }

        enum Kind
        {
            Air,
            Failing,
            Stopped
        }

        struct Cave(int width, int height, int size, List<Kind> kind, Kind floor, int count)
        {
            public int Width { get; set; } = width;
            public int Height { get; set; } = height;
            public int Size { get; set; } = size;
            public List<Kind> Kinds { get; set; } = kind;
            public Kind Floor { get; set; } = floor;
            public int Count { get; set; } = count;

            public bool Check(int index)
            {
                var kind = index >= Size ? Floor : Kinds[index] == Kind.Air ? Fall(index) : Kinds[index];
                return kind == Kind.Stopped;
            }

            public Kind Fall(int index)
            {
                var res = Check(index + Width) && Check(index + Width - 1) && Check(index + Width + 1);
                if(res)
                {
                    Count++;
                    Kinds[index] = Kind.Stopped;
                    return Kind.Stopped;
                }
                else
                {
                    Kinds[index] = Kind.Failing;
                    return Kind.Failing;
                }
            }

            public Cave Clone()
            {
                var clone = new Cave
                {
                    Width = Width,
                    Size = Size,
                    Height = Height,
                    Kinds = [.. Kinds],
                    Floor = Floor,
                    Count = Count
                };
                return clone;
            }
        }
        private static Cave cave;
        private static int Simulate(Cave input, Kind floor)
        {
            var cave = input.Clone();
            cave.Floor = floor;
            cave.Fall(cave.Height);
            return cave.Count;
        }

        private static void Parse()
        {
            try
            {
                var unsigned = (string line) => line.ExtractNumbers<int>();
                var points = input.Split("\n").Select(unsigned).ToList();
                var maxY = points.SelectMany(row => row.Skip(1).StepBy(2)).Max();
                var height = maxY + 2;
                var width = 2 * height + 1;
                var size = width * height;
                var kind = new List<Kind>(size);
                for (var i = 0; i < size; i++) kind.Add(Kind.Air);

                foreach (var row in points)
                {
                    foreach (var window in row.Windows(4).StepBy(2))
                    {
                        if (window.Length == 4)
                        {
                            var (x1, y1, x2, y2) = (window[0], window[1], window[2], window[3]);
                            for (var x = Math.Min(x1, x2); x <= Math.Max(x1, x2); x++)
                            {
                                for (var y = Math.Min(y1, y2); y <= Math.Max(y1, y2); y++)
                                {
                                    kind[(width * y) + (x + height - 500)] = Kind.Stopped;
                                }
                            }
                        }
                    }
                }
                cave = new Cave(width, height, size, kind, Kind.Air, 0);
            }
            catch (Exception ex) { Console.WriteLine(ex); }
        }

        public static int PartOne()
        {
            Parse();
            return Simulate(cave, Kind.Failing);
        }
        public static int PartTwo() => Simulate(cave, Kind.Stopped);
    }
}
