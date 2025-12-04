using System.Collections;
using System.Numerics;
using System.Text;
using aoc_fast.Extensions;

namespace aoc_fast.Years._2022
{
    internal class Day17
    {
        public static string input { get; set; }
        public readonly struct Rock(int size, uint shape)
        {
            public int Size { get; } = size;
            public uint Shape { get; } = shape;
        }

        private const byte FLOOR = 0xff;
        private const uint WALLS = 0x01010101;
        private static readonly Rock[] ROCKS =
        [
            new(1, 0x0000003c),
            new(3, 0x00103810),
            new(3, 0x00080838),
            new(4, 0x20202020),
            new(2, 0x00003030),
        ];

        //Creating a custom State class to be able to get a custom enumerator so I could do a transformation on iteration.
        public class State : IEnumerable<int>, IEnumerator<int>
        {
            private readonly IEnumerator<Rock> _rockEnumerator;
            private readonly IEnumerator<byte> _jetEnumerator;
            private readonly byte[] _tower;
            public int Height { get; private set; }
            public int Current { get; private set; }
            private readonly int _towerIndex = 13000;

            public State(byte[] jets)
            {
                _rockEnumerator = ROCKS.ToList().Cycle().GetEnumerator();
                _jetEnumerator = jets.ToList().Cycle().GetEnumerator();
                _tower = new byte[_towerIndex];
                _tower[0] = FLOOR;
                Height = 0;
                Current = 0;
            }
            public bool MoveNext()
            {
                _rockEnumerator.MoveNext();
                Rock currentRock = _rockEnumerator.Current;
                var shape = currentRock.Shape;
                var chunk = WALLS;
                var index = Height + 3;

                while (true)
                {
                    _jetEnumerator.MoveNext();
                    byte jet = _jetEnumerator.Current;
                    var candidate = (jet == (byte)'<')
                        ? BitOperations.RotateLeft(shape, 1)
                        : BitOperations.RotateRight(shape, 1);

                    if ((candidate & chunk) == 0) shape = candidate;

                    chunk = (chunk << 8) | WALLS | _tower[index];


                    if ((shape & chunk) == 0) index--;
                    else
                    {
                        byte[] bytes = BitConverter.GetBytes(shape);
                        _tower[index + 1] |= bytes[0];
                        _tower[index + 2] |= bytes[1];
                        _tower[index + 3] |= bytes[2];
                        _tower[index + 4] |= bytes[3];
                        Height = Math.Max(Height, index + currentRock.Size);
                        Current = Height;
                        break;
                    }
                }
                return true;
            }

            public void Reset() { }

            object IEnumerator.Current => Current;

            public IEnumerator<int> GetEnumerator() => this;

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

            public void Dispose()
            {
                _rockEnumerator.Dispose();
                _jetEnumerator.Dispose();
            }
        }

        private static byte[] bytes = [];

        public static int PartOne()
        {
            bytes = Encoding.ASCII.GetBytes(input.Trim());
            return new State(bytes).Skip(2021).First();
        }
        public static ulong PartTwo()
        {
            var guess = 1000;
            var height = new State(bytes).Take(5 * guess).ToList();
            var deltas = height.Scan(0).ToList();

            var end = deltas.Count - guess;
            var needle = deltas[end..];
            var start = deltas.Windows(guess).ToList().FindIndex(w => Enumerable.SequenceEqual(w, needle));
            var cycleHeight = height[end] - height[start];
            var cycleWidth = end - start;
            ulong offset = 1_000_000_000_000ul - 1ul - (ulong)start;
            var quotient = offset / (ulong)cycleWidth;
            var remainder = offset % (ulong)cycleWidth;
            return (quotient * (ulong)cycleHeight) + (ulong)height[start + (int)remainder];
        }
    }
}
