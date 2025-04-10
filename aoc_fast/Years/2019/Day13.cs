using System.Text;
using aoc_fast.Extensions;

namespace aoc_fast.Years._2019
{
    internal class Day13
    {
        public static string input { get; set; }
        private static long[] code = [];
        private static void Parse() => code = input.ExtractNumbers<long>().ToArray();

        public static int PartOne()
        {
            Parse();
            var comp = new Computer(code);
            var blocks = 0;

            while (true)
            {
                if (comp.Run(out var _) != State.Output) break;
                if (comp.Run(out var _) != State.Output) break;
                if (comp.Run(out var t) != State.Output) break;
                if (t == 2) blocks++;
            }
            return blocks;
        }

        public static long PartTwo()
        {
            var modified = code.ToArray();
            modified[0] = 2;
            var comp = new Computer(modified);
            var tiles = new long[1000];
            var stride = 0L;
            var score = 0L;
            var blocks = score;
            var ball = 0L;
            var paddle = 0L;
            while (true)
            {
                var X = 0L;
                switch (comp.Run(out var i))
                {
                    case State.Input:
                        var delta = long.Sign(ball - paddle);
                        comp.Input(delta);
                        continue;
                    case State.Output:
                        X = i; break;
                    case State.Halted:
                        throw new Exception();
                }
                if (comp.Run(out var y) != State.Output) throw new Exception();
                if (comp.Run(out var t) != State.Output) throw new Exception();

                if (X < 0)
                {
                    score = t;
                    if (blocks == 0) return score;
                }
                else
                {
                    if (X >= stride) stride = X + 1;
                    var index = stride * y + X;
                    if (index >= tiles.Length)
                    {
                        Array.Resize(ref tiles, (int)index + 1);
                        Array.Fill(tiles, 0, (int)(tiles.Length - (index + 1)) - 1, (int)index + 1);
                    }

                    switch (t)
                    {
                        case 0:
                            if (tiles[index] == 2) blocks--;
                            break;
                        case 2:
                            if (tiles[index] != 2) blocks++;
                            break;
                        case 3:
                            paddle = X;
                            break;
                        case 4:
                            ball = X;
                            break;
                    }
                    tiles[index] = t;
                }

                //Draw(tiles, stride, score, blocks);
            }
        }

        private const string RESET = "\x1b[0m";
        private const string BOLD = "\x1b[1m";
        private const string RED = "\x1b[31m";
        private const string GREEN = "\x1b[32m";
        private const string YELLOW = "\x1b[33m";
        private const string BLUE = "\x1b[94m";
        private const string WHITE = "\x1b[97m";
        private const string HOME = "\x1b[H";
        private const string CLEAR = "\x1b[J";

        public static void Draw(long[] tiles, long stride, long score, long blocks)
        {
            // Find the paddle's position
            int paddle = Array.LastIndexOf(tiles, 3);
            if (tiles.Skip(paddle).Count(t => t == 1) < 3)
            {
                return; // Not enough blocks, so we return early
            }

            var sb = new StringBuilder();
            sb.AppendLine($"{WHITE}{BOLD}Blocks: {blocks}\tScore: {score} {RESET}");

            long y = 0;
            // Iterate through rows, ensuring we don't go out of bounds
            while (stride * y < tiles.Length)
            {
                for (long X = 0; X < stride; X++)
                {
                    int index = (int)(stride * y + X);

                    // Ensure the index is within bounds
                    if (index >= tiles.Length)
                    {
                        break; // Prevent accessing out of bounds
                    }

                    switch (tiles[index])
                    {
                        case 0:
                            sb.Append(" ");
                            break;
                        case 1 when y == 0:
                            sb.Append($"{GREEN}_{RESET}");
                            break;
                        case 1:
                            sb.Append($"{GREEN}|{RESET}");
                            break;
                        case 2:
                            sb.Append($"{BLUE}#{RESET}");
                            break;
                        case 3:
                            sb.Append($"{WHITE}{BOLD}={RESET}");
                            break;
                        case 4:
                            sb.Append($"{YELLOW}{BOLD}o{RESET}");
                            break;
                        default:
                            throw new InvalidOperationException("Unknown tile type.");
                    }
                }
                sb.AppendLine();
                y++;
            }
            Console.Clear();
            Console.WriteLine($"{HOME}{CLEAR}{sb}");
            Thread.Sleep(2); // Delay to match the original sleep duration
        }

    }
}
