using System.Text;
using aoc_fast.Extensions;

namespace aoc_fast.Years._2016
{
    class Day4
    {
        public static string input
        {
            get;
            set;
        }

        record Room(string name, int sectorId);

        private static List<Room> Valid = [];

        private static void Parse()
        {
            try
            {

                var valid = new List<Room>();
                var toIndex = (byte b) => b - (byte)'a';
                var lines = input.Split("\n", StringSplitOptions.RemoveEmptyEntries);

                for (var i = 0; i < lines.Length; i++)
                {
                outer:
                    var line = lines[i].Trim();
                    var size = line.Length;
                    var name = line[..(size - 11)];
                    var sectorId = int.Parse(line[(size - 10)..(size - 7)]);
                    var checksum = Encoding.ASCII.GetBytes(line[(size - 6)..(size - 1)]);

                    var freq = Enumerable.Repeat(0, 26).ToArray();
                    var fof = Enumerable.Repeat(0, 64).ToArray();
                    var highest = 0;

                    foreach (var b in Encoding.ASCII.GetBytes(name))
                    {
                        if (b != (byte)'-')
                        {
                            var index = toIndex(b);
                            var current = freq[index];
                            var next = freq[index] + 1;

                            freq[index] = next;
                            fof[current]--;
                            fof[next]++;

                            highest = Math.Max(highest, next);
                        }
                    }

                    if (freq[toIndex(checksum[0])] != highest) continue;

                    foreach (var w in checksum.Windows(2))
                    {
                        var end = freq[toIndex(w[0])];
                        var start = freq[toIndex(w[1])];

                        if (start > end || (start == end && w[1] <= w[0]))
                        {
                            i++;
                            goto outer;
                        }
                        if (start + 1 > end) continue;
                        if (Enumerable.Range(start + 1, end - (start + 1)).Any(i => fof[i] != 0))
                        {
                            i++;
                            goto outer;
                        }
                    }

                    valid.Add(new Room(name, sectorId));
                }

                Valid = valid;
            }
            catch (Exception e) { Console.WriteLine(e); }
        }

        public static int PartOne()
        {
            Parse();
            return Valid.Select(r => r.sectorId).Sum();
        }

        public static int PartTwo()
        {
            foreach(var room in Valid)
            {
                var bytes = Encoding.ASCII.GetBytes(room.name);

                if(bytes.Length == 24 && bytes[9] == (byte)'-' && bytes[16] == (byte)'-')
                {
                    var buffer = new StringBuilder(24);

                    foreach(var b in bytes)
                    {
                        if (b == (byte)'-') buffer.Append(' ');
                        else
                        {
                            var rotate = (byte)(room.sectorId % 26);

                            var decrypted = (byte)((b - (byte)'a' + rotate) % 26 + (byte)'a');

                            buffer.Append((char)decrypted);
                        }
                    }
                    if(buffer.ToString() == "northpole object storage") return room.sectorId;
                }
            }

            return -1;
        }
    }
}
