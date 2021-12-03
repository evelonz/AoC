// See https://aka.ms/new-console-template for more information
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

var _ = BenchmarkRunner.Run<MemoryBenchmarkerDemo>();


[MemoryDiagnoser]
public class MemoryBenchmarkerDemo
{
    //[Benchmark]
    //public void ToList()
    //{
    //    Day1.Solve1(new FileInputResolver(1));
    //}
    //[Benchmark]
    //public void Enumerator()
    //{
    //    Day1.Solve1(new FileInputResolver(1));
    //}
}