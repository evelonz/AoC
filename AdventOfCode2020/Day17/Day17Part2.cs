using AdventOfCode2020.Utility;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace AdventOfCode2020.Day17
{
    internal static class Day17Part2
    {
        internal static long Solve(IInputResolver input)
        {
            var data = input.AsEnumerable().ToList();
            const int padsize = 7;
            const int maxCycles = 6;
            var rows = data.Count;
            var cols = data[0].Length;

            // We will pad the entire 2D map, and add 2 padded maps on z-1, z+1.
            var paddingLayer2D = new List<string>(data.Count);
            for (int i = 0; i < data.Count; i++)
            {
                paddingLayer2D.Add(new string('.', data[0].Length));
            }
            var maps = SetupMap4D(paddingLayer2D, padsize);

            maps[padsize][padsize] = SetupMap2D(data, padsize);
            var tempMaps = new char[(padsize * 2) + 1][][][];

            for (int cycle = 0; cycle < maxCycles; cycle++)
            {
                // To make this simpler, we will assume that we expand both w and z one step per cycle.
                for (int w = padsize - cycle - 1; w <= padsize + cycle + 1; w++)
                {
                    var tempMapsZ = Copy3DArrayLinq(maps[w]);
                    for (int z = padsize - cycle - 1; z <= padsize + cycle + 1; z++)
                    {
                        var map = maps[w][z];
                        var tempMap = Copy2DArrayLinq(map);
                        for (int r = padsize - cycle - 1; r < (rows + padsize + cycle + 1); r++)
                        {
                            for (int c = padsize - cycle - 1; c < (cols + padsize + cycle + 1); c++)
                            {
                                var curr = map[r][c];
                                // Game of life
                                int neighbours = 0;
                                for (int wDelta = -1; wDelta <= 1; wDelta++)
                                {
                                    var topLevelMap = maps[w + wDelta];
                                    for (int zDelta = -1; zDelta <= 1; zDelta++)
                                    {
                                        var zLevelMap = topLevelMap[z + zDelta];
                                        neighbours += GetNumberOfNeighbours(zLevelMap, r, c, zDelta == 0 && wDelta == 0);
                                    }
                                }
                                if (curr == '.' && neighbours == 3)
                                    tempMap[r][c] = '#';
                                else if (curr == '#' && !(neighbours == 2 || neighbours == 3))
                                    tempMap[r][c] = '.';
                            }
                        }
                        //System.Diagnostics.Debug.WriteLine(PrintMap(padsize, tempMap, z, w));
                        tempMapsZ[z] = tempMap;
                    }
                    tempMaps[w] = tempMapsZ;
                }
                for (int w = padsize - cycle - 1; w <= padsize + cycle + 1; w++)
                {
                    for (int z = padsize - cycle - 1; z <= padsize + cycle + 1; z++)
                    {
                        maps[w][z] = tempMaps[w][z];
                    }
                }
            }

            int ans2 = 0;
            for (int w = padsize - maxCycles - 1; w <= padsize + maxCycles + 1; w++)
            {
                for (int z = padsize - maxCycles - 1; z <= padsize + maxCycles + 1; z++)
                {
                    var map = maps[w][z];
                    for (int r = padsize - maxCycles - 1; r < (rows + padsize + maxCycles + 1); r++)
                    {
                        for (int c = padsize - maxCycles - 1; c < (cols + padsize + maxCycles + 1); c++)
                        {
                            if (map[r][c] == '#')
                                ans2++;
                        }
                    }
                }
            }
            return ans2;
        }

        private static char[][][][] SetupMap4D(List<string> paddingLayer2D, int padsize)
        {
            var paddingLayer4D = new char[(padsize * 2) + 1][][][];
            for (int w = 0; w < (padsize * 2) + 1; w++)
            {
                paddingLayer4D[w] = SetupMap3D(paddingLayer2D, padsize);
            }
            return paddingLayer4D;
        }

        private static char[][][] SetupMap3D(List<string> paddingLayer2D, int padsize)
        {
            var paddingLayer3D = new char[(padsize * 2) + 1][][];
            for (int z = 0; z < (padsize * 2) + 1; z++)
            {
                paddingLayer3D[z] = SetupMap2D(paddingLayer2D, padsize);
            }
            return paddingLayer3D;
        }

        private static char[][] SetupMap2D(List<string> data, int padsize)
        {
            var rows = (padsize * 2) + data.Count;
            var cols = (padsize * 2) + data[0].Length;
            var map = new char[rows][];
            var padding = new string('.', padsize);
            var fullRowPad = new string('.', cols);
            var mapIndex = 0;

            for (int i = 0; i < padsize; i++)
            {
                map[i] = fullRowPad.ToArray();
                mapIndex++;
            }
            for (int r = 0; r < data.Count; r++)
            {
                map[mapIndex++] = (padding + data[r] + padding).ToArray();
            }
            for (int i = mapIndex; i < rows; i++)
            {
                map[i] = fullRowPad.ToArray();
            }

            return map;
        }

        private static int GetNumberOfNeighbours(char[][] map, int r, int c, bool sameLayer)
        {
            int neighbours = 0;
            neighbours += GetNeighbourAdjacent(r, c, -1, -1, map);
            neighbours += GetNeighbourAdjacent(r, c, -1, 0, map);
            neighbours += GetNeighbourAdjacent(r, c, -1, +1, map);
            neighbours += GetNeighbourAdjacent(r, c, 0, -1, map);
            if(!sameLayer)
                neighbours += GetNeighbourAdjacent(r, c, 0, 0, map);
            neighbours += GetNeighbourAdjacent(r, c, 0, +1, map);
            neighbours += GetNeighbourAdjacent(r, c, +1, -1, map);
            neighbours += GetNeighbourAdjacent(r, c, +1, 0, map);
            neighbours += GetNeighbourAdjacent(r, c, +1, +1, map);
            return neighbours;
        }

        private static char[][] Copy2DArrayLinq(char[][] source)
        {
            return source.Select(s => s.ToArray()).ToArray();
        }

        private static char[][][] Copy3DArrayLinq(char[][][] source)
        {
            return source.Select(s => s.Select(ss => ss.ToArray()).ToArray()).ToArray();
        }

        private static int GetNeighbourAdjacent(int r, int c, int rd, int cd, char[][] map)
        {
            return map[r + rd][c + cd] == '#' ? 1 : 0;
        }

        private static string PrintMap(int padsize, char[][] map, int z, int w)
        {
            var sb = new StringBuilder();
            var rz = z - padsize;
            var rw = w - padsize;

            sb.Append("z=").Append(rz).Append(", w=").Append(rw).Append(Environment.NewLine);
            for (int i = 0; i < map.Length; i++)
            {
                sb.Append(map[i]).Append(Environment.NewLine);
            }
            sb.Append(Environment.NewLine);

            return sb.ToString();
        }
    }

    public class Test2020Day17Part2
    {
        [Theory]
        [MemberData(nameof(ExampleData))]
        public void SolveProblemExamples(string[] example, int expectedFirst)
        {
            var partTwo = Day17Part2
                .Solve(new MockInputResolver(example));
            partTwo.Should().Be(expectedFirst);
        }

        [Fact]
        public void SolveProblemInput()
        {
            var partTwo = Day17Part2
                .Solve(new FileInputResolver(17));
            partTwo.Should().Be(2308);
        }

        public readonly static List<object[]> ExampleData = new List<object[]>
        {
            new object[] {
                new [] {
                    ".#.",
                    "..#",
                    "###",
                }, 848
            }
        };
    }
}
