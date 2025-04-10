using System.Text;
using aoc_fast.Extensions;

namespace aoc_fast.Years._2021
{
    internal class Day23
    {
        public static string input { get; set; }
        const uint A = 0;
        const uint B = 1;
        const uint C = 2;
        const uint D = 3;
        const uint ROOM = 4;
        const uint EMPTY = 5;
        static readonly uint[] COST = [1, 10, 100, 1000];

        class Room(ushort packed) : IEquatable<Room>
        {
            public ushort Packed { get; set; } = packed;
            public static Room New(ulong[] spaces)
            {
                var packed = (1 << 12) | (spaces[0] << 9) | (spaces[1] << 6) | (spaces[2] << 3) | spaces[3];
                return new((ushort)packed);
            }
            public ulong Size() => (ulong)((15 - ushort.LeadingZeroCount(Packed)) / 3);
            public ulong? Peek() => Packed > 1 ? (ulong)(Packed & 0b111) : null;
            public ulong Pop()
            {
                var pod = (ulong)(Packed & 0b111u);
                Packed >>= 3;
                return pod;
            }

            public bool Open(ulong kind) => Packed == 1 || Packed == (1 << 3) + (ushort)kind || Packed == (1 << 6) + (ushort)(kind * 9) || Packed == (1 << 9) + (ushort)(kind * 73) || Packed == (1 << 12) + (ushort)(kind * 585);

            public void Push(ulong kind) => Packed = (ushort)((Packed << 3) | (ushort)kind);

            public ulong Space(ulong index)
            {
                var adjusted = 3 * (Size() - 1 - index);
                return (ulong)((Packed >> (int)adjusted) & 0b111);
            }

            public bool Equals(Room? other) => other is not null && Packed == other.Packed;

            public override bool Equals(object? obj) => obj is Room other && Equals(other);

            public override int GetHashCode() => Packed.GetHashCode();
            public override string ToString()
            {
                return Packed.ToString();
            }
        }
        class Hallway(ulong packed) : IEquatable<Hallway>
        {
            public ulong Packed { get; set; } = packed;

            public static Hallway New() => new(0x55454545455);

            public ulong Get(ulong index) => (Packed >> (int)(index * 4)) & 0xfu;

            public void Set(ulong index, ulong value)
            {
                var mask = ~(0xfuL << (int)(index * 4));
                var val = value << (int)(index * 4);
                Packed = (Packed & mask) | val;
            }

            public bool Equals(Hallway? other) => other is not null && Packed == other.Packed;

            public override bool Equals(object? obj) => obj is Hallway other && Equals(other);

            public override int GetHashCode() => Packed.GetHashCode();
            public override string ToString() => Packed.ToString();
        }


        class Burrow(Hallway hallway, Room[] rooms) : IEquatable<Burrow>
        {
            public Hallway Hallway { get; set; } = hallway;
            public Room[] Rooms { get; set; } = rooms;

            public static Burrow New(ulong[][] rooms)
            {
                var burrowRooms = new Room[4];
                for (var i = 0; i < 4; i++) burrowRooms[i] = Room.New(rooms[i]);
                return new(Hallway.New(), burrowRooms);
            }

            public bool Equals(Burrow? other)
            {
                if (other is null) return false;
                if (ReferenceEquals(this, other)) return true;
                if (Hallway.Packed != other.Hallway.Packed) return false;
                if (Rooms.Length != other.Rooms.Length) return false;
                for (int i = 0; i < Rooms.Length; i++)
                {
                    if (Rooms[i].Packed != other.Rooms[i].Packed)
                        return false;
                }

                return true;
            }

            public override bool Equals(object? obj) => obj is Burrow other && Equals(other);

            public override int GetHashCode()
            {
                int hash = Hallway.Packed.GetHashCode();
                foreach (var room in Rooms)
                {
                    hash = HashCode.Combine(hash, room.Packed);
                }
                return hash;
            }
            public Burrow DeepCopy()
            {
                Hallway newHallway = new(Hallway.Packed);
                var newRooms = new Room[Rooms.Length];
                for (var i = 0; i < Rooms.Length; i++)
                {
                    newRooms[i] = new Room(Rooms[i].Packed);
                }
                return new Burrow(newHallway, newRooms);
            }
            public override string ToString()
            {
                var sb = new StringBuilder();
                sb.AppendLine("Rooms:");
                foreach(var room in Rooms)
                {
                    sb.Append(room.ToString());
                    sb.Append(", ");
                }
                sb.AppendLine($"Hallway {Hallway}");
                return sb.ToString();
            }
        }

