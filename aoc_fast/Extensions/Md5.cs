
using System.Buffers.Binary;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace aoc_fast.Extensions
{
    internal class Md5
    {
        [ThreadStatic]
        private static uint[]? _m;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe (uint, uint, uint, uint) Hash(Span<byte> buffer, int size)
        {
            var end = buffer.Length - 8;
            var bits = (ulong)size * 8;
            buffer[size] = 0x80;
            buffer[end] = (byte)(bits & 0xFF);
            buffer[end + 1] = (byte)((bits >> 8) & 0xFF);
            buffer[end + 2] = (byte)((bits >> 16) & 0xFF);
            buffer[end + 3] = (byte)((bits >> 24) & 0xFF);
            buffer[end + 4] = (byte)((bits >> 32) & 0xFF);
            buffer[end + 5] = (byte)((bits >> 40) & 0xFF);
            buffer[end + 6] = (byte)((bits >> 48) & 0xFF);
            buffer[end + 7] = (byte)((bits >> 56) & 0xFF);
            var a0 = 0x67452301u;
            var b0 = 0xefcdab89u;
            var c0 = 0x98badcfeu;
            var d0 = 0x10325476u;
            var m = _m ??= new uint[16];

            fixed (byte* bufferPtr = buffer)
            fixed (uint* mPtr = m)
            {
                byte* mBytePtr = (byte*)mPtr;

                for (int pos = 0; pos < buffer.Length; pos += 64)
                {
                    byte* blockPtr = bufferPtr + pos;

                    Buffer.MemoryCopy(blockPtr, mBytePtr, 64, 64);

                    var a = a0;
                    var b = b0;
                    var c = c0;
                    var d = d0;

                    // Round 1
                    a = Round1(a, b, c, d, m[0], 7, 0xd76aa478);
                    d = Round1(d, a, b, c, m[1], 12, 0xe8c7b756);
                    c = Round1(c, d, a, b, m[2], 17, 0x242070db);
                    b = Round1(b, c, d, a, m[3], 22, 0xc1bdceee);
                    a = Round1(a, b, c, d, m[4], 7, 0xf57c0faf);
                    d = Round1(d, a, b, c, m[5], 12, 0x4787c62a);
                    c = Round1(c, d, a, b, m[6], 17, 0xa8304613);
                    b = Round1(b, c, d, a, m[7], 22, 0xfd469501);
                    a = Round1(a, b, c, d, m[8], 7, 0x698098d8);
                    d = Round1(d, a, b, c, m[9], 12, 0x8b44f7af);
                    c = Round1(c, d, a, b, m[10], 17, 0xffff5bb1);
                    b = Round1(b, c, d, a, m[11], 22, 0x895cd7be);
                    a = Round1(a, b, c, d, m[12], 7, 0x6b901122);
                    d = Round1(d, a, b, c, m[13], 12, 0xfd987193);
                    c = Round1(c, d, a, b, m[14], 17, 0xa679438e);
                    b = Round1(b, c, d, a, m[15], 22, 0x49b40821);

                    // Round 2
                    a = Round2(a, b, c, d, m[1], 5, 0xf61e2562);
                    d = Round2(d, a, b, c, m[6], 9, 0xc040b340);
                    c = Round2(c, d, a, b, m[11], 14, 0x265e5a51);
                    b = Round2(b, c, d, a, m[0], 20, 0xe9b6c7aa);
                    a = Round2(a, b, c, d, m[5], 5, 0xd62f105d);
                    d = Round2(d, a, b, c, m[10], 9, 0x02441453);
                    c = Round2(c, d, a, b, m[15], 14, 0xd8a1e681);
                    b = Round2(b, c, d, a, m[4], 20, 0xe7d3fbc8);
                    a = Round2(a, b, c, d, m[9], 5, 0x21e1cde6);
                    d = Round2(d, a, b, c, m[14], 9, 0xc33707d6);
                    c = Round2(c, d, a, b, m[3], 14, 0xf4d50d87);
                    b = Round2(b, c, d, a, m[8], 20, 0x455a14ed);
                    a = Round2(a, b, c, d, m[13], 5, 0xa9e3e905);
                    d = Round2(d, a, b, c, m[2], 9, 0xfcefa3f8);
                    c = Round2(c, d, a, b, m[7], 14, 0x676f02d9);
                    b = Round2(b, c, d, a, m[12], 20, 0x8d2a4c8a);

                    // Round 3
                    a = Round3(a, b, c, d, m[5], 4, 0xfffa3942);
                    d = Round3(d, a, b, c, m[8], 11, 0x8771f681);
                    c = Round3(c, d, a, b, m[11], 16, 0x6d9d6122);
                    b = Round3(b, c, d, a, m[14], 23, 0xfde5380c);
                    a = Round3(a, b, c, d, m[1], 4, 0xa4beea44);
                    d = Round3(d, a, b, c, m[4], 11, 0x4bdecfa9);
                    c = Round3(c, d, a, b, m[7], 16, 0xf6bb4b60);
                    b = Round3(b, c, d, a, m[10], 23, 0xbebfbc70);
                    a = Round3(a, b, c, d, m[13], 4, 0x289b7ec6);
                    d = Round3(d, a, b, c, m[0], 11, 0xeaa127fa);
                    c = Round3(c, d, a, b, m[3], 16, 0xd4ef3085);
                    b = Round3(b, c, d, a, m[6], 23, 0x04881d05);
                    a = Round3(a, b, c, d, m[9], 4, 0xd9d4d039);
                    d = Round3(d, a, b, c, m[12], 11, 0xe6db99e5);
                    c = Round3(c, d, a, b, m[15], 16, 0x1fa27cf8);
                    b = Round3(b, c, d, a, m[2], 23, 0xc4ac5665);

                    // Round 4
                    a = Round4(a, b, c, d, m[0], 6, 0xf4292244);
                    d = Round4(d, a, b, c, m[7], 10, 0x432aff97);
                    c = Round4(c, d, a, b, m[14], 15, 0xab9423a7);
                    b = Round4(b, c, d, a, m[5], 21, 0xfc93a039);
                    a = Round4(a, b, c, d, m[12], 6, 0x655b59c3);
                    d = Round4(d, a, b, c, m[3], 10, 0x8f0ccc92);
                    c = Round4(c, d, a, b, m[10], 15, 0xffeff47d);
                    b = Round4(b, c, d, a, m[1], 21, 0x85845dd1);
                    a = Round4(a, b, c, d, m[8], 6, 0x6fa87e4f);
                    d = Round4(d, a, b, c, m[15], 10, 0xfe2ce6e0);
                    c = Round4(c, d, a, b, m[6], 15, 0xa3014314);
                    b = Round4(b, c, d, a, m[13], 21, 0x4e0811a1);
                    a = Round4(a, b, c, d, m[4], 6, 0xf7537e82);
                    d = Round4(d, a, b, c, m[11], 10, 0xbd3af235);
                    c = Round4(c, d, a, b, m[2], 15, 0x2ad7d2bb);
                    b = Round4(b, c, d, a, m[9], 21, 0xeb86d391);

                    a0 += a;
                    b0 += b;
                    c0 += c;
                    d0 += d;
                }
            }



            return (BinaryPrimitives.ReverseEndianness(a0),
                    BinaryPrimitives.ReverseEndianness(b0),
                    BinaryPrimitives.ReverseEndianness(c0),
                    BinaryPrimitives.ReverseEndianness(d0));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static uint Round1(uint a, uint b, uint c, uint d, uint m, int s, uint k) => Common((b & c) | (~b & d), a, b, m, s, k);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static uint Round2(uint a, uint b, uint c, uint d, uint m, int s, uint k) => Common((b & d) | (c & ~d), a, b, m, s, k);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static uint Round3(uint a, uint b, uint c, uint d, uint m, int s, uint k) => Common(b ^ c ^ d, a, b, m, s, k);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static uint Round4(uint a, uint b, uint c, uint d, uint m, int s, uint k) => Common(c ^ (b | ~d), a, b, m, s, k);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static uint Common(uint f, uint a, uint b, uint m, int s, uint k) => b + BitOperations.RotateLeft(m + a + k + f, s);

    }
}
