using AdventOfCode2020.Utility;
using FluentAssertions;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace AdventOfCode2020.Day7
{
    internal static class Day7
    {
        internal static (int partOne, int partTwo) Solve(IInputResolver input)
        {
            var data = input.AsEnumerable();

            var rules = new Dictionary<string, List<(string, int)>>(100);

            foreach (var rule in data)
            {
                var outerBag = rule.Split("contain")[0];
                var outer = outerBag.Split(' ');
                var id = $"{outer[0]} {outer[1]}";
                var content = rule.Split("contain")[1];

                var bags = content.Split(',');
                var filledWith = new List<(string, int)>(bags.Length);
                if(bags.Length == 1 && bags[0] == " no other bags.")
                {
                    // Base bag
                }
                else
                {
                    foreach (var item in bags)
                    {
                        var words = item.Split(' ');
                        var innerBag = $"{words[2]} {words[3]}";
                        filledWith.Add((innerBag, int.Parse(words[1])));
                    }
                }
                rules.Add(id, filledWith);

            }

            int ans1 = 0;

            var roots = rules.Where(x => x.Value.Select(s => s.Item1).Contains("shiny gold"))
                .Select(s => (s.Key, s.Value)).ToList();
            var processed = new List<string>(10);

            while(roots.Count > 0)
            {
                var curr = roots.First();
                roots.RemoveAt(0);
                if (processed.Contains(curr.Key))
                    continue;

                ans1++;
                processed.Add(curr.Key);

                var nextLevel = rules
                    .Where(x => x.Value.Select(s => s.Item1).Contains(curr.Key));
                foreach (var item in nextLevel)
                {
                    roots.Add((item.Key, item.Value));
                }
            }

            // Count bags
            roots = rules.Where(x => x.Value.Count == 0)
                .Select(s => (s.Key, s.Value)).ToList();
            processed = new List<string>(10);
            var bagCounts = new Dictionary<string, int>();

            while (roots.Count > 0)
            {
                var curr = roots.First();

                if (processed.Contains(curr.Key))
                {
                    roots.RemoveAt(0);
                    continue;
                }

                bool valid = true;
                int total = 0;
                if (curr.Value.Count != 0)
                {
                    foreach (var (key, outerBagCountr) in curr.Value)
                    {
                        if (bagCounts.TryGetValue(key, out var innerBagCount))
                        {
                            total += outerBagCountr + (outerBagCountr * innerBagCount);
                        }
                        else
                        {
                            // We are missing a bag, put this back on the stack and try again.
                            valid = false;
                        }
                    }
                }

                if (valid)
                {
                    if (curr.Key == "shiny gold")
                    {
                        return (ans1, total);
                    }

                    bagCounts.Add(curr.Key, total);
                    roots.RemoveAt(0);
                    processed.Add(curr.Key);

                    var nextLevel = rules
                        .Where(x => x.Value.Select(s => s.Item1).Contains(curr.Key));
                    foreach (var item in nextLevel)
                    {
                        roots.Add((item.Key, item.Value));
                    }
                }
                else
                {
                    // move to last in list.
                    roots.RemoveAt(0);
                    roots.Add(curr);
                }

            }

            return (ans1, -1);
        }
    }

    public class Test2020Day7
    {
        [Theory]
        [MemberData(nameof(ExampleData))]
        public void FirstProblemExamples(string[] example, int expectedFirst, int expectedSecond)
        {
            var (partOne, partTwo) = Day7
                .Solve(new MockInputResolver(example));

            partOne.Should().Be(expectedFirst);
            partTwo.Should().Be(expectedSecond);
        }

        [Fact]
        public void FirstProblemInput()
        {
            var (partOne, partTwo) = Day7
                .Solve(new FileInputResolver(7));

            partOne.Should().Be(378);
            partTwo.Should().Be(27526);
        }

        public readonly static List<object[]> ExampleData = new List<object[]>
        {
            new object[] {
                new [] {
                    "light red bags contain 1 bright white bag, 2 muted yellow bags.",
                    "dark orange bags contain 3 bright white bags, 4 muted yellow bags.",
                    "bright white bags contain 1 shiny gold bag.",
                    "muted yellow bags contain 2 shiny gold bags, 9 faded blue bags.",
                    "shiny gold bags contain 1 dark olive bag, 2 vibrant plum bags.",
                    "dark olive bags contain 3 faded blue bags, 4 dotted black bags.",
                    "vibrant plum bags contain 5 faded blue bags, 6 dotted black bags.",
                    "faded blue bags contain no other bags.",
                    "dotted black bags contain no other bags.",
                }, 4, 32
            },
            new object[] {
                new [] {
                    "shiny gold bags contain 2 dark red bags.",
                    "dark red bags contain 2 dark orange bags.",
                    "dark orange bags contain 2 dark yellow bags.",
                    "dark yellow bags contain 2 dark green bags.",
                    "dark green bags contain 2 dark blue bags.",
                    "dark blue bags contain 2 dark violet bags.",
                    "dark violet bags contain no other bags.",
                }, 0, 126
            },
        };
    }
}
