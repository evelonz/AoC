using AdventOfCode2020.Utility;
using FluentAssertions;
using System.Collections.Generic;
using System.Linq;

using Xunit;

namespace AdventOfCode2020.Day2
{
    internal static class Day2
    {
        public static int Day => 2;
        private class Record
        {
            public int FirstInt { get; init; }
            public int SecondInt { get; init; }
            public char Character { get; init; }
            public string Password { get; init; } = "";

            public static Record Parse(string input)
            {
                var dash = input.Split('-');
                var spaces = dash[1].Split(' ');
                var colon = spaces[1].Split(':');
                return new Record
                {
                    FirstInt = int.Parse(dash[0]),
                    SecondInt = int.Parse(spaces[0]),
                    Character = colon[0].First(),
                    Password = spaces[2],
                };
            }
        }

        internal static string Solve1(IInputResolver input)
            => input.AsEnumerable()
                .Select(s => Record.Parse(s))
                .Count(c =>
                {
                    var matches = c.Password.Count(p => p == c.Character);
                    return matches >= c.FirstInt && matches <= c.SecondInt;
                })
                .ToString();

        internal static string Solve2(IInputResolver input)
            => input.AsEnumerable()
                .Select(s => Record.Parse(s))
                .Count(c =>
                {
                    var pos1 = c.Password[c.FirstInt - 1] == c.Character ? 1 : 0;
                    var pos2 = c.Password[c.SecondInt - 1] == c.Character ? 1 : 0;
                    return pos1 + pos2 == 1;
                })
                .ToString();
    }

    public class Test2020Day2
    {
        [Fact]
        public void FirstProblemExamples()
        {
            Day2
                .Solve1(new MockInputResolver(new string[] {
                    "1-3 a: abcde"
                    ,"1-3 b: cdefg"
                    ,"2-9 c: ccccccccc" }))
                .Should().Be("2");
        }

        [Fact]
        public void FirstProblemInput()
        {
            var result = Day2
                .Solve1(new FileInputResolver(Day2.Day));

            result.Should().Be("414");
        }

        [Fact]
        public void SecondProblemExamples()
        {
            Day2
                .Solve2(new MockInputResolver(new string[] {
                    "1-3 a: abcde"
                    ,"1-3 b: cdefg"
                    ,"2-9 c: ccccccccc" }))
                .Should().Be("1");
        }

        [Fact]
        public void SecondProblemInput()
        {
            var result = Day2
                .Solve2(new FileInputResolver(Day2.Day));

            result.Should().Be("413");
        }
    }
}
