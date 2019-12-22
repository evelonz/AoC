using AdventOfCode.Utility;
using FluentAssertions;
using System.Collections.Generic;
using Xunit;

namespace AdventOfCode.Year2019.Tests
{
    public class Tester2019_22
    {
        [Theory]
        [MemberData(nameof(FirstExampleData))]
        public void FirstProblemExamples(string[] example, string expected, int size, int? pos)
        {
            Solver2019_22.SolveFirst(new MockInputResolver(example), size, pos).Should().Be(expected);
        }

        [Fact]
        public void FirstProblemMyInput()
        {
            Solver2019_22
                .SolveFirst(new FileInputResolver(2019, 22), 10_007, 2019)
                .Should().Be("2519");
        }

        [Fact]
        public void SecondProblemMyInput()
        {
            //Solver2019_22.SolveSecond(new FileInputResolver(2019, 22)).Should().Be("1141251258");
        }

        public static IEnumerable<object[]> FirstExampleData()
        {
            return allData;
        }

        private readonly static List<object[]> allData = new List<object[]>
        {
            new object[] {
                new [] {
                    "deal into new stack" }, "9 8 7 6 5 4 3 2 1 0 ", 10, null
            },
            new object[] {
                new [] {
                    "cut 3" }, "3 4 5 6 7 8 9 0 1 2 ", 10, null
            },
            new object[] {
                new [] {
                    "cut -4" }, "6 7 8 9 0 1 2 3 4 5 ", 10, null
            },
            new object[] {
                new [] {
                    "deal with increment 3" }, "0 7 4 1 8 5 2 9 6 3 ", 10, null
            },
            new object[] {
                new [] {
                    "deal with increment 7",
                    "deal into new stack",
                    "deal into new stack"
                },
                "0 3 6 9 2 5 8 1 4 7 ", 10, null
            },
            new object[] {
                new [] {
                    "cut 6",
                    "deal with increment 7",
                    "deal into new stack"
                },
                "3 0 7 4 1 8 5 2 9 6 ", 10, null
            },
            new object[] {
                new [] {
                    "deal with increment 7",
                    "deal with increment 9",
                    "cut -2"
                },
                "6 3 0 7 4 1 8 5 2 9 ", 10, null
            },
            new object[] {
                new [] {
                    "deal into new stack",
                    "cut -2",
                    "deal with increment 7",
                    "cut 8",
                    "cut -4",
                    "deal with increment 7",
                    "cut 3",
                    "deal with increment 9",
                    "deal with increment 3",
                    "cut -1",
                },
                "9 2 5 8 1 4 7 0 3 6 ", 10, null
            },
        };
    }
}
