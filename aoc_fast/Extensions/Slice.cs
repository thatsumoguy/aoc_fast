using System.Numerics;
using System.Runtime.CompilerServices;

namespace aoc_fast.Extensions
{
    public static class Slice
    {
        /// <summary>
        /// Removes an element at the specified index, returning a new array.
        /// </summary>
        /// <typeparam name="T">The type of elements in the array.</typeparam>
        /// <param name="source">The array to operate on.</param>
        /// <param name="index">The index of the element to remove.</param>
        /// <returns>The array with the item removed.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T[] RemoveAt<T>(this T[] source, int index)
        {
            T[] dest = new T[source.Length - 1];
            if (index > 0)
                Array.Copy(source, 0, dest, 0, index);

            if (index < source.Length - 1)
                Array.Copy(source, index + 1, dest, index, source.Length - index - 1);

            return dest;
        }

        /// <summary>
        /// Removes an element at the specified index by swapping it with the last element.
        /// </summary>
        /// <typeparam name="T">The type of elements in the list.</typeparam>
        /// <param name="list">The list to operate on.</param>
        /// <param name="index">The index of the element to remove.</param>
        /// <returns>The removed element.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T SwapRemove<T>(this List<T> list, int index)
        {
            if (index < 0 || index >= list.Count) throw new ArgumentOutOfRangeException();
            int lastIndex = list.Count - 1;
            var output = list[index];
            if (index != lastIndex)
                (list[index], list[lastIndex]) = (list[lastIndex], list[index]);
            list.RemoveAt(lastIndex);
            return output;
        }
        /// <summary>
        /// Removes an element at the specified index by swapping it with the last element.
        /// </summary>
        /// <typeparam name="T?">The type of elements in the list.</typeparam>
        /// <param name="list">The list to operate on.</param>
        /// <param name="index">The index of the element to remove.</param>
        /// <returns>The removed element.</returns>
        public static T? SwapRemove<T>(this List<T?> list, int index) where T : struct
        {
            if (index < 0 || index >= list.Count) throw new ArgumentOutOfRangeException();
            int lastIndex = list.Count - 1;
            var output = list[index];
            if (index != lastIndex)
                (list[index], list[lastIndex]) = (list[lastIndex], list[index]);
            list[lastIndex] = null;
            return output;
        }

        /// <summary>
        /// Generates all possible permutations of a mutable array, passing them one at a time to a callback function.
        /// </summary>
        public static void Permutations<T>(this T[] array, Action<T[]> callback)
        {
            if (array == null || array.Length == 0) return;

            callback(array);
            int n = array.Length;
            var c = new int[n];
            int i = 1;

            while (i < n)
            {
                if (c[i] < i)
                {
                    if (i % 2 == 0)
                    {
                        array.Swap(0, i);
                    }
                    else
                    {
                        array.Swap(c[i], i);
                    }
                    callback(array);
                    c[i]++;
                    i = 1;
                }
                else
                {
                    c[i] = 0;
                    i++;
                }
            }
        }

        /// <summary>
        /// Swaps two elements in an array.
        /// </summary>
        private static void Swap<T>(this T[] array, int i, int j)
        {
            (array[i], array[j]) = (array[j], array[i]);
        }
        /// <summary>
        /// Swaps two elements in a list.
        /// </summary>
        private static void Swap<T>(this List<T> array, int i, int j)
        {
            (array[i], array[j]) = (array[j], array[i]);
        }



        /// <summary>
        /// Generates all possible permutations of a collection, passing them one at a time to a callback function.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Permutations<T>(this ICollection<T> collection, Action<List<T>> callback)
        {
            if (collection == null || collection.Count == 0) return;
            var list = new List<T>(collection);
            list.Permute(callback);
        }

