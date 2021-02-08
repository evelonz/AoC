using FluentAssertions;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace AdventOfCode2020.Day19.Optimization
{
    public static class IndexingWithOverloads
    {
        public static int Solve(List<string> messages, List<string> rawRules)
        {
            var nodes = GetNodes(rawRules);
            var (candidates, index) = nodes[0].GetStrings(nodes, messages, 0);
            // We need a last filter here for the length of the matches
            var results = candidates.Where(x => x.Length == index).Distinct();
            return results.Count();
        }

        private static Dictionary<int, INode> GetNodes(List<string> rawRules)
        {
            var nodes = new Dictionary<int, INode>(rawRules.Count);
            foreach (var rule in rawRules)
            {
                var split = rule.Split(": ");
                var number = int.Parse(split[0]);
                nodes.Add(number, NodeFactory.GetNode(split[1]));
            }

            return nodes;
        }

        private static class NodeFactory
        {
            public static INode GetNode(string value)
            {
                if (value.StartsWith("\""))
                {
                    return new EndNode(value);
                }
                else if (value.Contains('|'))
                {
                    return new BranchNode(value);
                }
                else
                {
                    return new SimpleNode(value);
                }
            }
        }

        private interface INode
        {
            public (IEnumerable<string> candidates, int index) GetStrings(Dictionary<int, INode> nodes, IEnumerable<string> candidates, int index);
        }

        private class EndNode : INode
        {
            public char EndNodeValue { get; set; }
            public EndNode(string value) => EndNodeValue = value[1];
            public (IEnumerable<string> candidates, int index) GetStrings(Dictionary<int, INode> nodes, IEnumerable<string> candidates, int index)
                => (candidates.Where(x => index < x.Length && x[index] == EndNodeValue), index + 1);
        }

        private class SimpleNode : INode
        {
            public List<int> Rules { get; }
            public SimpleNode(string value) => Rules = value.Split(' ').Select(s => int.Parse(s)).ToList();
            public (IEnumerable<string> candidates, int index) GetStrings(Dictionary<int, INode> nodes, IEnumerable<string> candidates, int index)
            {
                int newIndex = index;
                var filteredCandidates = candidates;
                foreach (var rule in Rules)
                {
                    var node = nodes[rule];
                    (filteredCandidates, newIndex) = node.GetStrings(nodes, filteredCandidates, newIndex);
                }

                return (filteredCandidates, newIndex);
            }
        }

        private class BranchNode : INode
        {
            public List<int> RulesLeft { get; }
            public List<int> RulesRight { get; }
            public BranchNode(string value)
            {
                var branches = value.Split(" | ");
                RulesLeft = branches[0].Split(' ').Select(s => int.Parse(s)).ToList();
                RulesRight = branches[1].Split(' ').Select(s => int.Parse(s)).ToList();
            }

            public (IEnumerable<string> candidates, int index) GetStrings(Dictionary<int, INode> nodes, IEnumerable<string> candidates, int index)
            {
                var (ret2, index2) = FetchNodes(RulesLeft, nodes, candidates, index);
                var (ret3, _) = FetchNodes(RulesRight, nodes, candidates, index);
                return (ret2.Concat(ret3).ToList(), index2);
            }

            private static (IEnumerable<string> candidates, int index) FetchNodes(List<int> rules, Dictionary<int, INode> nodes, IEnumerable<string> candidates, int index)
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

    public class Test2020Day19IndexingWithOverloads
    {
        [Fact]
        public void SolveProblemInput()
        {
            var (messages, rawRules) = Base.GetData();
            var partOne = IndexingWithOverloads.Solve(messages, rawRules);
            partOne.Should().Be(239);
        }
    }
}
