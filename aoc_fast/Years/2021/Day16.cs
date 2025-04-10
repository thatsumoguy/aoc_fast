using System.Collections;
using System.Text;
using aoc_fast.Extensions;

namespace aoc_fast.Years._2021
{
    internal class Day16
    {
        public static string input { get; set; }

        class BitStream(ulong available, ulong bits, ulong read, IEnumerator iter)
        {
            public ulong Avialable { get; set; } = available;
            public ulong Bits { get; set; } = bits;
            public ulong Read { get; set; } = read;
            public IEnumerator Iter { get; set; } = iter;

            public static BitStream From(string s) => new(0,0,0, Encoding.UTF8.GetBytes(s).GetEnumerator());

            public ulong Next(ulong amount)
            {
                while(Avialable < amount)
                {
                    Avialable += 4;
                    Bits = (Bits << 4) | HexToBinary();
                }

                Avialable -= amount;
                Read += amount;

                var mask = (1ul << (int)amount) - 1;
                return (Bits >> (int)Avialable) & mask;
            }
            public ulong HexToBinary()
            {
                Iter.MoveNext();
                var hextDigit = (byte)Iter.Current;
                return hextDigit.IsAsciiDigit() ? (ulong)hextDigit - 48 : (ulong)hextDigit - 55;
            }
        }

        record Packet()
        {
            public record Literal(ulong Version, ulong TypeId, ulong Value) : Packet;
            public record Operator(ulong Version, ulong TypeId, List<Packet> Packets) : Packet;

            public static Packet From(BitStream bitStream)
            {
                var version = bitStream.Next(3);
                var typeId = bitStream.Next(3);

                if(typeId == 4)
                {
                    var todo = true;

                    var value = 0ul;
                    while(todo)
                    {
                        todo = bitStream.Next(1) == 1;
                        value = (value << 4) | bitStream.Next(4);
                    }
                    return new Packet.Literal(version, typeId, value);
                }
                else
                {
                    var packets = new List<Packet>();

                    if(bitStream.Next(1) == 0)
                    {
                        var target = bitStream.Next(15) + bitStream.Read;
                        while(bitStream.Read < target) packets.Add(From(bitStream));
                    }
                    else
                    {
                        var subPackets = bitStream.Next(11);
                        for(var _ = 0ul; _ < subPackets; _++) packets.Add(From(bitStream));
                    }
                    return new Packet.Operator(version, typeId, packets);
                }
            }
        }
        private static Packet packet;
        public static ulong PartOne()
        {
            var bitStream = BitStream.From(input);
            packet = Packet.From(bitStream);

            static ulong eval(Packet packet) => packet switch
            {
                Packet.Literal(var version, var _, var _) => version,
                Packet.Operator(var version, var _, var packets) => version + packets.Select(eval).Sum(),
                _ => throw new NotImplementedException()
            };
            return eval(packet);
        }

        public static ulong PartTwo()
        {
            var bitStream = BitStream.From(input);
            packet = Packet.From(bitStream);

            static ulong eval(Packet packet) 
            {
                switch(packet)
                {
                    case Packet.Literal(var _, var _, var value): return value;
                    case Packet.Operator(var _, var typeId, var packets):
                        {
                            var iter = packets.Select(eval).GetEnumerator();
                            return typeId switch
                            {
                                0 => IterSum(iter),
                                1 => IterProduct(iter),
                                2 => IterMin(iter),
                                3 => IterMax(iter),
                                5 => (iter.MoveNext() ? iter.Current : throw new NotImplementedException()) > (iter.MoveNext() ? iter.Current : throw new NotImplementedException()) ? 1ul : 0ul,
                                6 => (iter.MoveNext() ? iter.Current : throw new NotImplementedException()) < (iter.MoveNext() ? iter.Current : throw new NotImplementedException()) ? 1ul : 0ul,
                                7 => (iter.MoveNext() ? iter.Current : throw new NotImplementedException()) == (iter.MoveNext() ? iter.Current : throw new NotImplementedException()) ? 1ul : 0ul,
                                _ => throw new NotImplementedException()
                            };
                        }
                }
                return 0;
            }

            //iter stuff to get the values as if it were a list still
            static ulong IterSum(IEnumerator<ulong> iter)
            {
                ulong sum = 0;
                while (iter.MoveNext()) sum += iter.Current;
                return sum;
            }

            static ulong IterProduct(IEnumerator<ulong> iter)
            {
                ulong product = 1;
                while (iter.MoveNext()) product *= iter.Current;
                return product;
            }

            static ulong IterMin(IEnumerator<ulong> iter)
            {
                if (!iter.MoveNext()) throw new InvalidOperationException();
                ulong min = iter.Current;
                while (iter.MoveNext()) min = Math.Min(min, iter.Current);
                return min;
            }

            static ulong IterMax(IEnumerator<ulong> iter)
            {
                if (!iter.MoveNext()) throw new InvalidOperationException();
                ulong max = iter.Current;
                while (iter.MoveNext()) max = Math.Max(max, iter.Current);
                return max;
            }

            return eval(packet);
        }
    }
}
