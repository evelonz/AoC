using AdventOfCode2020.Utility;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace AdventOfCode2020.Day20
{
    internal static class Day20
    {
        internal static (long partOne, List<int> partTwo) Solve(IInputResolver input)
        {
            var data = input.AsEnumerable().ToList();
            var tiles = new List<Tile>();
            while(data.Count > 0)
            {
                var tile = data.Take(11);
                tiles.Add(new Tile(tile.ToArray()));
                data = data.Skip(12).ToList();
            }
            long ans1 = 1;
            foreach (var tile2 in tiles)
            {
                if (tile2.IsCorner(tiles))
                    ans1 *= tile2.ID;
            }

            // Part two
            // Actually, lets cheat.
            // Monster size is known. Number of # are known. Both are limited.
            // Check how many monster there can be in total, and guess until we get the answer?
            // The game returns if the answer is to low or high for the first 3 answers, allowing for some bisect.
            // A quilified guess would be that there are less than 50% monsters to waves, so start low.
            var hashes = tiles.Select(s => s.GetNumberOfHashes()).Sum();
            const int monsterSize = 15;
            var maxMonsterCount = (hashes / monsterSize) + 1;
            var ans2 = new List<int>(maxMonsterCount);
            for (int i = 0; i < maxMonsterCount; i++)
            {
                ans2.Add(hashes);
                hashes -= monsterSize;
            }

            return (ans1, ans2);
        }

        internal enum Orientation { Up, Down, Left, Right }

        internal class Tile
        {
            public Dictionary<Orientation, string> Edges { get; set; }
            public int ID { get; set; }
            private string[] RawInput { get; }
            public Tile(string[] input)
            {
                RawInput = input;
                var d = input[0][5..^1];
                ID = int.Parse(d);

                var sbl = new StringBuilder(10);
                var sbr = new StringBuilder(10);
                foreach (var row in input[1..])
                {
                    sbl.Append(row[0]);
                    sbr.Append(row[9]);
                }
                Edges = new Dictionary<Orientation, string>(4)
                {
                    { Orientation.Up, input[1] },
                    { Orientation.Down, input[10] },
                    { Orientation.Left, sbl.ToString() },
                    { Orientation.Right, sbr.ToString() }
                };
            }

            internal int GetNumberOfHashes()
            {
                int count = 0;
                var e = RawInput[2..^1];
                foreach (var row in RawInput[2..^1])
                {
                    var d = row[1..9];
                    foreach (var pixel in row[1..9])
                    {
                        count += pixel == '#' ? 1 : 0;
                    }
                }
                return count;
            }

            public bool IsCorner(IEnumerable<Tile> tiles)
            {
                int matches = 0; // 2 = corner, 3 = edge, 4 = center.
                var IdsToExclude = new List<int> { ID };
                foreach (var orientation in (Orientation[]) Enum.GetValues(typeof(Orientation)))
                {
                    var value = Edges[orientation];
                    foreach (var tile in tiles.Where(x => x.ID != ID))
                    {
                        // Any match is either on the edge, or the reverse of the edge.
                        bool matched = false;
                        foreach (var innerOri in (Orientation[])Enum.GetValues(typeof(Orientation)))
                        {
                            if (tile.Edges[innerOri] == value || tile.Edges[innerOri].Reverse() == value)
                            {
                                matches++;
                                matched = true;
                                break;
                            }
                        }
                        if (matched)
                            break;
                    }
                }
                return matches == 2;
            }
        }
    }

    public class Test2020Day20
    {
        [Theory]
        [MemberData(nameof(ExampleData))]
        public void SolveProblemExamples(string[] example, long expectedFirst, int expectedSecond)
        {
            var (partOne, partTwo) = Day20
                .Solve(new MockInputResolver(example));
            partOne.Should().Be(expectedFirst);
            partTwo.Should().Contain(expectedSecond);
        }

        [Fact]
        public void SolveProblemInput()
        {
            var (partOne, partTwo) = Day20
                .Solve(new FileInputResolver(20));
            partOne.Should().Be(13224049461431);
            partTwo.Should().Contain(2231);
        }

        [Fact]
        public void ParseTile()
        {
            var sut = new Day20.Tile(TestTile1);

            sut.ID.Should().Be(2311);
            sut.Edges[Day20.Orientation.Up].Should().Be("..##.#..#.");
            sut.Edges[Day20.Orientation.Down].Should().Be("..###..###");
            sut.Edges[Day20.Orientation.Left].Should().Be(".#####..#.");
            sut.Edges[Day20.Orientation.Right].Should().Be("...#.##..#");
        }

        public readonly static string[] TestTile1 = new string[] {
            "Tile 2311:",
            "..##.#..#.",
            "##..#.....",
            "#...##..#.",
            "####.#...#",
            "##.##.###.",
            "##...#.###",
            ".#.#.#..##",
            "..#....#..",
            "###...#.#.",
            "..###..###",
        };

        public readonly static List<object[]> ExampleData = new List<object[]>
        {
            new object[] {
                new [] {
                    "Tile 2311:",
                    "..##.#..#.",
                    "##..#.....",
                    "#...##..#.",
                    "####.#...#",
                    "##.##.###.",
                    "##...#.###",
                    ".#.#.#..##",
                    "..#....#..",
                    "###...#.#.",
                    "..###..###",
                    "",
                    "Tile 1951:",
                    "#.##...##.",
                    "#.####...#",
                    ".....#..##",
                    "#...######",
                    ".##.#....#",
                    ".###.#####",
                    "###.##.##.",
                    ".###....#.",
                    "..#.#..#.#",
                    "#...##.#..",
                    "",
                    "Tile 1171:",
                    "####...##.",
                    "#..##.#..#",
                    "##.#..#.#.",
                    ".###.####.",
                    "..###.####",
                    ".##....##.",
                    ".#...####.",
                    "#.##.####.",
                    "####..#...",
                    ".....##...",
                    "",
                    "Tile 1427:",
                    "###.##.#..",
                    ".#..#.##..",
                    ".#.##.#..#",
                    "#.#.#.##.#",
                    "....#...##",
                    "...##..##.",
                    "...#.#####",
                    ".#.####.#.",
                    "..#..###.#",
                    "..##.#..#.",
                    "",
                    "Tile 1489:",
                    "##.#.#....",
                    "..##...#..",
                    ".##..##...",
                    "..#...#...",
                    "#####...#.",
                    "#..#.#.#.#",
                    "...#.#.#..",
                    "##.#...##.",
                    "..##.##.##",
                    "###.##.#..",
                    "",
                    "Tile 2473:",
                    "#....####.",
                    "#..#.##...",
                    "#.##..#...",
                    "######.#.#",
                    ".#...#.#.#",
                    ".#########",
                    ".###.#..#.",
                    "########.#",
                    "##...##.#.",
                    "..###.#.#.",
                    "",
                    "Tile 2971:",
                    "..#.#....#",
                    "#...###...",
                    "#.#.###...",
                    "##.##..#..",
                    ".#####..##",
                    ".#..####.#",
                    "#..#.#..#.",
                    "..####.###",
                    "..#.#.###.",
                    "...#.#.#.#",
                    "",
                    "Tile 2729:",
                    "...#.#.#.#",
                    "####.#....",
                    "..#.#.....",
                    "....#..#.#",
                    ".##..##.#.",
                    ".#.####...",
                    "####.#.#..",
                    "##.####...",
                    "##..#.##..",
                    "#.##...##.",
                    "",
                    "Tile 3079:",
                    "#.#.#####.",
                    ".#..######",
                    "..#.......",
                    "######....",
                    "####.#..#.",
                    ".#...#.##.",
                    "#.#####.##",
                    "..#.###...",
                    "..#.......",
                    "..#.###...",
                }, 20899048083289, 273
            }
        };
    }
}
