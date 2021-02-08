using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace AdventOfCode2020.Day19.Optimization
{
    public static class IndexingWithLambda
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
            private readonly Func<Dictionary<int, IndexedNodes>, List<string>, int, (List<string> candidates, int index)> Method;
            public IndexedNodes(string value)
            {
                var branches = value.Split(" | ");
                if (value.StartsWith("\""))
                {
                    Method = (Dictionary<int, IndexedNodes> _, List<string> candidates, int index) => (candidates.Where(x => index < x.Length && x[index] == value[1]).ToList(), index + 1);
                }
                else if (!value.Contains('|'))
                {
                    var rules = branches[0].Split(' ').Select(s => int.Parse(s)).ToList();
                    Method = (Dictionary<int, IndexedNodes> nodes, List<string> candidates, int index) => FetchNodes(rules, nodes, candidates, index);
                }
                else
                {
                    var rulesLeft = branches[0].Split(' ').Select(s => int.Parse(s)).ToList();
                    var rulesRight = branches[1].Split(' ').Select(s => int.Parse(s)).ToList();
                    Method = (Dictionary<int, IndexedNodes> nodes, List<string> candidates, int index) =>
                    {
                        var (ret2, index2) = FetchNodes(rulesLeft, nodes, candidates, index);
                        var (ret3, _) = FetchNodes(rulesRight, nodes, candidates, index);
                        return (ret2.Concat(ret3).ToList(), index2);
                    };
                }
            }

            public (List<string> candidates, int index) GetStrings(Dictionary<int, IndexedNodes> nodes, List<string> candidates, int index)
                => Method(nodes, candidates, index);

            private static (List<string> candidates, int index) FetchNodes(List<int> rules, Dictionary<int, IndexedNodes> nodes, List<string> candidates, int index)
            {
                int newIndex = index;
                var filteredCandidates = candidates;
                foreach (var rule in rules)
                {
                    var node = nodes[rule];
                    (filteredCandidates, newIndex) = node.GetStrings(nodes, filteredCandidates, newIndex);
                }

                return (filteredCandidates, newIndex);
            }
        }
    }

    public class Test2020Day19IndexingWithLambda
    {
        [Fact]
        public void SolveProblemInput()
        {
            var (messages, rawRules) = Base.GetData();
            var partOne = IndexingWithLambda.Solve(messages, rawRules);
            partOne.Should().Be(239);
        }
    }
}
