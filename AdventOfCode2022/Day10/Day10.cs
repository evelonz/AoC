using AdventOfCode2022.Utility;
using FluentAssertions;
using System.Diagnostics;
using Xunit;

namespace AdventOfCode2022.Day10;

internal static class Day10
{
    private static readonly int[] CheckSignalStrengthAt
        = new int[] { 20, 60, 100, 140, 180, 220 };

internal static int Solve(IInputResolver input)
    {
        int cycle = 0;
        int drawPos = 0;
        int x = 1;
        int signalStrength = 0;

        foreach (var item in input.AsEnumerable())
        {
            OneCycle(ref cycle, ref drawPos, x, ref signalStrength);

            if (item.StartsWith('a'))
            {
                OneCycle(ref cycle, ref drawPos, x, ref signalStrength);
                x += int.Parse(item.Split(' ')[1]);
            }
        }

        return signalStrength;
    }

    private static void OneCycle(ref int cycle, ref int drawPos, int x, ref int signalStrength)
    {
        Draw(drawPos, x);
        cycle++;
        drawPos = DrawPos(cycle);
        signalStrength += GetSignalStrengthChange(cycle, x);
    }

    private static int DrawPos(int cycle) => cycle % 40;

    private static int GetSignalStrengthChange(int cycle, int x)
        => CheckSignalStrengthAt.Contains(cycle)
        ? cycle * x : 0;

    private static void Draw(int cycle, int x)
    {
        var lit = cycle >= x - 1 && cycle <= x + 1 ? '#' : '.';
        Debug.Write(lit);
        if ((cycle+1) % 40 == 0)
            Debug.WriteLine("");
    }
}

public class Test2022Day10
{
    [Fact]
    public void FirstProblemExamples()
    {
        Day10
            .Solve(new MockInputResolver(Example))
            .Should().Be(13140);
        // Part two is printed to the Debug window.
        // You have to run the test in debug mode and
        // stop it somewhere to read the result.
    }

    [Fact]
    public void ProblemInput()
    {
        var partOne = Day10
            .Solve(new FileInputResolver(10));

        partOne.Should().Be(15140);
        // Part two is printed to the Debug window.
        // You have to run the test in debug mode and
        // stop it somewhere to read the result.
    }

    private static readonly string[] Example = new string[]
    {
        "addx 15",
        "addx -11",
        "addx 6",
        "addx -3",
        "addx 5",
        "addx -1",
        "addx -8",
        "addx 13",
        "addx 4",
        "noop",
        "addx -1",
        "addx 5",
        "addx -1",
        "addx 5",
        "addx -1",
        "addx 5",
        "addx -1",
        "addx 5",
        "addx -1",
        "addx -35",
        "addx 1",
        "addx 24",
        "addx -19",
        "addx 1",
        "addx 16",
        "addx -11",
        "noop",
        "noop",
        "addx 21",
        "addx -15",
        "noop",
        "noop",
        "addx -3",
        "addx 9",
        "addx 1",
        "addx -3",
        "addx 8",
        "addx 1",
        "addx 5",
        "noop",
        "noop",
        "noop",
        "noop",
        "noop",
        "addx -36",
        "noop",
        "addx 1",
        "addx 7",
        "noop",
        "noop",
        "noop",
        "addx 2",
        "addx 6",
        "noop",
        "noop",
        "noop",
        "noop",
        "noop",
        "addx 1",
        "noop",
        "noop",
        "addx 7",
        "addx 1",
        "noop",
        "addx -13",
        "addx 13",
        "addx 7",
        "noop",
        "addx 1",
        "addx -33",
        "noop",
        "noop",
        "noop",
        "addx 2",
        "noop",
        "noop",
        "noop",
        "addx 8",
        "noop",
        "addx -1",
        "addx 2",
        "addx 1",
        "noop",
        "addx 17",
        "addx -9",
        "addx 1",
        "addx 1",
        "addx -3",
        "addx 11",
        "noop",
        "noop",
        "addx 1",
        "noop",
        "addx 1",
        "noop",
        "noop",
        "addx -13",
        "addx -19",
        "addx 1",
        "addx 3",
        "addx 26",
        "addx -30",
        "addx 12",
        "addx -1",
        "addx 3",
        "addx 1",
        "noop",
        "noop",
        "noop",
        "addx -9",
        "addx 18",
        "addx 1",
        "addx 2",
        "noop",
        "noop",
        "addx 9",
        "noop",
        "noop",
        "noop",
        "addx -1",
        "addx 2",
        "addx -37",
        "addx 1",
        "addx 3",
        "noop",
        "addx 15",
        "addx -21",
        "addx 22",
        "addx -6",
        "addx 1",
        "noop",
        "addx 2",
        "addx 1",
        "noop",
        "addx -10",
        "noop",
        "noop",
        "addx 20",
        "addx 1",
        "addx 2",
        "addx 2",
        "addx -6",
        "addx -11",
        "noop",
        "noop",
        "noop"
    };
}
