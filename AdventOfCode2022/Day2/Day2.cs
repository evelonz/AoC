using AdventOfCode2022.Utility;
using FluentAssertions;
using Xunit;

namespace AdventOfCode2022.Day2;

internal static class Day2
{
    internal static (int partOne, int partTwo) Solve2(IInputResolver input)
    {
        var score = 0;
        var scoreTwo = 0;
        foreach (var item in input.AsEnumerable())
        {
            var oponent = item.Split(' ')[0];
            var you = item.Split(' ')[1];
            var result = you switch
            {
                "X" => PlayRock(oponent),
                "Y" => PlayPaper(oponent),
                "Z" => PlayScissors(oponent),
                _ => throw new Exception("Missed type")
            };
            score += result;

            // Part two
            var resultTwo = you switch
            {
                "X" => PlayLose(oponent),
                "Y" => PlayDraw(oponent),
                "Z" => PlayWin(oponent),
                _ => throw new Exception("Missed type")
            };
            scoreTwo += resultTwo;
        }

        return (score, scoreTwo);
    }

    private static int PlayLose(string oponent) => oponent switch
    {
        "A" => PlayScissors(oponent),
        "B" => PlayRock(oponent),
        "C" => PlayPaper(oponent),
        _ => throw new Exception("Missed lose play")
    };

    private static int PlayDraw(string oponent) => oponent switch
    {
        "A" => PlayRock(oponent),
        "B" => PlayPaper(oponent),
        "C" => PlayScissors(oponent),
        _ => throw new Exception("Missed draw play")
    };

    private static int PlayWin(string oponent) => oponent switch
    {
        "A" => PlayPaper(oponent),
        "B" => PlayScissors(oponent),
        "C" => PlayRock(oponent),
        _ => throw new Exception("Missed win play")
    };

    private static int PlayRock(string oponent) => oponent switch
    {
        "A" => 1 + 3, // Rock
        "B" => 1 + 0, // Paper
        "C" => 1 + 6, // Scissors
        _ => throw new Exception("Missed type Rock")
    };

    private static int PlayPaper(string oponent) => oponent switch
    {
        "A" => 2 + 6,
        "B" => 2 + 3,
        "C" => 2 + 0,
        _ => throw new Exception("Missed type Paper")
    };

    private static int PlayScissors(string oponent) => oponent switch
    {
        "A" => 3 + 0,
        "B" => 3 + 6,
        "C" => 3 + 3,
        _ => throw new Exception("Missed type Scissors")
    };
}

public class Test2020Day2
{
    [Fact]
    public void FirstProblemExamples()
    {
        Day2
            .Solve2(new MockInputResolver(new string[] {
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
            .Solve2(new FileInputResolver(2));

        partOne.Should().Be(9177);
        partTwo.Should().Be(12111);
    }
}
