using AdventOfCode2022.Utility;
using FluentAssertions;
using System.Text.RegularExpressions;
using Xunit;
using System.Text;

namespace AdventOfCode2022.Day6;

internal static class Day6
{
    internal static (int partOne, int partTwo) Solve(IInputResolver input)
    {
        const int lengthPartOne = 4;
        var partOne = 0;
        const int lengthPartTwo = 14;
        var partTwo = 0;
        var stream = input.AsEnumerable().First().AsSpan();

        for (int i = lengthPartOne; i < stream.Length; i++)
        {
            if (partOne == 0)
            {
                var marker = stream[(i- lengthPartOne)..i];

                if (marker[0] != marker[1]
                && marker[0] != marker[2]
                && marker[0] != marker[3]
                && marker[1] != marker[2]
                && marker[1] != marker[3]
                && marker[2] != marker[3])
                {
                    partOne = i;
                }
            }

            if (partTwo == 0 && i >= lengthPartTwo)
            {
                var message = stream[(i - lengthPartTwo)..i];
                var set = new HashSet<char>(lengthPartTwo);
                var found = true;
                foreach (var item in message)
                {
                    if (set.Contains(item))
                    {
                        found = false;
                        break;
                    }

                    set.Add(item);
                }
                if (found)
                {
                    partTwo = i;
                }
            }

            if (partOne != 0 && partTwo != 0)
            {
                break;
            }
        }

        return (partOne, partTwo);
    }
}

public class Test2022Day6
{
    [Theory]
    [InlineData("mjqjpqmgbljsphdztnvjfqwrcgsmlb", 7, 19)]
    [InlineData("bvwbjplbgvbhsrlpgdmjqwftvncz", 5, 23)]
    [InlineData("nppdvjthqldpwncqszvftbrmjlhg", 6, 23)]
    [InlineData("nznrnfrfntjfmvfwmzdfjlvtqnbhcprsg", 10, 29)]
    [InlineData("zcfzfwzzqfrljwzlrfnpqdbhtmscgvjw", 11, 26)]
    public void FirstProblemExamples(string input, int partOne, int partTwo)
    {
        Day6
            .Solve(new MockInputResolver(new string[] {
                input
            }))
            .Should().Be((partOne, partTwo));
    }

    [Fact]
    public void ProblemInput()
    {
        var (partOne, partTwo) = Day6
            .Solve(new FileInputResolver(6));

        partOne.Should().Be(1566);
        partTwo.Should().Be(2265);
    }
}
