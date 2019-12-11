using AdventOfCode.Utility;
using FluentAssertions;
using Xunit;
using System;

namespace AdventOfCode.Year2019.Tests
{
    public class Tester2019_11
    {
        [Fact]
        public void FirstProblemMyInput()
        {
            Solver2019_11_1.Solve(new FileInputResolver(2019, 11)).Should().Be("2041");
        }

        [Fact]
        public void SecondProblemMyInput()
        {
            var expected =
                ".####.###..####.###..#..#.####.####.###...." + Environment.NewLine +
                "....#.#..#....#.#..#.#.#..#.......#.#..#..." + Environment.NewLine +
                "...#..#..#...#..#..#.##...###....#..#..#..." + Environment.NewLine +
                "..#...###...#...###..#.#..#.....#...###...." + Environment.NewLine +
                ".#....#.#..#....#....#.#..#....#....#.#...." + Environment.NewLine +
                ".####.#..#.####.#....#..#.####.####.#..#..." + Environment.NewLine;

            Solver2019_11_2.Solve(new FileInputResolver(2019, 11)).Should().Be(expected);
        }
    }
}
