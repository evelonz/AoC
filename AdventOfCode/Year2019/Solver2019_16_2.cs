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
            var o = string.Join("", ff);
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

            //var patterns = new int[inData.Length][];
            //for (int j = 0; j < inData.Count(); j++)
            //{
            //    patterns[j] = GeneratePattern(pattern, j + 1, inData.Length);
            //}

            var length = inData.Length;
            var temp = new int[length];
            var sw = new System.Diagnostics.Stopwatch();
            for (int phase = 0; phase < phases; phase++)
            {
                //Console.WriteLine($"pashe {phase}, signal:");
                //inData.ToList().ForEach(f => Console.Write(f));
                //Console.WriteLine("");

                sw.Restart();

                for (int n = 1; n <= length; n++)
                {
                    var patLength = 4 * n;
                    var leading0 = n - 1;
                    var lengthLeft = length - leading0;
                    var a = lengthLeft / patLength;
                    var b = (lengthLeft - a * patLength) / n;
                    var c = lengthLeft - (a * patLength) - (b * n);

                    long sum = 0;

                    // Full patterns start with n*1, skip n, then n*-1 elements.
                    for (int i = 0; i < a; i++)
                    {
                        // Sum from 4n*i+1 to 4n*i+n // might have to fix start to be 0 indexed.
                        var start = 4 * n * i + leading0;
                        var end = 4 * n * i + n + leading0;
                        for (int k = start; k < end; k++)
                        {
                            var d = inData[k];
                            sum += d;
                        }
                        // Subtract the second range, which are -1.
                        start = 4 * n * i + 2 * n + leading0;
                        end = 4 * n * i + 3 * n + leading0;
                        for (int k = start; k < end; k++)
                        {
                            var d = inData[k];
                            sum -= d;
                        }

                        // We should now have skipped the two 0 ranges.
                    }

                    for (int i = 0; i < b; i++)
                    {
                        // b 1 and 3 are zero patterns.
                        if (i == 1 || i == 3)
                            continue;
                        // Positive.
                        if(i == 0)
                        {
                            var start = a * 4 * n + leading0;
                            var end = a * 4 * n + n + leading0;
                            for (int k = start; k < end; k++)
                            {
                                var d = inData[k];
                                sum += d;
                            }
                        }
                        // Negative.
                        else
                        {
                            var start = a * 4 * n + 2 * n + leading0;
                            var end = a * 4 * n + 3 * n + leading0;
                            for (int k = start; k < end; k++)
                            {
                                var d = inData[k];
                                sum -= d;
                            }
                        }
                    }

                    // c brings up the rest. Skip if zero.
                    var factor = b == 0 ? 1
                        : b == 2 ? -1
                        : 0;
                    if(c != 0 && b == 0 && a == 0)
                    {
                        // We have reached the bottom half of the matrix.
                        // Starting from the end, each row is the length-n+1 element + sum of previous row.
                        // up to the current n.
                        // When done, set n = Length+1 to go to next iteration.
                        for (int m = length-1; m >= n-1; m--)
                        {
                            sum += inData[m];

                            var single2 = sum % 10;
                            var r2 = Math.Abs(single2);
                            temp[m] = (int)r2;

                        }

                        n = length + 1;
                        continue;
                    }
                    else if(factor != 0 && c != 0)
                    {
                        var startC = length - c;
                        var endC = length;
                        for (int k = startC; k < endC; k++)
                        {
                            var d = inData[k];
                            sum += d * factor;
                        }
                    }

                    var single = sum % 10;
                    var r = Math.Abs(single);
                    temp[n-1] = (int)r;
                }

                //for (int j = 0; j < inData.Count(); j++)
                //{
                //    var pat = patterns[j]; // GeneratePattern(pattern, j + 1, inData.Count());
                //    for (int k = 0; k < inData.Count(); k++)
                //    {
                //        var newnum = inData[k] * pat[k];
                //        temp[j] += newnum;
                //    }
                //    var single = temp[j] % 10;
                //    var r = Math.Abs(single);
                //    temp[j] = r;
                //}
                temp.CopyTo(inData, 0);
                Array.Clear(temp, 0, temp.Length);

                sw.Stop();
                Console.WriteLine($"Time in {sw.Elapsed.TotalSeconds}");
            }
            //Console.WriteLine($"pashe {phases}, signal:");
            //inData.ToList().ForEach(f => Console.Write(f));
            //Console.WriteLine("");

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
