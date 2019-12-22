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
        public void FirstProblemMyInputWithIndex()
        {
            Solver2019_22
                .SolveFirstIndex(new FileInputResolver(2019, 22), 10_007, 2019)
                .Should().Be("2519");
        }

        [Theory]
        [MemberData(nameof(FirstExampleData))]
        public void SecondProblemExamples(string[] example, string expected, int size, int? pos)
        {
            Solver2019_22_2.Solve(new MockInputResolver(example), size, pos).Should().Be(expected);
        }

        [Fact]
        public void ManTest()
        {
            var data = new string[] { "deal with increment 3" };
            var res = Solver2019_22_2.Solve(new MockInputResolver(data), 13);
        }

        [Fact]
        public void SecondProblemMyInput()
        {
            Solver2019_22.SolveReverse(new FileInputResolver(2019, 22), 10_007, 2519)
                .Should().Be("2019");
            //Solver2019_22.SolveSecond(new FileInputResolver(2019, 22)).Should().Be("1141251258");
        }

        [Fact]
        public void DealIntoNewDeckIndex()
        {
            Solver2019_22.DealIntoNewDeckIndex(0, 10).Should().Be(9);
            Solver2019_22.DealIntoNewDeckIndex(9, 10).Should().Be(0);
            Solver2019_22.DealIntoNewDeckIndex(2, 10).Should().Be(7);
            Solver2019_22.DealIntoNewDeckIndex(5, 10).Should().Be(4);
        }

        [Fact]
        public void DealIntoNewDeckIndexRev()
        {
            Solver2019_22.DealIntoNewDeckIndexRev(0, 10).Should().Be(9);
            Solver2019_22.DealIntoNewDeckIndexRev(9, 10).Should().Be(0);
            Solver2019_22.DealIntoNewDeckIndexRev(2, 10).Should().Be(7);
            Solver2019_22.DealIntoNewDeckIndexRev(5, 10).Should().Be(4);
        }

        [Fact]
        public void CutDeckIndex()
        {
            Solver2019_22.CutDeckIndex(0, 3, 10).Should().Be(7);
            Solver2019_22.CutDeckIndex(9, 3, 10).Should().Be(6);
            Solver2019_22.CutDeckIndex(2, 3, 10).Should().Be(9);
            Solver2019_22.CutDeckIndex(5, 3, 10).Should().Be(2);
        }

        [Fact]
        public void CutDeckIndexRev()
        {
            Solver2019_22.CutDeckIndexRev(7, 3, 10).Should().Be(0);
            Solver2019_22.CutDeckIndexRev(6, 3, 10).Should().Be(9);
            Solver2019_22.CutDeckIndexRev(9, 3, 10).Should().Be(2);
            Solver2019_22.CutDeckIndexRev(2, 3, 10).Should().Be(5);
        }

        [Fact]
        public void CutDeckNegativeIndex()
        {
            Solver2019_22.CutDeckNegativeIndex(0, 4, 10).Should().Be(4);
            Solver2019_22.CutDeckNegativeIndex(9, 4, 10).Should().Be(3);
            Solver2019_22.CutDeckNegativeIndex(2, 4, 10).Should().Be(6);
            Solver2019_22.CutDeckNegativeIndex(6, 4, 10).Should().Be(0);
        }

        [Fact]
        public void CutDeckNegativeIndexRev()
        {
            Solver2019_22.CutDeckNegativeIndexRev(4, 4, 10).Should().Be(0);
            Solver2019_22.CutDeckNegativeIndexRev(3, 4, 10).Should().Be(9);
            Solver2019_22.CutDeckNegativeIndexRev(6, 4, 10).Should().Be(2);
            Solver2019_22.CutDeckNegativeIndexRev(0, 4, 10).Should().Be(6);
        }

        [Fact]
        public void DealWithIncNIndex()
        {
            Solver2019_22.DealWithIncNIndex(0, 3, 10).Should().Be(0);
            Solver2019_22.DealWithIncNIndex(9, 3, 10).Should().Be(7);
            Solver2019_22.DealWithIncNIndex(2, 3, 10).Should().Be(6);
            Solver2019_22.DealWithIncNIndex(6, 3, 10).Should().Be(8);
        }

        [Fact]
        public void DealWithIncNIndexRev()
        {
            Solver2019_22.DealWithIncNIndexRev(9799, 36, 10).Should().Be(2519);


            Solver2019_22.DealWithIncNIndexRev(0, 3, 10).Should().Be(0);
            Solver2019_22.DealWithIncNIndexRev(1, 3, 10).Should().Be(7);
            Solver2019_22.DealWithIncNIndexRev(2, 3, 10).Should().Be(4);

            Solver2019_22.DealWithIncNIndexRev(3, 3, 10).Should().Be(1);
            Solver2019_22.DealWithIncNIndexRev(4, 3, 10).Should().Be(8);
            Solver2019_22.DealWithIncNIndexRev(5, 3, 10).Should().Be(5);

            Solver2019_22.DealWithIncNIndexRev(6, 3, 10).Should().Be(2);
            Solver2019_22.DealWithIncNIndexRev(7, 3, 10).Should().Be(9);
            Solver2019_22.DealWithIncNIndexRev(8, 3, 10).Should().Be(6);
            Solver2019_22.DealWithIncNIndexRev(9, 3, 10).Should().Be(3);

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
