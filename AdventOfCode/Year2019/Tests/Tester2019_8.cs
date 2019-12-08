using AdventOfCode.Utility;
using FluentAssertions;
using Xunit;

namespace AdventOfCode.Year2019.Tests
{
    public class Tester2019_8
    {
        [Theory]
        [InlineData("1", "123456789012")]
        public void FirstProblemExamples(string expected, string example)
        {
            Solver2019_8_1.Solve(new MockInputResolver(new[] { example }), 3, 2).Should().Be(expected);
        }

        [Fact]
        public void FirstProblemMyInput()
        {
            Solver2019_8_1.Solve(new FileInputResolver(2019, 8)).Should().Be("828");
        }

        [Theory]
        [InlineData(" *\r\n* \r\n", "0222112222120000")]
        public void SecondProblemExamples(string expected, string example)
        {
            Solver2019_8_2.Solve(new MockInputResolver(new[] { example }), 2, 2).Should().Be(expected);
        }

        [Fact]
        public void SecondProblemMyInput()
        {
            var myExpected =
                "**** *    ***    ** **** \r\n" +
                "   * *    *  *    * *    \r\n" +
                "  *  *    ***     * ***  \r\n" +
                " *   *    *  *    * *    \r\n" +
                "*    *    *  * *  * *    \r\n" +
                "**** **** ***   **  *    \r\n";
            Solver2019_8_2.Solve(new FileInputResolver(2019, 8)).Should().Be(myExpected);
        }
    }
}
