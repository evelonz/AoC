using AdventOfCode2020.Utility;
using FluentAssertions;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace AdventOfCode2020.Day15
{
    internal static class Day15
    {
        internal static (long partOne, long partTwo) Solve(IInputResolver input)
        {
            var data = input.AsEnumerable().First().Split(',').Select(s => int.Parse(s)).ToList();
            int lastSpoken = 1;
            var spokenWorkds = data.ToDictionary(d => d, _ => lastSpoken++);
            int lastSpokenWord = data.Last();
            long ans1 = 0L;
            spokenWorkds.Remove(lastSpokenWord); // One word should not be added until checked if spoken before.
            for (int round = data.Count; round < 30000000; round++)
            {
                // If spoken before
                if(spokenWorkds.TryGetValue(lastSpokenWord, out lastSpoken))
                {
                    spokenWorkds[lastSpokenWord] = round;
                    lastSpokenWord = round - lastSpoken;
                }
                // else 0
                else
                {
                    spokenWorkds[lastSpokenWord] = round;
                    lastSpokenWord = 0;
                }
                if(round == 2019)
                {
                    ans1 = lastSpokenWord;
                }
            }

            return (ans1, lastSpokenWord);
        }

    }

    public class Test2020Day15
    {
        [Theory]
        [MemberData(nameof(ExampleData))]
        public void SolveProblemExamples2(string[] example, int expectedFirst, int expectedSecond)
        {
            var (partOne, partTwo) = Day15
                .Solve(new MockInputResolver(example));
            partOne.Should().Be(expectedFirst);
            partTwo.Should().Be(expectedSecond);
        }

        [Fact]
        public void SolveProblemInput()
        {
            var (partOne, partTwo) = Day15
                .Solve(new MockInputResolver(new string[] { "17,1,3,16,19,0" }));
            partOne.Should().Be(694);
            partTwo.Should().Be(21768614);
        }

        public readonly static List<object[]> ExampleData = new List<object[]>
        {
            new object[] { new [] { "0,3,6", }, 436, 175594 },
            new object[] { new [] { "1,3,2", }, 1, 2578 },
            new object[] { new [] { "2,1,3", }, 10, 3544142 },
            new object[] { new [] { "1,2,3", }, 27, 261214 },
            new object[] { new [] { "2,3,1", }, 78, 6895259 },
            new object[] { new [] { "3,2,1", }, 438, 18 },
            new object[] { new [] { "3,1,2", }, 1836, 362 },
        };
    }
}
