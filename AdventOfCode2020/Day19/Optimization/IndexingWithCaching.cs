using FluentAssertions;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace AdventOfCode2020.Day19.Optimization
{
    public static class IndexingWithCaching
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
            private readonly int Type;
            private readonly char EndNodeValue;
            private readonly List<int>? RulesLeft;
            private readonly List<int>? RulesRight;
            public IndexedNodes(string value)
            {
                var branches = value.Split(" | ");
                if (value.StartsWith("\""))
                {
                    Type = 1;
                    EndNodeValue = value[1];
                }
                else if (!value.Contains('|'))
                {
                    Type = 2;
                    RulesLeft = branches[0].Split(' ').Select(s => int.Parse(s)).ToList();
                }
                else
                {
                    Type = 3;
                    RulesLeft = branches[0].Split(' ').Select(s => int.Parse(s)).ToList();
                    RulesRight = branches[1].Split(' ').Select(s => int.Parse(s)).ToList();
                }
            }

            public (List<string> candidates, int index) GetStrings(Dictionary<int, IndexedNodes> nodes, List<string> candidates, int index)
            {
                if (Type == 1)
                {
                    return (candidates.Where(x => index < x.Length && x[index] == EndNodeValue).ToList(), index + 1);
                }
                else if (Type == 2)
                {
                    return FetchNodes(RulesLeft!, nodes, candidates, index);
                }
                else
                {
                    var (ret2, index2) = FetchNodes(RulesLeft!, nodes, candidates, index);
                    var (ret3, _) = FetchNodes(RulesRight!, nodes, candidates, index);
                    //if (index2 != index3) throw new System.Exception("This code does not handle variable length indexes for branches");
                    return (ret2.Concat(ret3).ToList(), index2);
                }
            }

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

    public class Test2020Day19IndexingWithCaching
    {
        [Fact]
        public void SolveProblemInput()
        {
            var (messages, rawRules) = Base.GetData();
            var partOne = IndexingWithCaching.Solve(messages, rawRules);
            partOne.Should().Be(239);
        }
    }
}
