using System.Runtime.CompilerServices;
using aoc_fast.Extensions;

namespace aoc_fast.Years._2022
{
    internal class Day19
    {
        public static string input { get; set; }

        struct Mineral(uint ore, uint clay, uint obsidian, uint geode)
        {
            public uint Ore { get; set; } = ore;
            public uint Clay { get; set; } = clay;
            public uint Obidian { get; set; } = obsidian;
            public uint Geode { get; set; } = geode;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public bool LessThanEqual(Mineral other) => Ore <= other.Ore && Clay <= other.Clay && Obidian <= other.Obidian;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Mineral operator +(Mineral a, Mineral b) => new(a.Ore + b.Ore, a.Clay + b.Clay, a.Obidian + b.Obidian, a.Geode + b.Geode);
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Mineral operator -(Mineral a, Mineral b) => new(a.Ore - b.Ore, a.Clay - b.Clay, a.Obidian - b.Obidian, a.Geode - b.Geode);
        }

        struct Blueprint(uint id, uint maxOre, uint maxClay, uint maxObsidian, Mineral oreCost, Mineral clayCost, Mineral obsidianCost, Mineral geodeCost)
        {
            public uint ID { get; set; } = id;
            public uint MaxOre { get; set; } = maxOre;
            public uint MaxClay { get; set;} = maxClay;
            public uint MaxObsidian { get; set;} = maxObsidian;
            public Mineral OreCost { get; set; } = oreCost;
            public Mineral ClayCost { get; set; } = clayCost;
            public Mineral ObidianCost { get; set; } = obsidianCost;
            public Mineral GeodeCost { get; set; } = geodeCost;

            public static Blueprint? From(uint[] chunk)
            {
                  if(chunk is [var id, var ore1, var ore2, var ore3, var clay, var ore4, var obdisidan]) 
                    return new(id, ore1.Max(ore2).Max(ore3).Max(ore4), clay, obdisidan, new Mineral(ore1, 0, 0, 0), new Mineral(ore2, 0, 0, 0), new Mineral(ore3, clay, 0, 0), new Mineral(ore2, 0, obdisidan, 0));
                return null;
            }
        }

        private static readonly Mineral ZERO = new(0,0,0,0);
        private static readonly Mineral OREBOT = new(1,0,0,0);
        private static readonly Mineral CLAYBOT = new(0,1,0,0);
        private static readonly Mineral OBSIDIANBOT = new(0,0,1,0);
        private static readonly Mineral GEODEBOT = new(0,0,0,1);

        private static List<Blueprint> Blueprints = [];
        private static void Next(Blueprint blueprint, ref uint result, uint time, Mineral bots, Mineral resources, Mineral newBot, Mineral cost)
        {
            for (var jump = 1u; jump < time; jump++)
            {
                if (cost.LessThanEqual(resources))
                {
                    DFS(blueprint, ref result, time - jump, bots + newBot, resources + bots - cost);
                    break;
                }
                resources += bots;
            }
        }

        private static bool Heuristic(Blueprint blueprint, uint result, uint time, Mineral bots, Mineral resources)
        {
            for(var _ = 0; _ < time; _++)
            {
                resources.Ore = blueprint.MaxOre;

                if(blueprint.GeodeCost.LessThanEqual(resources))
                {
                    resources = resources + bots - blueprint.GeodeCost;
                    bots += GEODEBOT;
                }
                else if(blueprint.ObidianCost.LessThanEqual(resources))
                {
                    resources = resources + bots - blueprint.ObidianCost;
                    bots += OBSIDIANBOT;
                }
                else resources += bots;
                bots += CLAYBOT;
            }
            return resources.Geode > result;
        }
        private static void DFS(Blueprint blueprint, ref uint result, uint time, Mineral bots, Mineral resources)
        {
            result = Math.Max(result, resources.Geode + bots.Geode * time);

            if(Heuristic(blueprint, result, time, bots, resources))
            {
                if (bots.Obidian > 0 && time > 1) Next(blueprint, ref result, time, bots, resources, GEODEBOT, blueprint.GeodeCost);
                if (bots.Obidian < blueprint.MaxObsidian && bots.Clay > 0 && time > 3) Next(blueprint, ref result, time, bots, resources, OBSIDIANBOT, blueprint.ObidianCost);
                if (bots.Ore < blueprint.MaxOre && time > 3) Next(blueprint, ref result, time, bots, resources, OREBOT, blueprint.OreCost);
                if (bots.Clay < blueprint.MaxClay && time > 5) Next(blueprint, ref result, time, bots, resources, CLAYBOT, blueprint.ClayCost);
            }
        }
        private static uint Maximize(Blueprint  blueprint, uint time)
        {
            var result = 0u;
            DFS(blueprint, ref result, time, OREBOT, ZERO);
            return result;
        }
        private static void Parse() => Blueprints = input.ExtractNumbers<uint>().Chunk(7).Select(slice => Blueprint.From(slice).Value).ToList();

        public static uint PartOne()
        {
            Parse();
            return Blueprints.Select(blueprint => blueprint.ID * Maximize(blueprint, 24)).Sum();
        }
        public static uint PartTwo() => Blueprints.Take(3).Select(blueprint => Maximize(blueprint, 32)).Aggregate(1u, (acc, i) => acc * i);
    }
}
