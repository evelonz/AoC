using AdventOfCode2022.Utility;
using FluentAssertions;
using System.Diagnostics;
using Xunit;

namespace AdventOfCode2022.Day3;

internal static class Day3
{
    internal static (int partOne, int partTwo) Solve(IInputResolver input)
    {
        const string prio = "_abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
        var sumOne = 0;
        var sumTwo = 0;
        int groupIndex = 0;
        var groupItems = new int[prio.Length];

        foreach (var item in input.AsEnumerable())
        {
            var hash = new HashSet<char>(item.Length / 2);
            for (int i = 0; i < item.Length/2; i++)
            {
                hash.Add(item[i]);
            }

            for (int i = item.Length / 2; i < item.Length; i++)
            {
                if (hash.Contains(item[i]))
                {
                    sumOne += prio.IndexOf(item[i]);
                    break;
                }
            }

            foreach (var item2 in item.Distinct())
            {
                groupItems[prio.IndexOf(item2)]++;
            }

            if(++groupIndex == 3)
            {
                sumTwo += Array.IndexOf(groupItems, 3);
                groupIndex = 0;
                Array.Clear(groupItems);
            }
        }

        return (sumOne, sumTwo);
    }

}

public class Test2020Day3
{
    [Fact]
    public void FirstProblemExamples()
    {
        Day3
            .Solve(new MockInputResolver(new string[] {
                "vJrwpWtwJgWrhcsFMMfFFhFp",
                "jqHRNqRjqzjGDLGLrsFMfFZSrLrFZsSL",
                "PmmdzqPrVvPwwTWBwg",
                "wMqvLMZHhHMvwLHjbvcjnnSBnvTQFn",
                "ttgJtRGJQctTZtZT",
                "CrZsJsPPZsGzwwsLwLmpwMDw"
            }))
            .Should().Be((157, 70));
    }

    [Fact]
    public void ProblemInput()
    {
        var (partOne, partTwo) = Day3
            .Solve(new FileInputResolver(3));

        partOne.Should().Be(7908);
        partTwo.Should().Be(2838);
    }
}
