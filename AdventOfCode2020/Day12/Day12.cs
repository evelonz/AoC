using AdventOfCode2020.Utility;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace AdventOfCode2020.Day12
{
    internal static class Day12
    {
        internal static (long partOne, long partTwo) Solve(IInputResolver input)
        {
            var data = input.AsEnumerable().ToList();
            var headings = new int[4]; // 0 E 1 S ....
            int CurrDir = 0;
            var waypoint = new int[4];

            waypoint[0] = 10; // E
            waypoint[3] = 1; // N
            var shipPos = new int[4]; // 0 E 1 S ....

            foreach (var item in data)
            {
                var action = item[0];
                var value = int.Parse(item[1..]);

                switch (action)
                {
                    case 'E':
                        headings[0] += value;
                        waypoint[0] += value;
                        break;
                    case 'S':
                        headings[1] += value;
                        waypoint[1] += value;
                        break;
                    case 'W':
                        headings[2] += value;
                        waypoint[2] += value;
                        break;
                    case 'N':
                        headings[3] += value;
                        waypoint[3] += value;
                        break;
                    case 'F':
                        var d = GetHeading(CurrDir);
                        headings[d] += value;
                        MoveShip(waypoint, shipPos, value);
                        break;
                    case 'R':
                        CurrDir += value;
                        waypoint = AlterWaypoint(value, waypoint);
                        break;
                    case 'L':
                        CurrDir -= value;
                        waypoint = AlterWaypoint(-value, waypoint);
                        break;
                    default:
                        throw new InvalidOperationException("Incorrect Action");
                }

            }

            var ew1 = Math.Abs(headings[0]- headings[2]);
            var ns1 = Math.Abs(headings[1] - headings[3]);
            var md1 = ew1 + ns1;

            var ew2 = Math.Abs(shipPos[0] - shipPos[2]);
            var ns2 = Math.Abs(shipPos[1] - shipPos[3]);
            var md2 = ew2 + ns2;

            return (md1, md2);
        }

        private static int GetHeading(int currDir)
            => NormalizeHeading(currDir / 90);
            

        private static int NormalizeHeading(int value)
        {
            var newDir = value % 4;
            return newDir switch
            {
                -1 => 3,
                -2 => 2,
                -3 => 1,
                _ => newDir
            };
        }

        private static int[] AlterWaypoint(int value, int[] waypoint)
        {
            var steps = value / 90;
            var newPositions = new int[4];
            for (int i = 0; i < waypoint.Length; i++)
            {
                var newDir = NormalizeHeading(i + steps);
                newPositions[newDir] = waypoint[i];
            }

            return newPositions;
        }

        private static void MoveShip(int[] waypoints, int[] shipPos, int steps)
        {
            for (int i = 0; i < waypoints.Length; i++)
            {
                shipPos[i] += waypoints[i] * steps;
            }
        }
    }

    public class Test2020Day12
    {
        [Fact]
        public void SolveProblemExamples()
        {
            var (partOne, partTwo) = Day12
                .Solve(new MockInputResolver(exampleData));
            partOne.Should().Be(25);
            partTwo.Should().Be(286);
        }

        [Fact]
        public void SolveProblemInput()
        {
            var (partOne, partTwo) = Day12
                .Solve(new FileInputResolver(12));
            partOne.Should().Be(1645);
            partTwo.Should().Be(35292);
        }

        private static readonly string[] exampleData = new string[] {
            "F10",
            "N3",
            "F7",
            "R90",
            "F11",
        };
    }
}
