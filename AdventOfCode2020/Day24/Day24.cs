using AdventOfCode2020.Utility;
using FluentAssertions;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace AdventOfCode2020.Day24
{
    internal static class Day24
    {
        internal static (long partOne, long partTwo) Solve(IInputResolver input)
        {
            var data = input.AsEnumerable();
            var tiles = new Dictionary<(int c, int r), bool>();
            foreach (var instruction in data)
            {
                var direction = string.Empty;
                (int c, int r) coord = (0, 0);
                foreach (var latitude in instruction)
                {
                    direction += latitude;
                    // We use offset coordinates with even-r
                    var even = coord.r % 2 == 0;
                    switch (direction)
                    {
                        case "e":
                            coord.c++;
                            direction = string.Empty;
                            break;
                        case "w":
                            coord.c--;
                            direction = string.Empty;
                            break;
                        case "se":
                            if(even)
                                coord.c++;
                            coord.r++;
                            direction = string.Empty;
                            break;
                        case "sw":
                            if(!even)
                                coord.c--;
                            coord.r++;
                            direction = string.Empty;
                            break;
                        case "ne":
                            if(even)
                                coord.c++;
                            coord.r--;
                            direction = string.Empty;
                            break;
                        case "nw":
                            if (!even)
                                coord.c--;
                            coord.r--;
                            direction = string.Empty;
                            break;
                    }
                }
                if (tiles.TryGetValue(coord, out var isBlack))
                    tiles[coord] = !isBlack;
                else
                    tiles[coord] = true;
            }
            long ans1 = tiles.Count(x => x.Value);

            // Part two.
            // For each black tile, check it, and all it's adjecent tiles.
            // Stay black of 1-2 adjecent tiles are black.
            // If white and 2 black adjecent tiles, flip to black.
            var tempTiles = new Dictionary<(int c, int r), bool>(tiles.Count);
            for (int day = 1; day <= 100; day++)
            {
                foreach (var tile in tiles.Where(x => x.Value))
                {
                    SetNextState(tiles, tempTiles, tile.Key);
                    foreach (var adjecentTile in GetAdjacentTiles(tile.Key))
                    {
                        SetNextState(tiles, tempTiles, adjecentTile);
                    }
                }
                tiles = tempTiles;
                tempTiles = new Dictionary<(int c, int r), bool>(tiles.Count);
            }
            long ans2 = tiles.Count(x => x.Value);

            return (ans1, ans2);
        }

        private static void SetNextState(
            Dictionary<(int c, int r), bool> tiles,
            Dictionary<(int c, int r), bool> tempTiles,
            (int c, int r) currentTile)
        {
            if (!tempTiles.ContainsKey(currentTile))
            {
                var adjecent = CountNeighbours(currentTile, tiles);
                _ = tiles.TryGetValue(currentTile, out var isBlack);
                if (isBlack)
                    tempTiles[currentTile] = adjecent == 1 || adjecent == 2;
                else
                    tempTiles[currentTile] = adjecent == 2;
            }
        }

        private static int CountNeighbours((int c, int r) coord, Dictionary<(int c, int r), bool> tiles)
        {
            int count = 0;
            foreach (var adjecentTile in GetAdjacentTiles(coord))
            {
                if (tiles.TryGetValue(adjecentTile, out var value))
                    count += value ? 1 : 0;
            }
            return count;
        }

        internal static IEnumerable<(int c, int r)> GetAdjacentTiles((int c, int r) coord)
        {
            var (c, r) = coord;
            var even = r % 2 == 0;
            yield return (c - 1, r); // w
            yield return (c + 1, r); // e
            yield return (c + (!even ? -1 : 0), r - 1); // nw
            yield return (c + (even ? 1 : 0), r - 1); // ne
            yield return (c + (!even ? -1 : 0), r + 1); // sw
            yield return (c + (even ? 1 : 0), r + 1); // se
        }
    }

    public class Test2020Day24
    {
        [Theory]
        [MemberData(nameof(ExampleData))]
        public void SolveProblemExamples(string[] example, int expectedFirst, int expectedSecond)
        {
            var (partOne, partTwo) = Day24
                .Solve(new MockInputResolver(example));
            partOne.Should().Be(expectedFirst);
            partTwo.Should().Be(expectedSecond);
        }

        [Fact]
        public void SolveProblemInput()
        {
            var (partOne, partTwo) = Day24
                .Solve(new FileInputResolver(24));
            partOne.Should().Be(436);
            partTwo.Should().Be(4133);
        }

        [Fact]
        public void AdjacentTilesEven()
        {
            var expected = new (int, int)[] {
                (2, 1), (3, 1),
                (1, 2), (3, 2),
                (2, 3), (3, 3),
            };
            var sut = Day24.GetAdjacentTiles((2, 2));
            sut.Should().BeEquivalentTo(expected);
        }
        [Fact]
        public void AdjacentTilesOdd()
        {
            var expected = new (int, int)[] {
                (2, 2), (3, 2),
                (2, 3), (4, 3),
                (2, 4), (3, 4),
            };
            var sut = Day24.GetAdjacentTiles((3, 3));
            sut.Should().BeEquivalentTo(expected);
        }

        public readonly static List<object[]> ExampleData = new List<object[]>
        {
            //new object[] {
            //    new [] {
            //        "esew",
            //        "nwwswee",
            //    }, 2, 0
            //},
            new object[] {
                new [] {
                    "sesenwnenenewseeswwswswwnenewsewsw",
                    "neeenesenwnwwswnenewnwwsewnenwseswesw",
                    "seswneswswsenwwnwse",
                    "nwnwneseeswswnenewneswwnewseswneseene",
                    "swweswneswnenwsewnwneneseenw",
                    "eesenwseswswnenwswnwnwsewwnwsene",
                    "sewnenenenesenwsewnenwwwse",
                    "wenwwweseeeweswwwnwwe",
                    "wsweesenenewnwwnwsenewsenwwsesesenwne",
                    "neeswseenwwswnwswswnw",
                    "nenwswwsewswnenenewsenwsenwnesesenew",
                    "enewnwewneswsewnwswenweswnenwsenwsw",
                    "sweneswneswneneenwnewenewwneswswnese",
                    "swwesenesewenwneswnwwneseswwne",
                    "enesenwswwswneneswsenwnewswseenwsese",
                    "wnwnesenesenenwwnenwsewesewsesesew",
                    "nenewswnwewswnenesenwnesewesw",
                    "eneswnwswnwsenenwnwnwwseeswneewsenese",
                    "neswnwewnwnwseenwseesewsenwsweewe",
                    "wseweeenwnesenwwwswnew",
                }, 10, 2208
            }
        };
    }
}
