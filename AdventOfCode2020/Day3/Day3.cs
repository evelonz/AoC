using AdventOfCode2020.Utility;
using FluentAssertions;
using System.Linq;
using Xunit;

namespace AdventOfCode2020.Day3
{
    internal static class Day3
    {
        internal static (long partOne, long partTwo) Solve(IInputResolver input)
        {
            var data = input.AsEnumerable().ToList();
            var numberOfLines = data.Count;
            var width = data[0].Length;
            var map = new string[numberOfLines];
            int m = 0;
            foreach (var row in data)
            {
                map[m++] = row;
            }

            var runshits = new long[5];

            int[,] runs = new int[,] { { 1, 1 }, { 3, 1 }, { 5, 1 }, { 7, 1 }, { 1, 2 } };

            for (int run = 0; run < 5; run++)
            {
                var right = runs[run, 0];
                var down = runs[run, 1];
                int line = 0;
                int kol = 0;
                for (int i = 0; i < numberOfLines - 1; i += down)
                {
                    line += down;
                    kol = (kol + right) % width;
                    var l = map[line];
                    System.Console.WriteLine(l + " " + kol);
                    var onmap = l[kol];

                    if (onmap == '#')
                        runshits[run]++;
                }
            }

            for (int j = 1; j < 5; j++)
            {
                runshits[0] *= runshits[j];
            }
            return (runshits[1], runshits[0]);
        }

    }

    public class Test2020Day3
    {
        [Fact]
        public void SolveProblemExamples()
        {
            var (partOne, partTwo) = Day3
                .Solve(new MockInputResolver(exampleData));
            partOne.Should().Be(7);
            partTwo.Should().Be(336);
        }

        [Fact]
        public void SolveProblemInput()
        {
            var (partOne, partTwo) = Day3
                .Solve(new FileInputResolver(3));

            partOne.Should().Be(262);
            partTwo.Should().Be(2698900776);
        }

        private static readonly string[] exampleData = new string[] {
            "..##.......",
            "#...#...#..",
            ".#....#..#.",
            "..#.#...#.#",
            ".#...##..#.",
            "..#.##.....",
            ".#.#.#....#",
            ".#........#",
            "#.##...#...",
            "#...##....#",
            ".#..#...#.#"
        };

    }
}
