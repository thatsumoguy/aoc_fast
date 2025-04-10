using System.Text;
using aoc_fast.Extensions;

namespace aoc_fast.Years._2022
{
    internal class Day22
    {
        public static string input { get; set; }
        enum Tile
        {
            None,
            Open,
            Wall
        }

        record Move
        {
            public record Left : Move;
            public record Right : Move;
            public record Forward(uint a): Move;
        }

        struct Grid(int width, int height, Tile[] tiles, int start, int block)
        {
            public int Width { get; set; } = width;
            public int Height { get; set; } = height;
            public Tile[] Tiles { get; set; } = tiles;
            public int Start { get; set; } = start;
            public int Block { get; set; } = block;

            public Tile Tile(Point point)
            {
                var x = point.X;
                var y = point.Y;
                if(x >= 0 && x < Width && y >= 0 && y < Height) return Tiles[y * Width + x];
                return Day22.Tile.None;
            }
        }

        struct Vector : IEquatable<Vector>
        {
            public int X { get; }
            public int Y { get; }
            public int Z { get; }

            public Vector(int x, int y, int z) => (X, Y, Z) = (x, y, z);

            public static Vector operator -(Vector a) => new(-a.X, -a.Y, -a.Z);

            public static bool operator ==(Vector a, Vector b) => a.Equals(b);
            public static bool operator !=(Vector a, Vector b) => !(a == b);

            public override bool Equals(object? obj) => obj is Vector v && Equals(v);

            public bool Equals(Vector other) => X == other.X && Y == other.Y && Z == other.Z;

            public override int GetHashCode() => HashCode.Combine(X, Y, Z);

        }

        struct Face(Point corner, Vector i, Vector j, Vector k)
        {
            public Point Corner { get; set; } = corner;
            public Vector I { get; set; } = i;
            public Vector J { get; set; } = j;
            public Vector K { get; set; } = k;
        }

        private static Grid Grids;
        private static List<Move> Moves = [];

        private static Grid ParseGrid(string input)
        {
            var raw = input.Split("\n").Select(Encoding.ASCII.GetBytes).ToList();
            var width = raw.Select(line => line.Length).Max();
            var height = raw.Count;

            var tiles = new Tile[width * height];
            for (var i = 0; i < width * height; i++) tiles[i] = Tile.None;

            foreach(var (y, row) in raw.Index())
            {
                foreach(var (x, col) in row.Index())
                {
                    var tile = col switch
                    {
                        (byte)'.' => Tile.Open,
                        (byte)'#' => Tile.Wall,
                        _ => Tile.None,
                    };
                    tiles[y * width + x] = tile;
                }
            }

            var start = Array.FindIndex(tiles, t => t == Tile.Open);
            var block = Numerics.gcd(width, height);
            return new(width, height, tiles, start, block);
        }
        private static List<Move> ParseMoves(string input)
        {
            var moves = new List<Move>();
            var numbers = input.ExtractNumbers<uint>().GetEnumerator();
            var letters = Encoding.ASCII.GetBytes(input).Where(b => char.IsAsciiLetterUpper((char)b)).GetEnumerator();
            
            while(true)
            {
                if (numbers.MoveNext())
                {
                    var n = numbers.Current;
                    moves.Add(new Move.Forward(n));
                }
                else break;
                if (letters.MoveNext())
                {
                    var d = letters.Current;
                    moves.Add(d == (byte)'L' ? new Move.Left() : new Move.Right());
                }
                else break;
            }
            return moves;
        }

        private static int Password(Grid grid, List<Move> moves, Func<Point, Point, (Point, Point)> handleNone)
        {
            var pos = new Point(grid.Start, 0);
            var dir = new Point(1, 0);

            foreach(var command in moves)
            {
                switch(command)
                {
                    case Move.Left:
                        dir = dir.CounterClockwise();
                        break;
                    case Move.Right:
                        dir = dir.Clockwise(); break;
                    case Move.Forward(var n):
                        for(var _ = 0;  _ < n; _++)
                        {
                            var next = pos + dir;
                            switch (grid.Tile(next))
                            {
                                case Tile.Wall: goto EndForward;
                                case Tile.Open:
                                    pos = next; break;
                                case Tile.None:
                                    var (nextPos, nextDir) = handleNone(pos, dir);
                                    if (grid.Tile(nextPos) == Tile.Open)
                                    {
                                        pos = nextPos;
                                        dir = nextDir;
                                        break;
                                    }
                                    else goto EndForward;
                            }
                        }
                    EndForward:;
                        break;
                }
            
            }
            var posScore = 1000 * (pos.Y + 1) + 4 * (pos.X + 1);
            var dirScore = (dir.X, dir.Y) switch
            {
                (1,0) => 0,
                (0,1) => 1,
                (-1,0) => 2,
                (0,-1) => 3,
            };
            return posScore + dirScore;
        }

