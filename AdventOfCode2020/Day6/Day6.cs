using AdventOfCode2020.Utility;
using FluentAssertions;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace AdventOfCode2020.Day6
{
    internal static class Day6
    {
        internal static (int partOne, int partTwo) Solve(IInputResolver input)
        {
            var data = input.AsEnumerable();
            var enumerator = data.GetEnumerator();
            int ans1 = 0;
            int ans2 = 0;

            while (enumerator.MoveNext())
            {
                var val = enumerator.Current;
                var answers = new List<char>(28);
                var answersWithAllYes = new List<char[]>(28);
                var hasMore = true;
                while (val.Length != 0 && hasMore)
                {
                    answers.AddRange(val.ToArray());
                    answersWithAllYes.Add(val.ToArray());
                    hasMore = enumerator.MoveNext();
                    if (hasMore)
                        val = enumerator.Current;
                }
                answers = answers.OrderBy(o => o).ToList();
                var groupAnswers = answers.Distinct().Count();
                ans1 += groupAnswers;

                var template = answersWithAllYes.First().ToList();
                foreach (var answer in answersWithAllYes)
                {
                    template = template.Where(x => answer.Contains(x)).ToList();
                }
                ans2 += template.Count;
            }

            return (ans1, ans2);
        }
    }

    public class Test2020Day6
    {
        [Fact]
        public void FirstProblemExamples()
        {
            var (partOne, partTwo) = Day6
                .Solve(new MockInputResolver(new string[] {
                    "abc",
                    "",
                    "a",
                    "b",
                    "c",
                    "",
                    "ab",
                    "ac",
                    "",
                    "a",
                    "a",
                    "a",
                    "a",
                    "",
                    "b",}));

            partOne.Should().Be(11);
            partTwo.Should().Be(6);
        }

        [Fact]
        public void FirstProblemInput()
        {
            var (partOne, partTwo) = Day6
                .Solve(new FileInputResolver(6));

            partOne.Should().Be(6273);
            partTwo.Should().Be(3254);
        }

    }
}
