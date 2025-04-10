using aoc_fast.Extensions;

namespace aoc_fast.Years._2019
{
    internal class Day20
    {
        public static string input { get; set; }
        public enum Kind
        {
            Inner,
            Outer,
            Start,
            End
        }

        public struct Key((byte, byte) pair, Day20.Kind kind)
        {
            public (byte, byte) Pair = pair;
            public Kind Kind = kind;
        }

        public class Tile
        {
            public enum TileType
            {
                Wall,
                Open,
                Portal
            }

            public TileType Type;
            public (Key, Kind) PortalKey;
        }

        public class Edge(int to, Day20.Kind kind, uint distance)
        {
            public int To = to;
            public Kind Kind = kind;
            public uint Distance = distance;
        }

        public class Maze(ulong start, List<List<Edge>> portals)
        {
            public ulong Start = start;
            public List<List<Edge>> Portals = portals;
        }

        private static (Point first, Point second, Point third) GetPortalOrientation(Grid<byte> grid, Point point)
        {
            if (grid[point + Directions.UP] == (byte)'.') return (point, point + Directions.DOWN, point + Directions.UP);
            if (grid[point + Directions.DOWN] == (byte)'.') return (point + Directions.UP, point, point + Directions.DOWN);
            if (grid[point + Directions.LEFT] == (byte)'.') return (point, point + Directions.RIGHT, point + Directions.LEFT);
            if (grid[point + Directions.RIGHT] == (byte)'.') return (point + Directions.LEFT, point, point + Directions.RIGHT);
            return (point, point, point);
        }


        private static Maze maze;
        private static void Parse()
        {
            var grid = Grid<byte>.Parse(input);

            var tiles = grid.data.Select(b => b == '.' ? new Tile { Type = Tile.TileType.Open} : new Tile { Type = Tile.TileType.Wall }).ToList();
            var map = new Dictionary<Key, int>();
            var found = new List<ulong>();
            var start = ulong.MaxValue;

            for(var y = 1; y < grid.height -1; y += 2)
            {
                for(var X = 1; X < grid.width -1; X += 2)
                {
                    var point = new Point(X, y);
                    if (!grid[point].IsAsciiUpperLetter()) continue;
                    var (first, second, third) = GetPortalOrientation(grid, point);
                    if ((first, second, third) == (point, point, point)) continue;

                    var pair = (grid[first], grid[second]);
                    var index = grid.width * third.Y + third.X;
                    var inner = 2 < X && X < grid.width - 3 && 2 < y && y < grid.height - 3;
                    Kind kind, opposite;
                    if(inner)
                    {
                        kind = Kind.Inner;
                        opposite = Kind.Outer;
                    }
                    else
                    {
                        switch(pair)
                        {
                            case ((byte)'A', (byte)'A'):
                                start = (ulong)found.Count;
                                kind = Kind.Start;
                                opposite = Kind.Start;
                                break;
                            case ((byte)'Z', (byte)'Z'):
                                kind = Kind.End;
                                opposite = Kind.End;
                                break;
                            default:
                                kind = Kind.Outer;
                                opposite= Kind.Inner;
                                break;
                        }
                    }

                    tiles[index] = new Tile { Type = Tile.TileType.Portal, PortalKey = (new Key(pair, opposite), kind) };
                    map[new Key(pair, kind)] = found.Count;
                    found.Add((ulong)index);
                }
            }

            var portals = new List<List<Edge>>();
            var todo = new Queue<(ulong, uint)>();
            var visited = new ulong[tiles.Count];
            var orthongonal = new[] { 1, -1, grid.width, -grid.width }.Select(i => ((ulong)i)).ToArray();
            foreach(var startIndx in found)
            {
                var edges = new List<Edge>();
                todo.Enqueue((startIndx, 0u));

                while(todo.TryDequeue(out var pair))
                {
                    var (index, steps) = pair;
                    visited[index] = startIndx;

                    foreach(var offset in orthongonal)
                    {
                        var nextIndex = index + offset;
                        var nextSteps = steps + 1;
                        if (visited[nextIndex] != startIndx)
                        {
                            switch(tiles[(int)nextIndex].Type)
                            {
                                case Tile.TileType.Wall:
                                    break;
                                case Tile.TileType.Open:
                                    todo.Enqueue((nextIndex, nextSteps));
                                    break;
                                case Tile.TileType.Portal:
                                    var key = tiles[(int)nextIndex].PortalKey.Item1;
                                    var kind = tiles[(int)nextIndex].PortalKey.Item2;
                                    var to = map[key];
                                    edges.Add(new Edge(to, kind, nextSteps));
                                    break;

                            }
                        }
                    }

                }
                portals.Add(edges);
            }
            maze = new Maze(start, portals);
        }

        public static uint PartOne()
        {
            Parse();
            var todo = new Queue<(uint, ulong)>();
            todo.Enqueue((0, maze.Start));
            while (todo.TryDequeue(out var next))
            {
                var (steps, index) = next;
                foreach (var edge in maze.Portals[(int)index])
                {
                    var nextSteps = steps + edge.Distance + 1;
                    switch (edge.Kind)
                    {
                        case Kind.Inner or Kind.Outer:
                            todo.Enqueue((nextSteps, (ulong)edge.To));
                            break;
                        case Kind.End: return nextSteps - 1;
                        case Kind.Start:
                            break;
                    }
                }
            }
            return 0;
        }

        public static uint PartTwo()
        {
            var cache = new Dictionary<(ulong, ulong), uint>(2000);
            var todo = new Queue<(uint, ulong, ulong)>();
            todo.Enqueue((0u, maze.Start, 0ul));

            while(todo.TryDequeue(out var next))
            {
                var (steps, index, level) = next;
                var key = (index, level);
                if(cache.TryGetValue(key, out var min))
                {
                    if (min <= steps) continue;
                }
                cache[key] = steps;

                foreach(var edge in maze.Portals[(int)index])
                {
                    var nextSteps = steps + edge.Distance + 1;

                    switch(edge.Kind)
                    {
                        case Kind.Inner when level < (ulong)maze.Portals.Count:
                            todo.Enqueue((nextSteps, (ulong)edge.To, level + 1));
                            break;
                        case Kind.Outer when level >= 0:
                            todo.Enqueue((nextSteps, (ulong)edge.To, level - 1));
                            break;
                        case Kind.End when level == 0:
                            return nextSteps - 1;
                    }
                }
            }
            return 0;
        }
    }
}
