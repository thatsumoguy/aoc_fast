using aoc_fast.Extensions;

namespace aoc_fast.Years._2025
{
    internal class Day10
    {
        public static string input
        {
            get;
            set;
        }

        private static List<(uint Lights, List<uint> Buttons, List<long> Joltage, List<List<int>> ButtonsList)> Machines = [];

        private static void Parse()
        {
            Machines = [.. input.Split("\n", StringSplitOptions.RemoveEmptyEntries).Select(l =>
            {
                var split = l.Split(' ');
                var lights = split[0];
                lights = lights[1..(lights.Length - 1)];
                var goalMask = 0u;
                foreach(var (i, c) in lights.ToCharArray().Index())
                {
                    if(c == '#') goalMask |= 1u << i;
                }
                var buttonList = new List<List<int>>();
                var joltage = new List<long>();
                var rest = split.Skip(1).ToList();
                var buttonMasks = new List<uint>();
                foreach(var part in rest)
                {
                    if(part.Contains('{')) joltage.AddRange(part[1..(part.Length - 1)].Split(',').Select(long.Parse));
                    else
                    {
                        var mask = 0u;
                        var button = new List<int>();
                        button.AddRange([.. part[1..(part.Length - 1)].Split(',').Select(int.Parse)]);
                        buttonList.Add(button);
                        foreach(var b in part[1..(part.Length - 1)].Split(',').Select(int.Parse)) mask |= 1u << b;
                        buttonMasks.Add(mask);
                    }
                    
                }
                return(goalMask, buttonMasks, joltage, buttonList);
            })];
        }
        const double EPS = 1e-9;
        private static (double, List<double>) Simplex(double[][] lhs, double[] c)
        {
            var m = lhs.Length;
            var n = lhs[0].Length - 1;

            var nIndices = Enumerable.Range(0, n).ToList();
            nIndices.Add(-1);

            var bIndices = new List<int>();
            for(var i = n; i < (n + m); i++) bIndices.Add(i);

            var d = new double[m + 2][];
            for(var i = 0; i < d.Length; i++)
            {
                d[i] = new double[n + 2];
                for(var j= 0; j < d[i].Length; j++) d[i][j] = 0.0;
            }

            for(var i = 0; i < m; i++)
            {
                Array.Copy(lhs[i], d[i], n + 1);
                d[i][n + 1] = -1.0;
            }

            for (var i = 0; i < m; i++) (d[i][n], d[i][n +1]) = (d[i][n + 1],  d[i][n]);

            Array.Copy(c, d[m], n);
            d[m + 1][n] = 1.0;

            void pivot(double[][] d, List<int> b_idx, List<int> n_idx, int r, int s)
            {
                var k = 1.0 / d[r][s];
                for (var i = 0; i < m + 2; i++)
                {
                    if (i == r) continue;
                    for (var j = 0; j < n + 2; j++)
                    {
                        if (j != s) d[i][j] -= d[r][j] * d[i][s] * k;
                    }
                }
                for (var i = 0; i < n + 2; i++) d[r][i] *= k;
                for (var i = 0; i < m + 2; i++) d[i][s] *= -k;
                d[r][s] = k;

                (b_idx[r], n_idx[s]) = (n_idx[s], b_idx[r]);
            }

            bool find(double[][] d, List<int> b_idx, List<int> n_idx, int p_idx)
            {
                while (true)
                {
                    var bestS = int.MaxValue;
                    var bestVal = (double.PositiveInfinity, int.MaxValue);
                    for (var i = 0; i <= n; i++)
                    {
                        if (p_idx != 0 || n_idx[i] != -1)
                        {
                            var val = d[m + p_idx][i];
                            var key = (val, n_idx[i]);
                            if (bestS == int.MaxValue || key.val < bestVal.PositiveInfinity - EPS || ((key.val - bestVal.PositiveInfinity).Abs() <= EPS && key.Item2 < bestVal.MaxValue))
                            {
                                bestS = i;
                                bestVal = key;
                            }
                        }
                    }
                    var s = bestS;

                    if (d[m + p_idx][s] > -EPS) return true;

                    var bestR = int.MaxValue;
                    var bestRKey = (double.PositiveInfinity, int.MaxValue);

                    for (var i = 0; i < m; i++)
                    {
                        if (d[i][s] > EPS)
                        {
                            var ratio = d[i][n + 1] / d[i][s];
                            var key = (ratio, b_idx[i]);
                            if (bestR == int.MaxValue || key.ratio < bestRKey.PositiveInfinity - EPS || ((key.ratio - bestRKey.PositiveInfinity).Abs() <= EPS && key.Item2 < bestRKey.MaxValue))
                            {
                                bestR = i;
                                bestRKey = key;
                            }
                        }
                    }
                    var r = bestR;

                    if (r == int.MaxValue) return false;

                    pivot(d, b_idx, n_idx, r, s);
                }
            }
            var splitR = 0;
            var minVal = d[0][n + 1];
            for (var i = 1; i < m; i++)
            {
                if (d[i][n + 1] < minVal)
                {
                    minVal = d[i][n + 1];
                    splitR = i;
                }
            }
            if (d[splitR][n + 1] < -EPS)
            {
                pivot(d, bIndices, nIndices, splitR, n);
                if (!find(d, bIndices, nIndices, 1) || d[m + 1][n + 1] < -EPS) return (double.NegativeInfinity, new List<double>());
                for(var i = 0; i < m; i++)
                {
                    if (bIndices[i] == -1)
                    {
                        var bestS = 0;
                        var bestKey = (d[i][0], nIndices[0]);

                        for(var j = 1; j < n; j++)
                        {
                            var key = (d[i][j], nIndices[j]);
                            if(key.Item1 < bestKey.Item1 - EPS || ((key.Item1 - bestKey.Item1).Abs() <= EPS && key.Item2 < bestKey.Item2))
                            {
                                bestS = j;
                                bestKey = key;
                            }
                        }
                        pivot(d, bIndices, nIndices, i, bestS);
                    }
                }
            }

            if(find(d, bIndices, nIndices, 0))
            {
                var x = new double[n];
                for(var i = 0; i < m;  i++)
                {
                    if (bIndices[i] >= 0 && bIndices[i] < n) x[bIndices[i]] = d[i][n + 1];
                }
                var sumVal = 0.0;
                for (var i = 0; i < n; i++) sumVal += c[i] * x[i];
                return (sumVal, x.ToList());
            }

            return (double.NegativeInfinity, new List<double>());
        }

