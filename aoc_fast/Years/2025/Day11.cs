using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static aoc_fast.Extensions.Hash;

namespace aoc_fast.Years._2025
{
    internal class Day11
    {
        public static string input
        {
            get;
            set;
        }

        private static FastMap<string, List<string>> Graph = [];

        private static void Parse()
        {
            var graph = new FastMap<string, List<string>>();

            foreach(var line in input.Split("\n", StringSplitOptions.RemoveEmptyEntries))
            {
                var tokens = line.Split(' ').ToList();
                var key = tokens[0];
                key = key[0..(key.Length - 1)];
                tokens.RemoveAt(0);
                graph[key] = tokens;
            }
            Graph = graph;
        }

        private static long DFS(FastMap<string, List<string>> graph, FastMap<string, long> cache, string node, string end)
        {
            if (node == end) return 1;
            else if (node == "out") return 0;
            else if (cache.TryGetValue(node, out var previous)) return previous;
            else
            {
                var result = graph[node].Select(next => DFS(graph, cache, next, end)).Sum();
                cache[node] = result;
                return result;
            }
        }

        public static long PartOne()
        {
            Parse();
            return DFS(Graph, [], "you", "out");
        }
        public static long PartTwo() => (DFS(Graph, [], "svr", "fft") * DFS(Graph, [], "fft", "dac") 
            * DFS(Graph, [], "dac", "out")) + (DFS(Graph, [], "svr", "dac") * DFS(Graph, [], "dac", "fft") * DFS(Graph, [], "fft", "out"));
        
    }
}
