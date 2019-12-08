using AdventOfCode.Utility;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Year2019
{
    static class Solver2019_8_2
    {
        public static string Solve(IInputResolver input, int width = 25, int hight = 6)
        {
            var data = input.AsEnumerable().First().ToCharArray().Select(s => int.Parse(s.ToString()));
            List<int[]> layers = new List<int[]>(100);

            using (var iter = data.GetEnumerator())
            {
                bool keepgoing = true;
                while (keepgoing)
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
                    if (keepgoing)
                        layers.Add(inn);
                }
            }

            //var minLayer = layers.Min(m => m.Where(x => x == 0).Count());
            //var min = layers.Where(x => x.Where(i => i == 0).Count() == minLayer).Single();
            //var ones = min.Where(x => x == 1).Count();
            //var twos = min.Where(x => x == 2).Count();
            //var result = ones * twos;

            // transcode layers
            var setValues = new bool[width * hight];
            var endImage = new int[width * hight];
            foreach (var layer in layers)
            {
                for (int y = 0; y < hight; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        var pos = (y * width) + x;
                        if (setValues[pos] == false)
                        {
                            var inn = layer[pos];
                            if(inn == 1 || inn == 0)
                            {
                                endImage[pos] = inn;
                                setValues[pos] = true;
                            }
                        }
                    }
                }
            }

            var sb = new System.Text.StringBuilder(
                (width + (System.Environment.NewLine.Length)) * hight);
            for (int y = 0; y < hight; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    var inn = endImage[(y * width) + x];
                    sb.Append(inn == 1 ? '*' : ' ');
                }
                sb.Append(System.Environment.NewLine);
            }

            return sb.ToString();
        }
    }
}
