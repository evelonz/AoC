using AdventOfCode2022.Utility;
using FluentAssertions;
using Xunit;

namespace AdventOfCode2022.Day4;

internal static class Day4
{
    internal static (int partOne, int partTwo) Solve(IInputResolver input) 
        => input.AsEnumerable()
            .Select(s => s.Split(','))
            .Select(s =>
            {
                var (leftStart, leftEnd) = s[0].Split('-') switch { var a => (int.Parse(a[0]), int.Parse(a[1])) };
                var (rightStart, rightEnd) = s[1].Split('-') switch { var a => (int.Parse(a[0]), int.Parse(a[1])) };
                return (leftStart, leftEnd, rightStart, rightEnd);
            })
            .Aggregate((0, 0), (sum, next) =>
            {
                var fulleInRagne = (next.leftStart <= next.rightStart && next.leftEnd >= next.rightEnd)
                    || (next.rightStart <= next.leftStart && next.rightEnd >= next.leftEnd)
                    ? 1 : 0;
                var partiallyInRange = next.leftStart <= next.rightEnd && next.rightStart <= next.leftEnd
                    ? 1 : 0;
                return (sum.Item1 + fulleInRagne, sum.Item2 + partiallyInRange);
            });

}

public class Test2022Day4
{
    [Fact]
    public void FirstProblemExamples()
    {
        Day4
            .Solve(new MockInputResolver(new string[] {
                "2-4,6-8",
                "2-3,4-5",
                "5-7,7-9",
                "2-8,3-7",
                "6-6,4-6",
                "2-6,4-8"
            }))
            .Should().Be((2, 4));
    }

    [Fact]
    public void ProblemInput()
    {
        var (partOne, partTwo) = Day4
            .Solve(new FileInputResolver(4));

        partOne.Should().Be(496);
        partTwo.Should().Be(847);
    }
}
