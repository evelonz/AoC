using AdventOfCode2020.Utility;
using FluentAssertions;
using System.Collections.Generic;
using System.Linq;

using Xunit;

namespace AdventOfCode2020.Day2
{
     internal static class Day2
    {
        class Record
        {
            public int Min { get; set; }
            public int Max { get; set; }
            public char Character { get; set; }
            public string Password { get; set; }
        }

        internal static string Solve1(IInputResolver input)
        {
            var data = input.AsEnumerable();
            var ddata = new List<Record>(1000);
            foreach (var line in data)
            {
                var a = new Record();
                var d = line.Split('-');
                a.Min = int.Parse(d[0]);
                var e = d[1].Split(' ');
                a.Max = int.Parse(e[0]);
                var f = e[1].Split(':');
                a.Character = f[0].First();
                a.Password = e[2];
                ddata.Add(a);
            }
            int corr = 0;
            foreach (var item in ddata)
            {
                var antal = item.Password.Where(x => x == item.Character).Count();
                if (antal >= item.Min && antal <= item.Max)
                    corr++;

            }
            return corr.ToString();
        }


        internal static string Solve2(IInputResolver input)
        {
            var data = input.AsEnumerable();
            var ddata = new List<Record>(1000);
            foreach (var line in data)
            {
                var a = new Record();
                var d = line.Split('-');
                a.Min = int.Parse(d[0]);
                var e = d[1].Split(' ');
                a.Max = int.Parse(e[0]);
                var f = e[1].Split(':');
                a.Character = f[0].First();
                a.Password = e[2];
                ddata.Add(a);
            }
            int corr = 0;
            foreach (var item in ddata)
            {
                var pos1 = item.Password[item.Min - 1] == item.Character ? 1 : 0;
                var pos2 = item.Password[item.Max - 1] == item.Character ? 1 : 0;
                if (pos1 + pos2 == 1)
                    corr++;
            }
            return corr.ToString();
        }
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
                .Solve1(new FileInputResolver(2));

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
                .Solve2(new FileInputResolver(2));

            result.Should().Be("413");
        }
    }
}
