using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using aoc_fast.Extensions;
using BenchmarkDotNet.Disassemblers;

namespace aoc_fast.Years._2025
{
    internal class Day3
    {
        public static string input
        {
            get;
            set;
        }

        private static List<byte[]> bytes = [];

        private static ulong Solve(int limit)
        {
            try
            {
                bytes = [.. input.Split("\n", StringSplitOptions.RemoveEmptyEntries).Select(Encoding.UTF8.GetBytes)];
                var ans = bytes.Select(bank =>
                {
                    var max = (byte)0;
                    var start = 0;

                    return Enumerable.Range(0, limit).Aggregate(0ul, (joltage, digit) =>
                    {
                        var end = bank.Length - limit + digit + 1;
                        (max, start) = Slice.Range(start, end).Aggregate(((byte)0, 0), (ms, i) => bank[i] > ms.Item1 ? (bank[i], i + 1) : ms);
                        return 10 * joltage + (ulong)(max - (byte)'0');
                    });
                    
                }).Sum();
                return ans;
            }
            catch (Exception ex) { Console.WriteLine(ex); }
            return 0;
        }

        public static ulong PartOne()
        {
            try
            {
                return Solve(2);
            }
            catch (Exception ex) { Console.WriteLine(ex); }
            return 0;
        }
        public static ulong PartTwo() => Solve(12);
    }
}
