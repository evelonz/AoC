using AdventOfCode2020.Utility;
using FluentAssertions;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace AdventOfCode2020.Day5
{
    internal static class Day5
    {
        internal static (int partOne, int partTwo) Solve1(IInputResolver input)
        {
            var data = input.AsEnumerable();
            int currentIndex = 0;
            int highestIndex = 0;
            var seatsTaken = new bool[(127 * 8) + 7];

            foreach (var boardingPass in data)
            {
                var rowInput = boardingPass[0..7];
                var rowMin = 0;
                var rowMax = 127;
                foreach (var chars in rowInput)
                {
                    BiSect(ref rowMin, ref rowMax, chars, 'F');
                }

                var columnInput = boardingPass[^3..];
                var columnMin = 0;
                var columnMax = 7;
                foreach (var chars in columnInput)
                {
                    BiSect(ref columnMin, ref columnMax, chars, 'L');
                }

                currentIndex = (8 * rowMin) + columnMin;
                if (currentIndex > highestIndex)
                    highestIndex = currentIndex;
                seatsTaken[currentIndex] = true;
            }

            bool trailingSeat = false;
            var leadingSeat = false;
            for (int i = 1; i < seatsTaken.Length - 1; i++)
            {
                trailingSeat = seatsTaken[i - 1];
                leadingSeat = seatsTaken[i + 1];
                if (trailingSeat && leadingSeat && !seatsTaken[i])
                    return (highestIndex, i);
            }

            return (highestIndex, -1);
        }

        private static void BiSect(ref int lowIndex, ref int highIndex, char currentChar, char highChar)
        {
            int delta = ((highIndex - lowIndex) / 2) + 1;
            if (currentChar == highChar)
            {
                highIndex -= delta;
            }
            else
            {
                lowIndex += delta;
            }
        }
    }

    public class Test2020Day4
    {
        [Theory]
        [InlineData("FBFBBFFRLR", 357)]
        [InlineData("BFFFBBFRRR", 567)]
        [InlineData("FFFBBBFRRR", 119)]
        [InlineData("BBFFBBFRLL", 820)]
        public void SolveProblemExamples(string example, int expected)
        {
            var (partOne, _) = Day5
                .Solve1(new MockInputResolver(new string[] { example }));

            partOne.Should().Be(expected);
        }

        [Fact]
        public void SolveProblemInput()
        {
            var (partOne, partTwo) = Day5
                .Solve1(new FileInputResolver(5));

            partOne.Should().Be(850);
            partTwo.Should().Be(599);
        }
    }
}
