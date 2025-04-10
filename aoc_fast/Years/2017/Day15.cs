using System.Threading.Channels;
using aoc_fast.Extensions;

namespace aoc_fast.Years._2017
{
    internal class Day15
    {
       
        private const int PartOneCount = 40000000;
        private const int PartTwoCount = 5000000;
        private const int BlockSize = 50000;

        public static string input { get;set; }
        private (uint PartOne, uint PartTwo) answers;
        private class Shared
        {
            public ulong First { get; init; }
            public ulong Second { get; init; }
            private int _start;

            public int GetNextStart() => Interlocked.Add(ref _start, BlockSize) - BlockSize;
        }

        private record Block(int Start, uint Ones, List<ushort> Fours, List<ushort> Eights);

        private static async Task Sender(Shared shared, ChannelWriter<Block> writer, CancellationToken cancellationToken)
        {
            try
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    var start = shared.GetNextStart();

                    ulong first = (shared.First * ModPow(16807, (ulong)start, 0x7fffffff)) % 0x7fffffff;
                    ulong second = (shared.Second * ModPow(48271, (ulong)start, 0x7fffffff)) % 0x7fffffff;

                    var ones = 0u;
                    var fours = new List<ushort>((BlockSize * 30) / 100);
                    var eights = new List<ushort>((BlockSize * 15) / 100);

                    for (int i = 0; i < BlockSize; i++)
                    {
                        first = (first * 16807) % 0x7fffffff;
                        second = (second * 48271) % 0x7fffffff;

                        var left = (ushort)first;
                        var right = (ushort)second;

                        if (left == right)
                        {
                            ones++;
                        }

                        if (left % 4 == 0)
                        {
                            fours.Add(left);
                        }

                        if (right % 8 == 0)
                        {
                            eights.Add(right);
                        }
                    }

                    await writer.WriteAsync(new Block(start, ones, fours, eights), cancellationToken);
                }
            }
            catch (OperationCanceledException)
            {
            }
            finally
            {
                writer.Complete();
            }
        }

        private static async Task<(uint, uint)> ReceiverAsync(Shared shared, ChannelReader<Block> reader, CancellationToken cancellationToken)
        {
            var remaining = PartTwoCount;
            var partTwo = 0u;

            var required = 0;
            var outOfOrder = new SortedDictionary<int, Block>();
            var blocks = new List<Block>();

            var foursBlock = 0;
            var foursIndex = 0;

            var eightsBlock = 0;
            var eightsIndex = 0;

            while (remaining > 0)
            {
                while (foursBlock >= blocks.Count || eightsBlock >= blocks.Count)
                {
                    var block = await reader.ReadAsync(cancellationToken);
                    outOfOrder[block.Start] = block;

                    while (outOfOrder.TryGetValue(required, out var next))
                    {
                        blocks.Add(next);
                        outOfOrder.Remove(required);
                        required += BlockSize;
                    }
                }

                var fours = blocks[foursBlock].Fours;
                var eights = blocks[eightsBlock].Eights;
                var iterations = Math.Min(Math.Min(remaining, fours.Count - foursIndex), eights.Count - eightsIndex);

                remaining -= iterations;

                for (var i = 0; i < iterations; i++)
                {
                    if (fours[foursIndex] == eights[eightsIndex])
                    {
                        partTwo++;
                    }
                    foursIndex++;
                    eightsIndex++;
                }

                if (foursIndex == fours.Count)
                {
                    foursBlock++;
                    foursIndex = 0;
                }
                if (eightsIndex == eights.Count)
                {
                    eightsBlock++;
                    eightsIndex = 0;
                }
            }

            while (required < PartOneCount)
            {
                var block = await reader.ReadAsync(cancellationToken);
                outOfOrder[block.Start] = block;

                while (outOfOrder.TryGetValue(required, out var next))
                {
                    blocks.Add(next);
                    outOfOrder.Remove(required);
                    required += BlockSize;
                }
            }

            var partOne = (uint)blocks.Take(PartOneCount / BlockSize).Sum(p => p.Ones);
            return (partOne, partTwo);
        }

        private static ulong ModPow(ulong baseValue, ulong exponent, ulong modulus)
        {
            if (modulus == 1)
                return 0;

            var result = 1ul;
            baseValue %= modulus;

            while (exponent > 0)
            {
                if ((exponent & 1) == 1)
                    result = (result * baseValue) % modulus;

                exponent >>= 1;
                baseValue = (baseValue * baseValue) % modulus;
            }

            return result;
        }
        private void Parse()
        {
            var numbers = input.ExtractNumbers<ulong>();

            var first = numbers[0];
            var second = numbers[1];

            var shared = new Shared { First = first, Second = second };
            var channel = Channel.CreateUnbounded<Block>();

            var workerCount = Math.Max(1, Environment.ProcessorCount - 1);
            var cts = new CancellationTokenSource();
            var workers = new List<Task>(workerCount);

            for (int i = 0; i < workerCount; i++)
            {
                workers.Add(Task.Run(() => Sender(shared, channel.Writer, cts.Token)));
            }

            answers = ReceiverAsync(shared, channel.Reader, cts.Token).GetAwaiter().GetResult();

            cts.Cancel();

            try
            {
                Task.WaitAll([.. workers]);
            }
            catch (AggregateException)
            {
            }
        }

        public uint PartOne()
        {
            Parse();
            return answers.PartOne;
        }

        public uint PartTwo() => answers.PartTwo;
    }
}

