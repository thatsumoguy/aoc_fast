using System.Runtime.CompilerServices;

namespace aoc_fast.Extensions
{
    internal class FastMath
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T CeilingDivide<T>(T numerator, T denominator) where T : IComparable, IConvertible
        {
            // Ensure the denominator is not zero
            if (Convert.ToDouble(denominator) == 0)
            {
                throw new DivideByZeroException("Denominator cannot be zero.");
            }
            // If both numbers are integer types (e.g., int, long, etc.)
            if ((typeof(T) == typeof(int) || typeof(T) == typeof(long) || typeof(T) == typeof(short) || typeof(T) == typeof(byte) ||
              typeof(T) == typeof(uint) || typeof(T) == typeof(ulong) || typeof(T) == typeof(ushort)))
            {
                long intNumerator = Convert.ToInt64(numerator);
                long intDenominator = Convert.ToInt64(denominator);

                // Perform the integer division and apply the ceiling behavior manually
                long result = (intNumerator + intDenominator - 1) / intDenominator;

                return (T)Convert.ChangeType(result, typeof(T));
            }
            // If both numbers are floating-point types (e.g., float, double, decimal)
            if (typeof(T) == typeof(float) || typeof(T) == typeof(double) || typeof(T) == typeof(decimal))
            {
                double num = Convert.ToDouble(numerator);
                double denom = Convert.ToDouble(denominator);

                // Perform the division and apply Math.Ceiling
                double result = Math.Ceiling(num / denom);

                // Convert back to the original type
                return (T)Convert.ChangeType(result, typeof(T));
            }

            // If the type is not supported, throw an exception
            throw new ArgumentException("Unsupported type for Ceiling Division operation.");
        }
        
        private static readonly int[] PowersOf10 =
        [1, 10, 100, 1000, 10000, 100000, 1000000, 10000000, 100000000, 1000000000];

        /// <summary>
        /// Computes the floor of the base-10 logarithm of a positive integer.
        /// </summary>
        /// <param name="value">The integer value (must be greater than 0).</param>
        /// <returns>The floor of the base-10 logarithm.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int ILog10(int value)
        {
            if (value <= 0)
                throw new ArgumentOutOfRangeException(nameof(value), "Value must be greater than 0.");

            int result = 0;

            if (value >= 1000000000) return 9;
            if (value >= 100000000) return 8;
            if (value >= 10000000) return 7;
            if (value >= 1000000) return 6;
            if (value >= 100000) return 5;
            if (value >= 10000) return 4;
            if (value >= 1000) return 3;
            if (value >= 100) return 2;
            if (value >= 10) return 1;

            return result; // value < 10, so log10(value) = 0
        }
        /// <summary>
        /// Computes the floor of the base-10 logarithm of a positive long integer.
        /// </summary>
        /// <param name="value">The long value (must be greater than 0).</param>
        /// <returns>The floor of the base-10 logarithm.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int ILog10(long value)
        {
            if (value <= 0)
                throw new ArgumentOutOfRangeException(nameof(value), "Value must be greater than 0.");

            if (value >= 1000000000000000000L) return 18;
            if (value >= 100000000000000000L) return 17;
            if (value >= 10000000000000000L) return 16;
            if (value >= 1000000000000000L) return 15;
            if (value >= 100000000000000L) return 14;
            if (value >= 10000000000000L) return 13;
            if (value >= 1000000000000L) return 12;
            if (value >= 100000000000L) return 11;
            if (value >= 10000000000L) return 10;
            if (value >= 1000000000L) return 9;
            if (value >= 100000000L) return 8;
            if (value >= 10000000L) return 7;
            if (value >= 1000000L) return 6;
            if (value >= 100000L) return 5;
            if (value >= 10000L) return 4;
            if (value >= 1000L) return 3;
            if (value >= 100L) return 2;
            if (value >= 10L) return 1;

            return 0;
        }

        /// <summary>
        /// Computes the result of raising a number to an integer power.
        /// </summary>
        /// <typeparam name="T">The type of the base and result (must be numeric).</typeparam>
        /// <param name="baseValue">The base value.</param>
        /// <param name="exponent">The exponent (must be non-negative).</param>
        /// <returns>The result of baseValue raised to the power of exponent.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Pow<T>(T baseValue, int exponent) where T : struct, IConvertible
        {
            if (exponent < 0)
                throw new ArgumentOutOfRangeException(nameof(exponent), "Exponent must be non-negative.");

            if (exponent == 0) return (T)Convert.ChangeType(1, typeof(T));
            if (exponent == 1) return baseValue;

            dynamic result = Convert.ChangeType(1, typeof(T));
            dynamic baseVal = baseValue;

            while (exponent > 0)
            {
                if ((exponent & 1) == 1) // If exponent is odd
                    result *= baseVal;

                baseVal *= baseVal;
                exponent >>= 1;
            }

            return (T)result;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T RemEuclid<T>(T value, T modulus) where T : struct, IConvertible
        {
            T result = (dynamic)value % modulus;
            return (dynamic)result < 0 ? (dynamic)result + modulus : result;
        }
    }
}
