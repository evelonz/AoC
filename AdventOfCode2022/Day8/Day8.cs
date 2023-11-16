using AdventOfCode2022.Utility;
using FluentAssertions;
using Xunit;

namespace AdventOfCode2022.Day8;

internal static class Day8
{
    internal static (int partOne, int partTwo) Solve(IInputResolver input, int size)
    {
        int[,] rows = new int[size, size];
        int[,] columns = new int[size, size];
        
        var r = 0;
        foreach (var item in input.AsEnumerable())
        {
            var c = 0;
            foreach (var tree in item)
            {
                var height = tree - '0';
                rows[r, c] = height;
                columns[c, r] = height;
                c++;
            }
            r++;
        }

        var partOne = PartOne(rows, columns, size);

        var partTwo = 0;
        for (int rr = 1; rr < size - 1; rr++)
        {
            for (int cc = 1; cc < size - 1; cc++)
            {
                var seight = LookAround(rows, columns, rr, cc, size);
                partTwo = seight > partTwo ? seight : partTwo;
            }
        }

        return (partOne, partTwo);
    }

    private static int PartOne(int[,] rows, int[,] columns, int size)
    {
        // Loops over each row and column, in each direction and check number of trees that are visable.
        // Ridiculously written and should be refactored!
        var partOne = 0;
        var visible = new bool[size, size];

        for (int rr = 0; rr < size; rr++)
        {
            var lastHeight = -1;
            for (int cc = 0; cc < size; cc++)
            {
                var tree = rows[rr, cc];
                if (tree > lastHeight)
                {
                    if (!visible[rr, cc])
                    {
                        partOne++;
                        visible[rr, cc] = true;
                    }
                    lastHeight = tree;
                }
                if (lastHeight >= 9)
                {
                    break;
                }
            }
        }

        for (int rr = size - 1; rr >= 0; rr--)
        {
            var lastHeight = -1;
            for (int cc = size - 1; cc >= 0; cc--)
            {
                var tree = rows[rr, cc];
                if (tree > lastHeight)
                {
                    if (!visible[rr, cc])
                    {
                        partOne++;
                        visible[rr, cc] = true;
                    }
                    lastHeight = tree;
                }
                if (lastHeight >= 9)
                {
                    break;
                }
            }
        }

        for (int cc = size - 1; cc >= 0; cc--)
        {
            var lastHeight = -1;
            for (int rr = size - 1; rr >= 0; rr--)
            {
                var tree = columns[cc, rr];
                if (tree > lastHeight)
                {
                    if (!visible[rr, cc])
                    {
                        partOne++;
                        visible[rr, cc] = true;
                    }
                    lastHeight = tree;
                }
                if (lastHeight >= 9)
                {
                    break;
                }
            }
        }

        for (int cc = 0; cc < size; cc++)
        {
            var lastHeight = -1;
            for (int rr = 0; rr < size; rr++)
            {
                var tree = columns[cc, rr];
                if (tree > lastHeight)
                {
                    if (!visible[rr, cc])
                    {
                        partOne++;
                        visible[rr, cc] = true;
                    }
                    lastHeight = tree;
                }
                if (lastHeight >= 9)
                {
                    break;
                }
            }
        }

        return partOne;
    }

    private static int LookAround(int[,] rows, int[,] columns, int r, int c, int size)
    {
        // Look in all directions, count the number of trees seen.
        // Return the product.
        // For edge peices, return 0.

        if (r == 0 || r == size || c == 0 || c == size)
        {
            return 0;
        }
        var height = rows[r, c];

        // Check right, left, up and down.
        var upCount = 0;
        for (int up = r-1; up >= 0; up--)
        {
            upCount++;
            if (columns[c, up] >= height)
                break;
        }
        var downCount = 0;
        for (int down = r + 1; down < size; down++)
        {
            downCount++;
            if (columns[c, down] >= height)
                break;
        }

        var leftCount = 0;
        for (int left = c - 1; left >= 0; left--)
        {
            leftCount++;
            if (rows[r, left] >= height)
                break;
        }
        var rightCount = 0;
        for (int right = c + 1; right < size; right++)
        {
            rightCount++;
            if (rows[r, right] >= height)
                break;
        }

        return upCount * downCount * leftCount * rightCount;
    }

}

public class Test2022Day8
{
    [Fact]
    public void FirstProblemExamples()
    {
        Day8
            .Solve(new MockInputResolver(new string[] {
                "30373",
                "25512",
                "65332",
                "33549",
                "35390"
            }), 5)
            .Should().Be((21, 8));
    }

    [Fact]
    public void ProblemInput()
    {
        var (partOne, partTwo) = Day8
            .Solve(new FileInputResolver(8), 99);

        partOne.Should().Be(1736);
        partTwo.Should().Be(268800);
    }
}
