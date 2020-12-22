using AdventOfCode2020.Utility;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace AdventOfCode2020.Day18
{
    internal static class Day18
    {
        // Note: A cleaner solution would have been to use Reverse Polish Notation.
        // A solution for it in C# with any operator order can be found here, https://rosettacode.org/wiki/Parsing/Shunting-yard_algorithm.
        internal static (long partOne, long partTwo) Solve(IInputResolver input)
        {
            var data = input.AsEnumerable();
            long ans1 = 0;
            long ans2 = 0;
            foreach (var row in data)
            {
                ans1 += CalculateRow(row);
                ans2 += CalculateRowPart2(row);
            }

            return (ans1, ans2);
        }

        private static long CalculateRow(string data)
        {
            var stack = new Stack<string>();

            foreach (var word in data.Replace("(", "( ").Replace(")", " )").Split(' ').Reverse())
            {
                if (word == "(")
                {
                    var poping = true;
                    while (poping)
                    {
                        var r = stack.Pop();
                        var o = stack.Pop();
                        var l = stack.Pop();
                        var res = Calc(l, o, r);
                        if (stack.Peek() == ")")
                        {
                            stack.Pop();
                            poping = false;
                        }
                        stack.Push(res);
                    }
                }
                else
                {
                    stack.Push(word);
                }
            }

            // Now all parentheses should be gone.
            while (stack.Count > 2)
            {
                var r = stack.Pop();
                var o = stack.Pop();
                var l = stack.Pop();
                var res = Calc(l, o, r);
                stack.Push(res);
            }

            return long.Parse(stack.Pop());
        }

        private static string Calc(string left, string op, string right)
        {
            long l = long.Parse(left);
            long r = long.Parse(right);
            l = op switch
            {
                "+" => l + r,
                "*" => l * r,
            };
            return l.ToString();
        }


        private static long CalculateRowPart2(string data)
        {
            var stack = new Stack<string>();

            // if (3 + 3 * 2) then round 1 we get 3 * 2.
            // Peek gives +.
            // Pop 2 more to get, + 3. Take 3 + 3 = 9. Push 9 * 2.
            // Peek gives (.
            // Calc as normal.
            // Peek gives *
            // Calc as normal, as order does not matter.
            foreach (var word in data.Replace("(", "( ").Replace(")", " )").Split(' ').Reverse())
            {
                if (word == "(")
                {
                    var poping = true;
                    while (poping)
                    {
                        var l = stack.Pop();
                        var o = stack.Pop();
                        var r = stack.Pop();
                        if (stack.Peek() == ")" || o == "+" || stack.Peek() == "*")
                        {
                            var res = Calc(l, o, r);
                            if(stack.Peek() == ")")
                            {
                                poping = false;
                                stack.Pop();
                            }
                            stack.Push(res);
                        }
                        else // peek == "+"
                        {
                            var o2 = stack.Pop();
                            var r2 = stack.Pop();
                            var res = Calc(r, o2, r2);
                            stack.Push(res);
                            stack.Push(o);
                            stack.Push(l);
                        }
                    }
                }
                else
                {
                    stack.Push(word);
                }
            }

            // Now all parentheses should be gone.
            while (stack.Count > 2)
            {
                var l = stack.Pop();
                var o = stack.Pop();
                var r = stack.Pop();
                if(o == "+" || stack.Count == 0 || (stack.Count > 0 && stack.Peek() == "*"))
                {
                    var resm = Calc(r, o, l);
                    stack.Push(resm);
                }
                else
                {
                    var o2 = stack.Pop();
                    var r2 = stack.Pop();
                    var res = Calc(r, o2, r2);
                    stack.Push(res);
                    stack.Push(o);
                    stack.Push(l);
                }
            }

            return long.Parse(stack.Pop());
        }
    }

    public class Test2020Day18
    {
        [Theory]
        [MemberData(nameof(ExampleData))]
        public void SolveProblemExamples(string[] example, int expectedFirst, int expectedSecond)
        {
            var (partOne, partTwo) = Day18
                .Solve(new MockInputResolver(example));
            partOne.Should().Be(expectedFirst);
            partTwo.Should().Be(expectedSecond);
        }

        [Fact]
        public void SolveProblemInput()
        {
            var (partOne, partTwo) = Day18
                .Solve(new FileInputResolver(18));
            partOne.Should().Be(2743012121210);
            partTwo.Should().Be(65658760783597);
        }

        public readonly static List<object[]> ExampleData = new List<object[]>
        {
            new object[] {
                new [] {
                    "1 + 2 * 3 + 4 * 5 + 6",
                }, 71, 231
            },
            new object[] {
                new [] {
                    "1 + (2 * 3) + (4 * (5 + 6))",
                }, 51, 51
            },
            new object[] {
                new [] {
                    "2 * 3 + (4 * 5)",
                }, 26, 46
            },
            new object[] {
                new [] {
                    "5 + (8 * 3 + 9 + 3 * 4 * 3)",
                }, 437, 1445
            },
            new object[] {
                new [] {
                    "5 * 9 * (7 * 3 * 3 + 9 * 3 + (8 + 6 * 4))",
                }, 12240, 669060
            },
            new object[] {
                new [] {
                    "((2 + 4 * 9) * (6 + 9 * 8 + 6) + 6) + 2 + 4 * 2",
                }, 13632, 23340
            }
        };
    }
}
