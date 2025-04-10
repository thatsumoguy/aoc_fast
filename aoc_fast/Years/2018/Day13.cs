using aoc_fast.Extensions;

namespace aoc_fast.Years._2018
{
    internal class Day13
    {
        public static string input
        {
            get;
            set;
        }

        [Serializable]
        class Cart
        {
            public Point Pos { get; set; }
            public Point Dir { get; set; }
            public uint Turns { get; set; }
            public bool Active { get; set; }

            public static Cart New(Point pos, Point dir) => new() { Pos = pos, Dir = dir, Turns = 0, Active = true };

            public void Tick(Grid<byte> grid)
            {
                Pos += Dir;

                switch (grid[Pos])
                {
                    case (byte)'\\':
                        Dir = Dir switch
                        {
                            Point when Dir == Directions.UP => Directions.LEFT,
                            Point when Dir == Directions.DOWN => Directions.RIGHT,
                            Point when Dir == Directions.LEFT => Directions.UP,
                            Point when Dir == Directions.RIGHT => Directions.DOWN,
                        };
                        break;
                    case (byte)'/':
                        Dir = Dir switch
                        {
                            Point when Dir == Directions.UP => Directions.RIGHT,
                            Point when Dir == Directions.DOWN => Directions.LEFT,
                            Point when Dir == Directions.LEFT => Directions.DOWN,
                            Point when Dir == Directions.RIGHT => Directions.UP,
                        };
                        break;
                    case (byte)'+':
                        Dir = Turns switch
                        {
                            0 => Dir.CounterClockwise(),
                            1 => Dir,
                            2 => Dir.Clockwise(),
                        };
                        Turns = (Turns + 1) % 3;
                        break;
                }
            }

            /// <summary>
            /// Deep clones the Cart object.
            /// </summary>
            public Cart DeepClone()
            {
                return new Cart
                {
                    Pos = Pos.Clone(),
                    Dir = Dir.Clone(),
                    Turns = this.Turns,
                    Active = this.Active
                };
            }
        }

        private static Grid<byte> grid = new();
        private static List<Cart> carts = [];

        private static void Parse()
        {
            grid = Grid<byte>.Parse(input);
            foreach(var (b,i) in grid.data.Select((b,i) => (b,i)))
            {
                switch (b)
                {
                    case (byte)'^':
                        var X = i % grid.width;
                        var y = i / grid.width;
                        carts.Add(Cart.New(new Point(X, y), Directions.UP));
                        break;
                    case (byte)'v':
                        X = i % grid.width;
                        y = i / grid.width;
                        carts.Add(Cart.New(new Point(X, y), Directions.DOWN));
                        break;
                    case (byte)'<':
                        X = i % grid.width;
                        y = i / grid.width;
                        carts.Add(Cart.New(new Point(X, y), Directions.LEFT));
                        break;
                    case (byte)'>':
                        X = i % grid.width;
                        y = i / grid.width;
                        carts.Add(Cart.New(new Point(X, y), Directions.RIGHT));
                        break;
                }
            }
        }

        public static string PartOne()
        {
            Parse();
            var p1Carts = new List<Cart>();
            var occupied = grid.DefaultCopy<byte,bool>();
            foreach(var c in carts) p1Carts.Add(c.DeepClone());
            while(true)
            {
                p1Carts.Sort((c,o) => (grid.width * c.Pos.Y + c.Pos.X).CompareTo(grid.width * o.Pos.Y + o.Pos.X));

                for(var i = 0; i < p1Carts.Count; i++)
                {
                    var cart = p1Carts[i];
                    occupied[cart.Pos] = false;
                    cart.Tick(grid);
                    var next = cart.Pos;

                    if (occupied[next]) return next.ToString();
                    occupied[next] = true;
                }
            }
        }
        public static string PartTwo()
        {
            var p2Carts = new List<Cart>();
            var occupied = grid.DefaultCopy<byte, bool>();
            foreach (var c in carts) p2Carts.Add(c.DeepClone());
            while(p2Carts.Count > 1)
            {
                p2Carts.Sort((c, o) => (grid.width * c.Pos.Y + c.Pos.X).CompareTo(grid.width * o.Pos.Y + o.Pos.X));
                
                for(var i = 0;i < p2Carts.Count;i++)
                {
                    if (p2Carts[i].Active)
                    {
                        occupied[p2Carts[i].Pos] = false;
                        p2Carts[i].Tick(grid);
                        var next = p2Carts[i].Pos;
                        if (occupied[next])
                        {
                            for (var j = 0; j < p2Carts.Count; j++)
                            {
                                if (p2Carts[j].Pos == next) p2Carts[j].Active = false;
                            }
                            occupied[next] = false;
                        }
                        else occupied[next] = true;
                    }
                }
                p2Carts = p2Carts.Where(c => c.Active).ToList();
            }

            return p2Carts[0].Pos.ToString();
        }
    }
}
