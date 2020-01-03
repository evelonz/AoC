using AdventOfCode.Utility;
using FluentAssertions;
using System.Collections.Generic;
using Xunit;

namespace AdventOfCode.Year2019.Tests
{
    public class Tester2019_16
    {
        [Fact]
        public void FirstProblemMyInput()
        {
            Solver2019_16_1.Solve(new FileInputResolver(2019, 16)).Should().Be("27831665");
        }

        [Theory]
        [MemberData(nameof(FirstExampleData))]
        public void FirstProblemExamples(string[] example, string expected)
        {
            Solver2019_16_1.Solve(new MockInputResolver(example)).Should().Be(expected);
        }

        [Fact]
        public void SecondProblemMyInput()
        {
            Solver2019_16_2.Solve(new FileInputResolver(2019, 16)).Should().Be("36265589");
        }

        [Theory]
        [MemberData(nameof(SecondExampleData))]
        public void SecondProblemExamples(string[] example, string expected)
        {
            Solver2019_16_2.Solve(new MockInputResolver(example)).Should().Be(expected);
        }

        [Fact]
        public void TestFft()
        {
            var inData = new[] { 1, 2, 3, 4, 5, 6, 7, 8 };
            var (first, second, third, forth) = (new int[inData.Length], new int[inData.Length],
                new int[inData.Length], new int[inData.Length]);
            var offset = 0;
            var cOnlyPart = 5;
            var bOnlyPart = 4;

            Solver2019_16_2.FlawedFrequencyTransmission(inData, first, offset, cOnlyPart, bOnlyPart);
            Solver2019_16_2.FlawedFrequencyTransmission(first, second, offset, cOnlyPart, bOnlyPart);
            Solver2019_16_2.FlawedFrequencyTransmission(second, third, offset, cOnlyPart, bOnlyPart);
            Solver2019_16_2.FlawedFrequencyTransmission(third, forth, offset, cOnlyPart, bOnlyPart);

            first.Should().Equal(4, 8, 2, 2, 6, 1, 5, 8);
            second.Should().Equal(3, 4, 0, 4, 0, 4, 3, 8);
            third.Should().Equal(0, 3, 4, 1, 5, 5, 1, 8);
            forth.Should().Equal(0, 1, 0, 2, 9, 4, 9, 8);
        }

        public readonly static List<object[]> FirstExampleData = new List<object[]>
        {
            new object[] {
                new [] {
                    "80871224585914546619083218645595"
                }, "24176176"
            },
            new object[] {
                new [] {
                    "19617804207202209144916044189917"
                }, "73745418"
            },
            new object[] {
                new [] {
                    "69317163492948606335995924319873"
                }, "52432133"
            },
        };

        public readonly static List<object[]> SecondExampleData = new List<object[]>
        {
            new object[] {
                new [] {
                    "03036732577212944063491565474664"
                }, "84462026"
            },
            new object[] {
                new [] {
                    "02935109699940807407585447034323"
                }, "78725270"
            },
            new object[] {
                new [] {
                    "03081770884921959731165446850517"
                }, "53553731"
            },
        };

    }

}
