using AdventOfCode2020.Utility;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace AdventOfCode2020.Day16
{
    internal static class Day16
    {
        internal static (long partOne, long partTwo) Solve(IInputResolver input)
        {
            var data = input.AsEnumerable();

            var enumerator = data.GetEnumerator();
            // Get rules
            var rules = new Dictionary<string, (int sOne, int eOne, int sTwo, int eTwo)>(100);

            enumerator.MoveNext();
            var val = enumerator.Current;
            var hasMore = true;
            while (val.Length != 0 && hasMore)
            {
                // Get rule:
                var name = val.Split(':')[0];
                var words = val.Split(':')[1].Split(' ');
                var rule1 = words[1].Split('-');
                var rule2 = words[3].Split('-');
                rules.Add(name, (int.Parse(rule1[0]), int.Parse(rule1[1]), int.Parse(rule2[0]), int.Parse(rule2[1])));

                hasMore = enumerator.MoveNext();
                if (hasMore)
                    val = enumerator.Current;
            }

            // Your ticket
            hasMore = enumerator.MoveNext();
            hasMore = enumerator.MoveNext();
            val = enumerator.Current;
            int[] yourTicket = val.Split(',').Select(s => int.Parse(s)).ToArray();

            // Other tickets
            var nearbyTickets = new List<int[]>(100);
            hasMore = enumerator.MoveNext();
            hasMore = enumerator.MoveNext();
            hasMore = enumerator.MoveNext();
            val = enumerator.Current;
            while (val.Length != 0 && hasMore)
            {
                var numbers = val.Split(',').Select(s => int.Parse(s)).ToArray();
                nearbyTickets.Add(numbers);

                hasMore = enumerator.MoveNext();
                if (hasMore)
                    val = enumerator.Current;
            }

            var invalidValues = new List<int>(100);
            var validTickets = new List<int[]>(100);
            foreach (var ticket in nearbyTickets)
            {
                var fullyValid = true;
                foreach (var value in ticket)
                {
                    var valid = false;
                    foreach (var rule in rules)
                    {
                        if ((rule.Value.sOne <= value && rule.Value.eOne >= value)
                            || (rule.Value.sTwo <= value && rule.Value.eTwo >= value))
                        {
                            valid = true;
                            break;
                        }
                    }
                    if(!valid)
                    {
                        invalidValues.Add(value);
                        fullyValid = false;
                    }
                }
                if(fullyValid)
                {
                    validTickets.Add(ticket);
                }
            }
            var ans1 = invalidValues.Sum();

            // Part Two
            var fieldPositions = new Dictionary<string, int>(100);
            var checks = new Dictionary<(string ruleName, int Pos), bool>();
            foreach (var rule in rules)
            {
                for (int pos = 0; pos < rules.Count; pos++)
                {
                    checks.Add((rule.Key, pos), true); // assume true, set to false if any is false.
                    foreach (var ticket in validTickets)
                    {
                        var value = ticket[pos];
                        var valid = (rule.Value.sOne <= value && rule.Value.eOne >= value)
                                || (rule.Value.sTwo <= value && rule.Value.eTwo >= value);
                        if (!valid)
                            checks[(rule.Key, pos)] = false;
                    }
                }
            }

            // Now check valid positions.
            while(fieldPositions.Count < rules.Count)
            {
                // Check rules
                foreach (var rule in rules.Where(x => !fieldPositions.Keys.Contains(x.Key)))
                {
                    var validValues = checks.Where(x => x.Key.ruleName == rule.Key && x.Value);
                    if(validValues.Count() == 1)
                    {
                        var pos = validValues.First().Key.Pos;
                        // Remove from other locations.
                        checks = checks.Where(x => x.Key.ruleName != rule.Key && x.Key.Pos != pos).ToDictionary(d => d.Key, d => d.Value);
                        fieldPositions.Add(rule.Key, pos); // May break the iteration below.
                    }
                }

                // Check positions
                for (int pos = 0; pos < rules.Count; pos++)
                {
                    var validValues = checks.Where(x => x.Key.Pos == pos && x.Value);
                    if (validValues.Count() == 1)
                    {
                        var rule = validValues.First().Key.ruleName;
                        // Remove from other locations.
                        checks = checks.Where(x => x.Key.Pos != pos && x.Key.ruleName != rule).ToDictionary(d => d.Key, d => d.Value);
                        fieldPositions.Add(rule, pos); // May break the iteration below.
                    }
                }
            }

            long ans2 = 1;
            foreach (var item in fieldPositions.Where(x => x.Key.StartsWith("departure")))
            {
                ans2 *= yourTicket[item.Value];
            }

            return (ans1, ans2);
        }
    }

    public class Test2020Day16
    {
        [Theory]
        [MemberData(nameof(ExampleData))]
        public void SolveProblemExamples(string[] example, int expectedFirst)
        {
            var (partOne, _) = Day16
                .Solve(new MockInputResolver(example));
            partOne.Should().Be(expectedFirst);
        }

        [Theory]
        [MemberData(nameof(ExampleData2))]
        public void SolveProblemExamples2(string[] example, int expectedFirst, int expectedSecond)
        {
            var (partOne, partTwo) = Day16
                .Solve(new MockInputResolver(example));
            partOne.Should().Be(expectedFirst);
            partTwo.Should().Be(expectedSecond);
        }

        [Fact]
        public void SolveProblemInput()
        {
            var (partOne, partTwo) = Day16
                .Solve(new FileInputResolver(16));
            partOne.Should().Be(22057);
            partTwo.Should().Be(1093427331937);
        }

        public readonly static List<object[]> ExampleData = new List<object[]>
        {
            new object[] {
                new [] {
                    "class: 1-3 or 5-7",
                    "row: 6-11 or 33-44",
                    "seat: 13-40 or 45-50",
                    "",
                    "your ticket:",
                    "7,1,14",
                    "",
                    "nearby tickets:",
                    "7,3,47",
                    "40,4,50",
                    "55,2,20",
                    "38,6,12",
                }, 71
            }
        };

        public readonly static List<object[]> ExampleData2 = new List<object[]>
        {
            new object[] {
                new [] {
                    "class: 0-1 or 4-19",
                    "departure row: 0-5 or 8-19",
                    "departure seat: 0-13 or 16-19",
                    "",
                    "your ticket:",
                    "11,12,13",
                    "",
                    "nearby tickets:",
                    "3,9,18",
                    "15,1,5",
                    "5,14,9",
                }, 0, 143
            }
        };
    }
}
