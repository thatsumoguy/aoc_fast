using System.Text;

namespace aoc_fast.Years._2020
{
    internal class Day23
    {
        public static string input { get; set; }

        private static void Play(uint[] cups, ulong current, int rounds)
        {
            for (var i = 0; i < rounds; i++)
            {
                var a = (ulong)cups[current];
                var b = (ulong)cups[a];
                var c = (ulong)cups[b];
                var dest = current > 1 ? (ulong)current - 1 : (ulong)cups.Length - 1;
                while (dest == a || dest == b || dest == c) dest = dest > 1 ? dest - 1 : (ulong)cups.Length - 1;

                cups[current] = cups[c];
                current = cups[c];

                cups[c] = cups[dest];
                cups[dest] = (uint)a;
            }
        }
        private static uint[] Cups = [];
        private static void Parse() => Cups = Encoding.UTF8.GetBytes(input.TrimEnd()).Select(b => (uint)(b - (byte)'0')).ToArray();

        public static uint PartOne()
        {
            Parse();
            var start = (ulong)Cups[0];
            var current = start;
            var cups = new uint[10];

            foreach(var next in Cups[1..])
            {
                cups[current] = next;
                current = (ulong)next;
            }
            cups[current] = (uint)start;
            Play(cups, start, 100);
            return Enumerable.Range(0, 8).Aggregate((0u, 1u), (a, _) => (10 * a.Item1 + cups[a.Item2], cups[a.Item2])).Item1; 
        }
        public static ulong PartTwo()
        {
            var start = (ulong)Cups[0];
            var current = start;
            var cups = Enumerable.Range(1, 1_000_001).Select(i => (uint)i).ToArray();

            foreach (var next in Cups[1..])
            {
                cups[current] = next;
                current = (ulong)next;
            }
            cups[current] = 10;
            cups[1_000_000] = (uint)start;

            Play(cups, start, 10_000_000);

            var first = (ulong)cups[1];
            var second = (ulong)cups[first];

            return first * second;
        }
    }
}
