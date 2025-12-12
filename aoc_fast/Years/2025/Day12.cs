using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aoc_fast.Years._2025
{
    internal class Day12
    {
        public static string input
        {
            get;
            set;
        }

        public static int PartOne()
        {
            var parts = input.Split("\n\n", StringSplitOptions.RemoveEmptyEntries);
            var presents = parts[..6];
            var sizes = new Dictionary<int, int>();
            foreach (var present in presents)
            {
                var lines = present.Split("\n", StringSplitOptions.RemoveEmptyEntries);
                var name = int.Parse(lines[0].Replace(':', ' '));
                var size = 0;
                foreach (var row in lines[1..])
                {
                    foreach (var c in row)
                    {
                        if (c == '#') size++;
                    }
                }
                sizes[name] = size;
            }
            var ans = 0;
            foreach (var line in parts[6].Split("\n"))
            {
                var split = line.Split(": ");
                var coords = split[0];
                var size = coords.Split('x').Select(long.Parse).ToArray();
                var required = split[1].Split(' ').Select(long.Parse).ToArray();
                ans += size[0] * size[1] > required.Index().ToList().Sum(n => n.Item * sizes[n.Index]) * 1.3 ? 1 : 0;
            }
            return ans;
        }
    }
}
