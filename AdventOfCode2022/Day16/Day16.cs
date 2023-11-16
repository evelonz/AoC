using AdventOfCode2022.Utility;
using FluentAssertions;
using System.Diagnostics;
using Xunit;

namespace AdventOfCode2022.Day16;

internal static class Day16
{
    internal static (int partOne, int partTwo) Solve(IInputResolver input, bool runPartTwo = true)
    {
        var valves = new List<Valve>();
        foreach (var line in input.AsEnumerable())
        {
            var words = line.Split(' ');
            var id = words[1];
            var flow = words[4];
            var i = flow.IndexOf("=");
            var j = flow.IndexOf(";");
            var flowRate = flow[(i + 1)..j];
            var connections = words[9..].Select(s => s.Replace(",", "")).ToArray();

            var newValve = new Valve(id, int.Parse(flowRate), connections);
            valves.Add(newValve);
        }
        var aa = valves.First(f => f.Id == "AA");
        var aaPath = ShortestPathFunction(valves, aa);
        var valesOfValue = valves.Where(x => x.FlowRate > 0).OrderByDescending(o => o.FlowRate).ToList();

        var paths = new Dictionary<string, Func<Valve, IEnumerable<Valve>>>();
        foreach (var item in valesOfValue)
        {
            paths.Add(item.Id, ShortestPathFunction(valves, item));
        }
        paths.Add("AA", aaPath);

        var minPath = int.MaxValue;
        for (int i = 0; i < paths.Count - 1; i++)
        {
            var start = valesOfValue[i];
            var startPath = paths[start.Id];
            for (int j = i+1; j < valesOfValue.Count; j++)
            {
                var end = valesOfValue[j];
                var len = startPath(end).Count();
                minPath = len < minPath ? len : minPath; // Includes opening the valve.
            }
        }
        MinPath = minPath;

        var partOne = DFS(aa, aa, valesOfValue.OrderByDescending(o => o.FlowRate), paths, 30, 0, 0, int.MinValue, true);

        if (runPartTwo)
        {
            var partTwo = DFS(aa, aa, valesOfValue.OrderByDescending(o => o.FlowRate), paths, 26, 26, 0, int.MinValue, true);
            return (partOne, partTwo);
        }

        return (partOne, -1);
    }

    private static int MinPath = int.MaxValue;

    private static int DFS(Valve current1, Valve current2, IOrderedEnumerable<Valve> valves, Dictionary<string, Func<Valve, IEnumerable<Valve>>> paths, int timer1, int timer2, int scoreAgg, int currentHighest, bool useTimer1)
    {
        var (timer, actuallyUseTimer1, current) = SelectCurrentAgent(current1, current2, timer1, timer2, useTimer1);

        if (timer <= 0)
        {
            return scoreAgg;
        }

        var potentialScore = MaxPotentialScore(valves, timer1, timer2) + scoreAgg;
        if (potentialScore < currentHighest)
        {
            Debug.WriteLine($"Broke early, {string.Join(',', valves)}");
            return scoreAgg;
        }

        var maxScore = scoreAgg;
        var pathFromCurrentValve = paths[current.Id];

        foreach (var valve in valves)
        {
            var path = pathFromCurrentValve(valve);
            var timeTaken = path.Count() - 1; // Includes the start position.

            timeTaken++; // For opening the valve.
            var newTimer = timer - timeTaken;

            var newScoreAgg = scoreAgg + (valve.FlowRate * newTimer);

            var newValves = valves.Where(x => x.Id != valve.Id).OrderByDescending(o => o.FlowRate);
            var newScore = actuallyUseTimer1
                ? DFS(valve, current2, newValves, paths, newTimer, timer2, newScoreAgg, maxScore, !useTimer1)
                : DFS(current1, valve, newValves, paths, timer1, newTimer, newScoreAgg, maxScore, !useTimer1);
            maxScore = newScore > maxScore ? newScore : maxScore;
        }

        return maxScore;
    }

