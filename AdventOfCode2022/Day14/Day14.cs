using AdventOfCode2022.Utility;
using FluentAssertions;
using System.Diagnostics;
using Xunit;

namespace AdventOfCode2022.Day14;

internal static class Day14
{
    internal static (int partOne, int partTwo) Solve(IInputResolver input)
    {
        var map = new HashSet<(int x, int y)>();
        var maxY = int.MinValue;
        foreach (var lines in input.AsEnumerable())
        {
            string prev = null;
            foreach (var point in lines.Split(" -> "))
            {
                if(prev is null)
                {
                    prev = point;
                    continue;
                }
                var coords2 = point.Split(',');
                var x2 = int.Parse(coords2[0]);
                var y2 = int.Parse(coords2[1]);
                var coords1 = prev.Split(',');
                var x1 = int.Parse(coords1[0]);
                var y1 = int.Parse(coords1[1]);
                if (x1 == x2)
                {
                    var start = y1 < y2 ? y1 : y2;
                    var end = y1 < y2 ? y2 : y1;
                    for (int i = start; i <= end; i++)
                    {
                        map.Add((x1, i));
                    }
                }
                else
                {
                    var start = x1 < x2 ? x1 : x2;
                    var end = x1 < x2 ? x2 : x1;
                    for (int i = start; i <= end; i++)
                    {
                        map.Add((i, y1));
                    }
                }

                maxY = y1 > maxY ? y1 : maxY;
                maxY = y2 > maxY ? y2 : maxY;

                prev = point;
            }

        }

        Debug.WriteLine(maxY);

        var sandCount = 0;
        var partOne = 0;
        while(true)
        {
            var rest = false;
            (int x, int y) sand = (500, 0);
            while (!rest)
            {
                var down = (sand.x, sand.y + 1);
                var left = (sand.x - 1, sand.y + 1);
                var right = (sand.x + 1, sand.y + 1);
                if (!map.Contains(down))
                {
                    sand = down;
                }
                else if (!map.Contains(left))
                {
                    sand = left;
                }
                else if (!map.Contains(right))
                {
                    sand = right;
                }
                else
                {
                    map.Add(sand);
                    rest = true;
                    sandCount++;

                    if (sand == (500, 0))
                    {
                        return (partOne, sandCount);
                    }
                }

                if (sand.y == maxY + 1)
                {
                    map.Add(sand);
                    rest = true;
                    sandCount++;
                }

                Debug.WriteLine(sand);

                if (partOne == 0 && sand.y >= maxY)
                {
                    partOne = sandCount;
                }
            }
        }
    }
}

public class Test2022Day14
{
    [Fact]
    public void FirstProblemExamples()
    {
        Day14
            .Solve(new MockInputResolver(new string[] {
                "498,4 -> 498,6 -> 496,6",
                "503,4 -> 502,4 -> 502,9 -> 494,9",
            }))
            .Should().Be((24, 93));
    }

    [Fact]
    public void ProblemInput()
    {
        var (partOne, partTwo) = Day14
            .Solve(new FileInputResolver(14));

        partOne.Should().Be(961);
        partTwo.Should().Be(26375);
    }
}
