using System.Text;
using aoc_fast.Extensions;
using static aoc_fast.Extensions.Hash;

namespace aoc_fast.Years._2019
{
    internal class Day17
    {
        public static string input { get; set; }

        public enum ControlFlow
        {
            Continue,
            Break
        }
        class InputStruct(List<long> code, FastSet<Point> scaffold, Point position, Point direction)
        {
            public List<long> Code { get; set; } = code;
            public FastSet<Point> scaffold { get; set; } = scaffold;
            public Point Position { get; set; } = position;
            public Point Direction { get; set; } = direction;
        }

        class Movement(string routine, List<string?> functions)
        {
            public string Routine { get; set; } = routine;
            public List<string?> Functions { get; set; } = functions;
        }

        private static InputStruct inputStruct;

        private static long Visit(Computer comp)
        {
            comp.InputAscii("n\n");
            var res = 0L;
            while(comp.Run(out var next) == State.Output) res = next;
            return res;
        }
        private static IEnumerable<(string, string)> Segments(string path)
        {
            var indices = new List<int>();
            for(var i = 0; i < path.Length && i < 21; i++)
            {
                if (path[i] == ',') indices.Add(i);
            }
            for(var i = 1; i < indices.Count; i+=2)
            {
                var index = indices[i];
                if (index + 1 < path.Length) yield return (path[0..(index + 1)], path[(index + 1)..]);
            }
        }
        private static ControlFlow Compress(string path, ref Movement movement)
        {
            if (string.IsNullOrEmpty(path)) return ControlFlow.Break;
            if(movement.Routine.Length > 21) return ControlFlow.Continue;
            var chars = new char[] { 'A', 'B', 'C' };
            foreach (var (i, name) in chars.Index())
            {
                movement.Routine += name;
                movement.Routine += ',';
                if (movement.Functions[i] != null)
                {
                    if(path.StartsWith(movement.Functions[i]))
                    {
                        var remaining = path.Substring(movement.Functions[i].Length);
                        if (Compress(remaining, ref movement) == ControlFlow.Break) return ControlFlow.Break;
                    }
                }
                else
                {
                    foreach(var (needle, remianing) in Segments(path))
                    {
                        movement.Functions[i] = needle;
                        if (Compress(remianing, ref movement) == ControlFlow.Break) return ControlFlow.Break;
                        movement.Functions[i] = null;
                    }
                }
                movement.Routine = movement.Routine.Remove(movement.Routine.Length - 1);
                movement.Routine = movement.Routine.Remove(movement.Routine.Length - 1);
            }
            return ControlFlow.Continue;
        }
        private static string BuildPath(InputStruct input)
        {
            var scaffold = input.scaffold;
            var pos = input.Position;
            var direction = input.Direction;
            var path = new StringBuilder();
            while(true)
            {
                var left = direction.CounterClockwise();
                var right = direction.Clockwise();
                if (scaffold.Contains(pos + left)) direction = left;
                else if (scaffold.Contains(pos + right)) direction = right;
                else return path.ToString();

                var next = pos + direction;
                var magnitude = 0;

                while(scaffold.Contains(next))
                {
                    pos = next;
                    next += direction;
                    magnitude++;
                }

                var dir = direction == left ? 'L' : 'R';
                path.Append($"{dir},{magnitude},");
            }
        }
        private static void Parse()
        {
            var code = input.ExtractNumbers<long>();
            var comp = new Computer(code);
            var X = 0;
            var y = 0;
            var scaffold = new FastSet<Point>();
            var position = Directions.ORIGIN;
            var direction = Directions.ORIGIN;

            while (comp.Run(out var next, true) == State.Output)
            {
                switch(next)
                {
                    case 10:
                        y++;
                        break;
                    case 35:
                        scaffold.Add(new Point(X, y)); 
                        break;
                    case 60:
                        position = new Point(X, y);
                        direction = Directions.LEFT;
                        break;
                    case 62:
                        position = new Point(X, y);
                        direction = Directions.RIGHT;
                        break;
                    case 94:
                        position = new Point(X, y);
                        direction = Directions.UP;
                        break;
                    case 118:
                        position = new Point(X, y);
                        direction = Directions.DOWN;
                        break;
                    default: break;
                }
                X = next == 10 ? 0 : X + 1;
            }
            inputStruct = new InputStruct(code, scaffold, position, direction);
        }

        public static int PartOne()
        {
            Parse();
            var scaffold = inputStruct.scaffold.ToHashSet();
            var res = 0;
            foreach (var point in scaffold)
            {
                if (Directions.ORTHOGONAL.All(delta => scaffold.Contains(point + delta))) res += point.X * point.Y;
            }
            return res;
        }
        public static long PartTwo()
        {
            var path = BuildPath(inputStruct);
            var movement = new Movement("", [null, null, null]);
            Compress(path, ref movement);
            var rules = new StringBuilder();
            void newLineEnding(string s)
            {
                rules.Append(s);
                rules.Remove(rules.Length - 1, 1);
                rules.Append('\n');
            }
            newLineEnding(movement.Routine);
            for (var i = 0; i < movement.Functions.Count; i++) newLineEnding(movement.Functions[i]);

            var modified = inputStruct.Code.ToArray();
            modified[0] = 2;
            var comp = new Computer(modified);
            comp.InputAscii(rules.ToString());

            return Visit(comp);
        }
    }
}
