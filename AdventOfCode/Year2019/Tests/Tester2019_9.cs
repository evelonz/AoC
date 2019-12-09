using AdventOfCode.Utility;
using FluentAssertions;
using Xunit;
using System;

namespace AdventOfCode.Year2019.Tests
{
    public class Tester2019_9
    {
        [Theory]
        [InlineData("109,1,204,-1,1001,100,1,100,1008,100,16,101,1006,101,0,99", "109,1,204,-1,1001,100,1,100,1008,100,16,101,1006,101,0,99")]
        [InlineData("1219070632396864", "1102,34915192,34915192,7,4,7,99,0")]
        [InlineData("1125899906842624", "104,1125899906842624,99")]
        public void ProblemExamples(string expected, string example)
        {
            var adam = Solver2019_9.SolveFirst(new MockInputResolver(new[] { example }));
            var bertil = expected;
            var a = adam.Length;
            var b = bertil.Length;
            adam.Should().Be(bertil);
        }

        [Fact]
        public void FirstProblemMyInput()
        {
            Solver2019_9.SolveFirst(new FileInputResolver(2019, 9)).Should().Be("2171728567");
        }

        [Fact]
        public void SecondProblemMyInput()
        {
            Solver2019_9.SolveSecond(new FileInputResolver(2019, 9)).Should().Be("49815");
        }
    }
}
