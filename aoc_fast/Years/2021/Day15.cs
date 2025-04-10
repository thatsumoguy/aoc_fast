using System.Text;

namespace aoc_fast.Years._2021
{
    internal class Day15
    {
        public static string input { get; set; }
        private static (int size, byte[] bytes) Bytes = (0, []);

        private static int Dijkstra(int size, byte[] bytes)
        {
            var edge = size - 1;
            var end = size * size - 1;

            var todo = Enumerable.Range(0, 10)
            .Select(_ => new List<int>(1000))
            .ToArray();
            var cost = new ushort[size * size];
            for (var i = 0; i < cost.Length; i++) cost[i] = ushort.MaxValue;

            var risk = 0;

            todo[0].Add(0);
            cost[0] = 0;

            while (true)
            {
                var i = risk % 10;

                for (var j = 0; j < todo[i].Count; j++)
                {
                    var curr = todo[i][j];
                    if (curr == end) return risk;


                    var check = (int next) =>
                    {
                        var nextCost = (ushort)((ushort)risk + (ushort)bytes[next]);
                        if (nextCost < cost[next])
                        {
                            todo[(nextCost % 10)].Add(next);
                            cost[next] = nextCost;
                        }
                    };

                    var X = curr % size;
                    var y = curr / size;

                    if (X > 0) check(curr - 1);
                    if (X < edge) check(curr + 1);
                    if (y > 0) check(curr - size);
                    if(y < edge) check(curr + size);
                }
                todo[i].Clear();
                risk++;
            }
        }

        private static void Parse()
        {
            var raw = input.TrimEnd().Split("\n").Select(Encoding.UTF8.GetBytes).ToList();

            var size = raw.Count;

            var bytes = new List<byte>(size * size);

            raw.ForEach(b => bytes.AddRange(b));
            for (var i = 0; i < bytes.Count; i++) bytes[i] = (byte)(bytes[i] - (byte)'0');

            Bytes = (size, bytes.ToArray());
        }

        public static int PartOne()
        {
            Parse();
            return Dijkstra(Bytes.size, Bytes.bytes);
        }
        public static int PartTwo()
        {
            var (expandedSize, expandedBytes) = (Bytes.size * 5, new byte[25 * Bytes.size * Bytes.size]);

            foreach(var (i,b) in Bytes.bytes.Index())
            {
                var x1 = i % Bytes.size;
                var y1 = i / Bytes.size;
                var baseNum = (int)b;

                for(var x2 = 0; x2 < 5;  x2++)
                {
                    for(var y2 = 0;  y2 < 5; y2++)
                    {
                        var index = (5 * Bytes.size) * (y2 * Bytes.size + y1) + (x2 * Bytes.size + x1);
                        expandedBytes[index] = (byte)(1 + (baseNum - 1 + x2 + y2) % 9);
                    }
                }
            }
            return Dijkstra(expandedSize, expandedBytes);
        }
    }
}
