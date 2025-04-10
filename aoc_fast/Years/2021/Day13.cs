using System.Text;
using aoc_fast.Extensions;

namespace aoc_fast.Years._2021
{
    internal class Day13
    {
        public static string input { get; set; }

        record Fold()
        {
            public record Horizontal(int a) : Fold;
            public record Vertical(int a) : Fold;
        }

        private static (List<Point> points, List<Fold> folds) PointAndFolds = ([], []);

        private static Point FoldHorzontal(int x, Point p) => p.X < x ? p : new Point(2 * x - p.X, p.Y);
        private static Point FoldVertical(int y, Point p) => p.Y < y ? p : new Point(p.X, 2 * y - p.Y);

        private static void Parse()
        {
            var parts = input.TrimEnd().Split("\n\n");
            var (prefix, suffix) = (parts[0], parts[1]);

            var points = prefix.ExtractNumbers<int>().Chunk(2).Select(a => new Point(a[0], a[1])).ToList();

            var folds = suffix.Split("\n").Select(line =>
            {
                var suffixParts = line.Split('=');
                var (pre, post) = (suffixParts[0], suffixParts[1]);
                Fold fold = (pre, post) switch
                {
                    ("fold along x", var x) => new Fold.Horizontal(int.Parse(x)),
                    ("fold along y", var y) => new Fold.Vertical(int.Parse(y))
                };
                return fold;
            }).ToList();

            PointAndFolds = (points, folds);
        }

        public static int PartOne()
        {
            try { Parse(); } catch(Exception ex) { Console.WriteLine(ex); }
            return PointAndFolds.folds[0] switch
            {
                Fold.Horizontal(var x) => PointAndFolds.points.Select(p => FoldHorzontal(x, p)).ToHashSet().Count,
                Fold.Vertical(var y) => PointAndFolds.points.Select(p => FoldVertical(y, p)).ToHashSet().Count
            };
        }
        public static string PartTwo()
        {
            var width = 0;
            var height = 0;

            foreach (var fold in PointAndFolds.folds)
            {
                switch(fold)
                {
                    case Fold.Horizontal(var x):
                        width = x;
                        break;
                    case Fold.Vertical(var y):
                        height = y;
                        break;
                }
            }

            var grid = new bool[width * height];

            foreach(var point in PointAndFolds.points)
            {
                var p  = point;
                foreach (var fold in PointAndFolds.folds)
                {
                    p = fold switch
                    {
                        Fold.Horizontal(var x) => FoldHorzontal(x, p),
                        Fold.Vertical(var y) => FoldVertical(y, p),
                    };
                }

                grid[(p.Y * width + p.X)] = true;
            }

            var code = new StringBuilder();
            for(var y = 0; y < height; y++)
            {
                code.Append('\n');
                for(var x = 0; x < width; x++)
                {
                    var c = grid[(y * width + x)] ? '#' : '.';
                    code.Append(c);
                }
            }
            return code.ToString();
        }
    }
}
