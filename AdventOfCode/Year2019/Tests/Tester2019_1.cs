using AdventOfCode.Utility;
using FluentAssertions;
using Xunit;

namespace AdventOfCode.Year2019.Tests
{
    public class Tester2019_1
    {
        [Theory]
        [InlineData("12", "2")]
        [InlineData("14", "2")]
        [InlineData("1969", "654")]
        [InlineData("100756", "33583")]
        public void FirstProblemExamples(string example, string expected)
        {
            var a = new string[] { example };
            Solver2019_1_1
                .Solve(new MockInputResolver(new string[] { example }))
                .Should().Be(expected);
        }

        [Fact]
        public void FirstProblemInput()
        {
            Solver2019_1_1
                .Solve(new FileInputResolver(2019, 1))
                .Should().Be("3520097");
        }
    }
}
