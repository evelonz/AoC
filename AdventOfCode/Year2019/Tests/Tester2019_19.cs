using AdventOfCode.Utility;
using FluentAssertions;
using Xunit;

namespace AdventOfCode.Year2019.Tests
{
    public class Tester2019_19
    {
        [Fact]
        public void FirstProblemMyInput()
        {
            Solver2019_19_1.Solve(new FileInputResolver(2019, 19)).Should().Be("173");
        }

        [Fact]
        public void SecondProblemMyInput()
        {
            Solver2019_19_2.Solve(new FileInputResolver(2019, 19), 600, 1000).Should().Be("6671097");
        }
    }
}
