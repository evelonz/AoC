using AdventOfCode2020.Utility;
using FluentAssertions;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace AdventOfCode2020.Day19
{
    internal static class Day19
    {
        internal static (long partOne, long partTwo) Solve(IInputResolver input, bool skipPartTwo = false)
        {
            var data = input.AsEnumerable();
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
            // So we want to match strings against rule 0.
            // We can either create all allowed strings (permutations), 
            // if the space is smalla enough to allow for it!
            // Regarding iterating from the start, we cannot match any char (a, b) against any rule.
            // So that seems futile.
            // If the search space is to large, perhaps we can start creating the patterns from the end
            // and move forward? 
            // Can we assume that all rules have matches, or do we want to start from 0 to prune dead ends?
            // We perhaps want to start from 0 regardless, since we want the path in order.
            // There are at most 2 branches.
            var nodes = new Dictionary<int, Node>(rawRules.Count);
            foreach (var rule in rawRules)
            {
                var number = int.Parse(rule.Split(": ")[0]);
                nodes.Add(number, new Node(number, rule.Split(": ")[1]));
            }

            int ans1 = SolvePartOne(messages, rawRules);
            if(skipPartTwo)
                return (ans1, 0);

            var ans2 = SolvePartTwo(messages, rawRules);
            return (ans1, ans2);
        }

        private static int SolvePartOne(List<string> messages, List<string> rawRules)
        {
            var nodes = new Dictionary<int, Node>(rawRules.Count);
            foreach (var rule in rawRules)
            {
                var number = int.Parse(rule.Split(": ")[0]);
                nodes.Add(number, new Node(number, rule.Split(": ")[1]));
            }

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

        private static int SolvePartTwo(List<string> messages, List<string> rawRules)
        {
            var nodes = new Dictionary<int, Node>(rawRules.Count);
            foreach (var rule in rawRules)
            {
                var number = int.Parse(rule.Split(": ")[0]);
                nodes.Add(number, new Node(number, rule.Split(": ")[1], true));
            }

            // rule31 and rule42 holds 128 distinct values each in 8 characters.
            // Concatunating them seem to give all permutations of 8 characters.
            // 8: 1 -> 42
            //    2 -> 42+8
            // 11: 1 -> 42+31
            //     2 -> 42+11+31
            // Longest message seem to be 88 characters. So should not be more then 11 permutations, even discounting the original rules length.
            // We can also check, if the original message starts with any permutation of 42 (50% chance),
            // And if mathcing 11, ends with any of the 31 permutations.
            // Perhaps there is a pattern to 31, and 42? Like them starting with different letters? Cannot find a simple pattern.

            var rule31 = nodes[31].GetStrings(nodes);
            var rule42 = nodes[42].GetStrings(nodes);
            var rule31Length = rule31.Select(s => s.Length).Distinct().Single();
            var rule42Length = rule42.Select(s => s.Length).Distinct().Single();

            var allRules = nodes[0].GetStrings(nodes);

            // Notes, since we know that it has to start with 42, and end with 31, we should perhaps filter on that first?

            int ans2 = 0;
            foreach (var message in messages)
            {
                foreach (var rule in allRules) // only one rule in my task.
                {
                    // So here we need to check for 8 or 11.
                    // If they are found, we have to replace them, each have 2 values, which give at most 4 branches.
                    // One with values, one with 8 in it, one with 11, and one with both.
                    // 8 and 11 expand to multiple vales,
                    // However, those values are fixed (thou recursive). We can perhaps prune the checks still.
                    // Partly by checking the message length. The 31 and 42 rule lists seem to be of a fixed length, so we know the length of a complete message.
                    // Partly by checking if the next part of the message matches any of the known values. If not, then we can prune that branch entierly.
                    // Have to check when we can do this kind of pruning thou, it may be as expensive as checking the complete message if it is a valid branch!
                    // For the one case where we have no unknowns, we just check the messages.
                    // SB => abB => abba
                    // Both start with rule 42, and one end with 31. So we should only be able to match strings with that structure.
                    // 81
                    // Actual
                    // ab
                    // a8b
                    // 8b1
                    // a8b1

                    // Current where only 8 or 11 is parsed.
                    // a1
                    // a81
                    // 8b
                    // 8b1
                    var rules = new Queue<string>();
                    rules.Enqueue(rule);
                    while(rules.Count > 0)
                    {
                        var nRule = rules.Dequeue();
                        if (!nRule.Contains("8") && !nRule.Contains("12") && nRule == message)
                        {
                            ans2++;
                            break;
                        }
                        // We need to get rid of the 8 as, it is always first (at least in my input)
                        // We only ever have one 8.
                        if (nRule.Contains("8"))
                        {
                            HandleEights(rule42, rule42Length, message, rules, nRule);
                        }
                        if (nRule.Contains("12"))
                        {
                            HandleElevens(rule42, rule31, rule42Length, rule31Length, message, rules, nRule);
                        }
                    }
                }
            }

            return ans2;
        }

        public static void HandleEights(List<string> rule42, int rule42Length, string message, Queue<string> rules, string nRule)
        {
            if (nRule.Length + rule42Length - 1 <= message.Length) // -1 since we replace one char.
            {
                // The length is short enough, we can replace the 8 with messages and queue them.
                // We may still want to check that if length is equal, and there is an 11, we should still skip.

                // Should we clean missmatches here? Else the queue may grow very long.
                var add1Again = nRule.Length + (2 * rule42Length) - 2 <= message.Length;
                var index = nRule.IndexOf("8") + rule42Length;
                var submessage = message[..index];
                foreach (var rule8s in rule42)
                {
                    var n2Rule = nRule.Replace("8", rule8s);
                    if (n2Rule.StartsWith(submessage))
                    {
                        // Left
                        // TODO: We could check length here as well, but then we have to check for 11 as well. May just pass it on perhaps.
                        rules.Enqueue(n2Rule);
                        // Right
                        if (add1Again)
                            rules.Enqueue(nRule.Replace("8", rule8s + "8"));
                    }
                }
            }
        }

        public static void HandleElevens(List<string> rule42, List<string> rule31, int rule42Length, int rule31Length, string message, Queue<string> rules, string nRule)
        {
            if (nRule.Length + rule42Length + rule31Length - 2 <= message.Length) // -2 since we replace two char.
            {
                // The length is short enough, we can replace the 8 with messages and queue them.
                // We may still want to check that if length is equal, and there is an 11, we should still skip.

                // Should we clean missmatches here? Else the queue may grow very long.
                var add11Again = nRule.Length + (2 * rule42Length) + (2 * rule31Length) - 4 <= message.Length;
                var index = nRule.IndexOf("1") + rule42Length;
                var submessage = message[..index];
                foreach (var rule42s in rule42)
                {
                    var n2Rule = nRule.Replace("1", rule42s);
                    if (n2Rule.StartsWith(submessage))
                    {
                        foreach (var rule31s in rule31)
                        {
                            // Check end, but only for left? No, should be in both cases, as we only check from end to Symbol.
                            var index2 = n2Rule.IndexOf("2");
                            var temp1 = n2Rule.Length - (index2 + 1);
                            var temp2 = temp1 + rule31Length;
                            var submessage2 = message[^temp2..];
                            var n3Rule = n2Rule.Replace("2", rule31s);
                            if (n3Rule.EndsWith(submessage2))
                            {
                                // Left
                                rules.Enqueue(n3Rule);
                                // Right
                                if (add11Again)
                                    rules.Enqueue(nRule.Replace("12", rule42s + "12" + rule31s));
                            }
                        }
                    }
                }
            }
        }

        internal class Node
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

            private List<string> Cache { get; set; }

            public List<string> GetStrings(Dictionary<int, Day19.Node> nodes)
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

    public class Test2020Day19
    {
        [Theory]
        [MemberData(nameof(ExampleData))]
        public void SolveProblemExamplesOne(string[] example, int expectedFirst)
        {
            var (partOne, _) = Day19
                .Solve(new MockInputResolver(example), true);
            partOne.Should().Be(expectedFirst);
        }

        [Theory]
        [MemberData(nameof(ExampleData2))]
        public void SolveProblemExamplesTwo(string[] example, int expectedFirst, int expectedSecond)
        {
            var (partOne, partTwo) = Day19
                .Solve(new MockInputResolver(example));
            partOne.Should().Be(expectedFirst);
            partTwo.Should().Be(expectedSecond);
        }

        [Fact]
        public void ParseEndNode()
        {
            var sut = new Day19.Node(0, "\"b\"");
            sut.GetStrings(null).Single().Should().Be("b");
        }

        [Fact]
        public void ParseSimpleNode()
        {
            var endNode = new Day19.Node(4, "\"b\"");
            var nodes = new Dictionary<int, Day19.Node>(1)
            {
                { 4, endNode }
            };
            var sut = new Day19.Node(0, "4 4");
            sut.GetStrings(nodes).Single().Should().Be("bb");
        }

        [Fact]
        public void ParseSplitNode()
        {
            var endNode = new Day19.Node(4, "\"b\"");
            var nodes = new Dictionary<int, Day19.Node>(1)
            {
                { 4, endNode }
            };
            var sut = new Day19.Node(0, "4 | 4");
            sut.GetStrings(nodes).First().Should().Be("b");
            sut.GetStrings(nodes).Last().Should().Be("b");
        }

        [Fact]
        public void ParseSimpleSplitNode()
        {
            var endNode = new Day19.Node(4, "\"b\"");
            var endNode2 = new Day19.Node(5, "\"a\"");
            var splitNode = new Day19.Node(2, "4 | 5");
            var nodes = new Dictionary<int, Day19.Node>(1)
            {
                { 4, endNode },
                { 5, endNode2 },
                { 2, splitNode },
            };
            var sut = new Day19.Node(0, "2 4");
            sut.GetStrings(nodes).First().Should().Be("bb");
            sut.GetStrings(nodes).Last().Should().Be("ab");
        }

        [Fact]
        public void ParseNestedSplitNode()
        {
            var endNode = new Day19.Node(4, "\"b\"");
            var endNode2 = new Day19.Node(5, "\"a\"");
            var splitNode = new Day19.Node(2, "4 | 5");
            var nodes = new Dictionary<int, Day19.Node>(1)
            {
                { 4, endNode },
                { 5, endNode2 },
                { 2, splitNode },
            };
            var sut = new Day19.Node(0, "2 4 | 5 2");
            sut.GetStrings(nodes)[0].Should().Be("bb");
            sut.GetStrings(nodes)[1].Should().Be("ab");
            sut.GetStrings(nodes)[2].Should().Be("ab");
            sut.GetStrings(nodes)[3].Should().Be("aa");
        }

        // Part two test
        [Fact]
        public void Replace8s()
        {
            var rule42s = new List<string>(){ "abc" };
            var queue = new Queue<string>();

            Day19.HandleEights(rule42s, 3, "abc", queue, "8");
            var res = queue.Dequeue();

            res.Should().Be("abc");
            queue.Count.Should().Be(0);
        }

        [Theory]
        [InlineData("ab")]
        [InlineData("cba")]
        public void Replace8sWrongOrShortmessage(string message)
        {
            var rule42s = new List<string>() { "abc" };
            var queue = new Queue<string>();

            Day19.HandleEights(rule42s, 3, message, queue, "8");
            queue.Count.Should().Be(0);
        }

        [Fact]
        public void Replace8sGetLeftAndRight()
        {
            var rule42s = new List<string>() { "abc" };
            var queue = new Queue<string>();

            Day19.HandleEights(rule42s, 3, "abcabc", queue, "8");
            var res = queue.Dequeue();
            res.Should().Be("abc");
            res = queue.Dequeue();
            res.Should().Be("abc8");
        }

        [Fact]
        public void Replace11s()
        {
            var rule42s = new List<string>() { "abc" };
            var rule31s = new List<string>() { "def" };
            var queue = new Queue<string>();

            Day19.HandleElevens(rule42s, rule31s, 3, 3, "abcdef", queue, "12");
            var res = queue.Dequeue();

            res.Should().Be("abcdef");
            queue.Count.Should().Be(0);
        }

        [Theory]
        //[InlineData("abcde")]
        //[InlineData("bacdef")]
        [InlineData("abcdeq")]
        public void Replace11sWrongOrShortmessage(string message)
        {
            var rule42s = new List<string>() { "abc" };
            var rule31s = new List<string>() { "def" };
            var queue = new Queue<string>();

            Day19.HandleElevens(rule42s, rule31s, 3, 3, message, queue, "12");
            queue.Count.Should().Be(0);
        }

        [Fact]
        public void Replace11sGetLeftAndRight()
        {
            var rule42s = new List<string>() { "abc" };
            var rule31s = new List<string>() { "def" };
            var queue = new Queue<string>();

            Day19.HandleElevens(rule42s, rule31s, 3, 3, "abcabcdefdef", queue, "12");
            var res = queue.Dequeue();
            res.Should().Be("abcdef");
            res = queue.Dequeue();
            res.Should().Be("abc12def");
        }

        [Fact]
        public void SolveProblemInput()
        {
            var (partOne, partTwo) = Day19
                .Solve(new FileInputResolver(19));
            partOne.Should().Be(239);
            partTwo.Should().Be(405);
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
                }, 3, 12
            }
        };
    }
}