        private static long SolveILPBNB(double[][] initialA, double[] objCoeffs)
        {
            var bestVal = double.PositiveInfinity;
            var stack = new Stack<double[][]>();
            stack.Push(initialA);

            while(stack.TryPop(out var curA))
            {
                var (val, xOpt) = Simplex(curA, objCoeffs);
                if (val == double.NegativeInfinity || val >= bestVal - EPS) continue;

                int? fractionalIdx = null;
                var fractionalVal = 0.0;
                
                if(xOpt.Count > 0)
                {
                    foreach(var (i, xv) in xOpt.Index())
                    {
                        if(xv - Math.Round(xv).Abs() > EPS)
                        {
                            fractionalIdx = i;
                            fractionalVal = xv;
                            break;
                        }
                    }
                    if (fractionalIdx != null)
                    {
                        var idx = fractionalIdx.Value;
                        var floorV = Math.Floor(fractionalVal);
                        var nCols = curA[0].Length;

                        var row1 = new double[nCols];
                        row1[idx] = 1.0;
                        row1[nCols - 1] = floorV;
                        var a1 = curA.ToArray();
                        var newSize = a1.Length + 1;
                        Array.Resize(ref a1, newSize);
                        a1[newSize - 1] = row1;
                        stack.Push(a1);

                        var ceilV = Math.Ceiling(fractionalVal);
                        var row2 = new double[nCols];
                        row2[idx] = -1.0;
                        row2[nCols - 1] = -ceilV;
                        var a2 = curA.ToArray();
                        var newSize2 = a2.Length + 1;
                        Array.Resize(ref a2, newSize2);
                        a2[newSize2 - 1] = row2;
                        stack.Push(a2);
                    }
                    
                    else if (val < bestVal) bestVal = val;
                }
            }
            
            if (bestVal == double.PositiveInfinity) return 0;
            else return (long)Math.Round(bestVal);
        }

        private static long SolveLP(List<long> goalCounters, List<uint> buttonMasks)
        {
            var numGoals = goalCounters.Count;
            var numButtons = buttonMasks.Count;

            var rows = 2 * numGoals + numButtons;
            var cols = numButtons + 1;

            var matrix = new double[rows][];
            for (var i = 0; i < rows; i++)
            {
                matrix[i] = new double[cols];
                for (var j = 0; j < cols; j++) matrix[i][j] = 0.0;
            }
            for (var j = 0; j < numButtons; j++)
            {
                var rowIdx = rows - 1 - j;
                matrix[rowIdx][j] = -1.0;
            }
            foreach (var (j, mask) in buttonMasks.Index())
            {
                for (var i = 0; i < numGoals; i++)
                {
                    if (((mask >> i) & 1) == 1)
                    {
                        matrix[i][j] = 1.0;
                        matrix[i + numGoals][j] = -1.0;
                    }
                }
            }
            for (var i = 0; i < numGoals; i++)
            {
                var val = (double)goalCounters[i];
                matrix[i][cols - 1] = val;
                matrix[i + numGoals][cols - 1] = -val;
            }

            var objCoeffs = Enumerable.Repeat(1.0, numButtons).ToArray();
            return SolveILPBNB(matrix, objCoeffs);
        }

        private static int BFS(List<uint> buttons, uint goalMask)
        {
            var visited = new HashSet<uint>
            {
                0
            };
            var queue = new Queue<(uint state, int steps)>();
            queue.Enqueue((0u, 0));
            while(queue.TryDequeue(out var cur))
            {
                var state = cur.state;
                var steps = cur.steps;
                if (state == goalMask) return steps;
                foreach(var button in buttons)
                {
                    var next = state ^ button;
                    
                    if(visited.Add(next)) queue.Enqueue((next, steps + 1));
                }
            }
            throw new Exception();

        }

        public static int PartOne()
        {
            Parse();
            return Machines.Select(m => BFS(m.Buttons, m.Lights)).Sum();
        }
        public static long PartTwo() => Machines.Select(m => SolveLP(m.Joltage, m.Buttons)).Sum() + 1;
    }

}
