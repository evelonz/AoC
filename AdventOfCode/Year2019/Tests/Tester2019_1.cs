using AdventOfCode.Utility;
using FluentAssertions;
using Xunit;

namespace AdventOfCode.Year2019.Tests
{
    public class Tester2019_1
    {
        [Theory]
        [InlineData(2, "2")]
        [InlineData(3, "2")]
        [InlineData(4, "654")]
        [InlineData(5, "33583")]
        public void FirstProblem(int example, string expected)
        {
            Solver2019_1_1.Solve(new FileInputResolver(2019, 1, example)).Should().Be(expected);
        }
    }
}
