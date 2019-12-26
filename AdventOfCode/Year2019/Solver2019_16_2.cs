using AdventOfCode.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;

namespace AdventOfCode.Year2019
{
    static class Solver2019_16_2
    {
        public static string Solve(IInputResolver input, int phases = 100, int repeat = 10_000)
        {
            var inData = input.AsEnumerable().First()
                .ToArray()
                .Select(s => int.Parse(s.ToString())).ToArray();

            var offset = int.Parse(string.Join("", inData.Take(7).Select(s => s.ToString())));

            inData = AmplifySignal(repeat, inData);

            var length = inData.Length;
            var temp = new int[length];
            // SIMD specific variables.
            var isSimdEnabled = Vector.IsHardwareAccelerated;
            var simdLength = Vector<int>.Count;
            var oneVector = Vector<int>.One;

            var sw = new System.Diagnostics.Stopwatch();
            var sw2 = new System.Diagnostics.Stopwatch();

            // Get important break points.
            var (bottomHalf, bHalf, startN) = 
                GetImportanBreakPoints(length, isSimdEnabled, simdLength);

            var n1Pat = new Vector<int>(new[] { 1, 0, -1, 0, 1, 0, -1, 0 });
            var n2Pat = new Vector<int>(new[] { 1, 1, 0, 0, -1, -1, 0, 0 });

            // Actual solution code.
            for (int phase = 0; phase < phases; phase++)
            {
                //Console.WriteLine($"pashe {phase}, signal:");
                //inData.ToList().ForEach(f => Console.Write(f));
                //Console.WriteLine("");
                sw.Restart();

                // Tryin out if it is better to use vector dot product for small N.
                // Currently only n = 1, think we can do it for at least a few more.
                // If we can do it for more, then we can reuse the data vector as well!
                for (int n = 1; n <= startN; ++n)
                {
                    var pat = n == 1 ? n1Pat : n2Pat;
                    long sum = 0;
                    var ii = 0;
                    for (ii = 0; ii <= length - simdLength; ii += simdLength)
                    {
                        var s1 = new Vector<int>(inData, ii);
                        var ss = Vector.Dot(s1, pat);
                        sum += ss;
                    }
                    temp[n - 1] = (int)Math.Abs(sum % 10);
                }

                Parallel.For(startN, bHalf, (n) =>
                //for (int n = 1; n < bottomHalf; ++n)
                {
                    //sw2.Restart();
                    // Some values to group the data into sets of 0, 1, and -1 patterns.
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
                        // Sum from 4n*i+1 to 4n*i+n
                        var start = 4 * n * i + leading0;
                        var end = 4 * n * i + n + leading0;

                        var ii = start;
                        for (ii = start; ii <= end - simdLength; ii += simdLength)
                        {
                            var s1 = new Vector<int>(inData, ii);
                            var ss = Vector.Dot(s1, oneVector);
                            sum += ss;
                        }
                        for (int k = ii; k < end; k++)
                        {
                            sum += inData[k];
                        }
                        // Subtract the second range, which are -1.
                        start = 4 * n * i + 2 * n + leading0;
                        end = 4 * n * i + 3 * n + leading0;
                        ii = start;
                        for (ii = start; ii <= end - simdLength; ii += simdLength)
                        {
                            var s1 = new Vector<int>(inData, ii);
                            var ss = Vector.Dot(s1, oneVector);
                            sum -= ss;
                        }
                        for (int k = start; k < end; k++)
                        {
                            sum -= inData[k];
                        }

                        // We should now have skipped the two 0 ranges.
                    }

                    for (int i = 0; i < b; i++)
                    {
                        // b 1 and 3 are zero patterns.
                        if (i == 1 || i == 3)
                            continue;
                        // Positive.
                        if (i == 0)
                        {
                            var start = a * 4 * n + leading0;
                            var end = a * 4 * n + n + leading0;
                            for (int k = start; k < end; k++)
                            {
                                sum += inData[k];
                            }
                        }
                        // Negative.
                        else
                        {
                            var start = a * 4 * n + 2 * n + leading0;
                            var end = a * 4 * n + 3 * n + leading0;
                            for (int k = start; k < end; k++)
                            {
                                sum -= inData[k];
                            }
                        }
                    }

                    // c brings up the rest. Skip if zero.
                    var factor = b == 0 ? 1
                        : b == 2 ? -1
                        : 0;
                    if (factor != 0 && c != 0)
                    {
                        var startC = length - c;
                        var endC = length;
                        for (int k = startC; k < endC; k++)
                        {
                            sum += inData[k] * factor;
                        }
                    }

                    temp[n - 1] = (int)Math.Abs(sum % 10);
                    //sw2.Stop();
                    //Console.WriteLine($"Time n: {n} - {sw2.Elapsed.TotalSeconds}");
                }
                );
                // Only B part.
                SumBOnlyPart(inData, temp, bottomHalf, bHalf, bHalf - 1, 0);
                // Bottom half.
                SumBottomHalf(inData, length, temp, bottomHalf);
                
                temp.CopyTo(inData, 0);
                Array.Clear(temp, 0, temp.Length);

                sw.Stop();
                Console.WriteLine($"Phase {phase} in {sw.Elapsed.TotalSeconds}");
            }

