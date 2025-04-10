using System.Reflection;
using BenchmarkDotNet.Attributes;
using Microsoft.VSDiagnostics;

namespace aoc_fast.Benchmarks.Years._2018
{
    [MemoryDiagnoser]
    [CPUUsageDiagnoser]
    public class YearBenchmark
    {
        private object _dayInstance;
        private MethodInfo _partOneMethod;
        private MethodInfo _partTwoMethod;
        private string _inputData;

        [Params(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25)]
        public int Day { get; set; }

        [GlobalSetup]
        public void Setup()
        {
            var typeName = $"aoc_fast.Years._2018.Day{Day}";
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
                    var inputPath = Path.Combine(projectDir, "Inputs", "2018", $"{Day}.txt");
                    _inputData = File.ReadAllText(inputPath);
                    inputProp.SetValue(_dayInstance, _inputData);
                }
            }
        }

        [Benchmark]
        public object BenchmarkPartOne() => _partOneMethod?.Invoke(_dayInstance, null);

        [Benchmark]
        public object BenchmarkPartTwo() => _partTwoMethod?.Invoke(_dayInstance, null);
    }
}
