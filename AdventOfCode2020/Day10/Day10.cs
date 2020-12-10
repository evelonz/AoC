using AdventOfCode2020.Utility;
using FluentAssertions;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace AdventOfCode2020.Day10
{
    internal static class Day10
    {
        internal static (long partOne, long partTwo) Solve(IInputResolver input)
        {
            var data = input.AsEnumerable().Select(s => int.Parse(s)).OrderBy(o => o).ToList();

            var deviceCount = data.Count;

            int jolt = 0;
            var diffs = new int[4];

            foreach (var item in data)
            {
                var diff = item - jolt;
                diffs[diff]++;
                jolt = item;
            }
            var ans1 = diffs[1] * ++diffs[3];

            // Part 2, permutations.
            data = data.Prepend(0).ToList();
            var cache = new Dictionary<int, long>(data.Count); // Used to store all permutations for a given value.
            long ans2 = Travel(data, 0, cache);

            return (ans1, ans2);
        }

        private static long Travel(List<int> data, int position, Dictionary<int, long> cache)
        {
            long hits = 0;
            var curr = data[position];
            if (position == data.Count - 1)
            {
                return hits + 1;
            }

            for (int j = 1; j <= 3; j++)
            {
                if (position + j > data.Count -1)
                {
                    break;
                }

                var next = data[position + j];
                if (next - curr <= 3)
                {
                    if (cache.TryGetValue(next, out var cachehits))
                    {
                        hits += cachehits;
                    }
                    else
                    {
                        var newHits = Travel(data, position + j, cache);
                        cache.Add(next, newHits);
                        hits += newHits;
                    }
                }
                else
                    break;
            }

            return hits;
        }

    }

    public class Test2020Day10
    {
        [Theory]
        [MemberData(nameof(ExampleData))]
        public void SolveProblemExamples(string[] example, int expectedFirst, int expectedSecond)
        {
            var (partOne, partTwo) = Day10
                .Solve(new MockInputResolver(example));
            partOne.Should().Be(expectedFirst);
            partTwo.Should().Be(expectedSecond);
        }

        [Fact]
        public void SolveProblemInput()
        {
            var (partOne, partTwo) = Day10
                .Solve(new FileInputResolver(10));

            partOne.Should().Be(2380);
            partTwo.Should().Be(48358655787008);
        }

        public readonly static List<object[]> ExampleData = new List<object[]>
        {
            new object[] {
                new [] {
                    "16",
                    "10",
                    "15",
                    "5",
                    "1",
                    "11",
                    "7",
                    "19",
                    "6",
                    "12",
                    "4",
                }, 35, 8
            },
            new object[] {
                new [] {
                    "28",
                    "33",
                    "18",
                    "42",
                    "31",
                    "14",
                    "46",
                    "20",
                    "48",
                    "47",
                    "24",
                    "23",
                    "49",
                    "45",
                    "19",
                    "38",
                    "39",
                    "11",
                    "1",
                    "32",
                    "25",
                    "35",
                    "8",
                    "17",
                    "7",
                    "9",
                    "4",
                    "2",
                    "34",
                    "10",
                    "3",
                }, 220, 19208
            }
        };
    }
}
