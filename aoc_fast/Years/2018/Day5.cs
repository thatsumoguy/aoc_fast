using System.Text;

namespace aoc_fast.Years._2018
{
    internal class Day5
    {
        public static string input
        {
            get;
            set;
        }
        private static List<byte> bytes = [];

        private static List<byte> Collapse(IEnumerable<byte> polymer)
        {
            var head = (byte)0;
            var stack = new List<byte>();

            foreach(var unit in polymer)
            {
                if((head ^ unit) == 32)
                {
                    if (stack.Count > 0)
                    {
                        head = stack[^1];
                        stack.RemoveAt(stack.Count - 1);
                    }
                    else head = 0;
                }
                else
                {
                    if (head != 0) stack.Add(head);
                    head = unit;
                }
            }

            if (head != 0) stack.Add(head);

            return stack;
        }

        public static int PartOne()
        {
            bytes = Collapse(Encoding.ASCII.GetBytes(input.Trim()));
            return bytes.Count;
        }

        public static int PartTwo()
        {
            var res = int.MaxValue;
            for(var kind = (byte)'a';  kind <= (byte)'z';kind++)
            {
                var c = Collapse(bytes.Where(b => (b | 32) != kind).ToArray());
                res = res < c.Count ? res : c.Count;
            }
            return res;
        }
    }
}
