using aoc_fast.Extensions;

namespace aoc_fast.Years._2015
{
    class Day20
    {
        const int BLOCK = 100_000;

        public static string input
        {
            get;
            set;
        }

        private static (int target, int start) info = (0, 0);
        private static void Parse()
        {
            (int,int)[] robinsInequality = 
                [
                (100000, 4352000),
                (200000, 8912250),
                (300000, 13542990),
                (400000, 18218000),
                (500000, 22925240),
                (600000, 27657740),
                (700000, 32410980),
                (800000, 37181790),
                (900000, 41967820),
                (1000000, 46767260),
                (1100000, 51578680),
                (1200000, 56400920),
                (1300000, 61233020),
                (1400000, 66074170),
                (1500000, 70923680),
                (1600000, 75780960),
                (1700000, 80645490),
                (1800000, 85516820),
                (1900000, 90394550),
                (2000000, 95278320),
                ];
            var (target, start) = (int.Parse(input), 0);

            foreach(var (key, val) in robinsInequality)
            {
                if (target >= val) start = key;
                else break;
            }
            info = (target, start);
        }

        public static int PartOne()
        {
            Parse();
            var (target, start) = info;
            var end = start + BLOCK;
            var houses = Enumerable.Repeat(0, BLOCK).ToArray();

            while (true)
            {
                for (var i = 0; i < BLOCK; i++) houses[i] = 10 * (1 + start + i);
                for (var i = BLOCK; i < end / 2; i++)
                {
                    var presents = 10 * i;
                    var j = start.NextMultipleOf(i) - start;

                    if (j < BLOCK) houses[j] += presents;
                }

                for (var i = 2; i < BLOCK; i++)
                {
                    var presents = 10 * i;
                    var j = start.NextMultipleOf(i) - start;
                    while (j < BLOCK)
                    {
                        houses[j] += presents;
                        j += i;
                    }
                }
                var found = Array.FindIndex(houses, p => p >= target);
                if (found > -1) return start + found;

                start += BLOCK;
                end += BLOCK;
            }
        }

        public static int PartTwo()
        {
            var (target, start) = info;
            var end = start + BLOCK;
            var houses = Enumerable.Repeat(0, BLOCK).ToArray();

            while (true)
            {
                for (var i = 0; i < BLOCK; i++) houses[i] = 11 * (1 + start + i);
                for (var i = BLOCK; i < end / 2; i++)
                {
                    var presents = 11 * i;
                    var j = start.NextMultipleOf(i) - start;

                    if (j < BLOCK) houses[j] += presents;
                }

                for (var i = start / 50; i < BLOCK; i++)
                {
                    var presents = 11 * i;
                    var j = start.NextMultipleOf(i) - start;
                    var remaining = 51 - (int)Math.Ceiling((double)start / i);

                    while (j < BLOCK && remaining > 0)
                    {
                        houses[j] += presents;
                        j += i;
                        remaining--;
                    }
                }
                var found = Array.FindIndex(houses, p => p >= target);
                if (found > -1) return start + found;

                start += BLOCK;
                end += BLOCK;
            }
        }
    }
}
