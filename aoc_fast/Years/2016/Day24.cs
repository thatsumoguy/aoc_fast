using aoc_fast.Extensions;

namespace aoc_fast.Years._2016
{
    class Day24
    {
        public static string input
        {
            get;
            set;
        }

        private static (int partOne, int partTwo) answer;

        private static void Parse()
        {
            var grid = Grid<byte>.Parse(input);
            var found = grid.data.Select((b,i) =>(b,i)).Where(g => char.IsAsciiDigit((char)g.b)).Select(g => g.i).ToList();
            var stride = found.Count;
            var distance = new int[stride * stride];

            var todo = new Queue<(int, int)>();
            var visited = new int[grid.data.Length];
            var orthogonal = new int[] { 1, -1, grid.width, -183 }.Select(i => (ulong)CastToUnsigned((long)i)).ToList();
            
            foreach(var start in found)
            {
                var from = grid.data[start] - '0';
                todo.Enqueue((start, 0));
                visited[start] = start;

                while(todo.TryDequeue(out var i))
                {
                    var index = i.Item1;
                    var steps = i.Item2;
                    if (char.IsAsciiDigit((char)grid.data[index]))
                    {
                        var to = grid.data[index] - '0';
                        distance[stride * from + to] = steps;
                    }
                    foreach(var offset in orthogonal)
                    {
                        var nextIndex = (int)(offset + (ulong)index) % int.MaxValue;
                        if (grid.data[nextIndex] != (byte)'#' && visited[nextIndex] != start)
                        {
                            visited[nextIndex] = start;
                            todo.Enqueue((nextIndex, steps + 1));
                        }
                    }
                }
            }
            var partOne = int.MaxValue;
            var partTwo = int.MaxValue;

            var indices = Enumerable.Range(1, stride - 1).ToList();
            indices.Permutations((slice) => {
                var link = (int from, int to) => distance[stride * from + to];
                var first = link(0, slice[0]);
                var middle = slice.Windows(2).Select(w => link(w[0], w[1])).Sum();
                var last = link(slice[^1], 0);
                partOne = Math.Min(first + middle, partOne);
                partTwo = Math.Min(first + middle + last, partTwo);
            });
            answer = (partOne, partTwo);
        }

        public static int PartOne()
        {
            Parse();
            return answer.partOne;
        }
        public static int PartTwo() => answer.partTwo;

        public static object CastToUnsigned<T>(T number) where T : struct
        {
            unchecked
            {
                switch (number)
                {
                    case long xlong: return (ulong)xlong;
                    case int xint: return (uint)xint;
                    case short xshort: return (ushort)xshort;
                    case sbyte xsbyte: return (byte)xsbyte;
                }
            }
            return number;
        }
    }
}
