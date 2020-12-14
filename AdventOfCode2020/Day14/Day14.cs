using AdventOfCode2020.Utility;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Xunit;

namespace AdventOfCode2020.Day14
{
    internal static class Day14
    {
        internal static (long partOne, long partTwo) Solve(IInputResolver input, bool skipPartTwo = false)
        {
            var data = input.AsEnumerable().ToList();

            var memory1 = new Dictionary<int, string>(10000);
            var memory2 = new Dictionary<long, long>(10000);
            var mask = "";

            foreach (var item in data)
            {
                if(item.StartsWith("mask"))
                {
                    mask = item[7..];
                }
                else
                {
                    var address = int.Parse(item[(item.IndexOf('[') + 1)..(item.IndexOf(']'))]);
                    var value = long.Parse(item[(item.IndexOf('=') + 2)..]);

                    // Part one
                    var valuePartOne = ToByteCharArray(value);
                    ApplyMask(mask, valuePartOne, (m) => m == '1' || m == '0');
                    memory1[address] = new string(valuePartOne);

                    // Part two
                    if (skipPartTwo)
                    {
                        continue;
                    }
                    char[] addressPartTwo = ToByteCharArray(address);
                    ApplyMask(mask, addressPartTwo, (m) => m == '1' || m == 'X');

                    // Now find all adresses
                    var addresses = new List<long>(100) { 0 };
                    for (int i = 35; i >= 0; i--)
                    {
                        var addVal = addressPartTwo[i];
                        if (addVal == 'X')
                        {
                            var addressesCount = addresses.Count;
                            for (int ii = 0; ii < addressesCount; ii++)
                            {
                                // Leave one as is (0).
                                // Add one as the permutation (1).
                                var currAdr = addresses[ii];
                                long newval2 = GetPowerOfTwo(35 - i);
                                addresses.Add(currAdr + newval2);
                            }
                        }
                        else if (addVal == '1')
                        {
                            var newval = GetPowerOfTwo(35 - i);
                            for (int ii = 0; ii < addresses.Count; ii++)
                            {
                                addresses[ii] += newval;
                            }
                        }
                    }
                    foreach (var adr in addresses)
                    {
                        memory2[adr] = value;
                    }
                }
            }
            // Part one
            long ans1 = 0L;
            foreach (var mem in memory1)
            {
                long count = 0L;
                for (int i = 35; i >= 0; i--)
                {
                    var val = mem.Value[i];
                    if(val == '1')
                    {
                        count += GetPowerOfTwo(35 - i);
                    }
                }
                ans1 += count;
            }

            // Part two
            long ans2 = 0L;
            if (!skipPartTwo)
            {
                foreach (var mem in memory2)
                {
                    ans2 += mem.Value;
                }
            }

            return (ans1, ans2);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static long GetPowerOfTwo(int power)
        {
            return (long)Math.Round(Math.Pow(2, power), MidpointRounding.AwayFromZero);
        }

        private static void ApplyMask(string mask, char[] valuePartOne, Func<char, bool> predicate)
        {
            for (int i = 0; i < mask.Length; i++)
            {
                var m = mask[i];
                if (predicate(m))
                    valuePartOne[i] = m;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static char[] ToByteCharArray(long intValue)
        {
            return Convert.ToString(intValue, 2).PadLeft(36, '0').ToCharArray();
        }
    }

    public class Test2020Day14
    {
        [Theory]
        [MemberData(nameof(ExampleData))]
        public void SolveProblemExamples(string[] example, int expectedFirst)
        {
            var (partOne, _) = Day14
                .Solve(new MockInputResolver(example), true);
            partOne.Should().Be(expectedFirst);
        }

        [Theory]
        [MemberData(nameof(ExampleData2))]
        public void SolveProblemExamples2(string[] example, int expectedSecond)
        {
            var (_, partTwo) = Day14
                .Solve(new MockInputResolver(example));
            partTwo.Should().Be(expectedSecond);
        }

        [Fact]
        public void SolveProblemInput()
        {
            var (partOne, partTwo) = Day14
                .Solve(new FileInputResolver(14));
            partOne.Should().Be(13476250121721);
            partTwo.Should().Be(4463708436768);
        }

        public readonly static List<object[]> ExampleData = new List<object[]>
        {
            new object[] {
                new [] {
                    "mask = XXXXXXXXXXXXXXXXXXXXXXXXXXXXX1XXXX0X",
                    "mem[8] = 11",
                    "mem[7] = 101",
                    "mem[8] = 0",
                }, 165
            }
        };

        public readonly static List<object[]> ExampleData2 = new List<object[]>
        {
            new object[] {
                new [] {
                    "mask = 000000000000000000000000000000X1001X",
                    "mem[42] = 100",
                    "mask = 00000000000000000000000000000000X0XX",
                    "mem[26] = 1",
                }, 208
            }
        };
    }
}
