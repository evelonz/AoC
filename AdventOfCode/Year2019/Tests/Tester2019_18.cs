using AdventOfCode.Utility;
using FluentAssertions;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace AdventOfCode.Year2019.Tests
{
    public class Tester2019_18
    {
        [Theory]
        [MemberData(nameof(FirstExampleData))]
        public void FirstProblemExamples(string[] example, string expected)
        {
            Solver2019_18_1.Solve(new MockInputResolver(example)).Should().Be(expected);
        }

        //[Fact]
        //public void FirstProblemMyInput()
        //{
        //    Solver2019_18_1
        //        .Solve(new FileInputResolver(2019, 22))
        //        .Should().Be("");
        //}

        //[Theory]
        //[MemberData(nameof(FirstExampleData))]
        //public void SecondProblemExamples(string[] example, string expected, int size, int? pos)
        //{
        //    Solver2019_18_1.Solve(new MockInputResolver(example)).Should().Be(expected);
        //}

        //[Fact]
        //public void SecondProblemMyInput()
        //{
        //    Solver2019_22.SolveReverse(new FileInputResolver(2019, 22), 10_007, 2519)
        //        .Should().Be("2019");
        //    //Solver2019_22.SolveSecond(new FileInputResolver(2019, 22)).Should().Be("1141251258");
        //}

        public static IEnumerable<object[]> FirstExampleData()
        {
            return allData;
        }

        private readonly static List<object[]> allData = new List<object[]>
        {
            new object[] {
                new [] {
                    "#########",
                    "#b.A.@.a#",
                    "#########"
                }, "8"
            },
            new object[] {
                new [] {
                    "########################",
                    "#f.D.E.e.C.b.A.@.a.B.c.#",
                    "######################.#",
                    "#d.....................#",
                    "########################"
                }, "86"
            },
            new object[] {
                new [] {
                    "########################",
                    "#...............b.C.D.f#",
                    "#.######################",
                    "#.....@.a.B.c.d.A.e.F.g#",
                    "########################"
                }, "132"
            },
            //new object[] {
            //    new [] {
            //        "#################",
            //        "#i.G..c...e..H.p#",
            //        "########.########",
            //        "#j.A..b...f..D.o#",
            //        "########@########",
            //        "#k.E..a...g..B.n#",
            //        "########.########",
            //        "#l.F..d...h..C.m#",
            //        "#################",
            //    }, "136"
            //},
            new object[] {
                new [] {
                    "########################",
                    "#@..............ac.GI.b#",
                    "###d#e#f################",
                    "###A#B#C################",
                    "###g#h#i################",
                    "########################"
                }, "81"
            }
        };
    }
}
