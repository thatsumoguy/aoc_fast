using System.Text;

namespace aoc_fast.Years._2017
{
    class Day4
    {
        public static string input
        {
            get;
            set;
        }

        private static List<string> lines = [];

        public static int PartOne()
        {
            lines = [.. input.Split('\n', StringSplitOptions.RemoveEmptyEntries)];
            var seen = new HashSet<byte[]>(new bytearraycomparer());
            return lines.Where(l =>
            {
                seen.Clear();
                return l.Split([' ', '\n', '\t'], StringSplitOptions.RemoveEmptyEntries).All(t => seen.Add(Encoding.ASCII.GetBytes(t)));
            }).Count();
        }

        public static int PartTwo()
        {
            byte[] convert(string token)
            {
                var freq = new byte[26];
                foreach (var b in Encoding.ASCII.GetBytes(token)) freq[(b - 'a')]++;
                return freq;
            }
            var seen = new HashSet<byte[]>(new bytearraycomparer());
            return lines.Where(l =>
            {
                seen.Clear();
                return l.Split([' ', '\n', '\t'], StringSplitOptions.RemoveEmptyEntries).All(t => seen.Add(convert(t)));
            }).Count();
        }

        public class bytearraycomparer : IEqualityComparer<byte[]>
        {
            public bool Equals(byte[] a, byte[] b)
            {
                if (a.Length != b.Length) return false;
                for (int i = 0; i < a.Length; i++)
                    if (a[i] != b[i]) return false;
                return true;
            }
            public int GetHashCode(byte[] a)
            {
                uint b = 0;
                for (int i = 0; i < a.Length; i++)
                    b = ((b << 23) | (b >> 9)) ^ a[i];
                return unchecked((int)b);
            }
        }
    }
}
