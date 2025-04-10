using System.Buffers.Binary;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Channels;
using aoc_fast.Extensions;

namespace aoc_fast.Years._2018
{
    internal class Day14
    {
        public static string input { get; set; }
        private static readonly byte[] PREFIX = [3, 7, 1, 0, 1, 0, 1, 2, 4, 5, 1, 5, 8, 9, 1, 6, 7, 7, 9, 2, 5, 1, 0];
        static CancellationTokenSource source = new();
        static CancellationToken cts = source.Token;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static ulong PrefixSum(ulong u)
        {
            var s = u;
            s += s >> 8;
            s += s >> 16;
            s += s >> 32;
            return s;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static ulong LSB(ulong u) => u & 0xff;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static (ulong, ulong, ulong) UnPack(ulong first, ulong second)
        {
            const ulong ONES = 0x0101010101010101u;
            const ulong SIXES = 0x0606060606060606u;
            const ulong INDICES = 0x0001020304050607u;

            var sum = first + second;

            var tens = ((sum + SIXES) >> 4) & ONES;

            var digits = sum - 10 * tens;

            var indices = PrefixSum(tens) + INDICES;

            var extra = 1 + LSB(indices);

            return (digits, indices, extra);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static ulong FromBeBytes(ReadOnlySpan<byte> slice, int index) => BinaryPrimitives.ReadUInt64BigEndian(slice.Slice(index, 8));

        private static int _done = 0;
        public static bool ThreadSafeBool
        {
            get { return (Interlocked.CompareExchange(ref _done, 1, 1) == 1); }
            set
            {
                if (value) Interlocked.CompareExchange(ref _done, 1, 0);
                else Interlocked.CompareExchange(ref _done, 0, 1);
            }
        }

        private static async Task<(string?, ulong?)> Reader(ChannelReader<byte[]> rx, string input)
        {
            var partOneTarget = input.ExtractNumbers<ulong>()[0] + 10;
            var partTwoTarget = Convert.ToUInt32(input.Trim(), 16);

            string? partOneRes = null;
            ulong? partTwoRes = null;

            var history = new List<byte[]>();
            var total = 0ul;
            var pattern = 0u;

            await foreach (var slice in rx.ReadAllAsync(cts))
            {
                history.Add(slice);
                total += (ulong)slice.Length;
                if(partOneRes == null && total >= partOneTarget)
                {
                    var index = 0;
                    var offset = partOneTarget - 10;
                    var res = new StringBuilder();

                    for(var  _ = 0;  _ < 10;  _++)
                    {
                        while(offset >= (ulong)history[index].Length)
                        {
                            offset -= (ulong)history[index].Length;
                            index++;
                        }

                        var digit = history[index][offset];
                        res.Append((char)(digit + (byte)'0'));
                        offset++;
                    }
                    partOneRes = res.ToString();
                }
                if (partTwoRes == null)
                {
                    foreach(var (i, n) in slice.ToList().Index())
                    {
                        pattern = ((pattern << 4) | ((uint)n)) & 0xffffff;
                        if(pattern == partTwoTarget)
                        {
                            partTwoRes = total - (ulong)slice.Length + (ulong)i - 5;
                            break;
                        }
                    }
                }
                if(partOneRes != null && partTwoRes != null)
                {
                    ThreadSafeBool = true;
                    source.Cancel();
                    break;
                }
            }
            return (partOneRes, partTwoRes);
        }

        private static async void Writer(ChannelWriter<byte[]> tx, byte[] recipes)
        {
            var elf1 = 0ul;
            var index1 = 0;
            var elf2 = 8ul;
            var index2 = 0;
            var baseNum = 0;
            var size = 23;
            var needed = 23;

            var write = 0;
            var snack = new byte[5000000];
            Array.Fill(snack, (byte)0);

            while (!ThreadSafeBool)
            {
                while (elf1 < 23 || elf2 < 23 || write - Math.Max(index1, index2) <= 16)
                {
                    byte recipe1;
                    if (elf1 < 23) recipe1 = PREFIX[elf1];
                    else
                    {
                        index1++;
                        recipe1 = snack[index1 - 1];
                    }
                    byte recipe2;
                    if (elf2 < 23) recipe2 = PREFIX[elf2];
                    else
                    {
                        index2++;
                        recipe2 = snack[index2 - 1];
                    }

                    var next = recipe1 + recipe2;
                    if (next < 10)
                    {
                        recipes[(int)size - baseNum] = (byte)next;
                        size++;
                    }
                    else
                    {
                        recipes[(int)size - baseNum + 1] = (byte)(next - 10);
                        size += 2;
                    }

                    if (needed < size)
                    {
                        var digit = recipes[(int)needed - baseNum];
                        needed += 1 + digit;

                        snack[write] = digit;
                        write++;
                    }

                    elf1 += (ulong)(1 + recipe1);
                    if ((int)elf1 >= size)
                    {
                        elf1 -= (ulong)size;
                        index1 = 0;
                    }
                    elf2 += (ulong)(1 + recipe2);
                    if (elf2 >= (ulong)size)
                    {
                        elf2 -= (ulong)size;
                        index2 = 0;
                    }
                }

                var batchSize = Math.Min(10000, (write - Math.Max(index1, index2) - 1) / 16);

                for (var _ = 0; _ < batchSize; _++)
                {
                    var first = FromBeBytes(snack, index1);
                    var second = FromBeBytes(snack, index2);
                    var third = FromBeBytes(snack, index1 + 8);
                    var fourth = FromBeBytes(snack, index2 + 8);
                    elf1 += 16 + LSB(PrefixSum(first)) + LSB(PrefixSum(third));
                    elf2 += 16 + LSB(PrefixSum(second)) + LSB(PrefixSum(fourth));

                    index1 += 16;
                    index2 += 16;

                    var (digits1, indices1, extra1) = UnPack(first, second);
                    var (digits2, indices2, extra2) = UnPack(third, fourth);
                    for (var shift = 0; shift < 64; shift += 8)
                    {
                        var digit = LSB(digits1 >> shift);
                        var index = LSB(indices1 >> shift);

                        recipes[(int)size - baseNum + (int)index] = (byte)digit;

                        var digit2 = LSB(digits2 >> shift);
                        var i2 = LSB(indices2 >> shift);

                        recipes[size - baseNum + (int)i2 + (int)extra1] = (byte)digit2;

                    }

                    size += (int)extra1 + (int)extra2;

                    while (needed < size)
                    {
                        var digit = recipes[needed - baseNum];

                        needed += 1 + digit;

                        snack[write] = digit;
                        write++;
                    }
                }

                var mid = size - baseNum;;
                var (head, tail) = (recipes[..mid], recipes[mid..]);
                await tx.WriteAsync(head);
                recipes = tail;
                baseNum = size;
            }

            tx.Complete();
        }

        private static (string? partOne, ulong? partTwo) answer;

        private static void Parse()
        {
            ThreadSafeBool = false;
            var channel = Channel.CreateUnbounded<byte[]>();
            var tx = channel.Writer;
            var rx = channel.Reader;

            var recipes = new byte[25000000];
            Array.Fill(recipes, (byte)1);

            var readerTask = Task.Run(() => Reader(rx, input));
            var writerTask = Task.Run(() => Writer(tx, recipes));
            

            Task.WaitAll(writerTask, readerTask);
            answer = readerTask.Result;
        }

        public static string PartOne()
        {
            Parse();
            return answer.partOne;
        }
        public static ulong? PartTwo() => answer.partTwo;

    }
    
}