    private static (int timer, bool actuallyUseTimer1, Valve current) SelectCurrentAgent(
        Valve current1,
        Valve current2,
        int timer1,
        int timer2,
        bool useTimer1)
    {
        int timer;
        bool actuallyUseTimer1 = useTimer1;
        Valve current;
        if (useTimer1 && timer1 > 0)
        {
            timer = timer1;
            current = current1;
        }
        else if (!useTimer1 && timer2 > 0)
        {
            timer = timer2;
            current = current2;
        }
        else
        {
            timer = timer1 >= timer2 ? timer1 : timer2;
            actuallyUseTimer1 = timer1 >= timer2;
            current = timer1 >= timer2 ? current1 : current2;
        }
        return (timer, actuallyUseTimer1, current);
    }

    private record Valve(string Id, int FlowRate, string[] ConnectedValves);

    private static int MaxPotentialScore(IOrderedEnumerable<Valve> valves, int timeLeft1, int timeLeft2)
    {
        var score = 0;
        foreach (var item in valves)
        {
            var highestTimer = timeLeft1 >= timeLeft2 ? timeLeft1 : timeLeft2;
            highestTimer -= MinPath;
            if (highestTimer <= 0)
                break;
            score += item.FlowRate * highestTimer;
            if (timeLeft1 >= timeLeft2)
            {
                timeLeft1 = highestTimer;
            }
            else
            {
                timeLeft2 = highestTimer;
            }
        }
        return score;
    }

    private static Func<Valve, IEnumerable<Valve>> ShortestPathFunction(IList<Valve> map, Valve start)
    {
        var previous = new Dictionary<Valve, Valve>();

        var queue = new Queue<Valve>();
        queue.Enqueue(start);

        while (queue.Count > 0)
        {
            var vertex = queue.Dequeue();
            foreach (var neighbor in vertex.ConnectedValves)
            {
                var nn = map.First(f => f.Id == neighbor);
                if (previous.ContainsKey(nn))
                    continue;

                previous[nn] = vertex;
                queue.Enqueue(nn);
            }
        }

        IEnumerable<Valve> shortestPath(Valve v)
        {
            var path = new List<Valve>();

            var current = v;
            while (!current.Equals(start))
            {
                path.Add(current);
                current = previous[current];
            };

            path.Add(start);
            path.Reverse();

            return path;
        }

        return shortestPath;
    }
}

public class Test2022Day16
{
    [Fact]
    public void FirstProblemExamples()
    {
        Day16
            .Solve(new MockInputResolver(new string[] {
                "Valve AA has flow rate=0; tunnels lead to valves DD, II, BB",
                "Valve BB has flow rate=13; tunnels lead to valves CC, AA",
                "Valve CC has flow rate=2; tunnels lead to valves DD, BB",
                "Valve DD has flow rate=20; tunnels lead to valves CC, AA, EE",
                "Valve EE has flow rate=3; tunnels lead to valves FF, DD",
                "Valve FF has flow rate=0; tunnels lead to valves EE, GG",
                "Valve GG has flow rate=0; tunnels lead to valves FF, HH",
                "Valve HH has flow rate=22; tunnel leads to valve GG",
                "Valve II has flow rate=0; tunnels lead to valves AA, JJ",
                "Valve JJ has flow rate=21; tunnel leads to valve II"
            }))
            .Should().Be((1651, 1707));
    }

    [Fact]
    public void ProblemInput()
    {
        var (partOne, partTwo) = Day16
            .Solve(new FileInputResolver(16), false);

        partOne.Should().Be(1789);
    }

    [Fact (Skip = "Part two takes to long, heuristics may be wrong")]
    public void ProblemInputPartTwo()
    {
        var (partOne, partTwo) = Day16
            .Solve(new FileInputResolver(16));

        partOne.Should().Be(1789);
        partTwo.Should().Be(0);
    }
}
