using FluentAssertions;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace AdventOfCode2020.Day19.Optimization
{
    public static class Indexing
    {
        public static int Solve(List<string> messages, List<string> rawRules)
        {
            var nodes = GetNodes(rawRules);
            var (candidates, index) = nodes[0].GetStrings(nodes, messages, 0);
            // We need a last filter here for the length of the matches
            var results = candidates.Where(x => x.Length == index).Distinct();
            return results.Count();
        }

        private static Dictionary<int, IndexedNodes> GetNodes(List<string> rawRules)
        {
            var nodes = new Dictionary<int, IndexedNodes>(rawRules.Count);
            foreach (var rule in rawRules)
            {
                var number = int.Parse(rule.Split(": ")[0]);
                nodes.Add(number, new IndexedNodes(rule.Split(": ")[1]));
            }

            return nodes;
        }

        private class IndexedNodes
        {
            public string RawString { get; }
            public IndexedNodes(string value) => RawString = value;

            public (List<string> candidates, int index) GetStrings(Dictionary<int, IndexedNodes> nodes, List<string> candidates, int index)
            {
                if(candidates.Count == 0)
                    return (new List<string>(), index);

                if (RawString.StartsWith("\"")) // TODO: improve, we revisit this node multiple times, no need to check it each time.
                {
                    var ret = (candidates.Where(x => index < x.Length && x[index] == RawString[1]).ToList(), index + 1);
                    if (ret.Item1.Count == 0)
                    { }
                    return ret;
                }
                else if (!RawString.Contains('|'))
                {
                    return FetchNodes(RawString, nodes, candidates, index);
                }
                else
                {
                    var left = RawString.Split(" | ")[0];
                    var right = RawString.Split(" | ")[1];
                    var (ret2, index2) = FetchNodes(left, nodes, candidates, index);
                    var (ret3, index3) = FetchNodes(right, nodes, candidates, index);
                    //if (index2 != index3) throw new System.Exception("This code does not handle variable length indexes for branches");
                    return (ret2.Concat(ret3).ToList(), index2);
                }
            }

            private static (List<string> candidates, int index) FetchNodes(string nodeString, Dictionary<int, IndexedNodes> nodes, List<string> candidates, int index)
            {
                int newIndex = index;
                var filteredCandidates = candidates;
                foreach (var word in nodeString.Split(' '))
                {
                    var node = nodes[int.Parse(word)];
                    (filteredCandidates, newIndex) = node.GetStrings(nodes, filteredCandidates, newIndex);
                    if (filteredCandidates.Count == 0)
                    { }
                }

                return (filteredCandidates, newIndex);
            }
        }
    }

    public class Test2020Day19Indexing
    {
        [Fact]
        public void SolveProblemInput()
        {
            var (messages, rawRules) = Base.GetData();
            var partOne = Indexing.Solve(messages, rawRules);
            partOne.Should().Be(239);
        }
    }
}
