global using AdventOfCode2021.Utility;
global using FluentAssertions;
global using Xunit;

namespace AdventOfCode2020.Puzzles.Day2;

internal static class Day2
{
    internal static string Solve1(IInputResolver input)
    {
        var data = input.AsEnumerable();
        var horisontal = 0;
        var depth = 0;
        foreach (var item in data)
        {
            var a = item.Split(' ');
            var direction = a[0];
            var value = int.Parse(a[1]);
            if (direction == "forward")
            {
                horisontal += value;
            }
            else if (direction == "down") { depth += value; }
            else { depth -= value; }
        }
        return (horisontal * depth).ToString();
    }

    internal static string Solve2(IInputResolver input)
    {
        var data = input.AsEnumerable();
        var horisontal = 0;
        var depth = 0;
        var aim = 0;
        foreach (var item in data)
        {
            var a = item.Split(' ');
            var direction = a[0];
            var value = int.Parse(a[1]);
            if (direction == "forward")
            {
                horisontal += value;
                depth += aim * value;
            }
            else if (direction == "down") 
            { 
                aim += value; 
            }
            else 
            { 
                aim -= value; 
            }
        }
        return (horisontal * depth).ToString();
    }
}

public class Test2020Day2
{
    [Fact]
    public void FirstProblemExamples()
    {
        Day2
            .Solve1(new MockInputResolver(new string[] {
                "forward 5",
                "down 5",
                "forward 8",
                "up 3",
                "down 8",
                "forward 2" }))
            .Should().Be("150");
    }

    [Fact]
    public void FirstProblemInput()
    {
        var result = Day2
            .Solve1(new FileInputResolver(2));

        result.Should().Be("1484118");
    }

    [Fact]
    public void SecondProblemExamples()
    {
        Day2
            .Solve2(new MockInputResolver(new string[] {
                "forward 5",
                "down 5",
                "forward 8",
                "up 3",
                "down 8",
                "forward 2" }))
            .Should().Be("900");
    }

    [Fact]
    public void SecondProblemInput()
    {
        var result = Day2
            .Solve2(new FileInputResolver(2));

        result.Should().Be("1463827010");
    }
}

