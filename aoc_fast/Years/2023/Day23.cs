using aoc_fast.Extensions;

namespace aoc_fast.Years._2023
{
    internal class Day23
    {
        public static string input { get; set; }

        record Input(uint extra, uint[][] horizontal, uint[][] vertical);

        struct State(byte letter, bool skipped, byte[][] grid, byte[] convert, uint result)
        {
            public byte Letter { get; set; } = letter;
            public bool Skipped { get; set; } = skipped;
            public byte[][] Grid { get; set; } = grid;
            public byte[] Convert { get; set; } = convert;
            public uint Result { get; set; } = result;
        }

        private static Input inputObj;

        private static void DFS(Input input, ref State state, int row, int col, uint steps)
        {
            if(col == 6)
            {
                if(row == 5)
                {
                    state.Result = state.Result.Max(steps);
                    return;
                }
                row++;
                col = 0;
            }

            if (state.Grid[row][col] == 0)
            {
                if (!state.Skipped || (row == 5 && col == 5))
                {
                    state.Skipped = true;
                    state.Grid[row + 1][col] = 0;
                    DFS(input, ref state, row, col + 1, steps);
                    state.Skipped = false;
                }

                if(row < 5)
                {
                    var id = state.Letter;

                    steps += input.vertical[row][col];

                    for(var end = (col + 1); end < 6; end++)
                    {
                        state.Grid[row + 1][end - 1] = 0;
                        steps += input.horizontal[row][end - 1];

                        if (state.Grid[row][end] == 0)
                        {
                            state.Grid[row + 1][col] = id;
                            state.Grid[row + 1][end] = id;

                            var extra = input.vertical[row][end];
                            state.Letter++;
                            DFS(input, ref state, row, end + 1, steps + extra);
                            state.Letter--;
                        }
                        else
                        {
                            state.Grid[row + 1][col] = state.Convert[state.Grid[row][end]];
                            state.Grid[row + 1][end] = 0;
                            DFS(input, ref state, row, end + 1, steps);
                            break;
                        }
                    }
                }
            }
            else
            {
                var index = (int)state.Grid[row][col];
                var id = state.Convert[index];

                if(row < 5 || col == 5)
                {
                    state.Grid[row + 1][col] = id;
                    var extra = input.vertical[row][col];
                    DFS(input, ref state, row, col + 1, steps + extra);
                }

                for(var end = (col + 1);  end < 6; end++)
                {
                    state.Grid[row + 1][end - 1] = 0;
                    steps += input.horizontal[row][end - 1];

                    if (state.Grid[row][end] == 0)
                    {
                        if(row < 5 || end == 5)
                        {
                            state.Grid[row + 1][end] = id;
                            var extra = input.vertical[row][end];
                            DFS(input, ref state, row, end + 1, steps + extra);
                        }
                    }
                    else
                    {
                        var other = state.Convert[(int)state.Grid[row][end]];

                        if(id != other)
                        {
                            state.Grid[row + 1][end] = 0;
                            state.Convert[index] = other;
                            DFS(input, ref state, row, end + 1, steps);
                            state.Convert[index] = id;
                        }
                        break;
                    }
                }
            }
        }

        private static Input GraphToGrid(Point start, Point end, Dictionary<Point, HashSet<Point>> edges, Dictionary<(Point, Point), uint> weight)
        {
            var extra = 2u;
            extra += edges[start].Select(e => weight[(start, e)]).Sum();
            extra += edges[end].Select(e => weight[(e, end)]).Sum();

            var places = new Point[6][];
            for (var i = 0; i < 6; i++)
            {
                places[i] = new Point[6];
                for (var j = 0; j < 6; j++) places[i][j] = new Point(0, 0);
            }

            var horizontal = new uint[6][];
            for (var i = 0; i < 6; i++) horizontal[i] = new uint[6];

            var vertical = new uint[6][];
            for (var i = 0; i < 6; ++i) vertical[i] = new uint[6];

            var point = edges[start].First();
            var seen = new HashSet<Point>();

            var nextPerimeter = (Point point) =>
            {
                seen.Add(point);
                foreach (var next in edges[point])
                {
                    if (edges[next].Count == 3 && !seen.Contains(next))
                        return next;
                }
                return new Point(0, 0);
            };
            for (var y = 0; y < 5; y++)
            {
                places[y][0] = point;
                point = nextPerimeter(point);
            }

            for (var x = 1; x < 6; x++)
            {
                places[5][x] = point;
                point = nextPerimeter(point);
            }
            for (var y = 4; y >= 1; y--)
            {
                places[y][5] = point;
                point = nextPerimeter(point);
            }
            for (var x = 4; x >= 1; x--)
            {
                places[0][x] = point;
                point = nextPerimeter(point);
            }
            for (var y = 1; y < 5; y++)
            {
                for (var x = 1; x < 5; x++)
                {
                    var above = places[y - 1][x];
                    var left = places[y][x - 1];
                    var (subPoint, _) = edges.Where(k => !seen.Contains(k.Key) && k.Value.Contains(above) && k.Value.Contains(left)).First();

                    places[y][x] = subPoint;
                    seen.Add(subPoint);
                }
            }

            places[0][5] = places[0][4];
            places[5][0] = places[5][1];

            for (var y = 0; y < 6; y++)
            {
                for (var x = 0; x < 5; x++)
                {
                    var key = (places[y][x], places[y][x + 1]);
                    horizontal[y][x] = weight.GetValueOrDefault(key, 0u);
                }
            }

            for (var y = 0; y < 5; y++)
            {
                for (var x = 0; x < 6; x++)
                {
                    var key = (places[y][x], places[y + 1][x]);
                    vertical[y][x] = weight.GetValueOrDefault(key, 0u);
                }
            }

            return new Input(extra, horizontal, vertical);
        }

