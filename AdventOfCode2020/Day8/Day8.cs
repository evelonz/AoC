using AdventOfCode2020.Utility;
using FluentAssertions;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace AdventOfCode2020.Day8
{
    internal static class Day8
    {
        internal static (int partOne, int partTwo) Solve(IInputResolver input)
        {
            var data = input.AsEnumerable().ToArray();
            int ans1 = -1;
            (ans1, _) = RunCode(data);

            int changeAttempt = 0;

            while (true)
            {
                bool hasSetInst = false;
                string lastInst = "";
                while(!hasSetInst)
                {
                    var inst = data[changeAttempt][..3];
                    var valu = int.Parse(data[changeAttempt][4..]);

                    if (inst == "nop" && valu == 0)
                    {
                        changeAttempt++;
                    }
                    else if(inst == "nop")
                    {
                        lastInst = data[changeAttempt];
                        data[changeAttempt] = $"acc {data[changeAttempt][4..]}";
                        hasSetInst = true;
                    }
                    else if (inst == "jmp")
                    {
                        lastInst = data[changeAttempt];
                        data[changeAttempt] = $"nop {data[changeAttempt][4..]}";
                        hasSetInst = true;
                    }
                    else { changeAttempt++; }
                }

                var (_, ans2) = RunCode(data);
                if(ans2 != -1)
                {
                    return (ans1, ans2);
                }
                data[changeAttempt++] = lastInst;
            }
        }

        private static (int, int) RunCode(string[] data)
        {
            var visited = new HashSet<int>(data.Length);
            int acc = 0;
            int pointer = 0;

            while (true)
            {
                if (visited.Contains(pointer))
                {
                    return (acc, -1);
                }
                if (pointer == data.Length)
                    return (-1, acc);

                visited.Add(pointer);
                var inst = data[pointer][..3];
                switch (inst)
                {
                    case "nop":
                        pointer++;
                        break;
                    case "acc":
                        var valu = data[pointer][4..];
                        acc += int.Parse(valu);
                        pointer++;
                        break;
                    case "jmp":
                        var valu2 = data[pointer][4..];
                        pointer += int.Parse(valu2);
                        break;
                }
            }
        }
    }

    public class Test2020Day8
    {
        [Fact]
        public void FirstProblemExamples()
        {
            var (partOne, partTwo) = Day8
                .Solve(new MockInputResolver(new string[] {
                    "nop +0",
                    "acc +1",
                    "jmp +4",
                    "acc +3",
                    "jmp -3",
                    "acc -99",
                    "acc +1",
                    "jmp -4",
                    "acc +6",
                }));

            partOne.Should().Be(5);
            partTwo.Should().Be(8);
        }

        [Fact]
        public void FirstProblemInput()
        {
            var (partOne, partTwo) = Day8
                .Solve(new FileInputResolver(8));

            partOne.Should().Be(1930);
            partTwo.Should().Be(1688);
        }

        public readonly static List<object[]> ExampleData = new List<object[]>
        {
            new object[] {
                new [] {
                    "light red bags contain 1 bright white bag, 2 muted yellow bags.",
                    "dark orange bags contain 3 bright white bags, 4 muted yellow bags.",
                    "bright white bags contain 1 shiny gold bag.",
                    "muted yellow bags contain 2 shiny gold bags, 9 faded blue bags.",
                    "shiny gold bags contain 1 dark olive bag, 2 vibrant plum bags.",
                    "dark olive bags contain 3 faded blue bags, 4 dotted black bags.",
                    "vibrant plum bags contain 5 faded blue bags, 6 dotted black bags.",
                    "faded blue bags contain no other bags.",
                    "dotted black bags contain no other bags.",
                }, 4, 32
            },
            new object[] {
                new [] {
                    "shiny gold bags contain 2 dark red bags.",
                    "dark red bags contain 2 dark orange bags.",
                    "dark orange bags contain 2 dark yellow bags.",
                    "dark yellow bags contain 2 dark green bags.",
                    "dark green bags contain 2 dark blue bags.",
                    "dark blue bags contain 2 dark violet bags.",
                    "dark violet bags contain no other bags.",
                }, 0, 126
            },
        };
    }
}
