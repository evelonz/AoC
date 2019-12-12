using AdventOfCode.Utility;
using FluentAssertions;
using Xunit;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Year2019.Tests
{
    public class Tester2019_10
    {
        [Theory]
        [MemberData(nameof(FirstExampleData))]
        public void FirstProblemExamples(string[] example, string expected)
        {
            Solver2019_10.SolveFirst(new MockInputResolver(example)).Should().Be(expected);
        }

        [Fact]
        public void FirstProblemMyInput()
        {
            Solver2019_10.SolveFirst(new FileInputResolver(2019, 10)).Should().Be("329");
        }

        [Theory]
        [MemberData(nameof(SecondExampleData))]
        public void SecondProblemExamples(string[] example, string expected, (int x, int y) center, int targetCount)
        {
            Solver2019_10.SolveSecond(new MockInputResolver(example), center, targetCount)
                .Should().Be(expected);
        }

        [Fact]
        public void SecondProblemMyInput()
        {
            Solver2019_10.SolveSecond(new FileInputResolver(2019, 10), (25, 31)).Should().Be("512");
        }

        public static IEnumerable<object[]> FirstExampleData()
        {
            return allData.Take(5).Select(s => s.Take(2).ToArray());
        }

        public static IEnumerable<object[]> SecondExampleData()
        {
            return allData.Skip(4).Select(s => new object[] { s[0], s[2], s[3], s[4] }); ;
        }

        private readonly static List<object[]> allData = new List<object[]>
        {
            new object[] {
                new [] {
                    ".#..#",
                    ".....",
                    "#####",
                    "....#",
                    "...##"}, "8"
            },
            new object[] {
                new [] {
                    "......#.#.",
                    "#..#.#....",
                    "..#######.",
                    ".#.#.###..",
                    ".#..#.....",
                    "..#....#.#",
                    "#..#....#.",
                    ".##.#..###",
                    "##...#..#.",
                    ".#....####"}, "33"
            },
            new object[] {
                new [] {
                    "#.#...#.#.",
                    ".###....#.",
                    ".#....#...",
                    "##.#.#.#.#",
                    "....#.#.#.",
                    ".##..###.#",
                    "..#...##..",
                    "..##....##",
                    "......#...",
                    ".####.###."}, "35"
            },
            new object[] {
                new [] {
                    ".#..#..###",
                    "####.###.#",
                    "....###.#.",
                    "..###.##.#",
                    "##.##.#.#.",
                    "....###..#",
                    "..#.#..#.#",
                    "#..#.#.###",
                    ".##...##.#",
                    ".....#.#.."}, "41"
            },
            new object[] {
                new [] {
                    ".#..##.###...#######",
                    "##.############..##.",
                    ".#.######.########.#",
                    ".###.#######.####.#.",
                    "#####.##.#.##.###.##",
                    "..#####..#.#########",
                    "####################",
                    "#.####....###.#.#.##",
                    "##.#################",
                    "#####.##.###..####..",
                    "..######..##.#######",
                    "####.##.####...##..#",
                    ".#####..#.######.###",
                    "##...#.##########...",
                    "#.##########.#######",
                    ".####.#.###.###.#.##",
                    "....##.##.###..#####",
                    ".#.#.###########.###",
                    "#.#.#.#####.####.###",
                    "###.##.####.##.#..##"}, "210", "802", (11, 13), 200
            },
            new object[] {
                new [] {
                    ".#....#####...#..",
                    "##...##.#####..##",
                    "##...#...#.#####.",
                    "..#.....X...###..",
                    "..#.#.....#....##",}, "-1", "1403", (8, 3), 36
            },
        };
    }
}
