using System.Text;
using aoc_fast.Extensions;

namespace aoc_fast.Years._2016
{
    class Day21
    {
        public static string input
        {
            get;
            set;
        }

        private abstract record Op()
        {
            public record SwapPosition(int left, int right) : Op;
            public record SwapLetter(byte left, byte right) : Op;
            public record RotateLeft(int val) : Op;
            public record RotateRight(int val) : Op;
            public record RotateLetterRight(byte letter) : Op;
            public record RotateLetterLeft(byte letter) : Op;
            public record Reverse(int left, int right) : Op;
            public record Move(int left, int right) : Op;

            public override string ToString() =>
                this switch
                {
                    SwapPosition(var f, var s) => $"Op.SwapPosition({f},{s})",
                    SwapLetter(var f, var s) => $"Op.SwapLett({(char)f}{(char)s})",
                    RotateLeft(var v) => $"Op.RotateLeft({v})",
                    RotateRight(var v) => $"Op.RotateRight({v})",
                    RotateLetterRight(var v) => $"Op.RotateLetterRight({(char)v})",
                    RotateLetterLeft(var v) => $"Op.RotateLetterLeft({(char)v})",
                    Reverse(var f, var s) => $"Op.Reverse({f},{s})",
                    Move(var f, var s) => $"Op.Move({f},{s})"
                };

            public static Op From(string line)
            {
                var tokens = line.Split([' ', '\n','\t'],StringSplitOptions.RemoveEmptyEntries);
                var digit = (int i) => int.Parse(tokens[i]);
                var letter = (int i) => Encoding.ASCII.GetBytes(tokens[i])[0];
                

                return tokens[0] switch
                {
                    "reverse" => new Reverse(digit(2), digit(4)),
                    "move" => new Move(digit(2), digit(5)),
                    _ => tokens[1] switch
                    {
                        "position" => new SwapPosition(digit(2), digit(5)),
                        "letter" => new SwapLetter(letter(2), letter(5)),
                        "left" => new RotateLeft(digit(2)),
                        "right" => new RotateRight(digit(2)),
                        "based" => new RotateLetterRight(letter(6)),
                        _ => throw new Exception()
                    }
                };
            }

            public void Transform(List<byte> password)
            {
                var position = (byte a) => password.FindIndex(b => b == a);
                switch(this)
                {
                    case SwapPosition(var first, var second):
                        (password[first], password[second]) = (password[second], password[first]);
                        break;
                    case SwapLetter(var first, var second):
                        var firstPos = position(first);
                        var secondPos = position(second);
                        (password[firstPos], password[secondPos]) = (password[secondPos], password[firstPos]);
                        break;
                    case RotateLeft(var first):
                        password.RotateLeft(first);
                        break;
                    case RotateRight(var first):
                        password.RotateRight(first);
                        
                        break;
                    case RotateLetterLeft(byte first):
                        var f = position(first);
                        for(var i = 0; i < password.Count; i++)
                        {
                            var s = i >= 4 ? 2 : 1;
                            var t = (2 * i + s) % password.Count;
                            if(f == t)
                            {
                                if (i < f) password.RotateLeft(f - i);
                                else password.RotateRight(i - f);
                            }
                        }
                        break;
                    case RotateLetterRight(byte first):
                        firstPos = position(first);
                        secondPos = firstPos >= 4 ? 2 : 1;
                        var third = (firstPos + secondPos) % password.Count;
                        password.RotateRight(third);
                        break;
                    case Reverse(int first, int second):
                        
                        password.Reverse(first, second - first + 1);
                        break;
                    case Move(int first, int second):
                        var letter = password[first];
                        password.RemoveAt(first);
                        password.Insert(second, letter);
                        break;
                }
            }

            public Op Inverse() =>
                this switch
                {
                    RotateLeft(var first) => new RotateRight(first),
                    RotateRight(var first) => new RotateLeft(first),
                    RotateLetterLeft(var first) => new RotateLetterRight(first),
                    RotateLetterRight(var first) => new RotateLetterLeft(first),
                    Move(var first, var second) => new Move(second, first),
                    _ => this
                };
        }

        private static string UnScramble(List<Op> input, byte[] slice)
        {
            var password = slice.ToList();
            input.Reverse();
            foreach (var op in input) op.Inverse().Transform(password);

            return Encoding.ASCII.GetString([.. password]);
        }

        private static string Scramble(List<Op> input, byte[] slice)
        {
            var password = slice.ToList();

            foreach (var op in input) op.Transform(password);

            return Encoding.ASCII.GetString([.. password]);
        }

        private static List<Op> Ops = [];

        private static void Parse() => Ops = input.Split("\n", StringSplitOptions.RemoveEmptyEntries).Select(Op.From).ToList();

        public static string PartOne()
        {
            Parse();
            return Scramble(Ops, Encoding.ASCII.GetBytes("abcdefgh"));
        }

        public static string PartTwo() => UnScramble(Ops, Encoding.ASCII.GetBytes("fbgdceah"));
    }
}
