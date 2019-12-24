using AdventOfCode.Utility;
using System;
using System.Collections.Generic;

namespace AdventOfCode.Year2019
{
    static class Solver2019_24_1
    {
        public static string Solve(IInputResolver input, bool second = false)
        {
            var inData = input.AsEnumerable();

            var hits = new HashSet<long>(1000);


            var map = new bool[5][];
            var tempMap = new bool[5][];
            int i = 0;
            foreach (var row in inData)
            {
                var mapRow = new bool[5];
                int j = 0;
                foreach (var cell in row)
                {
                    mapRow[j] = cell == '#';
                    j++;
                }
                map[i] = mapRow;
                tempMap[i] = new bool[5];
                i++;
            }

            PrintMap(map);
            hits.Add(CalculateScore(map));

            while (true)
            {
                i = 0;
                foreach (var row in map)
                {
                    // Get up, down, left, right.
                    // And count bugs. Then set new value based on it.
                    int j = 0;
                    foreach (var cell in row)
                    {
                        var up = i == 0 ? false : map[i - 1][j];
                        var down = i == map.Length - 1 ? false : map[i + 1][j];
                        var left = j == 0 ? false : map[i][j - 1];
                        var right = j == row.Length - 1 ? false : map[i][j + 1];
                        var sum = (up ? 1 : 0) + (down ? 1 : 0) 
                            + (left ? 1 : 0) + (right ? 1 : 0);
                        if (map[i][j])
                            tempMap[i][j] = sum == 1;
                        else
                            tempMap[i][j] = sum == 1 || sum == 2;
                        j++;
                    }
                    i++;
                }

                var sum2 = CalculateScore(tempMap);
                // Check end condition
                if (hits.Contains(sum2))
                {
                    PrintMap(tempMap);
                    // Calc score.
                    return sum2.ToString();
                }
                hits.Add(sum2);

                // Swap map around.
                var temp = map;
                map = tempMap;
                tempMap = temp;

                PrintMap(map);
            }

            return "";
        }

        private static void PrintMap(bool[][] map)
        {
            Console.WriteLine("");
            foreach (var row in map)
            {
                foreach (var cell in row)
                    Console.Write(cell ? '#' : '.');
                Console.WriteLine("");
            }
        }

        private static long CalculateScore(bool[][] map)
        {
            long sum = 0;
            int power = 0;
            foreach (var row in map)
            {
                foreach (var cell in row)
                {
                    if(cell)
                        sum += (long)Math.Pow(2, power);
                    power++;
                }
            }
            return sum;
        }
    }

}
