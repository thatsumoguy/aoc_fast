using System.Numerics;
using System.Runtime.CompilerServices;

namespace aoc_fast.Extensions
{
    public static class Numerics
    {
        /// <summary>
        /// Calculates the sum of a sequence of numeric values.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Sum<T>(this IEnumerable<T> source) where T : struct
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            T sum = default;

            foreach (var item in source)
            {
                sum = Add(sum, item);
            }

            return sum;
        }

        /// <summary>
        /// Adds two values of the same numeric type.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static T Add<T>(T left, T right) where T : struct
        {
            return (T)(((dynamic)left) + ((dynamic)right));
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T NextMultipleOf<T>(this T value, T multiple) where T : INumber<T>
        {
            if (multiple == T.Zero)
            {
                throw new ArgumentException("Multiple cannot be zero.", nameof(multiple));
            }

            if (value % multiple == T.Zero)
            {
                return value;
            }

            return value + (multiple - (value % multiple));
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T ModPow<T>(this T a, T e, T m) where T : INumber<T>
        {
            var b = a;
            var c = T.One;

            while (e > T.Zero)
            {
                if (((dynamic)e & (dynamic)T.One) == (dynamic)T.One)
                    c = (c * b) % m;

                b = (b * b) % m;
                e = (dynamic)e >> 1;
            }

            return c;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T gcd<T>(T a, T b) where T : INumber<T>
        {

            if (b == T.Zero)
                return a;
            else
                return gcd(b, a % b);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T lcm<T>(this T a, T b) where T : INumber<T>
        {
            return a * (b / gcd(a, b));
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<ulong> Biterator(this ulong value)
        {
            ulong t = value;

            while (t != 0)
            {
                var tz = ulong.TrailingZeroCount(t);
                yield return tz;
                t ^= (1ul << (int)tz);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<int> Biterator(this uint value)
        {
            uint t = value;

            while (t != 0)
            {
                int tz = BitOperations.TrailingZeroCount(t); 
                yield return tz;
                t ^= (1u << tz); 
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T ModInv<T>(this T a, T m) where T : INumber<T>
        {
            var t = T.Zero;
            var newt = T.One;
            var r = m;
            var newr = a;

            while (newr != T.Zero)
            {
                var quotient = r / newr;
                (t, newt) = (newt, t - quotient * newt);
                (r, newr) = (newr, r - quotient * newr);
            }

            if (r > T.One) return default;
            if (t < T.Zero) t += m;
            return t;
        }

        //Stole from https://github.com/dotnet/runtime/blob/main/src/tests/JIT/HardwareIntrinsics/Arm/Shared/Helpers.cs#L627 to reverse bits. Will try to make it generic
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong ReverseElementBits(this ulong op1)
        {
            ulong val = (ulong)op1;
            ulong result = 0;
            const int bitsize = sizeof(ulong) * 8;
            const ulong cst_one = 1;

            for (int i = 0; i < bitsize; i++)
            {
                if ((val & (cst_one << i)) != 0)
                {
                    result |= (ulong)(cst_one << (bitsize - 1 - i));
                }
            }

            return (ulong)result;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint SaturatingAdd(this uint a, uint b)
        {
            if (a == uint.MaxValue || b == uint.MaxValue) return uint.MaxValue;
            return (uint.MaxValue - a < b) ? uint.MaxValue : a + b;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int RemEuclid(this int a, int b)
        {
            var res = a % b;
            return res < 0 ? res + ((b < 0) ? -b : b) : res;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long RemEuclid(this long a, long b)
        {
            var res = a % b;
            return res < 0 ? res + Math.Abs(b) : res;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint SaturatingSub(this uint a, uint b) => (a <= b) ? 0 : a - b;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int SaturatingSub(this byte a, byte b) => (a <= b) ? 0 : a - b;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsAsciiDigit(this byte b) => b >= '0' && b <= '9';
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsAsciiLetter(this byte b) => (b >= (byte)'A' && b <= (byte)'Z') || (b >= (byte)'a' && b <= (byte)'z');
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsAsciiUpperLetter(this byte b) => b >= 'A' && b <= 'Z';
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Abs<T>(this T item) where T : INumber<T> => T.Abs(item);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Max<T>(this T item, T other) where T : INumber<T> => T.Max(item, other);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Sign<T>(this T item) where T : INumber<T> => T.Sign((dynamic)item);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Min<T>(this T item, T other) where T : INumber<T> => T.Min(item, other);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T FoldDecimal<T>(this IEnumerable<T> item) where T : INumber<T> => item.Aggregate(T.Zero, (acc, b) => T.CreateChecked(10) * acc + b);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UInt128 Sqrt(UInt128 n)
        {
            if (n < 2) return n;

            UInt128 left = 1, right = n / 2 + 1, result = 1;

            while (left <= right)
            {
                var mid = (left + right) / 2;
                var midSquared = mid * mid;

                if (midSquared == n) return mid;
                if (midSquared < n)
                {
                    result = mid;
                    left = mid + 1;
                }
                else right = mid - 1;
            }
            return result;
        }
    }

}