            //var res = inData.Take(8).Select(s => s.ToString());
            //return String.Join("", res);
            var res = inData.Skip(offset).Take(8).Select(s => s.ToString());
            return String.Join("", res);
        }

        private static (int bottomHalf, int bHalf, int startN) GetImportanBreakPoints(int length, bool isSimdEnabled, int simdLength)
        {
            int bottomHalf = -1;
            int bHalf = -1;
            int startN = 1;
            for (int n = 1; n <= length; n++)
            {
                var patLength = 4 * n;
                var leading0 = n - 1;
                var lengthLeft = length - leading0;
                var a = lengthLeft / patLength;
                var b = (lengthLeft - a * patLength) / n;
                var c = lengthLeft - (a * patLength) - (b * n);
                // When we only have positive B.
                if (bHalf == -1 && a == 0 && (b == 1 || (b == 2 && c == 0)))
                    bHalf = n;
                // Last breakpoint when we reached the half point. We can sum C.
                if (c != 0 && b == 0 && a == 0)
                {
                    bottomHalf = n;
                    break;
                }
                // See if we can use SIMD for the first two itterations.
                if (length % 4 == 0 && length % 8 == 0 && isSimdEnabled && simdLength == 8)
                {
                    if (n == 1 && b == 0 && c == 0)
                    {
                        startN = 2;
                    }
                    if (n == 2 && b == 0 && c == 0)
                    {
                        startN = 3;
                    }
                }
            }
            return (bottomHalf, bHalf, startN);
        }

        private static int[] AmplifySignal(int repeat, int[] inData)
        {
            // repeat indata x amount of times.
            var newSingal = new int[inData.Count() * repeat];
            for (int l = 0; l < repeat * inData.Count(); l += inData.Count())
            {
                for (int m = 0; m < inData.Count(); m++)
                {
                    newSingal[l + (m)] = inData[m];
                }
            }

            inData = newSingal;
            return inData;
        }

        private static void SumBOnlyPart(int[] inData, int[] temp, int bottomHalf, int n, int leading0, int a)
        {
            // In this interval, we can again just sum row after row.
            // We have to add 1 item, and subtrack two for each itteration.
            long sum = 0;
            // Get first sum.
            var start = leading0;
            var end = n + leading0;
            for (int k = start; k < end; k++)
            {
                sum += inData[k];
            }
            // Add it
            temp[n - 1] = (int)Math.Abs(sum % 10);

            // Then subtrack the first element from each row, and add two more.
            for (int m = n + 1; m < bottomHalf; m++)
            {
                sum -= inData[m - 2]; // subtrack the first element.
                sum += inData[end] + inData[end + 1]; // Add two new elements.
                end += 2;

                temp[m - 1] = (int)Math.Abs(sum % 10);
            }
        }

        private static void SumBottomHalf(int[] inData, int length, int[] temp, int n)
        {
            // We have reached the bottom half of the matrix.
            // Starting from the end, each row is the length-n+1 element + sum of previous row.
            // up to the current n.
            // When done, set n = Length+1 to go to next iteration.
            long sum = 0;
            for (int m = length - 1; m >= n - 1; m--)
            {
                sum += inData[m];
                temp[m] = (int)Math.Abs(sum%10);
            }
        }

        private static void PrintInData(int[] data)
        {
            foreach (var item in data)
            {
                Console.Write(item);
            }
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
