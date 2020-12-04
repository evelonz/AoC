using AdventOfCode2020.Utility;
using FluentAssertions;
using System.Linq;
using Xunit;

namespace AdventOfCode2020.DayX
{
    internal static class DayX
    {
        internal static string Solve1(IInputResolver input)
        {
            var data = input.AsEnumerable().Select(s => int.Parse(s)).ToList();
            for (int i = 0; i < data.Count; i++)
            {
                var current = data[i];
                var second = data.Skip(i+1).FirstOrDefault(x => x+current == 2020);
                if (second == default)
                    continue;
                return (current * second).ToString();
            }
            return "-1";
        }


        internal static string Solve2(IInputResolver input)
        {
            var data = input.AsEnumerable().Select(s => int.Parse(s)).ToList();
            for (int i = 0; i < data.Count; i++)
            {
                var current = data[i];
                for (int j = i; j < data.Count; j++)
                {
                    var second = data[j];
                    var third = data.Skip(j + 1).FirstOrDefault(x => x + current + second == 2020);

                    if (third == default)
                        continue;
                    return (current * second * third).ToString();
                }
            }

            return "-1";
        }
    }

    public class Test2020Day99
    {
        [Fact]
        public void FirstProblemExamples()
        {
            DayX
                .Solve1(new MockInputResolver(new string[] {
                    "1721"
                    ,"979"
                    ,"366"
                    ,"299"
                    ,"675"
                    ,"1456" }))
                .Should().Be("514579");
        }

        [Fact]
        public void FirstProblemInput()
        {
            var result = DayX
                .Solve1(new FileInputResolver(98));

            result.Should().Be("1019571");
        }

        [Theory]
        [InlineData("12", "2")]
        [InlineData("14", "2")]
        [InlineData("1969", "654")]
        [InlineData("100756", "33583")]
        public void SecondProblemExamples(string example, string expected)
        {
            DayX
                .Solve2(new MockInputResolver(new string[] { example }))
                .Should().Be(expected);
        }

        [Fact]
        public void SecondProblemInput()
        {
            var result = DayX
                .Solve2(new FileInputResolver(98));

            result.Should().Be("100655544");
        }
    }
}
