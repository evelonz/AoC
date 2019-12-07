using AdventOfCode.Utility;
using FluentAssertions;
using Xunit;

namespace AdventOfCode.Year2019.Tests
{
    public class Tester2019_6
    {
        [Fact]
        public void FirstProblemExamples()
        {
            var input = new string[]
            {
                "COM)B",
                "B)C",
                "C)D",
                "D)E",
                "E)F",
                "B)G",
                "G)H",
                "D)I",
                "E)J",
                "J)K",
                "K)L"
            };
            Solver2019_6_1.Solve(new MockInputResolver(input)).Should().Be("42");
        }

        [Fact]
        public void FirstProblemMyInput()
        {
            Solver2019_6_1.Solve(new FileInputResolver(2019, 6)).Should().Be("241064");
        }

        [Fact]
        public void SecondProblemExamples()
        {
            var input = new string[]
            {
                "COM)B",
                "B)C",
                "C)D",
                "D)E",
                "E)F",
                "B)G",
                "G)H",
                "D)I",
                "E)J",
                "J)K",
                "K)L",
                "K)YOU",
                "I)SAN" 
            };
            Solver2019_6_2.Solve(new MockInputResolver(input)).Should().Be("4");
        }

        [Fact]
        public void SecondProblemMyInput()
        {
            Solver2019_6_2.Solve(new FileInputResolver(2019, 6)).Should().Be("418");
        }
    }
}
