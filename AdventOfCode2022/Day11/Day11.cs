using AdventOfCode2022.Utility;
using FluentAssertions;
using System.Diagnostics;
using System.Numerics;
using System.Reflection.PortableExecutable;
using System.Security.Cryptography;
using Xunit;
using Xunit.Abstractions;

namespace AdventOfCode2022.Day11;

internal static class Day11
{
    //private static readonly int[] Checks = new[] { 1, 20, 1_000};
    private static readonly int[] Checks = new[] { 1, 20 };

    internal static Int64 Solve(List<Monkey> input)
    {
        checked
        {
            for (int i = 0; i < 20 /*1_000*/; i++)
            {
                foreach (var monkey in input)
                {
                    monkey.PlayTurn(input);
                }

                if (Checks.Contains(i+1))
                {
                    Debug.WriteLine($"== After round {i+1} ==");
                    for (int j = 0; j < input.Count; j++)
                    {
                        Debug.WriteLine($"Monkey {j} inspected items {input[j].Inspections} times.");
                    }
                }
            }

            var result = input.OrderByDescending(o => o.Inspections).Take(2).Aggregate(1L, (prod, m) => m.Inspections * prod);

            return result;
        }
    }
}

internal class Monkey
{
    public required Queue<int> Items { get; init;  }
    public required Func<int, int> Operation { get; init; }
    public required Predicate<int> Predicate { get; init; }
    public required int ThrowToIfTrue { get; init; }
    public required int ThrowToIfFalse { get; init; }
    public Int64 Inspections { get; private set; }

    public void PlayTurn(List<Monkey> monkeys)
    {
        while (Items.Count > 0)
        {
            //var a = Int64.MaxValue;
            //a = Operation(a);
            //int
            //int
            var val = Items.Dequeue();
            val = Operation(val);
            val = Relief(val);
            if (Predicate(val))
            {
                monkeys[ThrowToIfTrue].Items.Enqueue(val);
            }
            else
            {
                monkeys[ThrowToIfFalse].Items.Enqueue(val);
            }
            Inspections++;
            // 2147483647
            // 2713310158
        }
    }

    private static int Relief(int val) => val / 3;
    private static int Relief2(int val)
    {
        return GetPrimes(val).Distinct().Aggregate(1, (sum, next) => sum * next);
    }

    public override string ToString() => Inspections.ToString();

    static List<int> GetPrimes(decimal n)
    {
        List<int> storage = new List<int>();
        while (n > 1)
        {
            int i = 1;
            while (true)
            {
                if (IsPrime(i))
                {
                    if (((decimal)n / i) == Math.Round((decimal)n / i))
                    {
                        n /= i;
                        storage.Add(i);
                        break;
                    }
                }
                i++;
            }
        }
        return storage;
    }

    static bool IsPrime(int n)
    {
        if (n <= 1) return false;
        for (int i = 2; i <= Math.Sqrt(n); i++)
            if (n % i == 0) return false;
        return true;
    }
}

public class Test2022Day11
{
    [Fact]
    public void FirstProblemExamples()
    {
        Day11.Solve(new List<Monkey> {
                new Monkey
                {
                    Items = new Queue<int>(new int[] { 79, 98 }),
                    Operation = (old) => old * 19,
                    Predicate = (worried) => worried % 23 == 0,
                    ThrowToIfTrue = 2,
                    ThrowToIfFalse = 3
                },
                new Monkey
                {
                    Items = new Queue<int>(new int[] { 54, 65, 75, 74 }),
                    Operation = (old) => old + 6,
                    Predicate = (worried) => worried % 19 == 0,
                    ThrowToIfTrue = 2,
                    ThrowToIfFalse = 0
                },
                new Monkey
                {
                    Items = new Queue<int>(new int[] { 79, 60, 97 }),
                    Operation = (old) => old * old,
                    Predicate = (worried) => worried % 13 == 0,
                    ThrowToIfTrue = 1,
                    ThrowToIfFalse = 3
                },
                new Monkey
                {
                    Items = new Queue<int>(new int[] { 74 }),
                    Operation = (old) => old + 3,
                    Predicate = (worried) => worried % 17 == 0,
                    ThrowToIfTrue = 0,
                    ThrowToIfFalse = 1
                }
            })
            .Should().Be(10605);
            //.Should().Be(2713310158);
    }

    [Fact]
    public void ProblemInput()
    {
        Day11.Solve(new List<Monkey> {
                new Monkey
                {
                    Items = new Queue<int>(new int[] { 64 }),
                    Operation = (old) => old * 7,
                    Predicate = (worried) => worried % 13 == 0,
                    ThrowToIfTrue = 1,
                    ThrowToIfFalse = 3
                },
                new Monkey
                {
                    Items = new Queue<int>(new int[] { 60, 84, 84, 65 }),
                    Operation = (old) => old + 7,
                    Predicate = (worried) => worried % 19 == 0,
                    ThrowToIfTrue = 2,
                    ThrowToIfFalse = 7
                },
                new Monkey
                {
                    Items = new Queue<int>(new int[] { 52, 67, 74, 88, 51, 61 }),
                    Operation = (old) => old * 3,
                    Predicate = (worried) => worried % 5 == 0,
                    ThrowToIfTrue = 5,
                    ThrowToIfFalse = 7
                },
                new Monkey
                {
                    Items = new Queue<int>(new int[] { 67, 72 }),
                    Operation = (old) => old + 3,
                    Predicate = (worried) => worried % 2 == 0,
                    ThrowToIfTrue = 1,
                    ThrowToIfFalse = 2
                },
                new Monkey
                {
                    Items = new Queue<int>(new int[] { 80, 79, 58, 77, 68, 74, 98, 64 }),
                    Operation = (old) => old * old,
                    Predicate = (worried) => worried % 17 == 0,
                    ThrowToIfTrue = 6,
                    ThrowToIfFalse = 0
                },
                new Monkey
                {
                    Items = new Queue<int>(new int[] { 62, 53, 61, 89, 86 }),
                    Operation = (old) => old + 8,
                    Predicate = (worried) => worried % 11 == 0,
                    ThrowToIfTrue = 4,
                    ThrowToIfFalse = 6
                },
                new Monkey
                {
                    Items = new Queue<int>(new int[] { 86, 89, 82 }),
                    Operation = (old) => old + 2,
                    Predicate = (worried) => worried % 7 == 0,
                    ThrowToIfTrue = 3,
                    ThrowToIfFalse = 0
                },
                new Monkey
                {
                    Items = new Queue<int>(new int[] { 92, 81, 70, 96, 69, 84, 83 }),
                    Operation = (old) => old + 4,
                    Predicate = (worried) => worried % 3 == 0,
                    ThrowToIfTrue = 4,
                    ThrowToIfFalse = 5
                }
            })
            .Should().Be(55216);
            //.Should().Be(0);
    }

}