        private static bool DeadlockRoom(Burrow burrow, ulong kind)
        {
            var leftKind = burrow.Hallway.Get(1 + 2 * kind);
            var rightKind = burrow.Hallway.Get(3 + 2 * kind);

            return leftKind != EMPTY && rightKind != EMPTY && leftKind >= kind && rightKind <= kind && !(burrow.Rooms[kind].Open(kind) && (kind == rightKind || kind == leftKind));
        }
        private static bool DeadlockRight(Burrow burrow)
        {
            var room = burrow.Rooms[3];
            var size = room.Size();
            return burrow.Hallway.Get(7) == D && size >= 3 && room.Space(size - 3) != D;
        }
        private static bool DeadlockLeft(Burrow burrow)
        {
            var room = burrow.Rooms[0];
            var size = room.Size();
            return burrow.Hallway.Get(3) == A && size >= 3 && room.Space(size - 3) != A;
        }
        private static void Expand(PriorityQueue<Burrow, ulong> todo, Hash.FastMap<Burrow, ulong> seen, Burrow burrow, ulong energy, ulong roomIndex, IEnumerable<ulong> iter)
        {
            var burr = burrow.DeepCopy();
            var kind = burr.Rooms[roomIndex].Pop();

            foreach (var hallwayIndex in iter)
            {
                switch (burr.Hallway.Get(hallwayIndex))
                {
                    case ROOM:
                        break;
                    case EMPTY:
                        Burrow next = burr.DeepCopy();
                        next.Hallway.Set(hallwayIndex, kind);
                        if (DeadlockLeft(next)
                            || DeadlockRight(next)
                            || DeadlockRoom(next, 0)
                            || DeadlockRoom(next, 1)
                            || DeadlockRoom(next, 2)
                            || DeadlockRoom(next, 3))
                        {
                            continue;
                        }

                        var start = 2 + 2 * roomIndex;
                        var end = 2 + 2 * kind;
                        ulong adjust;

                        if (start == end)
                        {
                            var across = hallwayIndex > start ? hallwayIndex - start : start - hallwayIndex;
                            adjust = across - 1;
                        }
                        else
                        {
                            var lower = start.Min(end);
                            var upper = start.Max(end);
                            adjust = lower >= hallwayIndex ? lower - hallwayIndex : 0ul + hallwayIndex >= upper ? hallwayIndex - upper : 0ul;
                        }
                        var extra = COST[kind] * 2 * adjust;
                        if (kind != roomIndex && extra == 0)
                        {
                            continue;
                        }

                        var nextEnergy = energy + extra;
                        var min = seen.TryGetValue(next, out ulong value) ? value : ulong.MaxValue;

                        if (nextEnergy < min)
                        {
                            todo.Enqueue(next, nextEnergy);
                            seen[next] = nextEnergy;
                        }
                        break;

                    default:
                        goto OuterLoop;
                }
            }

        OuterLoop:;
        }

        private static bool Condense(Burrow burrow, ulong kind, IEnumerable<ulong> iter)
        {
            var changed = false;
            foreach (var halwayIndex in iter)
            {
                
                switch (burrow.Hallway.Get(halwayIndex))
                {
                    case EMPTY: break;
                    case ROOM:
                        var roomIndex = (halwayIndex - 2) / 2;
                        var peekedKind = burrow.Rooms[roomIndex].Peek();
                        while (peekedKind.HasValue && peekedKind.Value == kind)
                        {
                            burrow.Rooms[roomIndex].Pop();
                            burrow.Rooms[kind].Push(kind);
                            changed = true;
                            peekedKind = burrow.Rooms[roomIndex].Peek();
                        }
                        break;
                    case var pod when pod == kind:
                        burrow.Hallway.Set(halwayIndex, EMPTY);
                        burrow.Rooms[kind].Push(kind);
                        changed = true;
                        break;
                    default: goto OuterLoop;
                }
            }
        OuterLoop:;
            return changed;
        }

