using AdventOfCode2020.Utility;
using FluentAssertions;
using System.Linq;
using Xunit;

namespace AdventOfCode2020.Day9
{
    internal static class Day9
    {
        internal static (long partOne, long partTwo) Solve(IInputResolver input, int preamble)
        {
            var data = input.AsEnumerable().Select(s => long.Parse(s)).ToList();
            long ans1 = 0;
            var prevList = data.Take(preamble).ToArray();
            int pos = preamble - 1;

            foreach (var item in data.Skip(preamble))
            {
                var next = item;
                bool valid = false;
                for (int i = 0; i < preamble - 1; i++)
                {
                    for (int j = i+1; j < preamble; j++)
                    {
                        if (prevList[i] + prevList[j] == next)
                            valid = true;
                    }
                }
                if (!valid)
                {
                    ans1 = next;
                    break;
                }

                pos = (pos + 1) % preamble;
                prevList[pos] = next;
            }

            for (int i = 0; i < data.Count - 1; i++)
            {
                long sum = data[i];
                long min = sum;
                long max = sum;
                for (int j = i + 1; j < data.Count; j++)
                {
                    var val = data[j];
                    sum += val;
                    if (val < min)
                        min = val;
                    if (val > max)
                        max = val;
                    if (sum == ans1)
                    {
                        return (ans1, min + max);
                    }
                    if (sum > ans1)
                        break;
                }
            }

            return (-1, -1);
        }

    }

    public class Test2020Day9
    {
        [Fact]
        public void FirstProblemExamples()
        {
            var (partOne, partTwo) = Day9
                .Solve(new MockInputResolver(new string[] {
                    "35",
                    "20",
                    "15",
                    "25",
                    "47",
                    "40",
                    "62",
                    "55",
                    "65",
                    "95",
                    "102",
                    "117",
                    "150",
                    "182",
                    "127",
                    "219",
                    "299",
                    "277",
                    "309",
                    "576",
                }), 5);

            partOne.Should().Be(127);
            partTwo.Should().Be(62);
        }

        [Fact]
        public void FirstProblemInput()
        {
            var (partOne, partTwo) = Day9
                .Solve(new FileInputResolver(9), 25);

            partOne.Should().Be(14144619);
            partTwo.Should().Be(1766397);
        }
    }
}
