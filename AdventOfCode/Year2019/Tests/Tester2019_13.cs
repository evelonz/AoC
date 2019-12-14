using AdventOfCode.Utility;
using FluentAssertions;
using Xunit;
using System;

namespace AdventOfCode.Year2019.Tests
{
    public class Tester2019_13
    {
        [Fact]
        public void FirstProblemMyInput()
        {
            Solver2019_13_1.Solve(new FileInputResolver(2019, 13)).Should().Be("173");
        }

        [Fact]
        public void SecondProblemMyInput()
        {
            Solver2019_13_2.Solve(new FileInputResolver(2019, 13)).Should().Be("8942");
        }
    }
}
