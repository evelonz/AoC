using AdventOfCode2022.Utility;
using FluentAssertions;
using Xunit;

namespace AdventOfCode2022.Day13;

internal static class Day13
{
    internal static (int partOne, int partTwo) Solve(IInputResolver input)
    {
        var lines = 0;
        string left = string.Empty;
        string right = string.Empty;
        var index = 1;
        var sum = 0;
        foreach (var item in input.AsEnumerable())
        {
            if (item.Length == 0)
            {
                lines = 0;
                index++;
                continue;
            }

            lines++;
            if (++lines == 1)
            {
                left = item;
            }
            else
            {
                right = item;
                if (ComparePacketOrder(left, right))
                {
                    sum += index;
                }
            }
        }

        return (0, 0);
    }

    internal static bool ComparePacketOrder(string left, string right)
    {
        if (left.StartsWith('[') && right.StartsWith("["))
        {
            var left2 = left.Substring(1, left.LastIndexOf(']') - 1);
            var right2 = right.Substring(1, right.LastIndexOf(']') - 1);

            return ComparePacketOrder(left2, right2);
        }
        else
        {
            int index = 0;
            var rightItem = right.Split(',');
            foreach (var leftItem in left.Split(','))
            {
                if (index+1 > rightItem.Length)
                {
                    return true;
                }
                var l = int.Parse(leftItem);
                var r = int.Parse(rightItem[index]);
                index++;
                if (l != r)
                {
                    return l < r;
                }
            }
            if (index < rightItem.Length)
                return false;
        }

        throw new NotImplementedException($"Unhandled case, {left} - {right}");
    }
}

public class Test2022Day13
{
    [Theory]
    //[InlineData("[1,1,3,1,1]", "[1,1,5,1,1]", true)]
    [InlineData("[[1],[2,3,4]]", "[[1],4]", true)]
    public void FirstSmallExamples(string left, string right, bool expected)
    {
        Day13.ComparePacketOrder(left, right).Should().Be(expected);
    }

    [Fact]
    public void FirstProblemExamples()
    {
        Day13
            .Solve(new MockInputResolver(new string[] {
                "[1, 1, 3, 1, 1]",
                "[1, 1, 5, 1, 1]",
                "",
                "[[1],[2, 3, 4]]",
                "[[1], 4]",
                "",
                "[9]",
                "[[8, 7, 6]]",
                "",
                "[[4,4],4,4]",
                "[[4,4],4,4,4]",
                "",
                "[7,7,7,7]",
                "[7,7,7]",
                "",
                "[]",
                "[3]",
                "",
                "[[[]]]",
                "[[]]",
                "",
                "[1,[2,[3,[4,[5,6,7]]]],8,9]",
                "[1,[2,[3,[4,[5,6,0]]]],8,9]"
            }))
            .Should().Be((13, 0));
    }

    [Fact]
    public void ProblemInput()
    {
        var (partOne, partTwo) = Day13
            .Solve(new FileInputResolver(13));

        partOne.Should().Be(0);
        //partTwo.Should().Be(0);
    }
}
