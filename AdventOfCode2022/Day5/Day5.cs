using AdventOfCode2022.Utility;
using FluentAssertions;
using System.Text.RegularExpressions;
using Xunit;
using System.Text;

namespace AdventOfCode2022.Day5;

internal static class Day5
{
    internal static (string partOne, string partTwo) Solve(IInputResolver input, int columns, string[] columnValues)
    {
        var stacks = new Stack<char>[columns + 1];
        var stacks2 = new Stack<char>[columns + 1];
        for (int i = 0; i < columns; i++)
        {
            stacks[i + 1] = new Stack<char>();
            stacks2[i + 1] = new Stack<char>();
            foreach (var character in columnValues[i])
            {
                stacks[i + 1].Push(character);
                stacks2[i + 1].Push(character);
            }
        }

        var reg = new Regex("[0-9]+");
        var remainder = input.AsEnumerable().SkipWhile(x => !x.StartsWith("move"));

        foreach (var item in remainder)
        {
            // move 12 from 7 to 9
            // Columns always single digits. Count may be double (or more)
            var matches = reg.Matches(item);

            var vals = new List<int>(3);
            foreach (Match match in matches)
            {
                vals.Add(int.Parse(match.Value));
            }

            var count = vals[0];
            var from = stacks[vals[1]];
            var to = stacks[vals[2]];
            var from2 = stacks2[vals[1]];
            var to2 = stacks2[vals[2]];

            // And now we can finally code...
            var partTwoTempStack = new Stack<char>(count);
            for (int i = 0; i < count; i++)
            {
                if (from.TryPop(out var result))
                {
                    to.Push(result);
                }
                if (from2.TryPop(out var result2))
                {
                    partTwoTempStack.Push(result2);
                }
            }
            foreach (var crate in partTwoTempStack)
            {
                to2.Push(crate);
            }
        }

        var partOne = new StringBuilder(columns);
        var partTwo = new StringBuilder(columns);
        for (int i = 0; i < columns; i++)
        {
            partOne.Append(stacks[i + 1].Pop());
            partTwo.Append(stacks2[i + 1].Pop());
        }

        return (partOne.ToString(), partTwo.ToString());
    }

}

public class Test2022Day5
{
    [Fact]
    public void FirstProblemExamples()
    {
        Day5
            .Solve(new MockInputResolver(new string[] {
                "    [D]    ",
                "[N] [C]    ",
                "[Z] [M] [P]",
                " 1   2   3 ",
                "",
                "move 1 from 2 to 1",
                "move 3 from 1 to 3",
                "move 2 from 2 to 1",
                "move 1 from 1 to 2"
            }),
            3,
            new string[]
            {
                "ZN",
                "MCD",
                "P"
            })
            .Should().Be(("CMZ", "MCD"));
    }

        [Fact]
    public void ProblemInput()
    {
        var (partOne, partTwo) = Day5
            .Solve(new FileInputResolver(5), 9, new string[]
            {
                "RGHQSBTN",
                "HSFDPZJ",
                "ZHV",
                "MZJFGH",
                "TZCDLMSR",
                "MTWVHZJ",
                "TFPLZ",
                "QVWS",
                "WHLMTDNC"
            });

        partOne.Should().Be("PTWLTDSJV");
        partTwo.Should().Be("WZMFVGGZP");
    }
}
