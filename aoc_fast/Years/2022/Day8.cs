using System.Text;

namespace aoc_fast.Years._2022
{
    internal class Day8
    {
        public static string input { get; set; }
        private const ulong ONES = 0x0041041041041041;
        private const ulong MASK = 0x0fffffffffffffc0;

        private static (int width, List<sbyte> digits) Input = (default, []);

        private static void Parse()
        {
            var raw = input.Split("\n");
            var width = raw[0].Length;
            var digits = new List<sbyte>();

            foreach(var line in raw)
            {
                var iter = Encoding.UTF8.GetBytes(line).Select(b => (sbyte)(6 * (b - (byte)'0')));
                digits.AddRange(iter);
            }
            Input = (width, digits);
        }

        public static int PartOne()
        {
            Parse();
            var visible = new bool[Input.digits.Count];

            for(var i = 1; i < Input.width -1;  i++)
            {
                var leftMax = (sbyte)-1;
                var rightMax = (sbyte)-1;
                var topMax = (sbyte)-1;
                var bottomMax = (sbyte)-1;

                for(var j = 0; j < Input.width -1;  j++)
                {
                    var left = (i * Input.width) + j;
                    if (Input.digits[left] > leftMax)
                    {
                        visible[left] = true;
                        leftMax = Input.digits[left];
                    }

                    var right = (i * Input.width) + (Input.width - j - 1);
                    if (Input.digits[right] > rightMax)
                    {
                        visible[right] = true;
                        rightMax = Input.digits[right];
                    }

                    var top = (j * Input.width) + i;
                    if(Input.digits[top] > topMax)
                    {
                        visible[top] = true;
                        topMax = Input.digits[top];
                    }

                    var bottom = (Input.width - j - 1) * Input.width + i;
                    if( Input.digits[bottom] > bottomMax)
                    {
                        visible[bottom] = true;
                        bottomMax = Input.digits[bottom];
                    }
                }
            }
            return 4 + visible.Where(b => b).Count();
        }

        public static ulong PartTwo()
        {
            var (width, digits) = Input;
            var scenic = new ulong[digits.Count];
            for(var i = 0; i < scenic.Length; i++) scenic[i] = 1uL;

            for(var i = 1; i < width - 1; i++)
            {
                var leftMax = ONES;
                var rightMax = ONES;
                var topMax = ONES;
                var bottomMax = ONES;

                for(var j = 1; j < width - 1; j++)
                {
                    var left = (i * width) + j;
                    scenic[left] *= (leftMax >> digits[left]) & 0x3f;
                    leftMax = (leftMax & (MASK << digits[left])) + ONES;

                    var right = (i * width) + (width -j -1);
                    scenic[right] *= (rightMax >> digits[right]) & 0x3f;
                    rightMax = (rightMax & (MASK << digits[right])) + ONES;

                    var top = (j * width) + i;
                    scenic[top] *= (topMax >> digits[top]) & 0x3f;
                    topMax = (topMax & (MASK << digits[top])) + ONES;

                    var bottom = (width - j - 1) * width + i;
                    scenic[bottom] *= (bottomMax >> digits[bottom]) & 0x3f;
                    bottomMax = (bottomMax & (MASK << digits[bottom])) + ONES;
                }
            }
            return scenic.Max();
        }
    }
}
