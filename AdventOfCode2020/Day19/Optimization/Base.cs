using AdventOfCode2020.Utility;
using FluentAssertions;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace AdventOfCode2020.Day19.Optimization
{
    public static class Base
    {
        public static (List<string> messages, List<string> rawRules) GetData() => GetData(new FileInputResolver(19).AsEnumerable());

        internal static (List<string> messages, List<string> rawRules) GetData(IEnumerable<string> data)
        {
            var rawRules = new List<string>();
            var messages = new List<string>();
            bool rules = true;
            foreach (var row in data)
            {
                if (string.IsNullOrEmpty(row))
                {
                    rules = false;
                    continue;
                }
                if (rules)
                    rawRules.Add(row);
                else
                    messages.Add(row);
            }
            return (messages, rawRules);
        }

        public static Dictionary<int, Node> GetNodes(List<string> rawRules)
        {
            var nodes = new Dictionary<int, Node>(rawRules.Count);
            foreach (var rule in rawRules)
            {
                var number = int.Parse(rule.Split(": ")[0]);
                nodes.Add(number, new Node(number, rule.Split(": ")[1]));
            }

            return nodes;
        }

        public static int BaseSolution(List<string> messages, List<string> rawRules)
        {
            var nodes = new Dictionary<int, Node>(rawRules.Count);
            foreach (var rule in rawRules)
            {
                var number = int.Parse(rule.Split(": ")[0]);
                nodes.Add(number, new Node(number, rule.Split(": ")[1]));
            }

            var allRules = nodes[0].GetStrings(nodes);
            var count = allRules.Count;
            var dist = allRules.Distinct().Count();

            int ans1 = 0;
            foreach (var message in messages)
            {
                foreach (var rule in allRules)
                {
                    if (rule == message)
                    {
                        ans1++;
                        break;
                    }
                }
            }

            return ans1;
        }

        public static int SendingInNodes(List<string> messages, Dictionary<int, Node> nodes)
        {
            var allRules = nodes[0].GetStrings(nodes);

            int ans1 = 0;
            foreach (var message in messages)
            {
                foreach (var rule in allRules)
                {
                    if (rule == message)
                    {
                        ans1++;
                        break;
                    }
                }
            }

            return ans1;
        }

        public static int ChangeEnumerationOfResult(List<string> messages, List<string> rawRules)
        {
            var nodes = new Dictionary<int, Node>(rawRules.Count);
            foreach (var rule in rawRules)
            {
                var number = int.Parse(rule.Split(": ")[0]);
                nodes.Add(number, new Node(number, rule.Split(": ")[1]));
            }

            var allRules = nodes[0].GetStrings(nodes);
            var matches = messages.Intersect(allRules);

            return matches.Count();
        }

        public class Node
        {
            public string RawString { get; }
            public int RuleNumber { get; }
            public bool PartTwo { get; }
            public Node(int ruleNumber, string value, bool partTwo = false)
            {
                RawString = value;
                RuleNumber = ruleNumber;
                PartTwo = partTwo;
            }

            private List<string>? Cache { get; set; }

            public List<string> GetStrings(Dictionary<int, Node> nodes)
            {
                if (Cache is not null)
                    return Cache;

                if (PartTwo && RuleNumber == 8)
                {
                    Cache = new List<string> { "8" };
                }
                else if(PartTwo && RuleNumber == 11)
                {
                    Cache = new List<string> { "12" }; // We use 12 and not 11, so we can replace each of them separately with one rule match.
                }
                else if (RawString.StartsWith("\""))
                {
                    Cache = new List<string> { RawString.Substring(1, 1) };
                }
                else if(!RawString.Contains('|'))
                {
                    Cache = FetchNodes(RawString, nodes);
                }
                else
                {
                    var left = RawString.Split(" | ")[0];
                    var right = RawString.Split(" | ")[1];
                    var ret2 = FetchNodes(left, nodes);
                    var ret3 = FetchNodes(right, nodes);
                    Cache = ret2.Concat(ret3).ToList();
                }

                return Cache;
            }

            private static List<string> FetchNodes(string nodeString, Dictionary<int, Node> nodes)
            {
                List<string> returnStrings = new List<string>();
                foreach (var word in nodeString.Split(' '))
                {
                    var n = nodes[int.Parse(word)];
                    var nodeStrings = n.GetStrings(nodes);
                    if (returnStrings.Count == 0)
                        returnStrings = nodeStrings;
                    else
                    {
                        List<string> newStrings = new List<string>();
                        foreach (var baseString in returnStrings)
                        {
                            foreach (var extString in nodeStrings)
                            {
                                newStrings.Add(baseString + extString);
                            }
                        }
                        returnStrings = newStrings;
                    }
                }
                return returnStrings;
            }
        }
    }

    public class Test2020Day19Base
    {
        [Theory]
        [MemberData(nameof(ExampleData))]
        public void SolveProblemExamplesOne(string[] example, int expectedFirst)
        {
            var (messages, rawRules) = Base.GetData(new MockInputResolver(example).AsEnumerable());
            var partOne = Base.BaseSolution(messages, rawRules);
            partOne.Should().Be(expectedFirst);
        }

        [Theory]
        [MemberData(nameof(ExampleData2))]
        public void SolveProblemExamplesTwo(string[] example, int expectedFirst)
        {
            var (messages, rawRules) = Base.GetData(new MockInputResolver(example).AsEnumerable());
            var partOne = Base.BaseSolution(messages, rawRules);
            partOne.Should().Be(expectedFirst);
        }

        [Fact]
        public void SolveProblemInput()
        {
            var (messages, rawRules) = Base.GetData();
            var partOne = Base.BaseSolution(messages, rawRules);
            partOne.Should().Be(239);
        }

        [Fact]
        public void SolveProblemSendingInNodes()
        {
            var (messages, rawRules) = Base.GetData();
            var nodes = Base.GetNodes(rawRules);
            var partOne = Base.SendingInNodes(messages, nodes);
            partOne.Should().Be(239);
        }

        [Fact]
        public void SolveProblemWithNewEnumeration()
        {
            var (messages, rawRules) = Base.GetData();
            var partOne = Base.ChangeEnumerationOfResult(messages, rawRules);
            partOne.Should().Be(239);
        }

        public readonly static List<object[]> ExampleData = new List<object[]>
        {
            new object[] {
                new [] {
                    "0: 4 1 5",
                    "1: 2 3 | 3 2",
                    "2: 4 4 | 5 5",
                    "3: 4 5 | 5 4",
                    "4: \"a\"",
                    "5: \"b\"",
                    "",
                    "ababbb",
                    "bababa",
                    "abbbab",
                    "aaabbb",
                    "aaaabbb",
                }, 2
            }
        };

        public readonly static List<object[]> ExampleData2 = new List<object[]>
        {
            new object[] {
                new [] {
                    "42: 9 14 | 10 1",
                    "9: 14 27 | 1 26",
                    "10: 23 14 | 28 1",
                    "1: \"a\"",
                    "11: 42 31",
                    "5: 1 14 | 15 1",
                    "19: 14 1 | 14 14",
                    "12: 24 14 | 19 1",
                    "16: 15 1 | 14 14",
                    "31: 14 17 | 1 13",
                    "6: 14 14 | 1 14",
                    "2: 1 24 | 14 4",
                    "0: 8 11",
                    "13: 14 3 | 1 12",
                    "15: 1 | 14",
                    "17: 14 2 | 1 7",
                    "23: 25 1 | 22 14",
                    "28: 16 1",
                    "4: 1 1",
                    "20: 14 14 | 1 15",
                    "3: 5 14 | 16 1",
                    "27: 1 6 | 14 18",
                    "14: \"b\"",
                    "21: 14 1 | 1 14",
                    "25: 1 1 | 1 14",
                    "22: 14 14",
                    "8: 42",
                    "26: 14 22 | 1 20",
                    "18: 15 15",
                    "7: 14 5 | 1 21",
                    "24: 14 1",
                    "",
                    "abbbbbabbbaaaababbaabbbbabababbbabbbbbbabaaaa",
                    "bbabbbbaabaabba",
                    "babbbbaabbbbbabbbbbbaabaaabaaa",
                    "aaabbbbbbaaaabaababaabababbabaaabbababababaaa",
                    "bbbbbbbaaaabbbbaaabbabaaa",
                    "bbbababbbbaaaaaaaabbababaaababaabab",
                    "ababaaaaaabaaab",
                    "ababaaaaabbbaba",
                    "baabbaaaabbaaaababbaababb",
                    "abbbbabbbbaaaababbbbbbaaaababb",
                    "aaaaabbaabaaaaababaa",
                    "aaaabbaaaabbaaa",
                    "aaaabbaabbaaaaaaabbbabbbaaabbaabaaa",
                    "babaaabbbaaabaababbaabababaaab",
                    "aabbbbbaabbbaaaaaabbbbbababaaaaabbaaabba",
                }, 3
            }
        };
    }
}
