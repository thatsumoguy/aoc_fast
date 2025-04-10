using System.Text;
using System.Text.RegularExpressions;

namespace aoc_fast.Extensions
{
    public static partial class StringExtensions
    {
        /// <summary>
        /// Extract numbers from a given string in the data type provided by T.
        /// </summary>
        /// <param name="input">String to parse numbers from.</param>
        /// <returns>List <typeparamref name="T"/> of all the numbers from the given string.</returns>
        public static List<T> ExtractNumbers<T>(this string input) where T : struct
        {
            Regex regex;

            if (IsUnsigned<T>())
            {
                regex = UnsignedRegex();
            }
            else
            {
                regex = SignedRegex();
            }

            var matches = regex.Matches(input);
            List<T> resultList = [];

            foreach (Match match in matches)
            {
                string numberString = match.Value;
                T number;

                try
                {
                    number = ParseNumber<T>(numberString);
                    resultList.Add(number);
                }
                catch (InvalidCastException)
                {
                    Console.WriteLine($"Could not convert '{numberString}' to {typeof(T)}.");
                }
            }

            return resultList;
        }

        /// <summary>
        /// Retains only the characters that satisfy the given predicate.
        /// Modifies the StringBuilder in place.
        /// </summary>
        /// <param name="sb">The StringBuilder to filter.</param>
        /// <param name="predicate">A function to test each character for a condition.</param>
        /// <returns>The modified StringBuilder (allows chaining).</returns>
        public static StringBuilder Retain(this StringBuilder sb, Func<char, bool> predicate)
        {
            for (int i = sb.Length - 1; i >= 0; i--)
            {
                if (!predicate(sb[i]))
                {
                    sb.Remove(i, 1);
                }
            }
            return sb;
        }

        public static string[] SplitWhere(this string input, Func<char, bool> predicate)
        {
            if(string.IsNullOrEmpty(input)) return [];
            ArgumentNullException.ThrowIfNull(predicate);

            var result = new List<string>();
            var startIndex = -1;
            var length = input.Length;

            for(var i = 0; i < length; i++)
            {
                var c = input[i];
                if (predicate(c))
                {
                    if(startIndex == -1) startIndex = i;
                }
                else if(startIndex != -1)
                {
                   result.Add(input[startIndex..i]);
                    startIndex = -1;
                }
            }
            if(startIndex != -1) result.Add(input[startIndex..]);
            return [.. result];
        }
        private static T ParseNumber<T>(string numberString) where T : struct
        {
            if (typeof(T) == typeof(UInt128))
            {
                return (T)(object)UInt128.Parse(numberString);
            }
            return (T)Convert.ChangeType(numberString, typeof(T)); 
        }

        private static bool IsUnsigned<T>() where T : struct
        {
            return typeof(T) == typeof(byte) || typeof(T) == typeof(ushort) || typeof(T) == typeof(uint) || typeof(T) == typeof(ulong);
        }

        [GeneratedRegex(@"\d+(\.\d+)?")]
        private static partial Regex UnsignedRegex();

        [GeneratedRegex(@"[-+]?\d+(\.\d+)?")] 
        private static partial Regex SignedRegex();
    }
}
