using System.Text;
using aoc_fast.Extensions;

namespace aoc_fast.Years._2017
{
    internal class Day10
    {
        public static string input
        {
            get;
            set;
        }

        private static List<byte> Hash(int[] lengths, int rounds)
        {
            var knot = Enumerable.Range(0, 256).Select(i => (byte)i).ToList();
            var pos = 0;
            var skip = 0;

            for(var _ = 0; _ < rounds; _++)
            {
                foreach(var length in lengths)
                {
                    var next = length + skip;
                    Slice.ReverseList(ref knot, 0, length - 1);
                    knot.RotateLeft(next % 256);
                    
                    pos += next;
                    skip++;
                }
            }

            knot.RotateRight(pos % 256);
            return knot;
        }

        public static int PartOne()
        {
            var lengths = input.ExtractNumbers<int>().ToArray();

            var knot = Hash(lengths, 1);
            return knot.Take(2).Aggregate(1, (a, b) => a * b);
        }

        public static string PartTwo()
        {
            var lengths = Encoding.ASCII.GetBytes(input.Trim()).ToList();
            lengths.AddRange([17, 31, 73, 47, 23]);
            var knot = Hash([.. lengths], 64);
            var res = new StringBuilder();
            foreach(var chunk in knot.Chunk(16))
            {
                var reduced = chunk.Aggregate((byte)0, (acc, n) => (byte)(acc ^ n));
                res.Append(reduced.ToString("x2"));
            }
            return res.ToString();
        }
    }
}
