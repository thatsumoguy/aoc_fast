using System.Runtime.CompilerServices;
using System.Text;
using aoc_fast.Extensions;

namespace aoc_fast.Years._2023
{
    internal class Day15
    {
        public static string input { get; set; }
        struct Item(byte[] label, int lens)
        {
            public byte[] Label { get; set; } = label;
            public int Lens { get; set; } = lens;
        }
        private static (int partOne, int partTwo) answers;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int Hash(byte[] slice) => slice.Aggregate(0, (acc, b) => ((acc + b) * 17) & 0xff);

        private static void Parse()
        {

            var partOne = 0;
            var partTwo = 0;
            var boxes = new List<List<Item>>();

            for (var i = 0; i < 256; i++)
            {
                boxes.Add([]);
            }

            foreach (var step in Encoding.ASCII.GetBytes(input.Trim()).Split(b => b == (byte)','))
            {
                var size = step.Length;
                partOne += Hash(step);

                if (step[size - 1] == (byte)'-')
                {
                    var label = step[..(size - 1)];
                    var hash = Hash(label);
                    var labelIndex = boxes[hash].FindIndex(item => item.Label.SequenceEqual(label));
                    if (labelIndex != -1) boxes[hash].RemoveAt(labelIndex);
                }
                else
                {
                    var label = step[..(size - 2)];
                    var hash = Hash(label);
                    var lens = step[size - 1] - (byte)'0';
                    var labelIndex = boxes[hash].FindIndex(item => item.Label.SequenceEqual(label));

                    if (labelIndex != -1)
                    {
                        var item = boxes[hash][labelIndex];
                        item.Lens = lens;
                        boxes[hash][labelIndex] = item;
                    }
                    else
                    {
                        boxes[hash].Add(new Item(label, lens));
                    }
                }
            }

            for (var i = 0; i < boxes.Count; i++)
            {
                for (var j = 0; j < boxes[i].Count; j++)
                {
                    partTwo += (i + 1) * (j + 1) * boxes[i][j].Lens;
                }
            }

            answers = (partOne, partTwo);
        }

        public static int PartOne()
        {
            Parse();
            return answers.partOne;
        }
        public static int PartTwo() => answers.partTwo;
    }
}