        private static void Parse()
        {
            var grid = Grid<byte>.Parse(input.Trim());
            var width = grid.width;
            var height = grid.height;

            grid[1, 0] = (byte)'#';
            grid[width - 2, height - 1] = (byte)'#';

            var start = new Point(1, 1);
            var end = new Point(width - 2, height - 2);

            grid[start] = (byte)'P';
            grid[end] = (byte)'P';

            var poi = new List<Point>
            {
                start,
                end
            };

            for(var y = 1; y < height - 1; y++)
            {
                for(var x = 1;  x < width - 1; x++)
                {
                    var pos = new Point(x, y);

                    if (grid[pos] != (byte)'#')
                    {
                        var neighbors = Directions.ORTHOGONAL.Select(o => pos + o).Where(n => grid[n] != (byte)'#').Count();
                        if(neighbors > 2)
                        {
                            grid[pos] = (byte)'P';
                            poi.Add(pos);
                        }
                    }
                }
            }


            var todo = new Queue<(Point, uint)>();
            var edges = new Dictionary<Point, HashSet<Point>>();
            var weight = new Dictionary<(Point, Point), uint>();


            foreach(var from in poi)
            {
                todo.Enqueue((from, 0u));
                grid[from] = (byte)'#';
                weight[(from, from)] = 0u;

                while(todo.TryDequeue(out var next))
                {
                    var (pos, cost) = next;

                    foreach(var direction in Directions.ORTHOGONAL)
                    {
                        var to = pos + direction;
                        switch(grid[to])
                        {
                            case (byte)'#': break;
                            case (byte)'P':
                                if (!edges.TryGetValue(from, out var fromSet))
                                {
                                    fromSet = [];
                                    edges[from] = fromSet;
                                }
                                fromSet.Add(to);

                                if (!edges.TryGetValue(to, out var toSet))
                                {
                                    toSet = [];
                                    edges[to] = toSet;
                                }
                                toSet.Add(from);

                                weight[(from, to)] = cost + 1;
                                weight[(to, from)] = cost + 1;
                                break;
                            default:
                                todo.Enqueue((to, cost + 1));
                                grid[to] = (byte)'#';
                                break;
                        }
                    }
                }
            }

            inputObj = GraphToGrid(start, end, edges, weight);
        }

        public static uint PartOne()
        {
            Parse();
            var total = new uint[6][];
            for (var i = 0; i < total.Length; i++) total[i] = new uint[6];

            for (var y = 0; y < 6; y++)
            {
                for (var x = 0; x < 6; x++)
                {
                    var left = x == 0 ? 0 : total[y][x - 1] + inputObj.horizontal[y][x - 1];
                    var above = y == 0 ? 0 : total[y - 1][x] + inputObj.vertical[y - 1][x];
                    total[y][x] = left.Max(above);
                }
            }

            return inputObj.extra + total[5][5];
        }

        public static uint PartTwo()
        {
            var grid = new byte[7][];
            for (var i = 0; i < 7; i++) grid[i] = new byte[6];
            var state = new State(2, false, grid, new byte[32], 0);

            state.Grid[0][0] = 1;

            for (var i = 0; i < 32; i++) state.Convert[i] = (byte)i;

            DFS(inputObj, ref state, 0, 0, 0);
            return inputObj.extra + state.Result;
        }
    }
}
