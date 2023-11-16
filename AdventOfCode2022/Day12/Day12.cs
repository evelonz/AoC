using AdventOfCode2022.Utility;
using FluentAssertions;
using Xunit;

namespace AdventOfCode2022.Day12;

internal static class Day12
{
    internal static (int partOne, int partTwo) Solve(IInputResolver input, (int x, int y) end)
    {
        var map = new List<string>();
        map.AddRange(input.AsEnumerable());

        var path = BFS(map, end, map.Count, map[0].Length);

        var partOne = ShortestPath(path.path, end, path.start);

        var partTwo = int.MaxValue;
        for (int x = 0; x < map.Count; x++)
        {
            for (int y = 0; y < map[0].Length; y++)
            {
                var node = map[x][y];
                if (node == 'a' || node == 'S')
                {
                    var next = ShortestPath(path.path, end, (x, y));
                    partTwo = next < partTwo ? next : partTwo;
                }
            }
        }

        return (partOne, partTwo);
    }

    private static (Dictionary<(int x, int y), (int x, int y)> path, (int x, int y) start) BFS(List<string> map, (int x, int y) start, int sizeX, int sizeY)
    {
        var previous = new Dictionary<(int x, int y), (int x, int y)>();
        var queue = new Queue<(int x, int y)>();
        queue.Enqueue(start);
        var end = (-1, -1);

        while (queue.Count > 0)
        {
            var node = queue.Dequeue();
            var currentHieght = map[node.x][node.y];
            currentHieght = currentHieght == 'S' ? 'a' : currentHieght;
            currentHieght = currentHieght == 'E' ? 'z' : currentHieght;

            foreach (var (x, y) in Directions)
            {
                var neighbor = (node.x + x, node.y + y);
                if (previous.ContainsKey(neighbor))
                {
                    continue;
                }

                if (neighbor.Item1 < 0 || neighbor.Item1 >= sizeX
                    || neighbor.Item2 < 0 || neighbor.Item2 >= sizeY)
                {
                    continue;
                }

                var nextNode = map[neighbor.Item1][neighbor.Item2];
                var nextHeight = nextNode == 'E' ? 'z' : nextNode;
                nextHeight = nextHeight == 'S' ? 'a' : nextHeight;

                if (currentHieght - 1 > nextHeight)
                {
                    continue;
                }

                previous[neighbor] = node;
                queue.Enqueue(neighbor);

                if (nextNode == 'S')
                {
                    end = neighbor;
                }
            }
        }

        return (previous, end);
    }

    private static int ShortestPath(Dictionary<(int x, int y), (int x, int y)> map, (int x, int y) start, (int x, int y) target)
    {
        var path = new List<(int x, int y)>();

        var current = target;
        while (!current.Equals(start))
        {
            path.Add(current);
            if (!map.TryGetValue(current, out current))
                return int.MaxValue;
        }

        return path.Count;
    }

    private static readonly List<(int x, int y)> Directions = new List<(int x, int y)>
    {
        (-1, 0),
        (1, 0),
        (0, -1),
        (0, 1),
    };
}

public class Test2022Day12
{
    [Fact]
    public void FirstProblemExamples()
    {
        Day12
            .Solve(new MockInputResolver(new string[] {
                "Sabqponm",
                "abcryxxl",
                "accszExk",
                "acctuvwj",
                "abdefghi"
            }), (2, 5))
            .Should().Be((31, 29));
    }

    [Fact]
    public void ProblemInput()
    {
        var (partOne, partTwo) = Day12
            .Solve(new FileInputResolver(12), (20, 40));

        partOne.Should().Be(370);
        partTwo.Should().Be(363);
    }
}
