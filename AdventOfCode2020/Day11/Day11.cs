using AdventOfCode2020.Utility;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace AdventOfCode2020.Day11
{
    internal static class Day11
    {
        internal static (long partOne, long partTwo) Solve(IInputResolver input)
        {
            var data = input.AsEnumerable().ToList();
            var map = SetupMap(data);

            static int GetNeighbourAdjacent(int r, int c, int rd, int cd, int rows, int cols, char[,] map)
            {
                return map[r + rd, c + cd] == '#' ? 1 : 0;
            }

            long ans1 = RunUntilStable(map, 4, GetNeighbourAdjacent);
            // Reset
            map = SetupMap(data);
            long ans2 = RunUntilStable(map, 5, GetNeighbourInSight);

            return (ans1, ans2);
        }

        private static char[,] SetupMap(List<string> data)
        {
            char[,] map = new char[data.Count + 2, data[0].Length + 2];
            var rows = map.GetLength(0);
            var cols = map.GetLength(1);

            for (int r = 1; r < rows - 1; r++)
            {
                for (int c = 1; c < cols - 1; c++)
                {
                    map[r, c] = data[r - 1][c - 1];
                }
            }

            return map;
        }

        private static long RunUntilStable(char[,] map, int neighbourLimit, Func<int, int, int, int, int, int, char[,], int> getNeighbour)
        {
            var rows = map.GetLength(0);
            var cols = map.GetLength(1);
            char[,] tempMap = new char[rows, cols];
            Array.Copy(map, tempMap, map.GetLength(0) * map.GetLength(1));

            bool doLoop = true;
            while (doLoop)
            {
                for (int r = 1; r < rows - 1; r++)
                {
                    for (int c = 1; c < cols - 1; c++)
                    {
                        var curr = map[r, c];

                        if (curr == '.')
                            continue;
                        // Game of life
                        int neighbours = 0;
                        neighbours += getNeighbour(r, c, -1, -1, rows, cols, map);
                        neighbours += getNeighbour(r, c, -1,  0, rows, cols, map);
                        neighbours += getNeighbour(r, c, -1, +1, rows, cols, map);
                        neighbours += getNeighbour(r, c,  0, -1, rows, cols, map);
                        //neighbors += getNeighbour(r, c,  0, 0, rows, cols, map);
                        neighbours += getNeighbour(r, c, 0, +1, rows, cols, map);
                        neighbours += getNeighbour(r, c, +1, -1, rows, cols, map);
                        neighbours += getNeighbour(r, c, +1,  0, rows, cols, map);
                        neighbours += getNeighbour(r, c, +1, +1, rows, cols, map);

                        if (curr == 'L' && neighbours == 0)
                            tempMap[r, c] = '#';
                        else if (curr == '#' && neighbours >= neighbourLimit)
                            tempMap[r, c] = 'L';


                    }
                }

                var done = true;
                for (int r = 1; r < rows - 1; r++)
                {
                    for (int c = 1; c < cols - 1; c++)
                    {
                        if (map[r, c] != tempMap[r, c])
                        {
                            done = false;
                            break;
                        }
                    }
                    if (!done)
                        break;
                }
                // Check exit condition.
                doLoop = !done;
                Array.Copy(tempMap, map, map.GetLength(0) * map.GetLength(1));
            }

            long ans = 0;
            for (int r = 1; r < rows - 1; r++)
            {
                for (int c = 1; c < cols - 1; c++)
                {
                    if (map[r, c] == '#')
                        ans++;
                }
            }

            return ans;
        }

        private static int GetNeighbourInSight(int r, int c, int rd, int cd, int rows, int cols, char[,] map)
        {
            while (r > 0 && r < rows-1 && c > 0 && c < cols-1)
            {
                r += rd;
                c += cd;
                var nextInSight = map[r, c];
                if (nextInSight == 'L')
                    return 0;
                else if (nextInSight == '#')
                    return 1;
            }

            return 0;
        }
    }

    public class Test2020Day11
    {
        [Fact]
        public void SolveProblemExamples()
        {
            var (partOne, partTwo) = Day11
                .Solve(new MockInputResolver(exampleData));
            partOne.Should().Be(37);
            partTwo.Should().Be(26);
        }

        [Fact]
        public void SolveProblemInput()
        {
            var (partOne, partTwo) = Day11
                .Solve(new FileInputResolver(11));

            partOne.Should().Be(2299);
            partTwo.Should().Be(2047);
        }

        private static readonly string[] exampleData = new string[] {
            "L.LL.LL.LL",
            "LLLLLLL.LL",
            "L.L.L..L..",
            "LLLL.LL.LL",
            "L.LL.LL.LL",
            "L.LLLLL.LL",
            "..L.L.....",
            "LLLLLLLLLL",
            "L.LLLLLL.L",
            "L.LLLLL.LL",
        };
    }
}
