using AdventOfCode.Utility;
using FluentAssertions;
using Xunit;
using System;
using System.Linq;

namespace AdventOfCode.Year2019.Tests
{
    public class IntcodeComputerTests
    {
        [Theory]
        [InlineData(7, "01,3,4,3,4")]
        [InlineData(-4, "01,4,5,3,-4,0")]
        public void TestAddition(long expected, string example)
        {
            var input = new MockInputResolver(new[] { example });
            var instructions = input.AsEnumerable()
                .First().Split(',')
                .Select(s => long.Parse(s)).ToArray();
            var sut = new IntcodeComputer(instructions);
            var res = sut.Compute(0);
            sut.LastSetValue.Should().Be(expected);
        }

        [Theory]
        [InlineData(12, "02,3,4,3,4")]
        [InlineData(-8, "02,4,5,3,-4,2")]
        public void TestMultiplication(long expected, string example)
        {
            var input = new MockInputResolver(new[] { example });
            var instructions = input.AsEnumerable()
                .First().Split(',')
                .Select(s => long.Parse(s)).ToArray();
            var sut = new IntcodeComputer(instructions);
            var res = sut.Compute(0);
            sut.LastSetValue.Should().Be(expected);
        }

        [Theory]
        [InlineData(7, "01,3,4,3,4")]
        [InlineData(-2, "01,4,5,3,-4,2")]
        public void TestPositionMode(long expected, string example)
        {
            var input = new MockInputResolver(new[] { example });
            var instructions = input.AsEnumerable()
                .First().Split(',')
                .Select(s => long.Parse(s)).ToArray();
            var sut = new IntcodeComputer(instructions);
            var res = sut.Compute(0);
            sut.LastSetValue.Should().Be(expected);
        }

        [Theory]
        [InlineData(100, "1101,99,1,3")]
        [InlineData(9, "1101,4,5,3")]
        public void TestImmediateMode(long expected, string example)
        {
            var input = new MockInputResolver(new[] { example });
            var instructions = input.AsEnumerable()
                .First().Split(',')
                .Select(s => long.Parse(s)).ToArray();
            var sut = new IntcodeComputer(instructions);
            var res = sut.Compute(0);
            sut.LastSetValue.Should().Be(expected);
        }

        [Theory]
        [InlineData(8, "2201,3,4,3,5")]
        [InlineData(15, "2201,4,5,3,8,7")]
        public void TestRelativeMode(long expected, string example)
        {
            var input = new MockInputResolver(new[] { example });
            var instructions = input.AsEnumerable()
                .First().Split(',')
                .Select(s => long.Parse(s)).ToArray();
            var sut = new IntcodeComputer(instructions);
            var res = sut.Compute(0);
            sut.LastSetValue.Should().Be(expected);
        }

        [Theory]
        [InlineData(99, "03,2,0")]
        [InlineData(99, "203,2,0")]
        public void TestInput(long expected, string example)
        {
            var input = new MockInputResolver(new[] { example });
            var instructions = input.AsEnumerable()
                .First().Split(',')
                .Select(s => long.Parse(s)).ToArray();
            var sut = new IntcodeComputer(instructions);
            sut.Compute(99);
            sut.LastSetValue.Should().Be(expected);
            sut.NeedNewInput.Should().BeTrue();
        }

        [Theory]
        [InlineData(99, "04,2,99")]
        [InlineData(66, "104,66,0")]
        [InlineData(-13, "204,3,0,-13")]
        public void TestOutput(long expected, string example)
        {
            var input = new MockInputResolver(new[] { example });
            var instructions = input.AsEnumerable()
                .First().Split(',')
                .Select(s => long.Parse(s)).ToArray();
            var sut = new IntcodeComputer(instructions);
            sut.Compute(99);
            sut.LastOutput.Should().Be(expected);
        }
    }
}
