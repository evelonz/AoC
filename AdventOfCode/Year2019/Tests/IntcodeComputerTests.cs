using AdventOfCode.Utility;
using FluentAssertions;
using Xunit;
using System;
using System.Linq;
using System.Collections.Generic;

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
            sut.Compute(0);
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
            sut.Compute(0);
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
            sut.Compute(0);
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
            sut.Compute(0);
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
            sut.Compute(0);
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

        [Theory]
        [InlineData(2, "05,2,1,99")]
        [InlineData(4, "1005,2,4,99,99")]
        [InlineData(2005, "2005,1,0,99")]
        [InlineData(3, "05,3,1,0,99")]
        [InlineData(3, "105,0,4,99,99")]
        [InlineData(3, "205,2,0,99")]
        public void TestJumpIfTrue(int expected, string example)
        {
            var input = new MockInputResolver(new[] { example });
            var instructions = input.AsEnumerable()
                .First().Split(',')
                .Select(s => long.Parse(s)).ToArray();
            var sut = new IntcodeComputer(instructions);
            sut.Compute(0);
            sut.Pointer.Should().Be(expected);
        }

        [Theory]
        [InlineData(3, "06,2,1,99")]
        [InlineData(3, "1006,2,4,99,99")]
        [InlineData(3, "2006,1,0,99")]
        [InlineData(99, "06,3,4,0,99")]
        [InlineData(4, "1106,0,4,99,99")]
        [InlineData(206, "206,2,0,99")]
        public void TestJumpIfTFalse(int expected, string example)
        {
            var input = new MockInputResolver(new[] { example });
            var instructions = input.AsEnumerable()
                .First().Split(',')
                .Select(s => long.Parse(s)).ToArray();
            var sut = new IntcodeComputer(instructions);
            sut.Compute(0);
            sut.Pointer.Should().Be(expected);
        }

        [Theory]
        [InlineData(1, "07,1,2,3")]
        [InlineData(0, "1107,3,-99,3")]
        public void TestLessThan(long expected, string example)
        {
            var input = new MockInputResolver(new[] { example });
            var instructions = input.AsEnumerable()
                .First().Split(',')
                .Select(s => long.Parse(s)).ToArray();
            var sut = new IntcodeComputer(instructions);
            sut.Compute(0);
            sut.LastSetValue.Should().Be(expected);
        }

        [Theory]
        [InlineData(1, "1108,56,56,3")]
        [InlineData(0, "1108,3,-99,3")]
        public void TestEqual(long expected, string example)
        {
            var input = new MockInputResolver(new[] { example });
            var instructions = input.AsEnumerable()
                .First().Split(',')
                .Select(s => long.Parse(s)).ToArray();
            var sut = new IntcodeComputer(instructions);
            sut.Compute(0);
            sut.LastSetValue.Should().Be(expected);
        }

        [Theory]
        [InlineData(9, "1109,9,99")]
        [InlineData(-9, "1109,-9,99")]
        public void TestAdjustRelativeBAse(int expected, string example)
        {
            var input = new MockInputResolver(new[] { example });
            var instructions = input.AsEnumerable()
                .First().Split(',')
                .Select(s => long.Parse(s)).ToArray();
            var sut = new IntcodeComputer(instructions);
            sut.Compute(0);
            sut.RelativeBase.Should().Be(expected);
        }

        [Theory]
        [InlineData(new long[] { 109, 1, 204, -1, 1001, 100, 1, 100, 1008, 100, 16, 101, 1006, 101, 0, 99 },
            "109,1,204,-1,1001,100,1,100,1008,100,16,101,1006,101,0,99")]
        [InlineData(new [] {1219070632396864 }, "1102,34915192,34915192,7,4,7,99,0")]
        [InlineData(new [] {1125899906842624 }, "104,1125899906842624,99")]
        public void Day9ProblemExamples(long[] expected, string example)
        {
            var res = RunComputation(new MockInputResolver(new[] { example }), 1);

            res.Should().Equal(expected);
        }

        [Fact]
        public void FirstProblemMyInput()
        {
            var res = RunComputation(new FileInputResolver(2019, 9), 1);
            res.Should().Equal(new long[] { 2171728567 });
        }

        [Fact]
        public void SecondProblemMyInput()
        {
            var res = RunComputation(new FileInputResolver(2019, 9), 2);
            res.Should().Equal(new long[] { 49815 });
        }

        private List<long> RunComputation(IInputResolver input, long startData)
        {
            var instructions = new long[10_000];
            var inData = input.AsEnumerable()
                .First().Split(',')
                .Select(s => long.Parse(s)).ToArray();
            inData.CopyTo(instructions, 0);

            var sut = new IntcodeComputer(instructions);

            var outp = new List<long>();
            ProgramState state = ProgramState.None;
            while (state != ProgramState.Halted)
            {
                state = sut.RunProgram(startData);
                if (state == ProgramState.ProvidedOutput)
                {
                    outp.Add(sut.LastOutput);
                }
            }

            return outp;
        }
    }
}
