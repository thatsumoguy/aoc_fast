using System.Text.RegularExpressions;
using aoc_fast.Extensions;

namespace aoc_fast.Years._2016
{
    partial class Day11
    {
        public static string input
        {
            get;
            set;
        }
        class Floor : ICloneable, IEquatable<Floor>
        {
            public byte Both { get; set; }

            public static Floor New(int generators, int microchips) => new() { Both = (byte)((generators << 4) + microchips) };
            public byte Generators() => (byte)(Both >> 4);
            public byte Microchips() => (byte)(Both & 0xf);
            public byte Total() => (byte)(Generators() + Microchips());
            public bool GTE(Floor other) => Generators() >= other.Generators() && Microchips() >= other.Microchips();
            public bool Valid() => Generators() == 0 || Generators() >= Microchips();
            public Floor Add(Floor other) => new() { Both = (byte)(Both + other.Both)};
            public Floor Sub(Floor other) => new() { Both = (byte)(Both - other.Both) };
            public object Clone()
            {
                return new Floor { Both = Both };
            }

            public bool Equals(Floor? other)
            {
                return this.Both == other.Both;
            }

            public override bool Equals(object obj)
            {
                return Equals(obj as Floor);
            }

            public override int GetHashCode()
            {
                return this.Both.GetHashCode();
            }

            public override string ToString()
            {
                return Both.ToString();
            }
        }
        class State : ICloneable, IEquatable<State>
        {
            public uint Elevator { get;set; }
            public Floor[] Floors { get; set;}

            public static State Default() => new() { Elevator = 0u, Floors = [Floor.New(0, 0), Floor.New(0, 0), Floor.New(0, 0), Floor.New(0, 0)] };
            public object Clone()
            {
                return new State { Elevator = Elevator, Floors = Floors.Select(f => f.Clone() as Floor).ToArray() };
            }

            public bool Equals(State? other)
            {
                if (other == null || Elevator != other.Elevator)
                    return false;

                for (int i = 0; i < Floors.Length; i++)
                {
                    if (!Floors[i].Equals(other.Floors[i]))
                        return false;
                }

                return true;
            }

            public override bool Equals(object obj)
            {
                return Equals(obj as State);
            }

            public override int GetHashCode()
            {
                int hash = Elevator.GetHashCode();
                foreach (var floor in Floors)
                {
                    hash = HashCode.Combine(hash, floor.GetHashCode());
                }
                return hash;
            }
            public override string ToString()
            {
                return $"Elevator:{Elevator}\nFloors:{string.Join("\n\t", Floors.Select(f=> f.ToString()))}"; 
            }
        }

        private static uint BFS(State start)
        {
            var complete = start.Floors.Select(f => f.Total()).Sum();
            Floor[] moves = [Floor.New(2, 0), Floor.New(1,1), Floor.New(0,2), Floor.New(1, 0), Floor.New(0,1)];

            var todo = new Queue<(State, uint)>();
            var seen = new HashSet<State>(30000);

            todo.Enqueue((start, 0u));
            seen.Add(start);
            while(todo.TryDequeue(out var i))
            {
                var state = i.Item1 as State;
                var steps = i.Item2;
                if (state.Floors[3].Total() == complete) return steps;

                var cur = state.Elevator;

                if((state.Elevator == 1 && state.Floors[0].Total() > 0) || 
                    (state.Elevator == 2 && (state.Floors[0].Total() > 0 || 
                    state.Floors[1].Total() > 0)) || state.Elevator == 3)
                {
                    var below = cur - 1;
                    var min = 2;

                    foreach(var delta in moves.Reverse())
                    {
                        if (delta.Total() > min) break;
                        if (state.Floors[cur].GTE(delta))
                        {
                            var candidate = state.Floors[below].Add(delta);

                            if(candidate.Valid())
                            {
                                var next = state.Clone() as State;
                                next.Floors[cur] = next.Floors[cur].Sub(delta);
                                next.Floors[below] = candidate;
                                next.Elevator--;
                                if (seen.Add(next))
                                {
                                    min = delta.Total();
                                    todo.Enqueue((next, steps + 1));
                                }
                            }
                        }
                    }
                }

                if(state.Elevator < 3)
                {
                    var above = cur + 1;
                    var max = 0;

                    foreach(var delta in moves)
                    {
                        if (delta.Total() < max) break;

                        if (state.Floors[cur].GTE(delta))
                        {
                            var candidate = state.Floors[above].Add(delta);

                            if (candidate.Valid())
                            {
                                var next = state.Clone() as State;
                                next.Floors[cur] = next.Floors[cur].Sub(delta);
                                next.Floors[above] = candidate;
                                next.Elevator++;
                                if (seen.Add(next))
                                {
                                    max = delta.Total();
                                    todo.Enqueue((next, steps + 1));
                                }
                            }
                        }
                    }
                }
            }
            throw new Exception();
        }

        private static State Start = State.Default();

        private static void Parse()
        {
            var state = State.Default();
            foreach(var (line, i) in input.Split("\n", StringSplitOptions.RemoveEmptyEntries).Select((l,i) => (l,i)))
            {
                var generators = MyRegex().Matches(line).Count();
                var microchips = new Regex("microchip").Matches(line).Count();
                state.Floors[i] = Floor.New(generators, microchips);
            }

            Start = state;
        }

        public static uint PartOne()
        {
            Parse();
            return BFS(Start);
        }

        public static uint PartTwo()
        {
            var modified = Start.Clone() as State;
            modified.Floors[0] = modified.Floors[0].Add(Floor.New(2, 2));
            return BFS(modified);
        }

        [GeneratedRegex("generator")]
        private static partial Regex MyRegex();
    }
}
