using aoc_fast.Extensions;

namespace aoc_fast.Years._2021
{
    internal class Day17
    {
        public static string input { get; set; }

        private static int[] nums = [];

        public static int PartOne()
        {
            nums = input.ExtractNumbers<int>().Chunk(4).ToList()[0];
            var bottom = nums[2];
            var n = -(bottom + 1);
            return n * (n + 1) / 2;
        }
        public static int PartTwo()
        {
            var (left, right, bottom, top) = (nums[0], nums[1], nums[2], nums[3]);

            var n = 1;
            while (n * (n + 1) / 2 < left) n++;

            var minDx = n;
            var maxDx = right + 1;
            var minDy = bottom;
            var maxDy = -bottom;

            var maxT = 1 - 2 * bottom;
            var newOne = new int[maxT];

            var continuing = new int[maxT];
            var total = 0;

            for(var dx = minDx; dx < maxDx; dx++)
            {
                var X = 0;
                var subDx = dx;
                var first = true;

                for(var t = 0; t < maxT; t++)
                {
                    if (X > right) break;
                    if(X >= left)
                    {
                        if (first)
                        {
                            first = false;
                            newOne[t]++;
                        }
                        else continuing[t]++;
                    }
                    X += subDx;
                    subDx = Math.Max(subDx - 1, 0);
                }
            }
            for(var dy = minDy; dy < maxDy; dy++)
            {
                var y = 0;
                var subDy = dy;
                var t = 0;
                var first = true;

                while(y >= bottom)
                {
                    if (y <= top)
                    {
                        if(first)
                        {
                            first = false;
                            total += continuing[t];
                        }
                        total += newOne[t];
                    }
                    y += subDy;
                    subDy--;
                    t++;
                }
            }
            return total;
        }
    }
}
