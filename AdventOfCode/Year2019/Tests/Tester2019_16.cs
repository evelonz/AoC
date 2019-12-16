using AdventOfCode.Utility;
using FluentAssertions;
using Xunit;

namespace AdventOfCode.Year2019.Tests
{
    public class Tester2019_16
    {
        [Fact]
        public void FirstProblemMyInput()
        {
            Solver2019_16_1.Solve(new FileInputResolver(2019, 16)).Should().Be("208");
        }

        [Fact]
        public void SecondProblemMyInput()
        {
            Solver2019_16_2.Solve(new FileInputResolver(2019, 16)).Should().Be("208");
        }
    }
}