        private static ulong BestPossible(Burrow burrow)
        {
            var energy = 0ul;
            var needToMove = new ulong[4];

            foreach (var (originalKind, room) in burrow.Rooms.Index())
            {
                var blocker = false;

                for (var depth = 0ul; depth < room.Size(); depth++)
                {
                    var kind = room.Space(depth);
                    if (kind != (ulong)originalKind)
                    {
                        blocker = true;
                        needToMove[kind]++;

                        var up = 4 - depth;
                        var across = 2 * (kind >= (ulong)originalKind ? kind - (ulong)originalKind : (ulong)originalKind - kind);
                        var down = needToMove[kind];
                        energy += COST[kind] * (up + across + down);
                    }
                    else if (blocker)
                    {
                        needToMove[kind]++;
                        var up = 4 - depth;
                        var across = 2ul;
                        var down = needToMove[kind];
                        energy += COST[kind] * (up + across + down);
                    }
                }
            }
            return energy;
        }

        private static ulong Organize(Burrow burrow)
        {
            var todo = new PriorityQueue<Burrow, ulong>(20000);
            var seen = new Hash.FastMap<Burrow, ulong>(20000);

            todo.Enqueue(burrow, BestPossible(burrow));
            while (todo.TryDequeue(out var burr, out var energy))
            {
                var open = new bool[4];
                for (var i = 0ul; i < 4; i++) open[i] = burr.Rooms[i].Open(i);

                var changed = false;

                foreach (var (i, opened) in open.Index())
                {
                    if (opened && burr.Rooms[i].Size() < 4)
                    {
                        var offset = 2 + 2 * i;
                        var forward = Enumerable.Range(offset + 1, 11 - (offset + 1)).Select(i => (ulong)i);
                        var reverse = Enumerable.Range(0, offset).Reverse().Select(i => (ulong)i);
                        changed |= Condense(burr, (ulong)i, forward);
                        changed |= Condense(burr, (ulong)i, reverse);
                    }
                }
                if (changed)
                {
                    if (burr.Rooms.Index().All(r => open[r.Index] && r.Item.Size() == 4)) return energy;
                    var min = seen.TryGetValue(burr, out var val) ? val : ulong.MaxValue;
                    if (energy < min)
                    {
                        todo.Enqueue(burr, energy);
                        seen[burr] = energy;
                    }
                }
                else
                {
                    foreach (var (i, opened) in open.Index())
                    {
                        if (!opened)
                        {
                            var offset = 2 + 2 * i;
                            var forward = Enumerable.Range(offset + 1, 11 - (offset + 1)).Select(i => (ulong)i);
                            var reverse = Enumerable.Range(0, offset).Reverse().Select(i => (ulong)i);
                            
                            Expand(todo, seen, burr, energy, (ulong)i, forward);
                            Expand(todo, seen, burr, energy, (ulong)i, reverse);
                        }
                    }
                }
            }
            throw new NotSupportedException();
        }

        private static List<List<ulong>> Parts = [];

        private static void Parse() => Parts = input.Split("\n", StringSplitOptions.RemoveEmptyEntries).Select(line => Encoding.ASCII.GetBytes(line).Select(b => (ulong)(byte.CreateSaturating(b - (byte)'A'))).ToList()).ToList();

        public static ulong PartOne()
        {
            Parse();
            var burrow = Burrow.New([[A, A, Parts[3][3], Parts[2][3]], [B, B, Parts[3][5], Parts[2][5]], [C, C, Parts[3][7], Parts[2][7]], [D, D, Parts[3][9], Parts[2][9]]]);
            return Organize(burrow);
        }
        public static ulong PartTwo()
        {
            var burrow = Burrow.New([[Parts[3][3], D, D, Parts[2][3]], [Parts[3][5], B, C, Parts[2][5]], [Parts[3][7], A, B, Parts[2][7]], [Parts[3][9], C, A, Parts[2][9]]]);
            return Organize(burrow);
        }
    }
}

