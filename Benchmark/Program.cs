using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using System.Collections.Generic;
using AdventOfCode2020.Day19.Optimization;

namespace Benchmark
{
    [MemoryDiagnoser]
    [RankColumn]
    public class Program
    {
        static void Main(string[] args) => BenchmarkSwitcher.FromAssemblies(new[] { typeof(Program).Assembly }).Run(args);

        public List<string> RawRules { get; set; }
        public List<string> Messages { get; set; }

        public Dictionary<int, Base.Node> Nodes { get; set; }

        [GlobalSetup]
        public void Setup()
        {
            (Messages, RawRules) = Base.GetData();
            Nodes = Base.GetNodes(RawRules);
        }

        //[Benchmark(Baseline = true)]
        //public int BaseLine() => Base.BaseSolution(Messages, RawRules);

        //[Benchmark]
        //public int SendingInCachedNodes() => Base.SendingInNodes(Messages, Nodes);

        //[Benchmark]
        //public int BaseWithIntersect() => Base.ChangeEnumerationOfResult(Messages, RawRules);

        [Benchmark(Baseline = true)]
        public int UsingIndex() => Indexing.Solve(Messages, RawRules);

        [Benchmark]
        public int IndexWithSomeCaching() => IndexingWithCaching.Solve(Messages, RawRules);

        [Benchmark]
        public int IndexingWithLambdas() => IndexingWithLambda.Solve(Messages, RawRules);

        [Benchmark]
        public int IndexWithOverloads() => IndexingWithOverloads.Solve(Messages, RawRules);
    }
}
