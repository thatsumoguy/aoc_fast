using aoc_fast.Extensions;

namespace aoc_fast.Years._2024
{
    internal class Day2
    {
        public static string input
        {
            get;
            set;
        }

        private static int[][] reports = [];
        private static bool Safe(int[] report)
        {
            var rising = false;
            var falling = false;
            for (var i = 1; i < report.Length; i++)
            {
                var first = report[i - 1];
                var second = report[i];
                var dif = Math.Abs(first - second);
                //return early pattern 
                if (dif < 1 || dif > 3) return false;
                if (first > second)
                {
                    if (rising) continue;
                    else if (falling) return false;
                    else rising = true;
                }
                else
                {
                    if (falling) continue;
                    else if (rising) return false;
                    else falling = true;
                }
            }
            return true;
        }
        public static int PartOne()
        {
            reports = input.Split("\n").Select(x => x.Split(" ").Select(int.Parse).ToArray()).ToArray();
            var ans = 0;
            foreach (var report in reports)
            {
                if (Safe(report)) ans++;
            }
            return ans;
        }
        public static int PartTwo()
        {
            var ans = 0;
            foreach (var report in reports)
            {
                if (Safe(report)) ans++;
                else
                {
                    for (var i = 0; i < report.Length; i++)
                    {
                        //This is where I got hung up originally
                        //I read the problem as if we hit a failure remove those pairs
                        //Or rather if we hit a failure I would have it just skip those pairs and move
                        //to the next pair
                        //The problem actually was to remove items one at a time and see
                        //If it then passes. Turns out the fastest option is an array and my extension to remove in place
                        var temp = report.ToArray();
                        temp = temp.RemoveAt(i);
                        if (Safe(temp))
                        {
                            ans++;
                            //Forgot the break first go around also.
                            break;
                        }
                    }
                }
            }
            return ans;
        }
    }
}
