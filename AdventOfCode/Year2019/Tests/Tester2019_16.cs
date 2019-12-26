using AdventOfCode.Utility;
using FluentAssertions;
using System.Numerics;
using Xunit;

namespace AdventOfCode.Year2019.Tests
{
    public class Tester2019_16
    {
        [Fact]
        public void FirstProblemMyInput()
        {
            Solver2019_16_1.Solve(new FileInputResolver(2019, 16)).Should().Be("27831665");
        }

        [Fact(Skip = "This is not solved and could take hours to run.")]
        public void SecondProblemMyInput()
        {
            Solver2019_16_2.Solve(new FileInputResolver(2019, 16)).Should().Be("208");
        }

        [Fact(Skip = "This is simply a SIMD perforamnce test.")]
        public void SimdSumTest()
        {
            var sut = new SimdTests(6_500_000);
            var sw = new System.Diagnostics.Stopwatch();
            sw.Start();
            var res = sut.SIMDArraySum();
            sw.Stop();
            var time = sw.ElapsedMilliseconds;
            sw.Restart();
            var res2 = sut.SeqArraySum();
            sw.Stop();
            var time2 = sw.ElapsedMilliseconds;
            res.Should().Be(res2);
        }

        [Fact(Skip = "This is simply a SIMD perforamnce test.")]
        public void SimdSumTestShort()
        {
            var sut = new SimdTestsShort(6_500_000);
            var sw = new System.Diagnostics.Stopwatch();
            sw.Start();
            var res = sut.SIMDArraySum();
            sw.Stop();
            var time = sw.ElapsedMilliseconds;
            sw.Restart();
            var res2 = sut.SeqArraySum();
            sw.Stop();
            var time2 = sw.ElapsedMilliseconds;
            res.Should().Be(res2);
        }
    }

    public class SimdTests
    {
        public int[] DataArray { get; set; }
        public SimdTests(int count)
        {
            DataArray = new int[count];
            for (int i = 0; i < count; i++)
            {
                DataArray[i] = i;
            }
        }

        public long SIMDArraySum()
        {
            var simdLength = Vector<int>.Count;
            long sum = 0;
            int i;
            for (i = 0; i <= DataArray.Length - simdLength; i += simdLength)
            {
                var va = new Vector<int>(DataArray, i);
                sum += Vector.Dot(va, Vector<int>.One);
            }

            for (; i < DataArray.Length; ++i)
            {
                sum += DataArray[i];
            }

            return sum;
        }

        public long SeqArraySum()
        {
            long sum = 0;
            for (var i = 0; i < DataArray.Length; ++i)
            {
                sum += DataArray[i];
            }

            return sum;
        }
    }

    public class SimdTestsShort
    {
        public short[] DataArray { get; set; }
        public SimdTestsShort(int count)
        {
            DataArray = new short[count];
            for (int i = 0; i < count; i++)
            {
                DataArray[i] = (short)(i % 10);
            }
        }

        public long SIMDArraySum()
        {
            var simdLength = Vector<short>.Count;
            long sum = 0;
            int i;
            for (i = 0; i <= DataArray.Length - simdLength; i += simdLength)
            {
                var va = new Vector<short>(DataArray, i);
                sum += Vector.Dot(va, Vector<short>.One);
            }

            for (; i < DataArray.Length; ++i)
            {
                sum += DataArray[i];
            }

            return sum;
        }

        public long SeqArraySum()
        {
            long sum = 0;
            for (var i = 0; i < DataArray.Length; ++i)
            {
                sum += DataArray[i];
            }

            return sum;
        }
    }
}
