﻿using System.Reflection;
using BenchmarkDotNet.Attributes;
using Microsoft.VSDiagnostics;

namespace aoc_fast.Benchmarks.Years._2021
{
    [MemoryDiagnoser]
    [CPUUsageDiagnoser]
    public class Day10
    {
        private object _dayInstance;
        private MethodInfo _partOneMethod;
        private MethodInfo _partTwoMethod;

        [GlobalSetup]
        public void Setup()
        {
            var typeName = $"aoc_fast.Years._2021.Day10";
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
                    var inputPath = projectDir + $@"\Inputs\2021\10.txt";
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
