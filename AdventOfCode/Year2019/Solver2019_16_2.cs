using AdventOfCode.Utility;
using System;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;

namespace AdventOfCode.Year2019
{
    static class Solver2019_16_2
    {
        public static string Solve(IInputResolver input, int repeat = 10_000)
        {
            var inData = input.AsEnumerable().First()
                .ToArray()
                .Select(s => int.Parse(s.ToString())).ToArray();

            var offset = int.Parse(string.Join("", inData.Take(7).Select(s => s.ToString())));
            inData = AmplifySignal(repeat, inData);
            var length = inData.Length;
            var resultStore = new int[length];
            var (cOnlyPart, bOnlyPart) = GetImportanBreakPoints(length);
            var sw = new System.Diagnostics.Stopwatch();

            // Run FFT for the set number of phases.
            for (int phase = 0; phase < 100; phase++)
            {
                sw.Restart();
                FlawedFrequencyTransmission(inData, resultStore, offset, cOnlyPart, bOnlyPart);

                resultStore.CopyTo(inData, 0);
                Array.Clear(resultStore, 0, resultStore.Length);

                sw.Stop();
                Console.WriteLine($"Phase {phase} in {sw.Elapsed.TotalSeconds}");
            }

            var res = inData.Skip(offset).Take(8).Select(s => s.ToString());
            return String.Join("", res);
        }

        public static void FlawedFrequencyTransmission(int[] inData, int[] resultStore, int offset, int cOnlyPart, int bOnlyPart)
        {
            // Start at n = offset + 1. This becuase the n:th number only
            // depend on the n:th to last number from the previous run.
            // offest 0 => need all numbers.
            // offset length - 1 => only need the last number.
            var simdLength = Vector<int>.Count;
            var oneVector = Vector<int>.One;
            var length = inData.Length;

            Parallel.For(offset + 1, bOnlyPart, (n) =>
            //for (int n = 1; n < bottomHalf; ++n)
            {
                // Some values to group the data into sets of 0, 1, and -1 patterns.
                var (a, b, c, leadingZeros) = GetPatterns(length, n);
                long sum = 0;

                // Full patterns start with n*1, skip n, then n*-1 elements.
                for (int i = 0; i < a; i++)
                {
                    // Sum from 4n*i+1 to 4n*i+n
                    var start = 4 * n * i + leadingZeros;
                    var end = 4 * n * i + n + leadingZeros;

                    var index = start;
                    for (index = start; index <= end - simdLength; index += simdLength)
                    {
                        var vector = new Vector<int>(inData, index);
                        sum += Vector.Dot(vector, oneVector);
                    }
                    for (int k = index; k < end; k++)
                    {
                        sum += inData[k];
                    }
                    // Subtract the second range, which are -1.
                    start = 4 * n * i + 2 * n + leadingZeros;
                    end = 4 * n * i + 3 * n + leadingZeros;
                    index = start;
                    for (index = start; index <= end - simdLength; index += simdLength)
                    {
                        var vector = new Vector<int>(inData, index);
                        sum -= Vector.Dot(vector, oneVector);
                    }
                    for (int k = start; k < end; k++)
                    {
                        sum -= inData[k];
                    }
                }

                for (int i = 0; i < b; i++)
                {
                    // b 1 and 3 are zero patterns.
                    if (i == 1 || i == 3)
                        continue;
                    // Positive.
                    if (i == 0)
                    {
                        var start = a * 4 * n + leadingZeros;
                        var end = a * 4 * n + n + leadingZeros;
                        for (int k = start; k < end; k++)
                        {
                            sum += inData[k];
                        }
                    }
                    // Negative.
                    else
                    {
                        var start = a * 4 * n + 2 * n + leadingZeros;
                        var end = a * 4 * n + 3 * n + leadingZeros;
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

                resultStore[n - 1] = (int)Math.Abs(sum % 10);
            }
            );

            SumBOnlyPart(inData, resultStore, cOnlyPart, bOnlyPart, bOnlyPart - 1);

            SumBottomHalf(inData, resultStore, cOnlyPart);
        }

        private static (int cOnlyPart, int bOnlyPart) GetImportanBreakPoints(int length)
        {
            int bOnlyPart = -1;
            int cOnlyPart = -1;
            for (int n = 1; n <= length; n++)
            {
                var (a, b, c, _) = GetPatterns(length, n);
                // When we only have positive B.
                if (bOnlyPart == -1 && a == 0 && (b == 1 || (b == 2 && c == 0)))
                    bOnlyPart = n;
                // Last breakpoint when we reached the half point. We can sum C.
                if (c != 0 && b == 0 && a == 0)
                {
                    cOnlyPart = n;
                    break;
                }
            }
            return (cOnlyPart, bOnlyPart);
        }

        private static (int a, int b, int c, int leadingZeros) GetPatterns(int length, int n)
        {
            var patLength = 4 * n;
            var leadingZeros = n - 1;
            var lengthLeft = length - leadingZeros;
            var a = lengthLeft / patLength;
            var b = (lengthLeft - a * patLength) / n;
            var c = lengthLeft - (a * patLength) - (b * n);
            return (a, b, c, leadingZeros);
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

        private static void SumBOnlyPart(int[] inData, int[] resultStore, int cOnlyPart, int bOnlyPart, int leadingZeros)
        {
            // In this interval, we can again just sum row after row.
            // We have to add 1 item, and subtrack two for each itteration.
            long sum = 0;
            // Get the sum of the first row.
            var start = leadingZeros;
            var end = bOnlyPart + leadingZeros; // Same as length...
            for (int k = start; k < end; k++)
            {
                sum += inData[k];
            }
            resultStore[bOnlyPart - 1] = (int)Math.Abs(sum % 10);

            // Then subtrack the first element from each row, and add two more.
            for (int m = bOnlyPart + 1; m < cOnlyPart; m++)
            {
                sum -= inData[m - 2]; // subtrack the first element.
                sum += inData[end] + inData[end + 1]; // Add two new elements.
                end += 2;
                resultStore[m - 1] = (int)Math.Abs(sum % 10);
            }
        }

        private static void SumBottomHalf(int[] inData, int[] resultStore, int cOnlyPart)
        {
            // We have reached the bottom half of the matrix.
            // Starting from the end, each row is the length-n+1 element added to the 
            // sum of previous row, up to the start of the button half.
            // When done, set n = Length+1 to go to next iteration.
            long sum = 0;
            for (int m = inData.Length - 1; m >= cOnlyPart - 1; m--)
            {
                sum += inData[m];
                resultStore[m] = (int)Math.Abs(sum%10);
            }
        }
    }
}
