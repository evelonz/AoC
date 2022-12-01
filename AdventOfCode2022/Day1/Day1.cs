using AdventOfCode2022.Utility;
using FluentAssertions;
using Xunit;

namespace AdventOfCode2022.Day1;

internal static class Day1
{
    internal static (int partOne, int partTwo) Solve2(IInputResolver input)
    {
        var currentSum = 0;
        var count = new List<int>();
        foreach (var item in input.AsEnumerable())
        {
            if (item?.Length == 0)
            {
                count.Add(currentSum);
                currentSum = 0;
            }
            else
            {
                currentSum += int.Parse(item!);
            }
        }
        count.Add(currentSum); // Add last element.

        var orderedList = count.OrderByDescending(x => x);
        var partOne = orderedList.First();
        var partTwo = orderedList.Take(3).Sum();

        return (partOne, partTwo);
    }
}

public class Test2020Day1
{
    [Fact]
    public void FirstProblemExamples()
    {
        Day1
            .Solve2(new MockInputResolver(new string[] {
                "1000",
                "2000",
                "3000",
                "",
                "4000",
                "",
                "5000",
                "6000",
                "",
                "7000",
                "8000",
                "9000",
                "",
                "10000",
                 }))
            .Should().Be((24000 ,45000));
    }

    [Fact]
    public void ProblemInput()
    {
        var (partOne, partTwo) = Day1
            .Solve2(new FileInputResolver(1));

        partOne.Should().Be(72240);
        partTwo.Should().Be(210957);
    }
}
