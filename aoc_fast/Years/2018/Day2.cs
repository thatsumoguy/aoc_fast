using System.Text;

namespace aoc_fast.Years._2018
{
    internal class Day2
    {
        public static string input
        {
            get;
            set;
        }
        private static List<byte[]> bytes = [];

        public static int PartOne()
        {
            bytes = input.Split("\n", StringSplitOptions.RemoveEmptyEntries).Select(Encoding.ASCII.GetBytes).ToList();
            var totalTwo = 0;
            var totalThree = 0;

            foreach(var id in bytes)
            {
                var freq = new int[26];
                var twos = 0;
                var threes = 0;

                foreach(var b in id)
                {
                    var index = b - (byte)'a';
                    var cur = freq[index];

                    switch(cur)
                    {
                        case 0: break;
                        case 1: twos++; break;
                        case 2: twos--; threes++; break;
                        default: threes--; break;
                    }
                    freq[index]++;
                }
                if (twos > 0) totalTwo++;
                if(threes > 0) totalThree++;
            }

            return totalTwo * totalThree;
        }

        public static string PartTwo()
        {
            var width = bytes[0].Length;

            var seen = new HashSet<byte[]>(bytes.Count, new ByteArrayEqualityComparer());
            var buffer = new byte[32];

            for(var col = 0; col < width; col++)
            {
                foreach(var id in bytes)
                {
                    Array.Copy(id, buffer, width);
                    buffer[col] = (byte)'*';
                    if (!seen.Add(buffer))
                        return Encoding.ASCII.GetString(buffer.Where(b => char.IsAsciiLetterLower((char)b)).ToArray());
                }
                seen.Clear();
            }
            throw new Exception();
        }


        public class ByteArrayEqualityComparer : IEqualityComparer<byte[]>
        {
            public bool Equals(byte[]? X, byte[]? y)
            {
                if (X == null || y == null) return X == y;
                if (X.Length != y.Length) return false;
                for (int i = 0; i < X.Length; i++)
                {
                    if (X[i] != y[i]) return false;
                }

                return true;
            }

            public int GetHashCode(byte[] obj)
            {
                if (obj == null) return 0;
                unchecked
                {
                    int hash = 17;
                    foreach (byte b in obj)
                    {
                        hash = hash * 31 + b;
                    }
                    return hash;
                }
            }
        }
    }
}
