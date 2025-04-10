using System.Text;

namespace aoc_fast.Years._2015
{
    class Day8
    {
        const byte NEWLINE = 10;
        const byte QUOTE= 34;
        const byte SLASH = 92;
        const byte ESCAPE = 120;

        public static string input
        {
            get;
            set;
        }

        public static uint PartOne() => Encoding.ASCII.GetBytes(input).Aggregate((flag: false, count: 0u), (state, b) => state.flag switch
        {
            true when b == ESCAPE => (false, state.count + 3),
            true => (false, state.count + 1),
            false when b == SLASH => (true, state.count),
            false when b == NEWLINE => (false, state.count + 2),
            _ => (false, state.count)
        }).count;

        public static int PartTwo() => Encoding.ASCII.GetBytes(input).Select(b => b switch
        {
            QUOTE or SLASH => 1,
            NEWLINE => 2,
            _ => 0
        }).Sum();
    }
}
