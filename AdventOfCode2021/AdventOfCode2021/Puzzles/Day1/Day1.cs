global using AdventOfCode2021.Utility;
global using FluentAssertions;
global using Xunit;

namespace AdventOfCode2020.Puzzles.Day1;

internal static class Day1
{
    internal static string Solve1(IInputResolver input)
    {
        var data = input.AsEnumerable().Select(s => int.Parse(s)).ToList();
        var previous = data[0];
        int count = 0;
        for (int i = 1; i < data.Count; i++)
        {
            var current = data[i];
            if (current > previous)
                count++;
            previous = current;
        }
        return count.ToString();
    }

    internal static string Solve2(IInputResolver input)
    {
        var data = input.AsEnumerable().Select(s => int.Parse(s)).ToList();
        int count = 0;
        for (int i = 0; i < data.Count - 3; i++)
        {
            var firstGroup = data[i] + data[i + 1] + data[i + 2];
            var secondGroup = data[i + 1] + data[i + 2] + data[i + 3];
            if (firstGroup < secondGroup)
                count++;
        }

        return count.ToString();
    }
}

public class Test2020Day1
{
    [Fact]
    public void FirstProblemExamples()
    {
        Day1
            .Solve1(new MockInputResolver(new string[] {
                    "199",
                    "200",
                    "208",
                    "210",
                    "200",
                    "207",
                    "240",
                    "269",
                    "260",
                    "263" }))
            .Should().Be("7");
    }

    [Fact]
    public void FirstProblemInput()
    {
        var result = Day1
            .Solve1(new FileInputResolver(1));

        result.Should().Be("1553");
    }

    [Fact]
    public void SecondProblemExamples()
    {
        Day1
            .Solve2(new MockInputResolver(new string[] {
                    "199",
                    "200",
                    "208",
                    "210",
                    "200",
                    "207",
                    "240",
                    "269",
                    "260",
                    "263" }))
            .Should().Be("5");
    }

    [Fact]
    public void SecondProblemInput()
    {
        var result = Day1
            .Solve2(new FileInputResolver(1));

        result.Should().Be("1597");
    }
}

