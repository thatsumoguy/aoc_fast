using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace aoc_fast.Years._2021
{
    internal partial class Day22
    {
        public static string input { get; set; }

        public readonly struct Cube
        {
            public readonly int X1, X2, Y1, Y2, Z1, Z2;

             
            public Cube(int x1, int x2, int y1, int y2, int z1, int z2)
            {
                X1 = x1;
                X2 = x2;
                Y1 = y1;
                Y2 = y2;
                Z1 = z1;
                Z2 = z2;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Cube From(int[] points)
            {
                int x1 = Math.Min(points[0], points[1]);
                int x2 = Math.Max(points[0], points[1]);
                int y1 = Math.Min(points[2], points[3]);
                int y2 = Math.Max(points[2], points[3]);
                int z1 = Math.Min(points[4], points[5]);
                int z2 = Math.Max(points[4], points[5]);
                return new Cube(x1, x2, y1, y2, z1, z2);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public Cube? Intersect(in Cube other)
            {
                int x1 = Math.Max(X1, other.X1);
                int x2 = Math.Min(X2, other.X2);
                int y1 = Math.Max(Y1, other.Y1);
                int y2 = Math.Min(Y2, other.Y2);
                int z1 = Math.Max(Z1, other.Z1);
                int z2 = Math.Min(Z2, other.Z2);

                if (x1 <= x2 && y1 <= y2 && z1 <= z2)
                    return new Cube(x1, x2, y1, y2, z1, z2);

                return null;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public long Volume()
            {
                long w = (long)(X2 - X1 + 1);
                long h = (long)(Y2 - Y1 + 1);
                long d = (long)(Z2 - Z1 + 1);
                return w * h * d;
            }
        }

        [method: MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly struct RebootStep(bool on, Day22.Cube cube)
        {
            public readonly bool On = on;
            public readonly Cube Cube = cube;
        }

        private static readonly List<RebootStep> rebootSteps = new(512); 
        private static readonly Regex numberRegex = MyRegex();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static long SubSets(in Cube cube, long sign, List<Cube> candidates, int startIndex)
        {
            long total = 0;

            for (int i = startIndex; i < candidates.Count; i++)
            {
                var intersection = cube.Intersect(candidates[i]);
                if (intersection.HasValue)
                {
                    total += sign * intersection.Value.Volume() + SubSets(intersection.Value, -sign, candidates, i + 1);
                }
            }

            return total;
        }

        private static void Parse()
        {
            ReadOnlySpan<char> input = Day22.input.AsSpan();
            int startIdx = 0;
            bool isOn = false;
            List<int> points = new(6);

            for (int i = 0; i < input.Length; i++)
            {
                if (input[i] == '\n' || i == input.Length - 1)
                {
                    var line = input.Slice(startIdx, i - startIdx + (i == input.Length - 1 ? 1 : 0));
                    startIdx = i + 1;
                    if (line.StartsWith("on"))
                        isOn = true;
                    else if (line.StartsWith("off"))
                        isOn = false;
                    points.Clear();
                    var matches = numberRegex.Matches(line.ToString());
                    foreach (Match match in matches)
                    {
                        if (int.TryParse(match.Value, out int num))
                            points.Add(num);
                    }

                    if (points.Count == 6)
                    {
                        Cube cube = Cube.From([.. points]);
                        rebootSteps.Add(new RebootStep(isOn, cube));
                    }
                }
            }
        }

        public static long PartOne()
        {
            Parse();
            var region = new Cube(-50, 50, -50, 50, -50, 50);
            int count = rebootSteps.Count;
            List<RebootStep> filtered = new(count);

            for (int i = 0; i < count; i++)
            {
                var step = rebootSteps[i];
                var intersection = region.Intersect(step.Cube);
                if (intersection.HasValue)
                {
                    filtered.Add(new RebootStep(step.On, intersection.Value));
                }
            }
            return CalculateVolume(filtered);
        }

        public static long PartTwo() => CalculateVolume(rebootSteps);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static long CalculateVolume(List<RebootStep> steps)
        {
            long total = 0;
            var candidates = new List<Cube>(64); 

            int count = steps.Count;
            for (int i = 0; i < count; i++)
            {
                var step = steps[i];
                if (!step.On) continue;

                candidates.Clear();

                for (int j = i + 1; j < count; j++)
                {
                    var intersection = step.Cube.Intersect(steps[j].Cube);
                    if (intersection.HasValue)
                    {
                        candidates.Add(intersection.Value);
                    }
                }

                total += step.Cube.Volume() + SubSets(step.Cube, -1, candidates, 0);
            }

            return total;
        }

        [GeneratedRegex(@"-?\d+")]
        private static partial Regex MyRegex();
    }
}
