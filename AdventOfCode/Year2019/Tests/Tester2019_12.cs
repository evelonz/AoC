using AdventOfCode.Utility;
using FluentAssertions;
using Xunit;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Year2019.Tests
{
    public class Tester2019_12
    {
        [Theory]
        [MemberData(nameof(FirstExampleData))]
        public void FirstProblem(
            (int x, int y, int z, int vx, int vy, int vz)[] input,
            int steps,
            string expected)
        {
            Solver2019_12_1.Solve(input, steps).Should().Be(expected);
        }

        [Theory]
        [MemberData(nameof(SecondExampleData))]
        public void SecondProblemMyInput((int x, int y, int z, int vx, int vy, int vz)[] input,
            string expected)
        {
            Solver2019_12_2.Solve(input).Should().Be(expected);
        }

        public static IEnumerable<object[]> FirstExampleData()
        {
            return allData.Select(s => s.Take(3).ToArray());
        }

        public static IEnumerable<object[]> SecondExampleData()
        {
            return allData.Select(s => new object[] { s[0], s[3]});
        }

        private readonly static List<object[]> allData = new List<object[]>
        {
            new object[] {
                new (int x, int y, int z, int vx, int vy, int vz)[]
                {
                    (x: -1, y:0, z:2   , vx: 0, vy: 0, vz: 0),
                    (x: 2, y:-10, z:-7, vx: 0, vy: 0, vz: 0),
                    (x: 4, y:-8, z:8  , vx: 0, vy: 0, vz: 0),
                    (x: 3, y:5, z:-1  , vx: 0, vy: 0, vz: 0)
                }, 10, "179", "2772"
            },
            new object[] {
                new (int x, int y, int z, int vx, int vy, int vz)[]
                {
                    (x: -8, y:-10, z: 0   , vx: 0, vy: 0, vz: 0),
                    (x:  5,  y: 5, z: 10, vx: 0, vy: 0, vz: 0),
                    (x: 2, y:  -7, z: 3  , vx: 0, vy: 0, vz: 0),
                    (x: 9, y:-8, z: -3  , vx: 0, vy: 0, vz: 0)
                }, 100, "1940", "4686774924"
            },
            new object[]
            {
                new (int x, int y, int z, int vx, int vy, int vz)[]
                {
                    (x: -9, y:-1, z:-1   , vx: 0, vy: 0, vz: 0),
                    (x:  2,  y: 9, z: 5, vx: 0, vy: 0, vz: 0),
                    (x: 10, y: 18, z:-12  , vx: 0, vy: 0, vz: 0),
                    (x: -6, y:15, z:-7  , vx: 0, vy: 0, vz: 0)
                }, 1000, "12644", "290314621566528"
            }
        };
    }
}
