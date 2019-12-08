using AdventOfCode.Utility;
using FluentAssertions;
using Xunit;
using System;

namespace AdventOfCode.Year2019.Tests
{
    public class Tester2019_8
    {
        [Theory]
        [InlineData("1", "123456789012")]
        public void FirstProblemExamples(string expected, string example)
        {
            Solver2019_8.SolveFirst(new MockInputResolver(new[] { example }), 3, 2).Should().Be(expected);
        }

        [Fact]
        public void FirstProblemMyInput()
        {
            Solver2019_8.SolveFirst(new FileInputResolver(2019, 8)).Should().Be("828");
        }

        [Fact]
        public void SecondProblemExamples()
        {
            var expected =
                " *"+ Environment.NewLine +"* " + Environment.NewLine;
            Solver2019_8
                .SolveSecond(new MockInputResolver(new[] { "0222112222120000" }), 2, 2)
                .Should().Be(expected);
        }

        [Fact]
        public void SecondProblemMyInput()
        {
            var expected =
                "**** *    ***    ** **** " + Environment.NewLine +
                "   * *    *  *    * *    " + Environment.NewLine +
                "  *  *    ***     * ***  " + Environment.NewLine +
                " *   *    *  *    * *    " + Environment.NewLine +
                "*    *    *  * *  * *    " + Environment.NewLine +
                "**** **** ***   **  *    " + Environment.NewLine;
            Solver2019_8.SolveSecond(new FileInputResolver(2019, 8)).Should().Be(expected);
        }
    }
}
