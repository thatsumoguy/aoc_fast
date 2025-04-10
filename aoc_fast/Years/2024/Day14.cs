using aoc_fast.Extensions;

namespace aoc_fast.Years._2024
{
    internal class Day14
    {
        public static string input
        {
            get;
            set;
        }

        private const int ModX = 101;
        private const int ModY = 103;

        private static List<int[]> Robots = [];

        private static void Parse()
        {
            var nums = input.ExtractNumbers<int>();
            for (var i = 0; i < nums.Count; i += 4)
                Robots.Add([nums[i], nums[i + 1], FastMath.RemEuclid(nums[i + 2], ModX), FastMath.RemEuclid(nums[i + 3], ModY)]);
        }

        public static int PartOne()
        {
            Parse();
            var quadrants = new int[4];
            foreach (var robot in Robots)
            {
                var x = (robot[0] + 100 * robot[2]) % ModX;
                var y = (robot[1] + 100 * robot[3]) % ModY;

                var xComparison = x.CompareTo(50);
                var yComparison = y.CompareTo(51);

                if (xComparison < 0 && yComparison < 0)
                    quadrants[0]++;
                else if (xComparison < 0 && yComparison > 0)
                    quadrants[1]++;
                else if (xComparison > 0 && yComparison < 0)
                    quadrants[2]++;
                else if (xComparison > 0 && yComparison > 0)
                    quadrants[3]++;
            }
            return quadrants.Aggregate(1, (prod, count) => prod * count);
        }
        public static int PartTwo()
        {
            var rows = new List<int>();
            var columns = new List<int>();

            for (var time = 0; time < ModY; time++)
            {
                var xs = new int[ModX];
                var ys = new int[ModY];

                foreach (var robot in Robots)
                {
                    var x = (robot[0] + time * robot[2]) % ModX;
                    xs[x]++;
                    var y = (robot[1] + time * robot[3]) % ModY;
                    ys[y]++;
                }

                if (time < ModX && xs.Count(c => c >= 33) >= 2) columns.Add(time);
                if (ys.Count(c => c >= 31) >= 2) rows.Add(time);
            }

            if (rows.Count == 1 && columns.Count == 1)
            {
                var t = columns[0];
                var u = rows[0];
                return (5253 * t + 5151 * u) % (ModX * ModY);
            }

            var floor = new int[ModX * ModY];
            Array.Fill(floor, -1);

            foreach (var t in columns)
            {
                foreach (var u in rows)
                {
                    var time = (5253 * t + 5151 * u) % (ModX * ModY);
                    var unique = true;

                    foreach (var robot in Robots)
                    {
                        var x = (robot[0] + time * robot[2]) % ModX;
                        var y = (robot[1] + time * robot[3]) % ModY;
                        var index = ModX * y + x;

                        if (floor[index] == time)
                        {
                            unique = false;
                            break;
                        }

                        floor[index] = time;
                    }

                    if (unique) return time;
                }
            }

            throw new InvalidOperationException("No valid time found.");
        }
    }
}
