using System.Text;
using aoc_fast.Extensions;

namespace aoc_fast.Years._2019
{
    internal class Day6
    {
        public static string input { get; set; }

        private static List<int> nums = [];
        private static void Parse()
        {
            var digit = (byte b) => b.IsAsciiDigit() ? (int)(b - (byte)'0') : (int)(10 + b - (byte)'A');
            var perfectHash = (string obj) =>
            {
                var bytes = Encoding.ASCII.GetBytes(obj);
                return digit(bytes[0]) + 36 * digit(bytes[1]) + 1296 * digit(bytes[2]);
            };

            var indices = new ushort[36 * 36 * 36];
            indices[perfectHash("COM")] = 1;
            indices[perfectHash("SAN")] = 2;
            indices[perfectHash("YOU")] = 3;
            ushort curr = 4;

            var lookup = (string s) =>
            {
                var hash = perfectHash(s);
                if(indices[hash] == 0)
                {
                    var prev = curr;
                    indices[hash] = curr;
                    curr++;
                    return (int)prev;
                }
                else return (int)indices[hash];
            };

            var lines = input.Trim().Split("\n");
            var parent = Enumerable.Repeat(0, lines.Length +2).ToList();
            foreach(var line in lines)
            {
                var left = lookup(line[..3]);
                var right = lookup(line[4..]);
                parent[right] = left;
            }
            nums = parent;
        }

        public static int PartOne()
        {
            Parse();
            static int? orbits(List<int> parent, int?[] cache, int index)
            {
                if (cache[index] != null) return cache[index];
                else
                {
                    var res = 1 + orbits(parent, cache, parent[index]);
                    cache[index] = res;
                    return res;
                }
            }
            var cache = new int?[nums.Count];
            Array.Fill(cache, null);
            cache[0] = 0;
            cache[1] = 0;
            return Enumerable.Range(0, nums.Count).Select(index => orbits(nums, cache, index)).Sum().Value;
        }

        public static int PartTwo()
        {
            var distance = new ushort[nums.Count];
            var index = 2;
            ushort count = 0;

            while(index != 1)
            {
                distance[index] = count;
                index = nums[index];
                count++;
            }
            index = 3;
            count = 0;

            while (distance[index] == 0)
            {
                index = nums[index];
                count++;
            }
            return distance[index] + count - 2;
        }
    }
}
