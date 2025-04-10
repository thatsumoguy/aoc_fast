namespace aoc_fast.Years._2020
{
    internal class Day21
    {
        public static string input { get; set; }
        class Ingredient(ulong food, ulong candidates)
        {
            public ulong Food { get; set; } = food;
            public ulong Candidates { get; set; } = candidates;
        }

        private static Dictionary<string, Ingredient> ingredients = [];
        private static Dictionary<string, ulong> allergens = [];

        private static void Parse()
        {
            var ingreds = new Dictionary<string, Ingredient>();
            var allergen = new Dictionary<string, ulong>();
            var allergensPerFood = new List<ulong>();

            foreach(var (i, line) in input.TrimEnd().Split("\n").Index())
            {
                var parts = line.Split(" (contains ");
                var (prefix, suffix) = (parts[0],  parts[1]);

                foreach(var ing in prefix.Split([' ','\n','\t']))
                {
                    var entry = ingreds.TryGetValue(ing, out var val) ? val : new Ingredient(0,0);
                    entry.Food |= 1uL << i;
                    ingreds[ing] = entry;
                }
                var mask = 0ul;
                foreach(var aller in suffix.Split([' ',',',')']).Where(s => !string.IsNullOrEmpty(s)))
                {
                    var size = (ulong)allergen.Count;
                    ulong entry;
                    if (allergen.TryGetValue(aller, out var val)) entry = val;
                    else
                    {
                        entry = size;
                        allergen[aller] = size;
                    }
                    mask |= 1ul << (int)entry;
                }
                allergensPerFood.Add(mask);
            }

            for(var i = 0; i < ingreds.Count; i++)
            {
                var ingredient = ingreds.ElementAt(i);
                var possible = 0ul;
                var impossible = 0ul;

                foreach(var (j, allerg) in allergensPerFood.Index())
                {
                    if ((ingredient.Value.Food & (1ul << j)) == 0) impossible |= allerg;
                    else possible |= allerg;
                }
                ingredient.Value.Candidates = possible & ~impossible;

                ingreds[ingredient.Key] = ingredient.Value;
            }

            (ingredients, allergens) = (ingreds, allergen);
        }

        public static uint PartOne()
        {
            Parse();
            return ingredients.Values.Where(i => i.Candidates == 0).Select(i => (uint)ulong.PopCount(i.Food)).Aggregate(0u, (acc, i) => acc + i);
        }
        public static string PartTwo()
        {
            var inverseAllergens = allergens.Select(kvp => (1ul << (int)kvp.Value, kvp.Key)).ToDictionary();
            var todo = ingredients.Where(kvp => kvp.Value.Candidates != 0).Select(kvp => (kvp.Key, kvp.Value.Candidates)).ToArray();
            var done = new SortedDictionary<string, string>();

            while(done.Count < todo.Length)
            {
                var mask = 0ul;

                foreach(var (name, candidates) in todo)
                {
                    if(ulong.PopCount(candidates) == 1)
                    {
                        var allergen = inverseAllergens[candidates];
                        done.Add(allergen, name);
                        mask |= candidates;
                    }
                }
                for (var i = 0; i < todo.Length; i++) todo[i].Candidates &= ~mask;
            }
            return string.Join(",", done.Values);
        }
    }
}
