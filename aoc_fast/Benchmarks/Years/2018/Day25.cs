using System.Reflection;
using BenchmarkDotNet.Attributes;
using Microsoft.VSDiagnostics;

namespace aoc_fast.Benchmarks.Years._2018
{
    [MemoryDiagnoser]
    [CPUUsageDiagnoser]
    public class Day25
    {
        private object _dayInstance;
        private MethodInfo _partOneMethod;

        [GlobalSetup]
        public void Setup()
        {
            var typeName = $"aoc_fast.Years._2018.Day25";
            var type = Type.GetType(typeName);

            if (type != null)
            {
                _dayInstance = Activator.CreateInstance(type);
                _partOneMethod = type.GetMethod("PartOne");

                var inputProp = type.GetProperty("input");
                if (inputProp != null && inputProp.CanWrite)
                {
                    var projectDir = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, @"..\..\..\.."));
                    var inputPath = projectDir + $@"\Inputs\2018\25.txt";
                    inputProp.SetValue(_dayInstance, File.ReadAllText(inputPath));
                }
            }
        }

        [Benchmark]
        public object BenchmarkPartOne() => _partOneMethod?.Invoke(_dayInstance, null);
    }
}
