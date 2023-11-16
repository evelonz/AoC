using AdventOfCode2022.Utility;
using FluentAssertions;
using Xunit;

namespace AdventOfCode2022.Day18;

internal static class Day18
{
    internal static (int partOne, int partTwo) Solve(IInputResolver input)
    {
        var droplets = new HashSet<(int x, int y, int z)>();
        var sides = 0;

        foreach (var line in input.AsEnumerable())
        {
            var l = line.Split(',');
            var newDroplet = (x: int.Parse(l[0]), y: int.Parse(l[1]), z: int.Parse(l[2]));

            // Each droplet has 6 sides.
            // If any sides touches, then subtrackt 1 from each droplet.
            sides += 6;
            foreach (var (x, y, z) in droplets)
            {
                if (x == newDroplet.x && y == newDroplet.y)
                {
                    if (z == newDroplet.z - 1 || z == newDroplet.z + 1)
                    {
                        sides -= 2;
                    }
                }
                else if (x == newDroplet.x && z == newDroplet.z)
                {
                    if (y == newDroplet.y - 1 || y == newDroplet.y + 1)
                    {
                        sides -= 2;
                    }
                }
                else if (y == newDroplet.y && z == newDroplet.z)
                {
                    if (x == newDroplet.x - 1 || x == newDroplet.x + 1)
                    {
                        sides -= 2;
                    }
                }
            }
            droplets.Add(newDroplet);
        }

        // AirPockets
        var freePockets = new HashSet<(int x, int y, int z)>();
        var trappedPockets = new HashSet<(int x, int y, int z)>();

        foreach (var (x, y, z) in droplets)
        {
            foreach (var delta in Directions)
            {
                var trapped = true;
                var toCheck = new Queue<(int x, int y, int z)>();
                toCheck.Enqueue((x + delta.x, y + delta.y, z + delta.z));
                var touched = new HashSet<(int x, int y, int z)>();
                while (toCheck.TryDequeue(out var current))
                {
                    // trappedPockets should only hit for the first pocket checked from a droplet.
                    if (droplets.Contains(current) || touched.Contains(current) || trappedPockets.Contains(current))
                    {
                        continue;
                    }

                    touched.Add(current);
                    if (freePockets.Contains(current) || IsFree(current, droplets))
                    {
                        trapped = false;
                        freePockets.UnionWith(touched);
                        continue;
                    }

                    foreach (var n in Directions)
                    {
                        // Will add the cell that added the current one, but we check if already touched in start of loop.
                        toCheck.Enqueue((current.x + n.x, current.y + n.y, current.z + n.z));
                    }
                }

                if (trapped)
                {
                    trappedPockets.UnionWith(touched);
                }
            }
        }

        var sidesAgainstTrappedAir = 0;
        foreach (var (x, y, z) in trappedPockets)
        {
            foreach (var delta in Directions)
            {
                sidesAgainstTrappedAir += droplets.Contains((x + delta.x, y + delta.y, z + delta.z)) ? 1 : 0;
            }
        }

        return (sides, sides - sidesAgainstTrappedAir);
    }

    private static bool IsFree((int x, int y, int z) current, HashSet<(int x, int y, int z)> droplets)
    {
        var onXYgtZ = droplets.Any(a => a.x == current.x && a.y == current.y && a.z < current.z);
        var onXYltZ = droplets.Any(a => a.x == current.x && a.y == current.y && current.z < a.z);

        var onXZgtY = droplets.Any(a => a.x == current.x && a.z == current.z && a.y < current.y);
        var onXZltY = droplets.Any(a => a.x == current.x && a.z == current.z && current.y < a.y);

        var onYZgtX = droplets.Any(a => a.y == current.y && a.z == current.z && a.x < current.x);
        var onYZltX = droplets.Any(a => a.y == current.y && a.z == current.z && current.x < a.x);

        return !(onXYgtZ && onXYltZ && onXZgtY && onXZltY && onYZgtX && onYZltX);
    }

    private static readonly (int x, int y, int z)[] Directions = {
        (1, 0, 0),
        (-1, 0, 0),
        (0, 1, 0),
        (0, -1, 0),
        (0, 0, 1),
        (0, 0, -1),
    };
}

public class Test2022Day18
{
    [Fact]
    public void SmallExamples()
    {
        Day18
            .Solve(new MockInputResolver(new string[] {
                "1,1,1", "2,1,1"
            }))
            .Should().Be((10, 10));
    }

    [Fact]
    public void FirstProblemExamples()
    {
        Day18
            .Solve(new MockInputResolver(new string[] {
                "2,2,2",
                "1,2,2",
                "3,2,2",
                "2,1,2",
                "2,3,2",
                "2,2,1",
                "2,2,3",
                "2,2,4",
                "2,2,6",
                "1,2,5",
                "3,2,5",
                "2,1,5",
                "2,3,5"
            }))
            .Should().Be((64, 58));
    }

    [Fact]
    public void ProblemInput()
    {
        var (partOne, partTwo) = Day18
            .Solve(new FileInputResolver(18));

        partOne.Should().Be(3432);
        partTwo.Should().Be(2042);
    }
}
