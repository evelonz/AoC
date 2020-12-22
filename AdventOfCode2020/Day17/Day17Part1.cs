using AdventOfCode2020.Utility;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace AdventOfCode2020.Day17
{
    internal static class Day17Part1
    {
        internal static long Solve(IInputResolver input)
        {
            var data = input.AsEnumerable().ToList();
            // 6 cycles, + 1 padding gives us an end z from -7 to 7 => 15 total levels for z.
            // 6 cycles, + 1 padding gives us an end r|c from -7 to 7 + (r|c). (assuming square map in this task).
            // So total padding on each side is 7 for z|r|c.
            // Well we actually have to consider cells outside of the current map as well (as else it cannot expand). Perhaps this can be made easier?
            // This means we need a padding of 8 instead, and always have 2 padded z-layers at all times.
            // Use current cycle to access the correct index.
            const int padsize = 7;
            const int maxCycles = 6;
            var rows = data.Count;
            var cols = data[0].Length;
            var maps = new char[(padsize * 2) + 1][][];
            var tempMaps = new char[(padsize * 2) + 1][][];
            var activeZLayerMin = 0;
            var activeZLayerMax = 0;

            // We will pad the entire 2D map, and add 2 padded maps on z-1, z+1.
            maps[padsize] = SetupMap(data, padsize);

            var zlayerpadding = new List<string>(data.Count);
            for (int i = 0; i < data.Count; i++)
            {
                zlayerpadding.Add(new string('.', data[0].Length));
            }
            maps[padsize - 1] = SetupMap(zlayerpadding, padsize);
            maps[padsize + 1] = SetupMap(zlayerpadding, padsize);

            for (int cycle = 0; cycle < 6; cycle++)
            {
                // Check of outer z layer can be made simpler. It actually need to only check one layer for 3 active cells.
                var map = maps[padsize - activeZLayerMin];
                var newZMapNeg = SetupMap(zlayerpadding, padsize);
                var addZLayerNegative = CheckNewZLayer(padsize, newZMapNeg, map, cycle, rows, cols);

                if (addZLayerNegative)
                    System.Diagnostics.Debug.WriteLine(MapToString(newZMapNeg));

                for (int z = padsize - activeZLayerMin; z <= padsize + activeZLayerMax; z++)
                {
                    map = maps[z];
                    var tempMap = CopyArrayLinq(map);
                    for (int r = padsize - cycle - 1; r < (rows + padsize + cycle + 1); r++)
                    {
                        for (int c = padsize - cycle - 1; c < (cols + padsize + cycle + 1); c++)
                        {
                            var curr = map[r][c];
                            // Game of life
                            int neighbours = 0;
                            for (int zDelta = -1; zDelta <= 1; zDelta++)
                            {
                                var zLevelMap = maps[z + zDelta];
                                neighbours += GetNumberOfNeighbours(zLevelMap, r, c, zDelta == 0);
                            }
                            if (curr == '.' && neighbours == 3)
                                tempMap[r][c] = '#';
                            else if (curr == '#' && !(neighbours == 2 || neighbours == 3))
                                tempMap[r][c] = '.';
                        }
                    }
                    System.Diagnostics.Debug.WriteLine(MapToString(tempMap));
                    tempMaps[z] = tempMap;
                }
                
                map = maps[padsize - activeZLayerMin];
                var newZMapPos = SetupMap(zlayerpadding, padsize);
                var addZLayerPosetive = CheckNewZLayer(padsize, newZMapPos, map, cycle, rows, cols);
                
                for (int z = padsize - activeZLayerMin; z <= padsize + activeZLayerMax; z++)
                {
                    maps[z] = tempMaps[z];
                }

                if (addZLayerNegative)
                {
                    activeZLayerMin++;
                    maps[padsize - activeZLayerMin] = newZMapNeg;
                    maps[padsize - activeZLayerMin - 1] = SetupMap(zlayerpadding, padsize);
                    
                }
                if (addZLayerPosetive)
                {
                    System.Diagnostics.Debug.WriteLine(MapToString(newZMapPos));
                    activeZLayerMax++;
                    maps[padsize + activeZLayerMax] = newZMapPos;
                    maps[padsize + activeZLayerMax + 1] = SetupMap(zlayerpadding, padsize);
                }
            }
            int ans1 = 0;
            for (int z = padsize - activeZLayerMin; z <= padsize + activeZLayerMax; z++)
            {
                var map = maps[z];
                for (int r = padsize - maxCycles - 1; r < (rows + padsize + maxCycles + 1); r++)
                {
                    for (int c = padsize - maxCycles - 1; c < (cols + padsize + maxCycles + 1); c++)
                    {
                        if (map[r][c] == '#')
                            ans1++;
                    }
                }
            }

            return ans1;
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

        private static char[][] CopyArrayLinq(char[][] source)
        {
            return source.Select(s => s.ToArray()).ToArray();
        }

        private static int GetNeighbourAdjacent(int r, int c, int rd, int cd, char[][] map)
        {
            return map[r + rd][c + cd] == '#' ? 1 : 0;
        }

        private static string MapToString(char[][] map)
        {
            var sb = new StringBuilder();
            for (int i = 0; i < map.Length; i++)
            {
                sb.Append(map[i]);
                sb.Append(Environment.NewLine);
            }
            return sb.ToString();
        }

        private static char[][] SetupMap(List<string> data, int padsize)
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

        private static bool CheckNewZLayer(int padsize, char[][] newZMap, char[][] map, int cycle, int rows, int cols)
        {
            var addZLayer = false;
            for (int r = padsize - cycle -1; r < (rows + padsize + 1); r++)
            {
                for (int c = padsize - cycle - 1; c < (cols + padsize + 1); c++)
                {
                    // Game of life
                    int neighbours = GetNumberOfNeighbours(map, r, c, false);
                    if (neighbours == 3)
                    {
                        // Activate
                        newZMap[r][c] = '#';
                        addZLayer = true;
                    }
                }
            }

            return addZLayer;
        }
    }

    public class Test2020Day17
    {
        [Theory]
        [MemberData(nameof(ExampleData))]
        public void SolveProblemExamples(string[] example, int expectedFirst)
        {
            var partOne = Day17Part1
                .Solve(new MockInputResolver(example));
            partOne.Should().Be(expectedFirst);
        }

        [Fact]
        public void SolveProblemInput()
        {
            var partOne = Day17Part1
                .Solve(new FileInputResolver(17));
            partOne.Should().Be(207);
        }

        public readonly static List<object[]> ExampleData = new List<object[]>
        {
            new object[] {
                new [] {
                    ".#.",
                    "..#",
                    "###",
                }, 112
            }
        };
    }
}
