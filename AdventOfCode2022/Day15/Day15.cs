using AdventOfCode2022.Utility;
using FluentAssertions;
using FluentAssertions.Execution;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Text.RegularExpressions;
using Xunit;

namespace AdventOfCode2022.Day15;

internal static partial class Day15
{
    internal static (int partOne, long partTwo) Solve(IInputResolver input, int lineNumber, int limit)
    {
        var reg = MyRegex();
        var map = new List<(DeviceLocation sensor, DeviceLocation beacon, int manhattanDistance)>();
        var lineToCheck = new HashSet<DeviceLocation>();
        var alreadyOnLine = new HashSet<DeviceLocation>();
        foreach (var line in input.AsEnumerable())
        {
            var matches = reg.Matches(line);

            var vals = new List<int>(4);
            foreach (Match match in matches.Cast<Match>())
            {
                vals.Add(int.Parse(match.Value));
            }

            var sensor = new DeviceLocation(vals[0], vals[1]);
            var beacon = new DeviceLocation(vals[2], vals[3]);
            var distance = ManhattanDistance(sensor, beacon);
            map.Add((sensor, beacon, distance));

            //if (sensor.y == lineNumber)
            //    alreadyOnLine.Add(sensor);
            if (beacon.y == lineNumber)
                alreadyOnLine.Add(beacon);

            var (x1, x2) = GetLineRange(sensor, distance, lineNumber);
            for (int i = x1; i <= x2; i++)
            {
                var toAdd = new DeviceLocation(i, lineNumber);
                lineToCheck.Add(toAdd);
            }
        }
        // Filter list from beacons.
        lineToCheck.ExceptWith(alreadyOnLine);

        var partTwoResult = new ConcurrentBag<long>();
        var readOnlyMap = map.AsReadOnly();
        // Part Two
        Parallel.For(0, limit + 1, (y, state) =>
        {
            //Debug.WriteLine(y);
            for (int x = 0; x <= limit; x++)
            {
                var found = false;
                foreach (var (sensor, _, manhattanDistance) in readOnlyMap)
                {
                    var (x1, x2) = GetLineRange(sensor, manhattanDistance, y);
                    if (x1 <= x && x <= x2)
                    {
                        x = x2;
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    var partTwo = (4_000_000L * x) + y;
                    partTwoResult.Add(partTwo);
                    //return (lineToCheck.Count, partTwo);
                    state.Stop();
                    return;
                }
            }
        });
        //for (int y = 0; y <= limit; y++)
        //{
        //    Debug.WriteLine(y);
        //    for (int x = 0; x <= limit; x++)
        //    {
        //        var found = false;
        //        foreach (var (sensor, _, manhattanDistance) in map)
        //        {
        //            var (x1, x2) = GetLineRange(sensor, manhattanDistance, y);
        //            if (x1 <= x && x <= x2)
        //            {
        //                x = x2;
        //                found = true;
        //                break;
        //            }
        //        }
        //        if (!found)
        //        {
        //            var partTwo = (4_000_000 * x) + y;
        //            return (lineToCheck.Count, partTwo);
        //        }
        //    }
        //}

        return (lineToCheck.Count, partTwoResult.First());
    }

    private static (int x1, int x2) GetLineRange(DeviceLocation sensor, int distance, int lineNumber)
    {
        // Numbers from example.
        // 9 - (7 - 10) = 9 - 3 = 6
        // 9 - (10 - 7) = 9 - 3 = 6
        // 2 - (10-9) = 1
        var maxDeltaX = distance - Math.Abs(sensor.y - lineNumber);
        // TODO: What if sensor x is negative? No negative sensors in the file.
        // 8 - 6 = 2
        var minX = sensor.x - maxDeltaX;
        // 8 + 6 = 14
        var maxX = sensor.x + maxDeltaX;

        return (minX, maxX);
    }

    private static int ManhattanDistance(DeviceLocation sensor, DeviceLocation beacon)
        => Math.Abs(beacon.x - sensor.x) + Math.Abs(beacon.y - sensor.y);

    private record DeviceLocation(int x, int y);

    [GeneratedRegex("-?[0-9]+")]
    private static partial Regex MyRegex();
}

public class Test2022Day15
{
    [Fact]
    public void FirstProblemExamples()
    {
        Day15
            .Solve(new MockInputResolver(new string[] {
                "Sensor at x=2, y=18: closest beacon is at x=-2, y=15",
                "Sensor at x=9, y=16: closest beacon is at x=10, y=16",
                "Sensor at x=13, y=2: closest beacon is at x=15, y=3",
                "Sensor at x=12, y=14: closest beacon is at x=10, y=16",
                "Sensor at x=10, y=20: closest beacon is at x=10, y=16",
                "Sensor at x=14, y=17: closest beacon is at x=10, y=16",
                "Sensor at x=8, y=7: closest beacon is at x=2, y=10",
                "Sensor at x=2, y=0: closest beacon is at x=2, y=10",
                "Sensor at x=0, y=11: closest beacon is at x=2, y=10",
                "Sensor at x=20, y=14: closest beacon is at x=25, y=17",
                "Sensor at x=17, y=20: closest beacon is at x=21, y=22",
                "Sensor at x=16, y=7: closest beacon is at x=15, y=3",
                "Sensor at x=14, y=3: closest beacon is at x=15, y=3",
                "Sensor at x=20, y=1: closest beacon is at x=15, y=3",
            }), 10, 20)
            .Should().Be((26, 56000011));
    }

    [Theory]
    [InlineData("Sensor at x=8, y=7: closest beacon is at x=2, y=10", 10, 12)]
    [InlineData("Sensor at x=8, y=7: closest beacon is at x=2, y=10", 16, 1)]
    [InlineData("Sensor at x=8, y=7: closest beacon is at x=2, y=10", -2, 1)]
    [InlineData("Sensor at x=8, y=7: closest beacon is at x=2, y=10", 3, 11)]
    [InlineData("Sensor at x=0, y=11: closest beacon is at x=2, y=10", 10, 4)]
    [InlineData("Sensor at x=2, y=18: closest beacon is at x=-2, y=15", 11, 1)]
    public void SingleTests(string row, int rowToCheck, int expected)
    {
        var (partOne, _) = Day15
            .Solve(new MockInputResolver(new string[] {
                row
            }), rowToCheck, 20);

        partOne.Should().Be(expected);
    }

    [Fact]
    public void ProblemInput()
    {
        var (partOne, partTwo) = Day15
            .Solve(new FileInputResolver(15), 2_000_000, 4_000_000);

        // Issues:
        // 1. Regex did not include negative sign.
        // 2. Regex causes arithmatic overflow, had to disable the check.
        // 3. Part two overflowed the int value, had to change to long.
        partOne.Should().Be(4886370);
        partTwo.Should().Be(11374534948438);
    }
}
