using AdventOfCode2020.Utility;
using FluentAssertions;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace AdventOfCode2020.Day13
{
    internal static class Day13
    {
        internal static (long partOne, long partTwo) Solve(IInputResolver input)
        {
            var data = input.AsEnumerable().ToList();
            var earliest = int.Parse(data[0]);

            var busses = data.Skip(1).First().Split(',')
                .Select((s, index) => {
                    var skip = s == "x";
                    var busID = skip ? 0 : int.Parse(s);
                    return (skip, busID, index);
                })
                .Where(x => !x.skip)
                .ToList();

            // Part one
            (int bus, int time) first = (int.MaxValue, int.MaxValue);
            foreach (var (_, busID, index) in busses)
            {
                int time = ((earliest / busID) * busID) + busID;
                if (time < first.time)
                {
                    first = (busID, time);
                }
            }
            long ans1 = first.bus * (first.time - earliest);

            // Part two
            var firstBus = busses[0];
            busses.RemoveAt(0);

            // new solution
            // Found solutions for two vectors should repeat only at their LCD.
            // Start should be reset to where the first solution was found.
            // Example: 5, 7 with delta 1.
            // First solution 5*4 = 20, 7*3 = 21.
            // New vector, LCD -> 5*7 = 35, start = 20.
            // Next solution 20 + (35*1) = 55 (for 5*7 and for 7*8 gives 56).
            (long steps, long offset) vector = (firstBus.busID, 0);
            var departFirst = 0L;
            foreach (var (_, busID, index) in busses)
            {
                for (int i = 1; i > 0; i++)
                {
                    departFirst = (vector.steps * i) + vector.offset;
                    var expectedTime = departFirst + index;
                    var solution = expectedTime % busID == 0;
                    if (solution)
                    {
                        var lcd = MathHelpers.Lcm(vector.steps, busID);
                        vector = (lcd, departFirst);
                        break;
                    }
                }
            }

            return (ans1, departFirst);
        }
    }

    public class Test2020Day13
    {
        [Theory]
        [MemberData(nameof(ExampleData))]
        public void SolveProblemExamples(string[] example, int expectedFirst, int expectedSecond)
        {
            var (partOne, partTwo) = Day13
                .Solve(new MockInputResolver(example));
            partOne.Should().Be(expectedFirst);
            partTwo.Should().Be(expectedSecond);
        }

        [Fact]
        public void SolveProblemInput()
        {
            var (partOne, partTwo) = Day13
                .Solve(new FileInputResolver(13));
            partOne.Should().Be(3606);
            partTwo.Should().Be(379786358533423);
        }

        public readonly static List<object[]> ExampleData = new List<object[]>
        {
            new object[] {
                new [] {
                    "939",
                    "7,13,x,x,59,x,31,19",
                }, 295, 1068781
            },
            new object[] {
                new [] {
                    "1",
                    "17,x,13,19",
                }, 156, 3417
            },
            new object[] {
                new [] {
                    "1",
                    "67,7,59,61",
                }, 42, 754018
            },
            new object[] {
                new [] {
                    "1",
                    "67,x,7,59,61",
                }, 42, 779210
            },
            new object[] {
                new [] {
                    "1",
                    "67,7,x,59,61",
                }, 42, 1261476
            },
            new object[] {
                new [] {
                    "1",
                    "1789,37,47,1889",
                }, 1332, 1202161486
            },
        };
    }
}
