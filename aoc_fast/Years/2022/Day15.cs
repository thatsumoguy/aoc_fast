using aoc_fast.Extensions;
using static aoc_fast.Extensions.Hash;

namespace aoc_fast.Years._2022
{
    internal class Day15
    {
        public static string input { get; set; }

        private static List<(Point sensor, Point beacon, int manhattan)> Input = [];
        private static void Parse()
        {
            var helper = (int[] a) =>
            {
                var sensor = new Point(a[0], a[1]);
                var beacon = new Point(a[2], a[3]);
                var manhattan = sensor.Manhattan(beacon);
                return (sensor, beacon, manhattan);
            };
            Input = input.ExtractNumbers<int>().Chunk(4).Select(helper).ToList();
        }

        public static int PartOne()
        {
            Parse();
            var row = 2000000;
            static IEnumerable<int>? buildRange((Point sensor, Point beacon, int manhattan) input, int row)
            {
                var (sensor, beacon, manhattan) = input;
                var extra = manhattan - Math.Abs(sensor.Y - row);
                if (extra >= 0) return Enumerable.Range((sensor.X - extra), (sensor.X + extra) - (sensor.X - extra));
                return null;
            }

            static int? buildBeacons ((Point sensor, Point beacon, int manhattan) input, int row)
            {
                var beacon = input.beacon;
                return beacon.Y == row ? beacon.X : null;
            }

            var ranges = Input.Select(i => buildRange(i, row)).Where(x => x != null).ToList();
            ranges = [.. ranges.OrderBy(r => r.First())];
            var total = 0;
            var max = int.MinValue;

            foreach(var range in ranges)
            {
                var start = range.First();
                var end = range.Last();
                if (start >  max)
                {
                    total += end - start + 1;
                    max = end;
                }
                else
                {
                    total += Math.Max(0, end - max);
                    max = Math.Max(end, max);
                }
            }

            var beacons = Input.Select(i => buildBeacons(i, row)).Where(x => x != null).ToHashSet();
            return total - beacons.Count + 1;
        }

        public static ulong PartTwo()
        {
            var top = new FastSet<int>();
            var left = new FastSet<int>();
            var bottom = new FastSet<int>();
            var right = new FastSet<int>();
            var size = 4000000;

            foreach (var (sensor, _, manhattan) in Input)
            {
                top.Add(sensor.X + sensor.Y - manhattan - 1);
                left.Add(sensor.X - sensor.Y - manhattan - 1);
                bottom.Add(sensor.X + sensor.Y + manhattan + 1);
                right.Add(sensor.X - sensor.Y + manhattan + 1);
            }

            var horizontal = top.Intersect(bottom);
            var vertical = left.Intersect(right);
            var range = Enumerable.Range(0, size + 1);

            foreach(var x in vertical)
            {
                foreach(var y in horizontal)
                {
                    var point = new Point((x + y) /2, (y - x) /2);

                    if (range.Contains(point.X) && range.Contains(point.Y) && Input.All(i => i.sensor.Manhattan(point) > i.manhattan)) return 4000000uL * (ulong)(point.X) + (ulong)(point.Y);
                }
            }
            throw new Exception();
        }
    }
}
