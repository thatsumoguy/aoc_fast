﻿using System.Reflection;
using BenchmarkDotNet.Attributes;
using Microsoft.VSDiagnostics;

namespace aoc_fast.Benchmarks.Years._2023
{
    [MemoryDiagnoser]
    [CPUUsageDiagnoser]
    public class Day9
    {
        private object _dayInstance;
        private MethodInfo _partOneMethod;
        private MethodInfo _partTwoMethod;

        [GlobalSetup]
        public void Setup()
        {
            var typeName = $"aoc_fast.Years._2023.Day9";
            var type = Type.GetType(typeName);

            if (type != null)
            {
                _dayInstance = Activator.CreateInstance(type);
                _partOneMethod = type.GetMethod("PartOne");
                _partTwoMethod = type.GetMethod("PartTwo");

                var inputProp = type.GetProperty("input");
                if (inputProp != null && inputProp.CanWrite)
                {
                    var projectDir = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, @"..\..\..\.."));
                    var inputPath = projectDir + $@"\Inputs\2023\9.txt";
                    inputProp.SetValue(_dayInstance, File.ReadAllText(inputPath));
                }
            }
        }

        [Benchmark]
        public object BenchmarkPartOne() => _partOneMethod?.Invoke(_dayInstance, null);

        [Benchmark]
        public object BenchmarkPartTwo() => _partTwoMethod?.Invoke(_dayInstance, null);
    }
}
