using AdventOfCode2022.Utility;
using FluentAssertions;
using Xunit;

namespace AdventOfCode2022.Day9;

internal static class Day9
{
    internal static (int partOne, int partTwo) Solve(IInputResolver input)
    {
        var mapOne = new HashSet<(int x, int y)>();
        var mapTwo = new HashSet<(int x, int y)>();

        var head = (0, 0);
        var tail = (0, 0);
        var restOfTails = new (int, int)[8];
        for (int i = 0; i < restOfTails.Length; i++)
        {
            restOfTails[i] = (0, 0);
        }
        mapOne.Add(tail);
        mapTwo.Add(restOfTails[7]);

        foreach (var (direction, steps) in input.AsEnumerable().Select(s => s.Split(' ') switch { var a => (a[0], int.Parse(a[1])) })) {
            var headDelta = direction switch
            {
                "R" => (1, 0),
                "L" => (-1, 0),
                "U" => (0, 1),
                "D" => (0, -1),
                _ => throw new NotImplementedException("unexpected direction")
            };
            for (int i = 0; i < steps; i++)
            {
                var oldHead = head;
                head.Item1 += headDelta.Item1;
                head.Item2 += headDelta.Item2;

                var tailDelta = (head.Item1 - tail.Item1, head.Item2 - tail.Item2);
                if (tailDelta.Item1 < -1 || tailDelta.Item1 > 1 || tailDelta.Item2 < -1 || tailDelta.Item2 > 1)
                {
                    tail = oldHead;
                }

                var prevTail = tail;
                for (int j = 0; j < restOfTails.Length; j++)
                {
                    tailDelta = (prevTail.Item1 - restOfTails[j].Item1, prevTail.Item2 - restOfTails[j].Item2);

                    if (tailDelta.Item1 < -1)
                    {
                        restOfTails[j].Item1--;
                        restOfTails[j].Item2 += Math.Sign(tailDelta.Item2);
                    }
                    else if (tailDelta.Item1 > 1)
                    {
                        restOfTails[j].Item1++;
                        restOfTails[j].Item2 += Math.Sign(tailDelta.Item2);
                    }
                    else if (tailDelta.Item2 < -1)
                    {
                        restOfTails[j].Item2--;
                        restOfTails[j].Item1 += Math.Sign(tailDelta.Item1);
                    }
                    else if (tailDelta.Item2 > 1)
                    {
                        restOfTails[j].Item2++;
                        restOfTails[j].Item1 += Math.Sign(tailDelta.Item1);
                    }

                    prevTail = restOfTails[j];
                }

                mapOne.Add(tail);
                mapTwo.Add(restOfTails[7]);
            }
        }

        return (mapOne.Count, mapTwo.Count);
    }
}

public class Test2022Day9
{
    [Fact]
    public void FirstProblemExamples()
    {
        Day9
            .Solve(new MockInputResolver(new string[] {
                "R 4",
                "U 4",
                "L 3",
                "D 1",
                "R 4",
                "D 1",
                "L 5",
                "R 2"
            }))
            .Should().Be((13, 1));
    }

    [Fact]
    public void SecondProblemExamples()
    {
        var (_, partTwo) = Day9
            .Solve(new MockInputResolver(new string[] {
                "R 5",
                "U 8",
                "L 8",
                "D 3",
                "R 17",
                "D 10",
                "L 25",
                "U 20"
            }));

        partTwo.Should().Be(36);
    }

    [Fact]
    public void ProblemInput()
    {
        var (partOne, partTwo) = Day9
            .Solve(new FileInputResolver(9));

        partOne.Should().Be(6498);
        partTwo.Should().Be(2531);
    }
}
