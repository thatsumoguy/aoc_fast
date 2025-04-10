using System.Buffers.Binary;
using System.Text;

namespace aoc_fast.Years._2019
{
    internal class Day8
    {
        public static string input { get; set; }
        public static uint PartOne()
        {
            var bytes = Encoding.ASCII.GetBytes(input);
            var index = 0;
            var ones = 0u;
            var twos = 0u;
            var most = 0u;
            var res = 0u;

            for(var _ = 0; _ < 100; _++)
            {
                for (var i = 0; i < 18; i++)
                {
                    var innerSlice = bytes[index..(index + 8)];
                    var innerN = BinaryPrimitives.ReadUInt64BigEndian(innerSlice);
                    ones += (uint)ulong.PopCount(innerN & 0x0101010101010101);
                    twos += (uint)ulong.PopCount(innerN & 0x0202020202020202);
                    index += 8;
                }

                var slice = bytes[(index - 2)..(index + 6)];
                var n = BinaryPrimitives.ReadUInt64BigEndian(slice);
                ones += (uint)ulong.PopCount(n & 0x0000010101010101);
                twos += (uint)ulong.PopCount(n & 0x0000020202020202);
                index += 6;

                if (ones + twos > most)
                {
                    most = ones + twos;
                    res = ones * twos;
                }
                ones = 0;
                twos = 0;
            }
            return res;
        }

        public static string PartTwo()
        {
            var bytes = Encoding.ASCII.GetBytes(input);
            var image = new char[150];
            Array.Fill(image, '.');

            for(var i = 0; i < 150; i++)
            {
                var j = i;
                while (bytes[j] == '2') j += 150;
                if (bytes[j] == '1') image[i] = '#';
            }
            var res = string.Join("\n\t\t\t ", image.Chunk(25).Select(row => new string(row)));
            res = res.Insert(0, "\n\t\t\t ");
            return res;
        }
    }
}
