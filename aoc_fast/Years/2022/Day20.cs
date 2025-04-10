using aoc_fast.Extensions;

namespace aoc_fast.Years._2022
{
    internal class Day20
    {
        public static string input { get; set; }
        private static long[] nums = [];
        internal static readonly int[] sourceArray = [1000, 2000, 3000];

        private static long Decrypt(long[] input, long key, int rounds)
        {
            var size = input.Length -1;
            var indices = Enumerable.Range(0, input.Length).ToArray();
            var numbers = input.Select(n => (n * key).RemEuclid(size)).ToArray();

            var lookup = new List<(int, int)>();
            var mixed = new List<List<List<int>>>();
            var skip = new List<int>();

            foreach(var first in indices.Chunk(289))
            {
                var outer = new List<List<int>>();

                foreach(var second in first.Chunk(17))
                {
                    for(var i = 0; i < second.Length; i++) lookup.Add((mixed.Count, outer.Count));

                    var inner = new List<int>(100);
                    inner.AddRange(second);
                    outer.Add(inner);
                }
                mixed.Add(outer);
                skip.Add(first.Length);
            }

            for(var _ = 0; _ < rounds; _++)
            {

                for (var index = 0; index < input.Length; index++)
                {
                    var number = numbers[index];
                    var (first, second) = lookup[index];

                    var third = mixed[first][second].FindIndex(i =>  i == index);

                    var pos = third + skip[0..first].Sum() + mixed[first][0..second].Select(i => i.Count).Sum();
                    var next = (ulong)((pos + number) % size);

                    mixed[first][second].RemoveAt(third);
                    skip[first]--;

                    foreach(var (firstIndex, outer) in mixed.Index())
                    {
                        if(next > (ulong)skip[firstIndex]) next -= (ulong)skip[firstIndex];
                        else
                        {
                            foreach(var (secondIndex, inner) in outer.Index())
                            {
                                if(next > (ulong)inner.Count) next -= (ulong)inner.Count;
                                else
                                {
                                    inner.Insert((int)next, index);
                                    skip[firstIndex]++;
                                    lookup[index] = (firstIndex, secondIndex);
                                    goto mix;
                                }
                            }
                        }
                    }
                mix: continue;
                }
            }
            var subIndices = mixed.SelectMany(i => i).SelectMany(i => i).ToList();
            var zeroth = subIndices.FindIndex(i => input[i] == 0);
            
            return sourceArray.Select(offset => (zeroth + offset) % subIndices.Count).Select(index => input[subIndices[index]] * key).Sum();
        }

        public static long PartOne()
        {
            nums = [.. input.ExtractNumbers<long>()];
            return Decrypt(nums, 1, 1);
        }
        public static long PartTwo() => Decrypt(nums, 811589153, 10);
    }
}
