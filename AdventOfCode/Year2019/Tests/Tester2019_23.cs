using AdventOfCode.Utility;
using FluentAssertions;
using System.Collections.Generic;
using Xunit;

namespace AdventOfCode.Year2019.Tests
{
    public class Tester2019_23
    {
        [Fact]
        public void FirstProblemMyInput()
        {
            Solver2019_23
                .Solve(new FileInputResolver(2019, 23))
                .Should().Be("23057");
        }

        [Fact]
        public void SecondProblemMyInput()
        {
            Solver2019_23
                .Solve(new FileInputResolver(2019, 23), second: true)
                .Should().Be("15156");
        }
    }
}
