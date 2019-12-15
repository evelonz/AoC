using AdventOfCode.Utility;
using FluentAssertions;
using Xunit;

namespace AdventOfCode.Year2019.Tests
{
    public class Tester2019_15
    {
        [Fact]
        public void FirstProblemMyInput()
        {
            Solver2019_15.SolveFirst(new FileInputResolver(2019, 15)).Should().Be("208");
        }

        [Fact]
        public void SecondProblemMyInput()
        {
            Solver2019_15.SolveSecond(new FileInputResolver(2019, 15)).Should().Be("306");
        }
    }
}
