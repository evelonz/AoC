using AdventOfCode2022.Utility;
using FluentAssertions;
using Xunit;

namespace AdventOfCode2022.Day4;

internal static class Day4
{
    internal static (int partOne, int partTwo) Solve(IInputResolver input)
    {
        var partOne = 0;
        var partTwo = 0;
        foreach (var item in input.AsEnumerable())
        {
            var left = item.Split(',')[0];
            var right = item.Split(',')[1];

            var (leftStart, leftEnd) = left.Split('-') switch { var a => (int.Parse(a[0]), int.Parse(a[1])) };
            var (rightStart, rightEnd) = right.Split('-') switch { var a => (int.Parse(a[0]), int.Parse(a[1])) };

            // Left within Right.
            if (leftStart <= rightStart && leftEnd >= rightEnd)
            {
                partOne++;
            }
            // Right within Left
            else if (rightStart <= leftStart && rightEnd >= leftEnd)
            {
                partOne++;
            }

            if (leftStart <= rightEnd && rightStart <= leftEnd)
            {
                partTwo++;
            }

        }

        return (partOne, partTwo);
    }

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
