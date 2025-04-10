using System.Text;
using aoc_fast.Extensions;

namespace aoc_fast.Years._2018
{
    internal class Day17
    {
        public static string input { get; set; }

        enum Kind
        {
            Sand,
            Moving,
            Stopped
        }
        record struct Scan(int width, int top, int bottom, List<Kind> kind, int moving, int stopped);

        private static Kind Flow(ref Scan scan, int index)
        {
            if (index >= scan.bottom) return Kind.Moving;
            else if (scan.kind[index] != Kind.Sand) return scan.kind[index];
            else if(Flow(ref scan, index + scan.width) == Kind.Moving)
            {
                scan.kind[index] = Kind.Moving;
                if (index >= scan.top) scan.moving++;
                return Kind.Moving;
            }
            else
            {
                var left = index;
                var right = index;

                while (scan.kind[left - 1] == Kind.Sand && Flow(ref scan, left + scan.width) == Kind.Stopped) left--;
                while (scan.kind[right + 1] == Kind.Sand && Flow(ref scan, right + scan.width) == Kind.Stopped) right++;

                if (scan.kind[left -1] == Kind.Stopped && scan.kind[right + 1] == Kind.Stopped)
                {
                    for(index = left; index < right + 1; index++) scan.kind[index] = Kind.Stopped;
                    if (index >= scan.top) scan.stopped += right + 1 - left;
                    return Kind.Stopped;
                }
                else
                {
                    for (index = left; index < right + 1; index++) scan.kind[index] = Kind.Moving;
                    if(index >= scan.top) scan.moving += right + 1 - left;
                    return Kind.Moving;
                }
            }
        }

        private static Scan answer;

        private static void Parse()
        {
            var first = input.Split("\n").Select(s => Encoding.ASCII.GetBytes(s)[0]).ToArray();
            var second = input.ExtractNumbers<int>().Chunk(3);
            var clay = first.Zip(second).ToList();

            var minX = int.MaxValue;
            var maxX = 0;
            var minY = int.MaxValue;
            var maxY = 0;

            foreach(var(dir, triple) in clay)
            {
                (int x1, int x2, int y1, int y2) = (0,0,0,0);
                if (dir == 'X') (x1, x2, y1, y2) = (triple[0], triple[0], triple[1], triple[2]);
                else (x1, x2, y1, y2) = (triple[1], triple[2], triple[0], triple[0]);
                minX = Math.Min(x1, minX);
                maxX = Math.Max(x2, maxX);
                minY = Math.Min(y1, minY);
                maxY = Math.Max(y2, maxY);
            }

            var width = maxX - minX + 3;
            var top = width * minY;
            var bottom = width * (maxY + 1);
            var kind = new Kind[bottom];
            Array.Fill(kind, Kind.Sand);

            foreach(var (dir, triple) in clay)
            {
                if(dir == 'X') for (var y = triple[1]; y < triple[2] + 1; y++) kind[(width * y) + (triple[0] - minX + 1)] = Kind.Stopped;
                else for (var X = triple[1]; X < triple[2] + 1; X++) kind[(width * triple[0]) + (X - minX + 1)] = Kind.Stopped;
            }

            var scan = new Scan { width = width, top = top, bottom = bottom, kind = [.. kind], moving = 0, stopped = 0 };
            Flow(ref scan, 500 - minX + 1);
            answer = scan;
        }
        public static int PartOne()
        {
            Parse();
            return answer.moving + answer.stopped;
        }
        public static int PartTwo() => answer.stopped;
    }
}
