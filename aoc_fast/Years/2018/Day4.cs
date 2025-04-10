using System.Text;
using aoc_fast.Extensions;

namespace aoc_fast.Years._2018
{
    internal class Day4
    {
        public static string input
        {
            get;
            set;
        }

        private static int ToIndex(byte[] slice) => slice.Aggregate(0, (acc, n) => 10 * acc + unchecked(n - (byte)'0'));

        private static Dictionary<int, uint[]> Input = [];

        private static void Parse()
        {
            var records = input.Split("\n", StringSplitOptions.RemoveEmptyEntries).Select(Encoding.ASCII.GetBytes).ToArray();
            Array.Sort(records, (x, y) =>
            {
                if (x == null) return y == null ? 0 : -1;
                if (y == null) return 1;

                int minLength = Math.Min(x.Length, y.Length);
                for (int i = 0; i < minLength; i++)
                {
                    int comparison = x[i].CompareTo(y[i]);
                    if (comparison != 0) return comparison;
                }

                return x.Length.CompareTo(y.Length);
            });

            var id = 0;
            var start = 0;
            var guards = new Dictionary<int, uint[]>();

            foreach (var record in records)
            {
                switch (record.Length)
                {
                    case 31: start = ToIndex(record[15..17]); break;
                    case 27:
                        var end = ToIndex(record[15..17]);
                        var minutes = new uint[60];
                        if (!guards.ContainsKey(id)) guards[id] = new uint[60];
                        minutes = guards[id];
                        foreach (var item in Enumerable.Range(start, end - start))
                        {
                            minutes[item]++;
                        }
                        guards[id] = minutes;
                        break;
                    default: id = ToIndex(record[26..(record.Length - 13)]); break;
                }
            }

            Input = guards;
        }

        private static int Choose(Dictionary<int, uint[]> input, Func<(int id, uint[] minutes), uint> strat)
        {
            (int id, uint[] minutes) = input.Select(x => (x.Key, x.Value)).MaxBy(strat);
            var minute = minutes
                 .Select((m, index) => (Minute: index, Count: m))
                 .MaxBy(x => x.Count)
                 .Minute;
            return id * minute;
        }

        public static int PartOne()
        {
            Parse();
            return Choose(Input, x => x.minutes.Sum());
        }
        public static int PartTwo() => Choose(Input, x => x.minutes.Max());
    }
}
