using aoc_fast.Extensions;

namespace aoc_fast.Years._2017
{
    class Day6
    {
        const ulong REMOVE = 0x0fffffffffffffff;
        private static ulong[] SPREAD = [
            0x0000000000000000,
            0x0100000000000000,
            0x0110000000000000,
            0x0111000000000000,
            0x0111100000000000,
            0x0111110000000000,
            0x0111111000000000,
            0x0111111100000000,
            0x0111111110000000,
            0x0111111111000000,
            0x0111111111100000,
            0x0111111111110000,
            0x0111111111111000,
            0x0111111111111100,
            0x0111111111111110,
            0x0111111111111111,
        ];

        public static string input
        {
            get;
            set;
        }

        private static (uint partOne, uint partTwo) answer;

        private static void Parse()
        {
            var memory = input.ExtractNumbers<ulong>().Aggregate(0UL, (acc, n) => (acc << 4) + n);
            var seen = new Dictionary<ulong, uint>(200000);
            var cycles = 0u;

            seen.Add(memory, cycles);

            while(true)
            {
                var mask = 0x8888888888888888;
                var first = memory & mask;
                mask = first == 0 ? mask : first;

                var second = (memory << 1) & mask;
                mask = second == 0 ? mask : second;

                var third = (memory << 2) & mask;
                mask = third == 0 ? mask : third;

                var fourth = (memory << 3) & mask;
                mask = fourth == 0 ? mask : fourth;
                var offset = (int)ulong.LeadingZeroCount(mask);
                var max = ulong.RotateLeft(memory, offset + 4) & 0xf;

                memory = (memory & ulong.RotateRight(REMOVE, offset)) + ulong.RotateRight(SPREAD[max], offset);
                cycles++;

                if (seen.TryGetValue(memory, out var previous))
                {
                    answer = (cycles, cycles - previous);
                    break;
                }
                else seen[memory] = cycles;
            }
        }
        public static uint PartOne()
        {
            Parse();
            return answer.partOne;
        }
        public static uint PartTwo() => answer.partTwo;
    }
}
