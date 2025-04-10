using aoc_fast.Extensions;

namespace aoc_fast.Years._2017
{
    internal class Day20
    {
        public static string input
        {
            get;
            set;
        }

        class Vector : IEquatable<Vector>
        {
            public int X { get; set; }
            public int Y { get; set; }
            public int Z { get; set; }

            public static Vector New(int[] cs) => new() { X = cs[0], Y = cs[1], Z = cs[2] };

            public bool Equals(Vector? other)
            {
                return X == other.X && Y == other.Y && Z == other.Z;
            }

            public int Manhattan() => Math.Abs(X) + Math.Abs(Y) + Math.Abs(Z);

            public void Tick(Vector other)
            {
                X += other.X;
                Y += other.Y;
                Z += other.Z;
            }

            public override bool Equals(object obj)
            {
                return Equals(obj as Vector);
            }

            public override int GetHashCode()
            {
                return HashCode.Combine(X, Y, Z);
            }
        }

        class Particle : IEquatable<Particle>
        {
            public int Id { get; set; }
            public Vector position { get; set; }
            public Vector velocity { get; set; }
            public Vector acceleration { get; set; }

            public bool Equals(Particle? other)
            {
                return Id == other.Id && position == other.position && velocity == other.velocity && acceleration == other.acceleration;
            }

            public override bool Equals(object obj)
            {
                return Equals(obj as Particle);
            }

            public override int GetHashCode()
            {
                return HashCode.Combine(Id, position, velocity, acceleration);
            }

            public void Tick()
            {
                velocity.Tick(acceleration);
                position.Tick(velocity);
            }
        }

        private static List<Particle> particles = [];

        private static void Parse()
        {
            particles = input.ExtractNumbers<int>().Chunk(3).Chunk(3)
                .Select((cs, i) => (cs, i))
                .Select(cs => new Particle 
                { 
                    Id = cs.i, 
                    position = Vector.New(cs.cs[0]), 
                    velocity = Vector.New(cs.cs[1]), 
                    acceleration = Vector.New(cs.cs[2]) 
                }).ToList();
        }

        public static int PartOne()
        {
            Parse();
            var candidate = new List<Particle>();
            var min = int.MaxValue;

            foreach(var particle in particles)
            {
                var next = particle.acceleration.Manhattan();

                if(next < min)
                {
                    candidate.Clear();
                    min = next;
                }
                if(next == min) candidate.Add(particle);
            }

            for(var _ = 0; _ < 1000; _++)  candidate.ForEach(p => p.Tick());

            candidate.Sort((a,b) => 
            {
                var first = a.velocity.Manhattan().CompareTo(b.velocity.Manhattan());
                var second = a.position.Manhattan().CompareTo(b.position.Manhattan());
                return first == second ? second : first;
            });

            return candidate[0].Id;
        }

        public static int PartTwo()
        {
            var subParticles = particles.ToList();
            var collisions = new Dictionary<Vector, int>(particles.Count);
            var exists = Enumerable.Repeat(long.MaxValue, subParticles.Count).ToList();

            for(var time = 1; time < 40; time++)
            {
                foreach(var (particle, i) in subParticles.Select((p,i)=>(p,i)))
                {
                    if (exists[i] >= time)
                    {
                        particle.Tick();

                        if(collisions.TryGetValue(particle.position, out var j))
                        {
                            exists[i] = time;
                            exists[j] = time;
                        }
                        collisions[particle.position] = i;
                    }
                }
                collisions.Clear();
            }
            return exists.Where(t => t == long.MaxValue).Count() - 1;
        }
    }
}
