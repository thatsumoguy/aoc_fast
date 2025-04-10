using aoc_fast.Extensions;

namespace aoc_fast.Years._2017
{
    internal class Day12
    {
        public static string input
        {
            get;
            set;
        }
        private static List<int> Groups = [];
        private static int DFS(string[] lines, bool[] visited, int index)
        {
            var size = 1;

            foreach(var next in lines[index][6..].ExtractNumbers<int>())
            {
                if(!visited[next])
                {
                    visited[next] = true;
                    size += DFS(lines, visited, next);
                }
            }
            return size;
        }

        private static void Parse()
        {
            var lines = input.Split("\n", StringSplitOptions.RemoveEmptyEntries);
            var size = lines.Length;

            var visited = new bool[size];
            var groups = new List<int>();

            for(var start = 0; start < size; start++)
            {
                if (!visited[start])
                {
                    visited[start] = true;
                    var dfsSize = DFS(lines, visited, start);
                    groups.Add(dfsSize);
                }    
            }

            Groups = groups;
        }

        public static int PartOne()
        {
            Parse();
            return Groups[0];
        }
        public static int PartTwo() => Groups.Count;
    }
}
