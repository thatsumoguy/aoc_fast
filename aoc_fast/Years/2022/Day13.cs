using System.Text;
using aoc_fast.Extensions;

namespace aoc_fast.Years._2022
{
    internal class Day13
    {
        public static string input { get; set; }
        private static List<string> Input = [];

        class Packet
        {
            public byte[] Slice { get; set; }
            public int Index { get; set; }
            public List<byte> Extra { get; set; }

            public static Packet New(string str) => new() { Slice = Encoding.ASCII.GetBytes(str), Index = 0, Extra = []};

            public bool Next(out byte result)
            {

                if(Extra.PopCheck(out var res))
                {
                    result = res;
                    return true;
                }
                var index = Index;
                var slice = new byte[Slice.Length];
                for (var i = 0; i < slice.Length; i++) slice[i] = Slice[i];
                if (slice[index] == (byte)'1' && slice[index +1] == (byte)'0')
                {
                    Index += 2;
                    result = (byte)'A';
                    return true;
                }
                else
                {
                    Index++;
                    result = slice[index];
                    return true;
                }
            }
        }

        private static bool Compare(string left, string right)
        {
            var leftPacket = Packet.New(left);
            var rightPacket = Packet.New(right);

            while(leftPacket.Next(out var a) && rightPacket.Next(out var b))
            {
                switch((a,b))
                {
                    case (var c, var d) when (c == d): break;
                    case ((byte)']', _): return true;
                    case (_, (byte)']'): return false;
                    case ((byte)'[', var e):
                        rightPacket.Extra.Add((byte)']');
                        rightPacket.Extra.Add(e);
                        break;
                    case (var f, (byte)'['):
                        leftPacket.Extra.Add((byte)']');
                        leftPacket.Extra.Add(f);
                        break;
                    case (var g, var h): return g < h;
                }
            }
            throw new NotImplementedException();
        }

        private static void Parse() => Input = input.Split("\n").Where(l => !string.IsNullOrEmpty(l)).ToList();

        public static int PartOne()
        {
            Parse();
            return Input.ChunkExact(2).Chunks.Index().Select(c => 
            {
                var ordered = Compare(c.Item[0], c.Item[1]);
                return ordered ? c.Index + 1 : 0;
            }).Sum();
        }

        public static int PartTwo()
        {
            var first = 1;
            var second = 2; 

            foreach(var packet in Input)
            {
                if(Compare(packet, "[[2]]"))
                {
                    first++;
                    second++;
                }
                else if(Compare(packet,"[[6]]")) second++;
            }
            return first * second;
        }
    }
}
