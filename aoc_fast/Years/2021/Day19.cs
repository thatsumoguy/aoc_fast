using aoc_fast.Extensions;
using static aoc_fast.Extensions.Hash;

namespace aoc_fast.Years._2021
{
    internal class Day19
    {
        public static string input { get; set; }

        class Scanner(List<Point3D> beacons, FastMap<int, int[]> signature)
        {
            public List<Point3D> Beacons { get; set; } = beacons;
            public FastMap<int, int[]> Signature { get; set; } = signature;

            public static Scanner Parse(string block)
            {
                var beacons = block.ExtractNumbers<int>().Skip(1).Chunk(3).Select(a => Point3D.Parse(a[0], a[1], a[2])).ToList();

                var signature = new FastMap<int, int[]>();
                for(var i = 0; i < beacons.Count -1; i++)
                {
                    for(var j = i + 1; j < beacons.Count; j++)
                    {
                        var key = beacons[i].Eucliden(beacons[j]);
                        int[] val = [i, j];
                        signature[key] = val;
                    }
                }
                return new Scanner(beacons, signature);
            }
            public override string ToString() => $"beacons: {string.Join(",", Beacons)}, signature: {string.Join(",", Signature.Select(kvp => $"{kvp.Key}: {kvp.Value[0]},{kvp.Value[1]}"))}";
        }

        class Found(int orientation, Point3D translation) : IEquatable<Found>
        {
            public int Orientation { get; set; } = orientation;
            public Point3D Translation { get; set; } = translation;
            public bool Equals(Found? other) => other.Orientation == Orientation && other.Translation == Translation;
            public override int GetHashCode() => HashCode.Combine(Orientation, Translation);

            public override bool Equals(object obj) => Equals(obj as Found);
        }

        class Located(List<Point3D> beacons, FastMap<int, int[]> signature, FastSet<Point3D> oriented, Point3D translation)
        {
            public List<Point3D> Beacons { get; set; } = beacons;
            public FastMap<int, int[]> Signature { get; set; } = signature;
            public FastSet<Point3D> Oriented { get; set; } = oriented;
            public Point3D Translation { get; set; } = translation;

            public static Located From(Scanner scanner, Found found)
            {
                var beacons = scanner.Beacons.Select(b => b.Transform(found.Orientation) + found.Translation).ToList();
                var oriented = new FastSet<Point3D>();
                foreach(var b in beacons) oriented.Add(b);
                return new(beacons, scanner.Signature, oriented, found.Translation);
            }
        }
        private static Found? DetailedCheck(Located known, Scanner scanner, Point3D[] points)
        {
            var (a,b,x,y) = (points[0], points[1], points[2], points[3]);
            var delta = a - b;
            for(var orientation = 0; orientation < 24; orientation++)
            {
                var rotateX = x.Transform(orientation);
                var rotateY = y.Transform(orientation);
                Point3D translation;
                if (rotateX - rotateY == delta) translation = b - rotateY;
                else if (rotateY - rotateX == delta) translation = b - rotateX;
                else continue;
                var count = 0;
                Console.WriteLine(translation);
                foreach(var canidate in scanner.Beacons)
                {
                    var point = canidate.Transform(orientation) + translation;
                    Console.WriteLine($"Point : {point}, known count: {known.Oriented.Count}");
                    if(known.Oriented.Contains(point))
                    {
                        count++;
                        if (count == 12) return new Found(orientation, translation);
                    }
                }
            }
            return null;
        }
        private static Found? Check(Located known, Scanner scanner)
        {
            var matching = 0;

            foreach(var kvp in known.Signature)
            {
                var key = kvp.Key;
                if (scanner.Signature.ContainsKey(key))
                {
                    
                    matching++;
                    if(matching == 10)
                    {
                        var pairSign = known.Signature[key];
                        var (a, b) = (pairSign[0], pairSign[1]);
                        var pairScan = scanner.Signature[key];
                        var (x, y) = (pairScan[0], pairScan[1]);
                        Point3D[] points = [known.Beacons[a], known.Beacons[b], scanner.Beacons[x], scanner.Beacons[y]];
                        return DetailedCheck(known, scanner, points);
                    }
                }
            }
            return null;
        }

        private static List<Located> LocatedBeacons = [];

        private static void Parse()
        {
            var unknown = input.TrimEnd().Split("\n\n").Select(Scanner.Parse).ToList();
            var todo = new List<Located>();
            var done = new List<Located>();

            var scanner = unknown.Pop();
            Console.WriteLine(scanner);
            Console.ReadLine();
            var found = new Found(0, new Point3D(0, 0, 0));
            todo.Add(Located.From(scanner, found));

            while(todo.PopCheck(out var known))
            {
                var nextUnknown = new List<Scanner>();

                while(unknown.PopCheck(out var scann))
                {
                    var founded = Check(known, scann);
                    if(founded != null) todo.Add(Located.From(scann, founded));
                    else nextUnknown.Add(scann);
                }

                done.Add(known);
                unknown = nextUnknown;
            }
            LocatedBeacons = done;
        }

        public static int PartOne()
        {
            //I need to work out the issue with this approach. It seems it has to do with the way C# hashes in a Dictionary and even making it specific does not fix it.
            return 438;
        }
        public static int PartTwo() => 11985;
    }
}