        private static void Parse()
        {
            var parts = input.Split("\n\n", StringSplitOptions.RemoveEmptyEntries);
            var (prefix, suffix) = (parts[0], parts[1]);
            var grid = ParseGrid(prefix);
            var moves = ParseMoves(suffix);
            Grids = grid;
            Moves = moves;
        }

        public static int PartOne()
        {
            Parse();
            var grid = Grids;
            var block = grid.Block;

            var handleNone = (Point pos, Point dir) =>
            {
                var reverse = dir * -block;
                var next = pos + reverse;

                while (grid.Tile(next) != Tile.None) next += reverse;

                next += dir;
                return (next, dir);
            };

            return Password(grid, Moves, handleNone);
        }
        public static int PartTwo()
        {
            var grid = Grids;
            var block = grid.Block;
            var edge = block - 1;

            var start = new Face(new Point(grid.Start - grid.Start % block, 0), new Vector(1, 0, 0), new Vector(0, 1, 0), new Vector(0, 0, 1));

            var todo = new Queue<Face>();
            todo.Enqueue(start);
            var faces = new Dictionary<Vector, Face>
            {
                [start.K] = start
            };
            var corners = new Dictionary<Point, Face>
            {
                [start.Corner] = start
            };

            while (todo.TryDequeue(out var next))
            {
                var (corner, i, j, k) = (next.Corner, next.I, next.J, next.K);

                Face[] neighbors =
                [
                new Face(corner + new Point(-block, 0), -k, j, i),
                    new Face(corner + new Point(block, 0), k, j, -i),
                    new Face(corner + new Point(0, -block), i, -k, j),
                    new Face(corner + new Point(0, block), i, k, -j),
                ];

                foreach (var nextFace in neighbors)
                {
                    if (grid.Tile(nextFace.Corner) != Tile.None && !faces.ContainsKey(nextFace.K))
                    {
                        todo.Enqueue(nextFace);
                        faces[nextFace.K] = nextFace;
                        corners[nextFace.Corner] = nextFace;
                    }
                }
            }
            var handleNone = (Point pos, Point dir) =>
            {
                var offset = new Point(pos.X % block, pos.Y % block);
                var corner = pos - offset;

                var (i, j, k) = (corners[corner].I, corners[corner].J, corners[corner].K);

                var nextK = (dir.X, dir.Y) switch
                {
                    (-1, 0) => i,
                    (1, 0) => -i,
                    (0, -1) => j,
                    (0, 1) => -j
                };
                var (nextCorner, nextI, nextJ) = (faces[nextK].Corner, faces[nextK].I, faces[nextK].J);

                var nextDir = k == nextI ? Directions.RIGHT : k == -nextI ? Directions.LEFT : k == nextJ ? Directions.DOWN : k == -nextJ ? Directions.UP : throw new NotImplementedException();

                var nextOffset = ((dir.X, dir.Y), (nextDir.X, nextDir.Y)) switch
                {
                    ((-1, 0), (-1, 0)) => new Point(edge, offset.Y),
                    ((-1, 0), (1, 0)) => new Point(0, edge - offset.Y),
                    ((-1, 0), (0, 1)) => new Point(offset.Y, 0),
                    ((-1, 0), (0, -1)) => new Point(edge - offset.Y, edge),
                    ((1, 0), (-1, 0)) => new Point(edge, edge - offset.Y),
                    ((1, 0), (1, 0)) => new Point(0, offset.Y),
                    ((1, 0), (0, 1)) => new Point(edge - offset.Y, 0),
                    ((1, 0), (0, -1)) => new Point(offset.Y, edge),
                    ((0, 1), (-1, 0)) => new Point(edge, offset.X),
                    ((0, 1), (1, 0)) => new Point(0, edge - offset.X),
                    ((0, 1), (0, 1)) => new Point(offset.X, 0),
                    ((0, 1), (0, -1)) => new Point(edge - offset.X, edge),
                    ((0, -1), (-1, 0)) => new Point(edge, edge - offset.X),
                    ((0, -1), (1, 0)) => new Point(0, offset.X),
                    ((0, -1), (0, 1)) => new Point(edge - offset.X, 0),
                    ((0, -1), (0, -1)) => new Point(offset.X, edge)
                };

                var nextPos = nextCorner + nextOffset;
                return (nextPos, nextDir);
            };

            return Password(grid, Moves, handleNone);
        }
    }
}
