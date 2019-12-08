using AdventOfCode.Utility;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Year2019
{
    static class Solver2019_8
    {
        public static string SolveFirst(IInputResolver input, int width = 25, int hight = 6)
        {
            var layers = GetLayers(input, width, hight);

            var minLayer = layers.Min(m => m.Where(x => x == 0).Count());
            var result = layers.Where(x => x.Where(i => i == 0).Count() == minLayer)
                .Select(s => s.Where(x => x == 1).Count() * s.Where(x => x == 2).Count()).Single();

            return result.ToString();
        }

        public static string SolveSecond(IInputResolver input, int width = 25, int hight = 6)
        {
            var layers = GetLayers(input, width, hight);

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
                            if (inn == 1 || inn == 0)
                            {
                                endImage[pos] = inn;
                                setValues[pos] = true;
                            }
                        }
                    }
                }
            }

            var sb = new System.Text.StringBuilder(
                (width + System.Environment.NewLine.Length) * hight);
            for (int y = 0; y < hight; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    var pixelData = endImage[(y * width) + x];
                    sb.Append(pixelData == 1 ? '*' : ' ');
                }
                sb.Append(System.Environment.NewLine);
            }

            return sb.ToString();
        }

        private static List<int[]> GetLayers(IInputResolver input, int width = 25, int hight = 6)
        {
            var data = input.AsEnumerable().First().ToCharArray().Select(s => int.Parse(s.ToString())).ToList();
            List<int[]> layers = new List<int[]>(data.Count / width * hight);

            var batch = data.Take(width * hight).ToArray();
            while (batch.Length > 0)
            {
                layers.Add(batch);
                batch = data.Skip(width * hight * layers.Count).Take(width * hight).ToArray();
            }

            return layers;
        }
    }
}
