using AdventOfCode2022.Utility;
using FluentAssertions;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Xunit;

namespace AdventOfCode2022.Day20;

internal static class Day20
{
    internal static (int partOne, int partTwo) Solve(IInputResolver input)
    {
        var numbers = new ObservableCollection<int>();
        var order = new List<int>();
        foreach (var line in input.AsEnumerable())
        {
            var digit = int.Parse(line);
            numbers.Add(digit);
            order.Add(digit);
        }
        var itemCount = numbers.Count;
        var unique = numbers.Distinct().Count();
        if (itemCount != unique)
            throw new Exception();

        foreach (var item in order)
        {
            var index = numbers.IndexOf(item);
            if (item == 0)
                continue;
            else if (item > 0)
            {
                var newIndex = (index + item) % itemCount;

                numbers.Move(index, newIndex);
            }
            else
            {
                var a = (index + item);
                var newIndex = (Math.Abs(a * itemCount) + a) % itemCount;
                if (newIndex > 0)
                    numbers.Move(index, newIndex - 1);
                else
                    numbers.Move(index, itemCount - 1);
            }

            Debug.WriteLine(string.Join(", ", numbers));
        }

        var index2 = numbers.IndexOf(0);
        var one = numbers[(index2 + 1000) % itemCount];
        var two = numbers[(index2 + 2000) % itemCount];
        var three = numbers[(index2 + 3000) % itemCount];

        return (one + two + three, -1);
    }

}

public class Test2022Day20
{
    [Fact]
    public void FirstProblemExamples()
    {
        Day20
            .Solve(new MockInputResolver(new string[] {
                "1",
                "2",
                "-3",
                "3",
                "-2",
                "0",
                "4"
            }))
            .Should().Be((3, -1));
    }

    [Fact]
    public void ProblemInput()
    {
        var (partOne, partTwo) = Day20
            .Solve(new FileInputResolver(20));

        partOne.Should().Be(0);
        //partTwo.Should().Be(0);
    }
}
