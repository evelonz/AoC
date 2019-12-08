using AdventOfCode.Utility;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Year2019
{
    static class Solver2019_8_1
    {
        public static string Solve(IInputResolver input, int width = 25, int hight = 6)
        {
            var data = input.AsEnumerable().First().ToCharArray().Select(s => int.Parse(s.ToString()));
            List<int[]> layers = new List<int[]>(100);
            
            using(var iter = data.GetEnumerator())
            {
                bool keepgoing = true;
                while(keepgoing)
                {
                    var inn = new int[width * hight];
                    for (int y = 0; y < hight; y++)
                    {
                        for (int x = 0; x < width; x++)
                        {
                            if (iter.MoveNext())
                                inn[(y * width) + x] = iter.Current;
                            else
                            {
                                keepgoing = false;
                                break;
                            }
                        
                        }
                        if (keepgoing == false)
                            break;
                    }
                    if(keepgoing)
                        layers.Add(inn);
                }
            }

            var minLayer = layers.Min(m => m.Where(x => x == 0).Count());
            var min = layers.Where(x => x.Where(i => i == 0).Count() == minLayer).Single();
            var ones = min.Where(x => x == 1).Count();
            var twos = min.Where(x => x == 2).Count();
            var result = ones * twos;

            return result.ToString();
        }
    }
}
