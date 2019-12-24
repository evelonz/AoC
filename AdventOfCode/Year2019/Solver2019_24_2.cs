using AdventOfCode.Utility;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Year2019
{
    enum Direction : int
    {
        Up = 0,
        Down = 1,
        Left = 2,
        Right = 3
    }
    class RecMaps
    {
        // Includes start (0)
        public bool[][][] PosMaps { get; set; }
        public bool[][][] NegMaps { get; set; }

        public int MinMap { get; set; }
        public int MaxMap { get; set; }

        public void PrintMaps()
        {
            for (int i = MinMap; i < 0; i++)
            {
                Console.WriteLine($"Depth: {i}");
                Solver2019_24_2.PrintMap(NegMaps[Math.Abs(i)]);
            }
            for (int i = 0; i <= MaxMap; i++)
            {
                Console.WriteLine($"Depth: {i}");
                Solver2019_24_2.PrintMap(PosMaps[i]);
            }
        }

        public int SumBugs()
        {
            int sum = 0;
            for (int i = MinMap; i < 0; i++)
            {
                sum += CountBugs(NegMaps[Math.Abs(i)]);
            }
            for (int i = 0; i <= MaxMap; i++)
            {
                sum += CountBugs(PosMaps[i]);
            }
            return sum;
        }

        private static int CountBugs(bool[][] map)
        {
            var sum = 0;
            foreach (var row in map)
            {
                foreach (var cell in row)
                {
                    sum += cell ? 1 : 0;
                }
            }
            return sum;
        }

        public RecMaps(bool[][] center)
        {
            PosMaps = new bool[1000][][];
            NegMaps = new bool[1000][][];
            PosMaps[0] = center;
        }

        public void UpdateMaps(List<(int dim, bool[][] map)> newMaps)
        {
            PosMaps = new bool[1000][][];
            NegMaps = new bool[1000][][];
            MinMap = newMaps.Min(m => m.dim);
            MaxMap = newMaps.Max(m => m.dim);
            foreach (var item in newMaps.Where(x => x.dim >= 0))
            {
                PosMaps[item.dim] = item.map;
            }
            foreach (var item in newMaps.Where(x => x.dim < 0))
            {
                NegMaps[Math.Abs(item.dim)] = item.map;
            }
        }

        public int GetOuterSum(int index, Direction direction)
        {
            var maps = (index >= 0) ? PosMaps : NegMaps;
            var index2 = (index >= 0) ? index : -index;
            var ret = maps[index2];
            if (ret == null)
                return 0;
            if (direction == Direction.Up)
                return ret[1][2] ? 1 : 0;
            else if (direction == Direction.Down)
                return ret[3][2] ? 1 : 0;
            else if (direction == Direction.Left)
                return ret[2][1] ? 1 : 0;
            return ret[2][3] ? 1 : 0;
        }

        public int GetInnerSum(int index, Direction direction)
        {
            var maps = (index >= 0) ? PosMaps : NegMaps;
            var index2 = (index >= 0) ? index : -index;
            var ret = maps[index2];
            if (ret == null)
                return 0;
            if (direction == Direction.Up)
                return (ret[4][0] ? 1 : 0) + (ret[4][1] ? 1 : 0)
                    + (ret[4][2] ? 1 : 0) + (ret[4][3] ? 1 : 0) + (ret[4][4] ? 1 : 0);
            else if (direction == Direction.Down)
                return (ret[0][0] ? 1 : 0) + (ret[0][1] ? 1 : 0)
                    + (ret[0][2] ? 1 : 0) + (ret[0][3] ? 1 : 0) + (ret[0][4] ? 1 : 0);
            else if (direction == Direction.Left)
                return (ret[0][4] ? 1 : 0) + (ret[1][4] ? 1 : 0)
                    + (ret[2][4] ? 1 : 0) + (ret[3][4] ? 1 : 0) + (ret[4][4] ? 1 : 0);
            return (ret[0][0] ? 1 : 0) + (ret[1][0] ? 1 : 0)
                    + (ret[2][0] ? 1 : 0) + (ret[3][0] ? 1 : 0) + (ret[4][0] ? 1 : 0);
        }

        public bool[][] GetMap(int index)
        {
            var maps = (index >= 0) ? PosMaps : NegMaps;
            var index2 = (index >= 0) ? index : -index;
            var ret = maps[index2];
            if (ret == null)
            {
                //ret = Solver2019_24_2.CreateEmptyMap();
                //maps[index2] = ret;
                //if (index < MinMap)
                //    MinMap = index;
                //else
                //    MaxMap = index;
                return null;
            }
            return ret;
        }

        public bool[][] CreateIfNeededInnerMap(bool[][] outerMap, int index)
        {
            // Inner map is only created if two of the ones around middle is set.
            if((outerMap[1][2] && (outerMap[2][1] || outerMap[2][3]))
                || (outerMap[3][2] && (outerMap[2][1] || outerMap[2][3])))
            {
                var newMap = Solver2019_24_2.CreateEmptyMap();
                // TODO: This is wrong, entire side may have cells!!!
                if(outerMap[1][2])
                {
                    newMap[0][0] = true;
                    newMap[0][1] = true;
                    newMap[0][2] = true;
                    newMap[0][3] = true;
                    newMap[0][4] = true;
                }
                if (outerMap[2][1])
                {
                    newMap[0][0] = true;
                    newMap[1][0] = true;
                    newMap[2][0] = true;
                    newMap[3][0] = true;
                    newMap[4][0] = true;
                }
                if (outerMap[2][3])
                {
                    newMap[0][4] = true;
                    newMap[1][4] = true;
                    newMap[2][4] = true;
                    newMap[3][4] = true;
                    newMap[4][4] = true;
                }
                if (outerMap[3][2])
                {
                    newMap[4][0] = true;
                    newMap[4][1] = true;
                    newMap[4][2] = true;
                    newMap[4][3] = true;
                    newMap[4][4] = true;
                }
                return newMap;
            }
            return null;
        }

        public bool[][] CreateIfNeededOuterMap(int index)
        {
            // Outer map is needed if there are 1 or 2 adjecent to the outer boarder.
            var bottomCount = GetInnerSum(index + 1, Direction.Up);
            var botton = bottomCount == 1 || bottomCount == 2;
            var topCount = GetInnerSum(index + 1, Direction.Down);
            var top = topCount == 1 || topCount == 2;
            var leftCount = GetInnerSum(index + 1, Direction.Right);
            var left = leftCount == 1 || leftCount == 2;
            var rightCount = GetInnerSum(index + 1, Direction.Left);
            var right = rightCount == 1 || rightCount == 2;

            if (botton || left || right || top)
            {
                var newMap = Solver2019_24_2.CreateEmptyMap();
                newMap[1][2] = top;
                newMap[3][2] = botton;
                newMap[2][1] = left;
                newMap[2][3] = right;
                return newMap;
            }

            return null;
        }

        
    }
    static class Solver2019_24_2
    {
        public static string Solve(IInputResolver input, int minutes = 200)
        {
            var inData = input.AsEnumerable();

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

            var dims = new RecMaps(map);
            var TempDims = new RecMaps(tempMap);

            //PrintMap(map);

            for (int minute = 0; minute < minutes; minute++)
            {
                var tempMapDim = new List<(int dim, bool[][] map)>(dims.MaxMap - dims.MinMap);

                for (int dim = dims.MinMap; dim <= dims.MaxMap; dim++)
                {
                    var currentMap = dims.GetMap(dim);

                    i = 0;
                    foreach (var row in currentMap)
                    {
                        // Get up, down, left, right.
                        // And count bugs. Then set new value based on it.
                        int j = 0;
                        foreach (var cell in row)
                        {
                            // If center, skip
                            if (i == 2 && j == 2)
                            {
                                j++;
                                continue;
                            }

                            var up = i == 0 ? dims.GetOuterSum(dim - 1, Direction.Up)
                                : i == 3 && j == 2 ? dims.GetInnerSum(dim + 1, Direction.Up)
                                : currentMap[i - 1][j] ? 1 : 0;
                            var down = i == currentMap.Length - 1 ? dims.GetOuterSum(dim - 1, Direction.Down)
                                : i == 1 && j == 2 ? dims.GetInnerSum(dim + 1, Direction.Down) 
                                : currentMap[i + 1][j] ? 1 : 0;
                            var left = j == 0 ? dims.GetOuterSum(dim - 1, Direction.Left)
                                : i == 2 && j == 3 ? dims.GetInnerSum(dim + 1, Direction.Left)
                                : currentMap[i][j - 1] ? 1 : 0;
                            var right = j == row.Length - 1 ? dims.GetOuterSum(dim - 1, Direction.Right)
                                : i == 2 && j == 1 ? dims.GetInnerSum(dim + 1, Direction.Right)
                                : currentMap[i][j + 1] ? 1 : 0;
                            var sum = up + down + left + right;
                            if (currentMap[i][j])
                                tempMap[i][j] = sum == 1;
                            else
                                tempMap[i][j] = sum == 1 || sum == 2;
                            j++;
                        }
                        i++;
                    }
                    tempMapDim.Add((dim, tempMap));
                    tempMap = CreateEmptyMap();
                    if(dim == dims.MinMap)
                    {
                        var newOuter = dims.CreateIfNeededOuterMap(dim - 1);
                        if(newOuter != null)
                        {
                            tempMapDim.Add((dim - 1, newOuter));
                            //Console.WriteLine("Added outer:");
                            //PrintMap(newOuter);
                        }
                    }
                    if (dim == dims.MaxMap)
                    {
                        var newInner = dims.CreateIfNeededInnerMap(currentMap, dim + 1);
                        if (newInner != null)
                        {
                            tempMapDim.Add((dim + 1, newInner));
                            //Console.WriteLine("Added inner:");
                            //PrintMap(newInner);
                        }
                    }
                    
                }
                dims.UpdateMaps(tempMapDim);
                //dims.PrintMaps();
            }

            var result = dims.SumBugs();
            return result.ToString();
        }

        public static void PrintMap(bool[][] map)
        {
            foreach (var row in map)
            {
                foreach (var cell in row)
                    Console.Write(cell ? '#' : '.');
                Console.WriteLine("");
            }
            Console.WriteLine("");
        }

        public static bool[][] CreateEmptyMap()
        {
            var map = new bool[5][];
            for (int i = 0; i < 5; i++)
            {
                map[i] = new bool[5];
            }
            return map;
        }

    }

}
