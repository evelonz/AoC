using AdventOfCode.Utility;
using FluentAssertions;
using Xunit;

namespace AdventOfCode.Year2019.Tests
{
    public class Tester2019_21
    {
        [Fact]
        public void FirstProblemMyInput()
        {
            Solver2019_21.SolveFirst(new FileInputResolver(2019, 21)).Should().Be("19352638");
        }

        [Fact]
        public void SecondProblemMyInput()
        {
            Solver2019_21.SolveSecond(new FileInputResolver(2019, 21)).Should().Be("1141251258");
        }
    }
}
