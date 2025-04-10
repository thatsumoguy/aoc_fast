using System.Text;
using aoc_fast.Extensions;

namespace aoc_fast.Years._2017
{
    internal class Day16
    {
        public static string input
        {
            get;
            set;
        }

        class Dance
        {
            public ulong[] position { get; set; }
            public ulong[] exchange { get; set; }

            public static Dance New() => new() { position = [.. Enumerable.Range(0, 16).Select(i => (ulong)i)], exchange = [.. Enumerable.Range(0, 16).Select(i => (ulong)i)] };

            public string Apply() => string.Join("", position.Select(i => ToChar(exchange[i])));

            public Dance Compose(Dance other)
            {
                var pos = position.Select(i => other.position[i]).ToArray();
                var newExchange = exchange.Select(i => other.exchange[i]).ToArray();
                return new Dance { position = pos, exchange = newExchange };
            }
        }

        private static ulong FromByte(byte b) => (ulong)(b - (byte)'a');

        private static char ToChar(ulong i) => (char)((byte)i + (byte)'a');

        private static Dance answer;

        private static void Parse()
        {
            try
            {
                var letters = Encoding.ASCII.GetBytes(input).Where(b => char.IsAsciiLetterLower((char)b)).GetEnumerator();
                var numbers = input.ExtractNumbers<uint>().GetEnumerator();

                var offset = 0u;
                var lookup = Enumerable.Range(0, 16).Select(i => (ulong)i).ToArray();

                var dance = Dance.New();
                while (letters.MoveNext())
                {
                    var op = letters.Current;

                    switch (op)
                    {
                        case (byte)'s':
                            numbers.MoveNext();
                            var a = numbers.Current;
                            offset += 16 - a;
                            break;
                        case (byte)'X':
                            numbers.MoveNext();
                            var first = numbers.Current;
                            numbers.MoveNext();
                            var second = numbers.Current;
                            (dance.position[(first + offset) % 16], dance.position[(second + offset) % 16]) = (dance.position[(second + offset) % 16], dance.position[(first + offset) % 16]);
                            break;
                        case (byte)'p':
                            letters.MoveNext();
                            var firstP = FromByte(letters.Current);
                            letters.MoveNext();
                            var secondP = FromByte(letters.Current);
                            (lookup[firstP], lookup[secondP]) = (lookup[secondP], lookup[firstP]);
                            (dance.exchange[lookup[firstP]], dance.exchange[lookup[secondP]]) = (dance.exchange[lookup[secondP]], dance.exchange[lookup[firstP]]);
                            break;
                    }
                }

                dance.position.RotateLeft((int)(offset % 16));
                answer = dance;
            }
            catch (Exception e) { Console.WriteLine(e); }
        }
        public static string PartOne()
        {
            Parse();
            return answer.Apply();
        }

        public static string PartTwo()
        {
            var e = 1_000_000_000;
            var dance = answer;
            var res = Dance.New();

            while(e > 0)
            {
                if ((e & 1) == 1) res = res.Compose(dance);
                e >>= 1;
                dance = dance.Compose(dance);
            }

            return res.Apply();
        }
    }
}
