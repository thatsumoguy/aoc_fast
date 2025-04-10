using System.Text;

namespace aoc_fast.Years._2022
{
    internal class Day24
    {
        public static string input { get; set; }

        record Input(int width, int height, List<UInt128> horizontal, List<UInt128> vertical);

        private static Input Inputobj;

        private static int Expedition(Input input, int start, bool forward)
        {
            var (width, height, horizontal, vertical) = input;
            var time = start;
            var state = new UInt128[height + 1];

            while(true)
            {
                time++;

                UInt128 prev;
                var cur = (UInt128)0;
                var next = state[0];

                for(var i = 0; i < height; i++)
                {
                    prev = cur;
                    cur = next;
                    next = state[i + 1];

                    state[i] = (cur | (cur >> 1) | (cur << 1) | prev | next) & horizontal[height * (time % width) + i] & vertical[height * (time % height) + i];
                }

                if(forward)
                {
                    state[0] |= (UInt128)1 << (width - 1);
                    if ((state[height - 1] & 1) != 0) return time + 1;
                }
                else
                {
                    state[height - 1] |= (UInt128)1;
                    if ((state[0] & ((UInt128)1 << (width - 1))) != 0) return time + 1;
                }
            }
        }

        private static void Parse()
        {
            var raw = input.Split("\n", StringSplitOptions.RemoveEmptyEntries).Select(line => 
            {
                var bytes = Encoding.ASCII.GetBytes(line);
                return bytes[1..(bytes.Length - 1)];
            }).ToList();

            var width = raw.First().Length;
            var height = raw.Count() - 2;

            var build = (byte kind) =>
            {
                var fold = (byte[] row) => row.Aggregate((UInt128)0, (acc, b) => (acc << 1) | (b != kind ? (UInt128)1 : (UInt128)0));
                return raw[1..(height +1)].Select(fold).ToList();
            };

            var left = build((byte)'<');
            var right = build((byte)'>');
            var up = build((byte)'^');
            var down = build((byte)'v');

            var horizontal = new List<UInt128>(width * height);

            for(var time = 0; time < width; time++)
            {
                for(var i = 0; i < height; i++)
                {
                    var subLeft = (left[i] << time) | (left[i] >> (width - time));
                    var subRight = (right[i] >> time) | (right[i] << (width - time));
                    horizontal.Add(subLeft & subRight);
                }
            }

            var vertical = new List<UInt128>(height * height);

            for(var time = 0; time < height; time++)
            {
                for(var i = 0; i < height; i++)
                {
                    var subUp = up[(i + time) % height];
                    var subDown = down[(height + i - time % height) % height];
                    vertical.Add(subUp & subDown);
                }
            }

            Inputobj = new Input(width, height, horizontal, vertical);
        }

        public static int PartOne()
        {
            Parse();
            return Expedition(Inputobj, 0, true);
        }
        public static int PartTwo()
        {
            var first = Expedition(Inputobj, 0, true);
            var second = Expedition(Inputobj, first, false);
            return Expedition(Inputobj, second, true);
        }
    }
}
