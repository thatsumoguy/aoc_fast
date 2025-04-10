using aoc_fast.Extensions;

namespace aoc_fast.Years._2018
{
    internal class Day9
    {
        public static string input
        {
            get;
            set;
        }

        private static (int players, int lastScore) inputObj;

        private static void Parse() => inputObj = input.ExtractNumbers<int>() switch { var a => (a[0], a[1]) };

        private static ulong Game(int players, int last)
        {
            var blocks = last / 23;

            var needed = 2 + 16 * blocks;

            var circle = new int[needed + 37];

            var scores = new ulong[players];

            var pickup = 9;

            var head = 23;

            var tail = 0;

            var placed = 22;

            int[] start = [2, 20, 10, 21, 5, 22, 11, 1, 12, 6, 13, 3, 14, 7, 15, 0, 16, 8, 17, 4, 18, 19];

            Array.Copy(start, circle, 22);

            for(var _  = 0; _ < blocks; _++)
            {
                scores[head % players] += (ulong)(head + pickup);

                pickup = circle[tail + 18];

                if(placed <= needed)
                {
                    int[] slice = 
                    [
                        circle[tail],
                        head + 1,
                        circle[tail + 1],
                        head + 2,
                        circle[tail + 2],
                        head + 3,
                        circle[tail + 3],
                        head + 4,
                        circle[tail + 4],
                        head + 5,
                        circle[tail + 5],
                        head + 6,
                        circle[tail + 6],
                        head + 7,
                        circle[tail + 7],
                        head + 8,
                        circle[tail + 8],
                        head + 9,
                        circle[tail + 9],
                        head + 10,
                        circle[tail + 10],
                        head + 11,
                        circle[tail + 11],
                        head + 12,
                        circle[tail + 12],
                        head + 13,
                        circle[tail + 13],
                        head + 14,
                        circle[tail + 14],
                        head + 15,
                        circle[tail + 15],
                        head + 16,
                        circle[tail + 16],
                        head + 17,
                        circle[tail + 17],
                        head + 18,
                        // circle[tail + 18] 19th marble is picked up and removed.
                        head + 19,
                    ];
                    Array.Copy(slice, 0, circle, placed, 37);

                    slice = 
                    [
                        circle[tail + 19],
                        head + 20,
                        circle[tail + 20],
                        head + 21,
                        circle[tail + 21],
                        head + 22,
                    ];

                    Array.Copy(slice, 0, circle, tail + 16, 6);

                    placed += 37;
                }

                head += 23;
                tail += 16;
            }

            return scores.Max();
        }

        public static ulong PartOne()
        {
            Parse();
            return Game(inputObj.players, inputObj.lastScore);
        }

        public static ulong PartTwo() => Game(inputObj.players, inputObj.lastScore * 100);
    }
}
