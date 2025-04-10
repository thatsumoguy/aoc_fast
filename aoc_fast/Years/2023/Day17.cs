using System.Runtime.CompilerServices;
using System.Buffers;
using aoc_fast.Extensions;

namespace aoc_fast.Years._2023
{
    internal class Day17
    {
        public static string input { get; set; }
        private static Grid<int> Grid;

        private readonly struct State
        {
            public readonly int X;
            public readonly int Y;
            public readonly byte Direction;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public State(int x, int y, byte direction)
            {
                X = x;
                Y = y;
                Direction = direction;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        private static int AStar(int L, int U, Grid<int> grid)
        {
            var size = grid.width;
            var stride = size;
            var heat = grid.data;

            var bucketSize = Math.Max(size * size / 10, 1000);
            var todo = ArrayPool<State[]>.Shared.Rent(100);
            var todoCount = new int[100];

            for (var i = 0; i < 100; i++)
            {
                todo[i] = ArrayPool<State>.Shared.Rent(bucketSize);
                todoCount[i] = 0;
            }

            var costSize = heat.Length * 2;
            var cost = ArrayPool<int>.Shared.Rent(costSize);

            for (var i = 0; i < costSize; i++)
            {
                cost[i] = int.MaxValue;
            }

            todo[0][todoCount[0]++] = new State(0, 0, 0);
            todo[0][todoCount[0]++] = new State(0, 0, 1);

            cost[0] = 0;
            cost[1] = 0;

            var target = (size - 1) * stride + (size - 1);
            var index = 0;

            var priorityOffset = 2 * size;

            while (true)
            {
                var currentBucket = index % 100;
                ref var currentTodoCount = ref todoCount[currentBucket];

                while (currentTodoCount > 0)
                {
                    var state = todo[currentBucket][--currentTodoCount];
                    var x = state.X;
                    var y = state.Y;
                    var dir = state.Direction;

                    var flatIndex = y * stride + x;
                    var costIndex = flatIndex * 2 + dir;
                    var steps = cost[costIndex];

                    if (flatIndex == target)
                    {
                        for (var i = 0; i < 100; i++)
                        {
                            ArrayPool<State>.Shared.Return(todo[i]);
                        }
                        ArrayPool<State[]>.Shared.Return(todo);
                        ArrayPool<int>.Shared.Return(cost);

                        return steps;
                    }

                    [MethodImpl(MethodImplOptions.AggressiveInlining)]
                    int GetBucketIndex(int nextX, int nextY, int costValue)
                    {
                        var manhattan = (size - 1 - nextX) + (size - 1 - nextY);
                        var priority = Math.Min(priorityOffset - nextX - nextY, size + size / 2);
                        return (costValue + priority) % 100;
                    }

                    if (dir == 0) 
                    {
                        var nextX = x;
                        var nextIndex = flatIndex;
                        var extraCost = steps;

                        for (int i = 1; i <= U; i++)
                        {
                            nextX++;
                            if (nextX >= size) break;

                            nextIndex++;
                            extraCost += heat[nextIndex];

                            if (i >= L)
                            {
                                var nextCostIndex = nextIndex * 2 + 1;
                                if (extraCost < cost[nextCostIndex])
                                {
                                    var bucket = GetBucketIndex(nextX, y, extraCost);
                                    todo[bucket][todoCount[bucket]++] = new State(nextX, y, 1);
                                    cost[nextCostIndex] = extraCost;
                                }
                            }
                        }

                        nextX = x;
                        nextIndex = flatIndex;
                        extraCost = steps;

                        for (int i = 1; i <= U; i++)
                        {
                            nextX--;
                            if (nextX < 0) break;

                            nextIndex--;
                            extraCost += heat[nextIndex];

                            if (i >= L)
                            {
                                var nextCostIndex = nextIndex * 2 + 1;
                                if (extraCost < cost[nextCostIndex])
                                {
                                    var bucket = GetBucketIndex(nextX, y, extraCost);
                                    todo[bucket][todoCount[bucket]++] = new State(nextX, y, 1);
                                    cost[nextCostIndex] = extraCost;
                                }
                            }
                        }
                    }
                    else
                    {
                        var nextY = y;
                        var nextIndex = flatIndex;
                        var extraCost = steps;

                        for (int i = 1; i <= U; i++)
                        {
                            nextY++;
                            if (nextY >= size) break;

                            nextIndex += stride;
                            extraCost += heat[nextIndex];

                            if (i >= L)
                            {
                                var nextCostIndex = nextIndex * 2;
                                if (extraCost < cost[nextCostIndex])
                                {
                                    var bucket = GetBucketIndex(x, nextY, extraCost);
                                    todo[bucket][todoCount[bucket]++] = new State(x, nextY, 0);
                                    cost[nextCostIndex] = extraCost;
                                }
                            }
                        }

                        nextY = y;
                        nextIndex = flatIndex;
                        extraCost = steps;

                        for (var i = 1; i <= U; i++)
                        {
                            nextY--;
                            if (nextY < 0) break;

                            nextIndex -= stride;
                            extraCost += heat[nextIndex];

                            if (i >= L)
                            {
                                var nextCostIndex = nextIndex * 2; // Direction 0 (vertical)
                                if (extraCost < cost[nextCostIndex])
                                {
                                    var bucket = GetBucketIndex(x, nextY, extraCost);
                                    todo[bucket][todoCount[bucket]++] = new State(x, nextY, 0);
                                    cost[nextCostIndex] = extraCost;
                                }
                            }
                        }
                    }
                }
                index++;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        private static void Parse()
        {
            var grid = Grid<byte>.Parse(input);
            var data = new int[grid.data.Length];

            for (var i = 0; i < grid.data.Length; i++)
            {
                data[i] = grid.data[i] - '0';
            }

            Grid = grid.NewWith(0);
            Grid.data = data;
        }

        public static int PartOne()
        {
            Parse();
            return AStar(1, 3, Grid);
        }

        public static int PartTwo() => AStar(4, 10, Grid);
    }
}