        /// <summary>
        /// Internal method to generate permutations for lists using Heap's algorithm.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void Permute<T>(this List<T> list, Action<List<T>> callback)
        {
            callback([.. list]);
            int n = list.Count;
            var c = new int[n];
            int i = 1;

            while (i < n)
            {
                if (c[i] < i)
                {
                    list.Swap(i % 2 == 0 ? 0 : c[i], i);
                    callback(new List<T>(list)); 
                    c[i]++;
                    i = 1;
                }
                else
                {
                    c[i] = 0;
                    i++;
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<T[]> Windows<T>(this T[] ts, int size)
        {
            for(var i = 0; i < ts.Length - size + 1; i++)
            {
                var slice = new T[size];
                Array.Copy(ts, i, slice, 0, size);
                yield return slice;
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<T[]> Windows<T>(this IList<T> ts, int size)
        {
            if (size <= 0)
                throw new ArgumentOutOfRangeException(nameof(size), "Window size must be greater than zero.");

            if (ts == null || ts.Count < size)
                yield break;

            for (var i = 0; i <= ts.Count - size; i++)
            {
                var slice = new T[size];
                for (var j = 0; j < size; j++)
                {
                    slice[j] = ts[i + j];
                }
                yield return slice;
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void RotateRight<T>(this T[] arr, int d)
        {
            var n = arr.Length;
            d = n - d;
            d %= n;
            int i, j, k;
            T temp;
            var g_c_d = Numerics.gcd(d, n);

            for (i = 0; i < g_c_d; i++)
            {

                temp = arr[i];
                j = i;

                while (true)
                {
                    k = j + d;
                    if (k >= n)
                        k -= n;
                    if (k == i)
                        break;
                    arr[j] = arr[k];
                    j = k;
                }
                arr[j] = temp;
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void RotateLeft<T>(this T[] arr, int d)
        {
            var n = arr.Length;
            d %= n;
            int i, j, k;
            T temp;
            var g_c_d = Numerics.gcd(d, n);

            for (i = 0; i < g_c_d; i++)
            {
                temp = arr[i];
                j = i;

                while (true)
                {
                    k = j + d;
                    if (k >= n)
                        k -= n;
                    if (k == i)
                        break;
                    arr[j] = arr[k];
                    j = k;
                }
                arr[j] = temp;
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void RotateRight<T>(this List<T> arr, int d)
        {
            var n = arr.Count;
            d = n - d;
            d %= n;
            int i, j, k;
            T temp;
            var g_c_d = Numerics.gcd(d, n);

            for (i = 0; i < g_c_d; i++)
            {
                temp = arr[i];
                j = i;

                while (true)
                {
                    k = j + d;
                    if (k >= n)
                        k -= n;
                    if (k == i)
                        break;
                    arr[j] = arr[k];
                    j = k;
                }
                arr[j] = temp;
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void RotateLeft<T>(this List<T> arr, int d)
        {
            var n = arr.Count;
            d %= n;
            int i, j, k;
            T temp;
            var g_c_d = Numerics.gcd(d, n);

            for (i = 0; i < g_c_d; i++)
            {
                temp = arr[i];
                j = i;

                while (true)
                {
                    k = j + d;
                    if (k >= n)
                        k = k - n;
                    if (k == i)
                        break;
                    arr[j] = arr[k];
                    j = k;
                }
                arr[j] = temp;
            }
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ReverseList<T>(ref List<T> list, int start, int end)
        {
            while (start < end)
            {
                (list[end], list[start]) = (list[start], list[end]);
                start++;
                end--;
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IList<T> Clone<T>(this IList<T> listToClone) where T : ICloneable
        {
            return [.. listToClone.Select(item => (T)item.Clone())];
        }
        /// <summary>
        /// Pops the first item off the List
        /// </summary>
        /// <typeparam name="T">The type of elements in the list.</typeparam>
        /// <param name="list">The list to operate on.</param>
        /// <returns>The removed element.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T PopFront<T>(this ICollection<T> collection)
        {
            if (collection == null || collection.Count == 0) throw new InvalidOperationException("The collection is empty or null.");

            using var enumerator = collection.GetEnumerator();
            enumerator.MoveNext();
            var first = enumerator.Current;
            collection.Remove(first);
            return first;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryPopFront<T>(this ICollection<T> collection, out  T result)
        {
            if(collection == null) throw new InvalidOperationException("The collection is null.");
            if(collection.Count == 0)
            {
                result = default(T);
                return false;
            }
            using var enumerator = collection.GetEnumerator();
            enumerator.MoveNext(); 
            var first = enumerator.Current;
            collection.Remove(first);
            result = first;
            return true;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void PushFront<T>(this List<T> collection, T item)
        {
            if(collection == null) throw new InvalidOperationException("The collection is null.");
            collection.Add(item);
            (collection[0], collection[^1]) = (collection[^1], collection[0]);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<T> Concat<T>(this List<T> list, IEnumerable<T> second)
        {
            foreach (var item in list)
            {
                yield return item;
            }
            foreach (var item in second)
            {
                yield return item;
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ChunkResult<TSource> ChunkExact<TSource>(this IEnumerable<TSource> source, int size)
        {
            if (source == null)
            {
                throw new ArgumentNullException(source.ToString());
            }

            if (size < 1)
            {
                throw new ArgumentOutOfRangeException(size.ToString());
            }

            var chunks = new List<TSource[]>();
            var remainder = new List<TSource>();

            using IEnumerator<TSource> e = source.GetEnumerator();
            while (e.MoveNext())
            {
                var chunk = new List<TSource>(size)
                {
                    e.Current
                };

                for (int i = 1; i < size && e.MoveNext(); i++)
                {
                    chunk.Add(e.Current);
                }

                if (chunk.Count == size)
                {
                    chunks.Add([.. chunk]);
                }
                else
                {
                    remainder.AddRange(chunk);
                }
            }

            return new ChunkResult<TSource>(chunks, remainder.ToArray());
        }

        /// <summary>
        /// Pops the last item off the List
        /// </summary>
        /// <typeparam name="T">The type of elements in the list.</typeparam>
        /// <param name="list">The list to operate on.</param>
        /// <returns>The removed element.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]

        public static T Pop<T>(this IList<T> list)
        {
            if (list.Count == 0) throw new Exception("The list must be of count 1 or more");

            T last = list[^1];
            list.RemoveAt(list.Count - 1);
            return last;
        }
        /// <summary>
        /// Pops the last item off the List
        /// </summary>
        /// <typeparam name="T">The type of elements in the list.</typeparam>
        /// <param name="list">The list to operate on.</param>
        /// <returns>The removed element.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool PopCheck<T>(this IList<T> list, out T item)
        {
            if (list.Count == 0)
            {
                item = default;
                return false;
            }

            T last = list[^1];
            list.RemoveAt(list.Count - 1);
            item = last;
            return true;
        }

        /// <summary>
        /// Repeatedly invokes the given function to generate an infinite sequence of values.
        /// </summary>
        /// <typeparam name="T">The type of the elements.</typeparam>
        /// <param name="generator">A function to generate each value.</param>
        /// <returns>An enumerable that lazily generates values.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<T> RepeatWith<T>(Func<T> generator)
        {
            ArgumentNullException.ThrowIfNull(generator);

            while (true)
            {
                yield return generator();
            }
        }
        /// <summary>
        /// Repeatedly invokes the given function to generate a sequence of values of count length.
        /// </summary>
        /// <typeparam name="T">The type of the elements.</typeparam>
        /// <param name="generator">A function to generate each value.</param>
        /// <typeparam name="int"></typeparam>
        /// <param name="count">The length of the sequence</param>
        /// <returns>An enumerable that lazily generates values.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<T> RepeatWith<T>(Func<T> generator, int count)
        {
            ArgumentNullException.ThrowIfNull(generator);
            ArgumentOutOfRangeException.ThrowIfNegative(count);

            for (int i = 0; i < count; i++)
            {
                yield return generator();
            }
        }

        public static T FoldDecimal<T>(this List<T> list) where T : INumber<T>
        {
            T ten = T.CreateChecked(10);
            return list.Aggregate(T.Zero, (acc, b) => ten * acc + b);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T FoldDecimal<T>(this T[] list) where T : INumber<T>
        {
            T ten = T.CreateChecked(10);
            return list.Aggregate(T.Zero, (acc, b) => ten * acc + b);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<T> StepBy<T>(this IEnumerable<T> list, int steps)
        {
            for (var i = 0; i < list.Count(); i += steps) yield return list.ElementAt(i);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<T> StepBy<T>(this T[] list, int steps)
        {
            for (var i = 0; i < list.Length; i += steps) yield return list[i];
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static (List<T> Matches, List<T> NonMatches) Partition<T>(
        this IEnumerable<T> source, Func<T, bool> predicate)
        {
            var matches = new List<T>();
            var nonMatches = new List<T>();

            foreach (var item in source)
            {
                if (predicate(item))
                    matches.Add(item);
                else
                    nonMatches.Add(item);
            }

            return (matches, nonMatches);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static List<T[]> Split<T>(this T[] array, Func<T, bool> predicate)
        {
            var result = new List<T[]>();
            var tempList = new List<T>();

            foreach (var item in array)
            {
                if (predicate(item))
                {
                    if (tempList.Count > 0)
                    {
                        result.Add([.. tempList]);
                        tempList.Clear();
                    }
                }
                else
                {
                    tempList.Add(item);
                }
            }

            if (tempList.Count > 0) result.Add(tempList.ToArray());

            return result;
        }
        /// <summary>
        /// Replace the collection over and over again.
        /// </summary>
        /// <typeparam name="T">The type of the elements.</typeparam>
        /// <param name="source">Collection to be repeated.</param>
        /// <returns>Enumerable of the source repeated.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<T> Cycle<T>(this IEnumerable<T> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            while (true)
            {
                foreach (var item in source)
                {
                    yield return item;
                }
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<int> Scan(this IEnumerable<int> source, int seed)
        {
            int state = seed;
            foreach (var item in source)
            {
                int delta = item - state;
                state = item;
                yield return delta;
            }
        }
    }
    public class ChunkResult<TSource>(IEnumerable<TSource[]> chunks, TSource[] remainder)
    {
        public IEnumerable<TSource[]> Chunks { get; } = chunks;
        public TSource[] Remainder { get; } = remainder;
    }

}
