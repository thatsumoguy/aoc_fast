using System.Collections.Concurrent;
using System.Text;
using aoc_fast.Extensions;

namespace aoc_fast.Years._2016
{
    class Day17
    {
        public static string input { get; set; }

        private record struct PathItem(byte X, byte Y, int Size, byte[] Path);

        private class Shared
        {
            public List<PathItem> Todo = [];
            public string MinPath = "";
            public int MaxLength = 0;
            public int Prefix;
            public int Inflight = 0;
        }

        private static object mutex = new();

        private static void Worker(Shared shared)
        {
            var localTodo = new List<PathItem>();
            var localMin = "";
            var localMax = 0;

            while (true)
            {
                PathItem item;

                lock (mutex)
                {
                    while (shared.Todo.Count == 0)
                    {
                        if (shared.Inflight == 0)
                        {
                            return;
                        }
                        Monitor.Wait(mutex);
                    }

                    item = shared.Todo.Pop();
                    shared.Inflight++;
                }

                localTodo.Add(item);

                for (int i = 0; i < 100 && localTodo.Count > 0; i++)
                {
                    var current = localTodo.Pop();
                    var (x, y, size, path) = current;

                    if (x == 3 && y == 3)
                    {
                        var adjusted = size - shared.Prefix;
                        if (string.IsNullOrEmpty(localMin) || adjusted < localMin.Length)
                        {
                            var middle = new byte[adjusted];
                            Array.Copy(path, shared.Prefix, middle, 0, adjusted);
                            localMin = Encoding.UTF8.GetString(middle);
                        }
                        localMax = Math.Max(localMax, adjusted);
                    }
                    else
                    {
                        var (result, _, _, _) = Md5.Hash(path, size);

                        if (y > 0 && ((result >> 28) & 0xf) > 0xa)
                        {
                            localTodo.Add(new PathItem(x, (byte)(y - 1), size + 1, ExtendBuffer(path, size, (byte)'U')));
                        }
                        if (y < 3 && ((result >> 24) & 0xf) > 0xa)
                        {
                            localTodo.Add(new PathItem(x, (byte)(y + 1), size + 1, ExtendBuffer(path, size, (byte)'D')));
                        }
                        if (x > 0 && ((result >> 20) & 0xf) > 0xa)
                        {
                            localTodo.Add(new PathItem((byte)(x - 1), y, size + 1, ExtendBuffer(path, size, (byte)'L')));
                        }
                        if (x < 3 && ((result >> 16) & 0xf) > 0xa)
                        {
                            localTodo.Add(new PathItem((byte)(x + 1), y, size + 1, ExtendBuffer(path, size, (byte)'R')));
                        }
                    }
                }

                lock (mutex)
                {
                    shared.Todo.AddRange(localTodo);
                    localTodo.Clear();

                    if (string.IsNullOrEmpty(shared.MinPath) || (!string.IsNullOrEmpty(localMin) && localMin.Length < shared.MinPath.Length))
                    {
                        shared.MinPath = localMin;
                    }
                    shared.MaxLength = Math.Max(shared.MaxLength, localMax);

                    localMin = "";
                    localMax = 0;
                    shared.Inflight--;
                    Monitor.PulseAll(mutex);
                }
            }
        }

        private static byte[] ExtendBuffer(byte[] src, int size, byte b)
        {
            var padded = BufferSize(size + 1);
            var next = new byte[padded];
            Array.Copy(src, 0, next, 0, size);
            next[size] = b;
            return next;
        }

        private static int BufferSize(int size)
        {
            return ((size + 8) / 64 + 1) * 64;
        }

        private static (string, int) result;

        private static void Parse()
        {
            var inputBytes = Encoding.UTF8.GetBytes(input.Trim());
            var prefix = inputBytes.Length;
            var startPath = new byte[BufferSize(prefix)];
            Array.Copy(inputBytes, startPath, prefix);
            var start = new PathItem(0, 0, prefix, startPath);

            var shared = new Shared
            {
                Todo = [start],
                MinPath = "",
                MaxLength = 0,
                Prefix = prefix,
                Inflight = 0
            };

            Threads.Spawn(() => Worker(shared));

            result = (shared.MinPath, shared.MaxLength);
        }
        public static string PartOne()
        {
            Parse();
            return result.Item1;
        }

        public static int PartTwo()
        {
            return result.Item2;
        }
    }
}