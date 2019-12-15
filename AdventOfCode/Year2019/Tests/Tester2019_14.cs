using AdventOfCode.Utility;
using FluentAssertions;
using Xunit;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Year2019.Tests
{
    public class Tester2019_14
    {
        [Theory]
        [MemberData(nameof(FirstExampleData))]
        public void FirstProblemExamples(string[] example, string expected)
        {
            Solver2019_14.SolveFirst(new MockInputResolver(example)).Should().Be(expected);
        }

        [Fact]
        public void FirstProblemMyInput()
        {
            Solver2019_14.SolveFirst(new FileInputResolver(2019, 14)).Should().Be("751038");
        }

        [Fact]
        public void SecondProblemExample()
        {
            var input = new[] {
                    "10 ORE => 10 A",
                    "1 ORE => 1 B",
                    "7 A, 1 B => 1 C",
                    "7 A, 1 C => 1 D",
                    "7 A, 1 D => 1 E",
                    "7 A, 1 E => 1 FUEL" };
            Solver2019_14.SolveSecond(new MockInputResolver(input), 10).Should().Be("0");
            Solver2019_14.SolveSecond(new MockInputResolver(input), 31).Should().Be("1");
            // We have 2 extra A per fuel. 28 A is needed for one fuel.
            // So after 28/2 = 14 iterations, we have enough A left to make one more fuel
            // without extra ore. We still need one ore to make the last B though.
            // 14*31 = 434. 434+1 = 435. Which should give us 15 fuel.
            Solver2019_14.SolveSecond(new MockInputResolver(input), 435).Should().Be("15");
        }

        //[Fact(Skip = "Using brute force, each of the part 2 solutions takes about 17 minutes")]
        //public void SecondProblemMyInput()
        //{
        //    Solver2019_14.SolveSecond(new FileInputResolver(2019, 14)).Should().Be("2074843");
        //}

        public static IEnumerable<object[]> FirstExampleData()
        {
            return allData.Select(s => s.Take(2).ToArray());
        }

        public static IEnumerable<object[]> SecondExampleData()
        {
            return allData.Skip(2).Select(s => new object[] { s[0], s[2] });
        }

        private readonly static List<object[]> allData = new List<object[]>
        {
            new object[] {
                new [] {
                    "10 ORE => 10 A",
                    "1 ORE => 1 B",
                    "7 A, 1 B => 1 C",
                    "7 A, 1 C => 1 D",
                    "7 A, 1 D => 1 E",
                    "7 A, 1 E => 1 FUEL" }, "31"
            },
            new object[] {
                new [] {
                    "9 ORE => 2 A",
                    "8 ORE => 3 B",
                    "7 ORE => 5 C",
                    "3 A, 4 B => 1 AB",
                    "5 B, 7 C => 1 BC",
                    "4 C, 1 A => 1 CA",
                    "2 AB, 3 BC, 4 CA => 1 FUEL" }, "165"
            },
            new object[] {
                new [] {
                    "157 ORE => 5 NZVS",
                    "165 ORE => 6 DCFZ",
                    "44 XJWVT, 5 KHKGT, 1 QDVJ, 29 NZVS, 9 GPVTF, 48 HKGWZ => 1 FUEL",
                    "12 HKGWZ, 1 GPVTF, 8 PSHF => 9 QDVJ",
                    "179 ORE => 7 PSHF",
                    "177 ORE => 5 HKGWZ",
                    "7 DCFZ, 7 PSHF => 2 XJWVT",
                    "165 ORE => 2 GPVTF",
                    "3 DCFZ, 7 NZVS, 5 HKGWZ, 10 PSHF => 8 KHKGT" }, "13312", "82892753"
            },
            new object[] {
                new [] {
                    "2 VPVL, 7 FWMGM, 2 CXFTF, 11 MNCFX => 1 STKFG",
                    "17 NVRVD, 3 JNWZP => 8 VPVL",
                    "53 STKFG, 6 MNCFX, 46 VJHF, 81 HVMC, 68 CXFTF, 25 GNMV => 1 FUEL",
                    "22 VJHF, 37 MNCFX => 5 FWMGM",
                    "139 ORE => 4 NVRVD",
                    "144 ORE => 7 JNWZP",
                    "5 MNCFX, 7 RFSQX, 2 FWMGM, 2 VPVL, 19 CXFTF => 3 HVMC",
                    "5 VJHF, 7 MNCFX, 9 VPVL, 37 CXFTF => 6 GNMV",
                    "145 ORE => 6 MNCFX",
                    "1 NVRVD => 8 CXFTF",
                    "1 VJHF, 6 MNCFX => 4 RFSQX",
                    "176 ORE => 6 VJHF" }, "180697", "5586022"
            },
            new object[] {
                new [] {
                    "171 ORE => 8 CNZTR",
                    "7 ZLQW, 3 BMBT, 9 XCVML, 26 XMNCP, 1 WPTQ, 2 MZWV, 1 RJRHP => 4 PLWSL",
                    "114 ORE => 4 BHXH",
                    "14 VRPVC => 6 BMBT",
                    "6 BHXH, 18 KTJDG, 12 WPTQ, 7 PLWSL, 31 FHTLT, 37 ZDVW => 1 FUEL",
                    "6 WPTQ, 2 BMBT, 8 ZLQW, 18 KTJDG, 1 XMNCP, 6 MZWV, 1 RJRHP => 6 FHTLT",
                    "15 XDBXC, 2 LTCX, 1 VRPVC => 6 ZLQW",
                    "13 WPTQ, 10 LTCX, 3 RJRHP, 14 XMNCP, 2 MZWV, 1 ZLQW => 1 ZDVW",
                    "5 BMBT => 4 WPTQ",
                    "189 ORE => 9 KTJDG",
                    "1 MZWV, 17 XDBXC, 3 XCVML => 2 XMNCP",
                    "12 VRPVC, 27 CNZTR => 2 XDBXC",
                    "15 KTJDG, 12 BHXH => 5 XCVML",
                    "3 BHXH, 2 VRPVC => 7 MZWV",
                    "121 ORE => 7 VRPVC",
                    "7 XCVML => 6 RJRHP",
                    "5 BHXH, 4 VRPVC => 5 LTCX" }, "2210736", "460664"
            },
        };
    }
}
