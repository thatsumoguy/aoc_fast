using System.Runtime.CompilerServices;

namespace aoc_fast.Extensions
{
    public static class DictionaryExtensions
    {
        /// <summary>
        /// Ensures that a key exists in the dictionary. If the key does not exist, inserts the default value
        /// and returns the value. Otherwise, returns the existing value.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TValue OrInsert<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key, TValue defaultValue)
        {
            if (!dictionary.TryGetValue(key, out var value))
            {
                dictionary[key] = defaultValue;
                return defaultValue;
            }
            return value;
        }

        /// <summary>
        /// Ensures that a key exists in the dictionary. If the key does not exist, inserts the result of the factory
        /// function and returns the value. Otherwise, returns the existing value.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TValue OrInsertWith<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key, Func<TValue> defaultValueFactory)
        {
            if (!dictionary.TryGetValue(key, out var value))
            {
                value = defaultValueFactory();
                dictionary[key] = value;
            }
            return value;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool PopFront<TKey, TValue>(this SortedDictionary<TKey, TValue> dictionary, out (TKey Key, TValue Value) result)
        {
            if (dictionary == null || dictionary.Count == 0)
            {
                result = default;
                return false;
            }

            var first = dictionary.First();
            dictionary.Remove(first.Key);
            result = (first.Key, first.Value);
            return true;
        }
    }
}
