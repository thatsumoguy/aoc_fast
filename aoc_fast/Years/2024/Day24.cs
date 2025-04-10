using System.Text;
using System.Text.RegularExpressions;

namespace aoc_fast.Years._2024
{
    internal partial class Day24
    {
        public static string input
        {
            get;
            set;
        }

        private static (string prefix, List<string[]> gates) prefixGates = ("", []);

        private static void Parse()
        {
            var split = input.Split("\n\n");
            var gates = MyRegex().Split(split[1]).Chunk(5).ToList();
            prefixGates = (split[0], gates);
        }

        public static ulong PartOne()
        {
            Parse();
            var (prefix, gates) = prefixGates;
            var todo = new Queue<string[]>(gates);
            var cache = Enumerable.Repeat(byte.MaxValue, 1 << 15).ToArray();

            var res = 0uL;

            var toIndex = (string s) =>
            {
                var b = Encoding.UTF8.GetBytes(s);
                return (((int)b[0] & 31) << 10) + (((int)b[1] & 31) << 5) + ((int)b[2] & 31);
            };

            foreach (var line in prefix.Split("\n", StringSplitOptions.RemoveEmptyEntries))
            {
                var pre = line[..3];
                var suffix = line[5..];
                cache[toIndex(pre)] = byte.Parse(suffix);
            }

            while (todo.TryDequeue(out var gate))
            {
                var left = cache[toIndex(gate[0])];
                var right = cache[toIndex(gate[2])];

                if (left == byte.MaxValue || right == byte.MaxValue) todo.Enqueue(gate);
                else cache[toIndex(gate[4])] = gate[1] switch
                {
                    "AND" => (byte)(left & right),
                    "OR" => (byte)(left | right),
                    "XOR" => (byte)(left ^ right)

                };
            }

            for (var i = toIndex("z64") - 1; i > toIndex("z00") - 1; i--)
            {
                if (cache[i] != byte.MaxValue) res = (res << 1) | ((ulong)cache[i]);
            }
            return res;
        }

        public static string PartTwo()
        {
            var (_, gates) = prefixGates;

            var lookup = new HashSet<(string, string)>();
            var swapped = new HashSet<string>();

            foreach (var g in gates)
            {
                lookup.Add((g[0], g[1]));
                lookup.Add((g[2], g[1]));
            }

            foreach (var g in gates)
            {
                if (g[1] == "AND")
                {
                    if (g[0] != "x00" && g[2] != "x00" && !lookup.Contains((g[4], "OR"))) swapped.Add(g[4]);
                }
                if (g[1] == "OR")
                {
                    if (g[4].StartsWith('z') && g[4] != "z45") swapped.Add(g[4]);
                    if (lookup.Contains((g[4], "OR"))) swapped.Add(g[4]);
                }
                if (g[1] == "XOR")
                {
                    if (g[0].StartsWith('x') || g[2].StartsWith('x'))
                    {
                        if (g[0] != "x00" && g[2] != "x00" && !lookup.Contains((g[4], "XOR"))) swapped.Add(g[4]);
                    }
                    else
                    {
                        if (!g[4].StartsWith('z')) swapped.Add(g[4]);
                    }
                }
            }

            var res = swapped.ToList();
            res.Sort();
            return string.Join(",", res);
        }

        [GeneratedRegex(@"[\x09\x0A\x0B\x0C\x0D\x20]+")]
        private static partial Regex MyRegex();
    }
}
