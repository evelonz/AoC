using AdventOfCode.Utility;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Year2019
{
    static class Solver2019_16_2
    {
        public static string Solve(IInputResolver input)
        {
            int phases = 100;
            var repeat = 10_000;

            var inData = input.AsEnumerable().First()
                .ToArray()
                .Select(s => int.Parse(s.ToString())).ToArray();

            var ff = inData.Take(7).Select(s => s.ToString());
            var o = String.Join("", ff);
            var offset = int.Parse(o);

            // repeat indata 10_000 times.
            var newSingal = new int[inData.Count() * repeat];
            for (int l = 0; l < repeat *inData.Count(); l += inData.Count())
            {
                for (int m = 0; m < inData.Count(); m++)
                {
                    newSingal[l + (m)] = inData[m];
                }
            }

            inData = newSingal;

            var pattern = new[] { 0, 1, 0, -1 };

            var patterns = new int[inData.Length][];
            for (int j = 0; j < inData.Count(); j++)
            {
                patterns[j] = GeneratePattern(pattern, j + 1, inData.Length);
            }


            var temp = new int[inData.Count()];
            for (int i = 0; i < phases; i++)
            {
                //Console.WriteLine($"pashe {i}, signal:");
                //inData.ToList().ForEach(f => Console.Write(f));
                //Console.WriteLine("");
                for (int j = 0; j < inData.Count(); j++)
                {
                    var pat = patterns[j]; // GeneratePattern(pattern, j + 1, inData.Count());
                    for (int k = 0; k < inData.Count(); k++)
                    {
                        var newnum = inData[k] * pat[k];
                        temp[j] += newnum;
                    }
                    var single = temp[j] % 10;
                    var r = Math.Abs(single);
                    temp[j] = r;
                }
                temp.CopyTo(inData, 0);
                Array.Clear(temp, 0, temp.Length);
            }
            Console.WriteLine($"pashe {phases}, signal:");
            inData.ToList().ForEach(f => Console.Write(f));
            Console.WriteLine("");

            var res = inData.Skip(offset).Take(8).Select(s => s.ToString());
            return String.Join("", res);
        }

        private static int[] GeneratePattern(int[] pattern, int position, int length)
        {
            var res = new List<int>(length + 1);

            while (res.Count < length + 1)
            {
                for (int i = 0; i < pattern.Length; i++)
                {
                    for (int j = 0; j < position; j++)
                    {
                        res.Add(pattern[i]);
                        if (res.Count == length + 1)
                            return res.Skip(1).Take(length).ToArray();
                    }
                }
            }


            return res.Skip(1).Take(length).ToArray();
        }
    }
}
