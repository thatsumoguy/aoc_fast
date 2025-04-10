using aoc_fast.Extensions;

namespace aoc_fast.Years._2015
{
    class Day21
    {
        public static string input
        {
            get;
            set;
        }

        class Item
        {
            public int Cost { get; set; }
            public int Damage { get; set; }
            public int Armor { get; set; }

            public Item() { }

            public static Item operator +(Item left, Item right) => new() { Cost = left.Cost + right.Cost, Damage = left.Damage + right.Damage, Armor = left.Armor + right.Armor };
        }
        private static List<(bool, int)> results = [];

        private static void Parse()
        {
            var (bossHealth, bossDamage, bossArmor) = input.ExtractNumbers<int>() switch { var a => (a[0], a[1], a[2]) };

            Item[] weapon = [
                    new Item {Cost = 8, Damage = 4, Armor = 0},
                    new Item { Cost = 10, Damage = 5, Armor = 0 },
                    new Item { Cost = 25, Damage = 6, Armor = 0 },
                    new Item { Cost = 40, Damage = 7, Armor = 0 },
                    new Item { Cost = 74, Damage = 8, Armor = 0 },
                ];
            Item[] armor = [
                    new Item { Cost = 0, Damage = 0, Armor = 0 },
                    new Item { Cost = 13, Damage = 0, Armor = 1 },
                    new Item { Cost = 31, Damage = 0, Armor = 2 },
                    new Item { Cost = 53, Damage = 0, Armor = 3 },
                    new Item { Cost = 75, Damage = 0, Armor = 4 },
                    new Item { Cost = 102, Damage = 0, Armor = 5 },
                ];
            Item[] rings = [
                    new Item { Cost = 25, Damage = 1, Armor = 0 },
                    new Item { Cost = 50, Damage = 2, Armor = 0 },
                    new Item { Cost = 100,Damage = 3, Armor = 0 },
                    new Item { Cost = 20, Damage = 0, Armor = 1 },
                    new Item { Cost = 40, Damage = 0, Armor = 2 },
                    new Item { Cost = 80, Damage = 0, Armor = 3 },
                ];

            var combinations = new List<Item>(22)
            {
                new() { Cost = 0, Damage = 0, Armor = 0 }
            };

            for(var i = 0; i < 6;i++)
            {
                combinations.Add(rings[i]);
                for(var j = i + 1; j < 6;j++) combinations.Add(rings[i] + rings[j]);
            }

            var res = new List<(bool, int)>(660);

            foreach(var first in weapon)
            {
                foreach(var second in armor)
                {
                    foreach(var third in combinations)
                    {
                        var selfItem = first + second + third;

                        var heroTurns = bossHealth / Math.Max(1, (selfItem.Damage - bossArmor));
                        var bossTurns = 100 / Math.Max(1, bossDamage - selfItem.Armor);
                        var win = heroTurns <= bossTurns;
                        res.Add((win, selfItem.Cost));
                    }
                }    
            }
            results = res;
        }

        public static int PartOne()
        {
            Parse();
            return results.Where(i => i.Item1).Min(i => i.Item2);
        }

        public static int PartTwo() => results.Where(i => !i.Item1).Max(i => i.Item2) - 10;
    }
}
