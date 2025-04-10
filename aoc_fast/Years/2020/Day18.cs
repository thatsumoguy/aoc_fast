using System.Text;

namespace aoc_fast.Years._2020
{
    internal class Day18
    {
        public static string input { get; set; }
        private static bool Next(IEnumerator<byte> bytes, out byte b)
        {
            if (!bytes.MoveNext() || bytes.Current == (byte)')')
            {
                b = default; 
                return false;
            }
            if (bytes.Current == (byte)' ')
            {
                b = bytes.MoveNext() ? bytes.Current : default;
                return true;
            }
            b = bytes.Current;
            return true;
        }
        private static ulong Value(IEnumerator<byte> enumerator, Func<IEnumerator<byte>, ulong> helper)
        {
            if (!Next(enumerator, out var b)) throw new Exception();
            

            return b == (byte)'(' ? helper(enumerator) : ToDecimal(b);
        }

        private static ulong ToDecimal(byte b) => (ulong)(b - '0');

        private static string[] Lines = [];

        public static ulong PartOne()
        {
            Lines = input.TrimEnd().Split("\n");
            static ulong helper(IEnumerator<byte> bytes)
            {
                var total = Value(bytes, helper);
                while (Next(bytes, out var operation))
                {
                    var val = Value(bytes, helper);
                    if (operation == '+') total += val;
                    else total *= val;
                }
                return total;
            }
            return Lines.Select(line =>
            {
                var bytes = Encoding.UTF8.GetBytes(line);
                return helper(((IEnumerable<byte>)bytes).GetEnumerator());
            }).Aggregate(0ul, (acc, i) => acc + i);
        }
        public static ulong PartTwo()
        {
            static ulong helper(IEnumerator<byte> bytes)
            {
                var total = Value(bytes, helper);
                while (Next(bytes, out var operation))
                {
                    if (operation == '+') total += Value(bytes, helper);
                    else
                    {
                        total *= helper(bytes);
                        break;
                    }
                }
                return total;
            }
            return Lines.Select(line =>
            {
                var bytes = Encoding.UTF8.GetBytes(line);
                return helper(((IEnumerable<byte>)bytes).GetEnumerator());
            }).Aggregate(0ul, (acc, i) => acc + i);
        }

    }
}
