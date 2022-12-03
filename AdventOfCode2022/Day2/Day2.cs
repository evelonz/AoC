using AdventOfCode2022.Utility;
using FluentAssertions;
using Xunit;

namespace AdventOfCode2022.Day2;

internal static class Day2
{
    internal static (int partOne, int partTwo) Solve(IInputResolver input)
        => input.AsEnumerable().Select(s => s switch
        {
            // A = Rock, B = Paper, C = Scissors,
            // X = Rock/Lose, Y = Paper/Draw, Z = Scissors/Win
            "A X" => (1 + 3, 3 + 0),
            "A Y" => (2 + 6, 1 + 3),
            "A Z" => (3 + 0, 2 + 6),
            "B X" => (1 + 0, 1 + 0),
            "B Y" => (2 + 3, 2 + 3),
            "B Z" => (3 + 6, 3 + 6),
            "C X" => (1 + 6, 2 + 0),
            "C Y" => (2 + 0, 3 + 3),
            "C Z" => (3 + 3, 1 + 6),
            _ => throw new Exception($"Uncaught case {s}")
        })
        .Aggregate((sum, next) => (sum.Item1 + next.Item1, sum.Item2 + next.Item2));

}

public class Test2022Day2
{
    [Fact]
    public void FirstProblemExamples()
    {
        Day2
            .Solve(new MockInputResolver(new string[] {
                "A Y",
                "B X",
                "C Z",
            }))
            .Should().Be((15, 12));
    }

    [Fact]
    public void ProblemInput()
    {
        var (partOne, partTwo) = Day2
            .Solve(new FileInputResolver(2));

        partOne.Should().Be(9177);
        partTwo.Should().Be(12111);
    }
}
