global using AdventOfCode2021.Utility;
global using FluentAssertions;
global using Xunit;

namespace AdventOfCode2020.Puzzles.Day5;

internal static class Day5
{
    internal static string Solve1(IInputResolver input) => Solve(input, false);

    internal static string Solve2(IInputResolver input) => Solve(input, true);

    private static string Solve(IInputResolver input, bool part2)
    {
        var data = input.AsEnumerable();
        var map = new int[1000, 1000]; // x, y
        foreach (var line in data)
        {
            var baseline = line.Split(' ');
            var first = baseline[0].Split(',');
            var x1 = int.Parse(first[0].ToString());
            var y1 = int.Parse(first[1].ToString());
            var second = baseline[2].Split(',');
            var x2 = int.Parse(second[0].ToString());
            var y2 = int.Parse(second[1].ToString());

            if (x1 == x2)
            {
                // asume always increasing from x1 to x2?
                var start = Math.Min(y1, y2);
                var end = Math.Max(y1, y2);
                for (int y = start; y <= end; y++)
                {
                    map[x1, y]++;
                }
            }
            else if (y1 == y2)
            {
                var start = Math.Min(x1, x2);
                var end = Math.Max(x1, x2);
                // asume always increasing from x1 to x2?
                for (int x = start; x <= end; x++)
                {
                    map[x, y1]++;
                }
            }
            else if (part2)// Diagonal, known to be 45 degrees.
            {
                var deltaX = x1 - x2;
                var deltaY = y1 - y2;
                var signX = deltaX < 0 ? -1 : 1;
                var signY = deltaY < 0 ? -1 : 1;

                if (Math.Abs(deltaY) != Math.Abs(deltaX))
                {
                    throw new Exception("Not a 45 degree line");
                }

                for (int i = 0; i <= Math.Abs(deltaX); i++)
                {
                    map[x2 + (signX * i), y2 + (signY * i)]++;
                }
            }
        }
        var count = 0;
        foreach (var point in map)
        {
            if (point > 1) count++;
        }
        return count.ToString();
    }
}

public class Test2020Day5
{
    [Fact]
    public void FirstProblemExamples()
    {
        Day5
            .Solve1(new MockInputResolver(new string[] {
                "0,9 -> 5,9",
                "8,0 -> 0,8",
                "9,4 -> 3,4",
                "2,2 -> 2,1",
                "7,0 -> 7,4",
                "6,4 -> 2,0",
                "0,9 -> 2,9",
                "3,4 -> 1,4",
                "0,0 -> 8,8",
                "5,5 -> 8,2" }))
            .Should().Be("5");
    }

    [Fact]
    public void FirstProblemInput()
    {
        var result = Day5
            .Solve1(new FileInputResolver(5));

        result.Should().Be("7414");
    }

    [Fact]
    public void SecondProblemExamples()
    {
        Day5
            .Solve2(new MockInputResolver(new string[] {
                "0,9 -> 5,9",
                "8,0 -> 0,8",
                "9,4 -> 3,4",
                "2,2 -> 2,1",
                "7,0 -> 7,4",
                "6,4 -> 2,0",
                "0,9 -> 2,9",
                "3,4 -> 1,4",
                "0,0 -> 8,8",
                "5,5 -> 8,2" }))
            .Should().Be("12");
    }

    [Fact]
    public void SecondProblemInput()
    {
        var result = Day5
            .Solve2(new FileInputResolver(5));

        result.Should().Be("19676");
    }
}

