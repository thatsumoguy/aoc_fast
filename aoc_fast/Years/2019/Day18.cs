using System.Text;
using aoc_fast.Extensions;
using static aoc_fast.Extensions.Hash;

namespace aoc_fast.Years._2019
{
    internal class Day18
    {
        public static string input { get; set; }

        class State(uint position, uint remaining) : IEquatable<State>
        {
            public uint Position { get; set; } = position;
            public uint Remaining { get; set; } = remaining;

            public bool Equals(State? other)
            {
                if (other != null)
                {
                    return other.Position == Position && other.Remaining == Remaining;
                }
                else return false;
            }
            public override int GetHashCode() => HashCode.Combine(Position, Remaining);
        }
        class Door(uint distance, uint needed)
        {
            public uint Distance { get; set; } = distance;
            public uint Needed { get; set; } = needed;
        }

        class MazeStruct(State initial, Door[][] maze)
        {
            public State Ininital { get; set; } = initial;
            public Door[][] Maze { get; set; } = maze;
        }
        private static bool IsKey(byte b, out int val)
        {
            if(char.IsAsciiLetterLower((char)b))
            {
                val = (int)(b - 'a');
                return true;
            }
            val = 0;
            return false;
        }
        private static bool IsDoor(byte b, out int val)
        {
            if (char.IsAsciiLetterUpper((char)b))
            {
                val = (int)(b - 'a');
                return true;
            }
            val = 0;
            return false;
        }
        private static MazeStruct ParseMaze(int width, byte[] bytes)
        {
            var initial = new State(0, 0);
            var found = new List<(int, int)>();
            var robots = 26;

            foreach(var (i, b) in bytes.Index())
            {
                if(IsKey(b, out var key))
                {
                    initial.Remaining |= 1u << key;
                    found.Add((i, key));
                }
                if(b == '@')
                {
                    initial.Position |= 1u << robots;
                    found.Add((i, robots));
                    robots++;
                }
            }

            int[] vals = [1, -1, width, -width];
            var orthongal = vals.Select(i => (ulong)i).ToArray();
            var maze = new Door[30][];
            for(var i = 0; i < 30; i++)
            {
                maze[i] = new Door[30];
                for (var j = 0; j < 30; j++) maze[i][j] = new Door(int.MaxValue, 0);
            }
            var visited = Enumerable.Repeat(ulong.MaxValue, bytes.Length).ToArray();
            var todo = new List<(ulong, uint, uint)>();
            foreach(var (start, from) in found)
            {
                todo.PushFront(((ulong)start, (uint)0, (uint)0));
                visited[start] = (ulong)from;

                while(todo.TryPopFront(out var value))
                {
                    var (index, distance, needed) = value;
                    if (IsDoor(bytes[index], out var door)) needed |= 1u << door;
                    if(IsKey(bytes[index], out var to))
                    {
                        if(distance > 0)
                        {
                            maze[from][to] = new Door(distance, needed);
                            maze[to][from] = new Door(distance, needed);
                            continue;
                        }
                    }
                    foreach(var delta in orthongal)
                    {
                        var nextIndex = index + delta;
                        if (bytes[nextIndex] != '#' && visited[nextIndex] != (ulong)from)
                        {
                            todo.Add((nextIndex, distance + 1, needed));
                            visited[nextIndex] = (ulong)from;
                        }
                    }
                }
            }

            for (var i = 0; i < 30; i++) maze[i][i].Distance = 0;

            for(var k  = 0; k < 30; k++)
            {
                for(var i = 0; i < 30; ++i)
                {
                    for(var  j = 0; j < 30; j++)
                    {
                        var candidate = maze[i][k].Distance + maze[k][j].Distance;
                        if (maze[i][j].Distance >  candidate)
                        {
                            maze[i][j].Distance = candidate;
                            maze[i][j].Needed = maze[i][k].Needed | (1u << k) | maze[k][j].Needed;
                        }
                    }
                }
            }
            return new MazeStruct(initial, maze);
        }
        private static uint Explore(int width, byte[] bytes)
        {
            var todo = new PriorityQueue<State, uint>(5000);
            var cache = new FastMap<State, uint>(5000);
            var maze = ParseMaze(width, bytes);
            todo.Enqueue(maze.Ininital, 0);

            while(todo.TryDequeue(out var state, out var total))
            {
                if (state.Remaining == 0) return total;

                foreach(var from in state.Position.Biterator())
                {
                    foreach(var to in state.Remaining.Biterator())
                    {
                        var door = maze.Maze[from][to];
                        if (door.Distance != uint.MaxValue && (state.Remaining & door.Needed) == 0)
                        {
                            var nextTotal = total + door.Distance;
                            var fromMask = 1u << from;
                            var toMask = 1u << to;
                            var nextState = new State(state.Position ^ fromMask ^ toMask, state.Remaining ^ toMask);
                            if (cache.TryGetValue(nextState, out uint existingTotal))
                            {
                                if (nextTotal < existingTotal)
                                {
                                    todo.Enqueue(nextState, nextTotal);
                                    cache[nextState] = nextTotal;
                                }
                            }
                            else
                            {
                                todo.Enqueue(nextState, nextTotal);
                                cache[nextState] = nextTotal;
                            }
                        }
                    }
                }
            }
            throw new Exception();
        }
        private static Grid<byte> grid;
        private static void Parse() => grid = Grid<byte>.Parse(input);

        public static uint PartOne()
        {
            Parse();
            return Explore(grid.width, grid.data);
        }
        public static uint PartTwo()
        {
            var modified = grid.data.ToArray();
            var patch = (string s, int offset) =>
            {
                var middle = (grid.width * grid.height) / 2;
                var index = middle + offset * grid.width - 1;
                Encoding.UTF8.GetBytes(s).CopyTo(modified, index);
            };

            patch("@#@", -1);
            patch("###", 0);
            patch("@#@", 1);
            return Explore(grid.width, modified);
        }

    }
}
