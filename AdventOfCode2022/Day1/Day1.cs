using AdventOfCode2022.Utility;
using FluentAssertions;
using Xunit;

namespace AdventOfCode2022.Day1;

internal static class Day1
{
    internal static (int partOne, int partTwo) Solve2(IInputResolver input)
    {
        var a = input.AsEnumerable()
            //.Select(s => int.Parse(s))
            .Aggregate((new List<int>(), 0), (sum, current) => {
                if (current.Length == 0)
                {
                    sum.Item1.Add(sum.Item2);
                    return (sum.Item1, 0);
                }

                sum.Item2 += int.Parse(current);
                return sum;
                });

        a.Item1.Add(a.Item2); // Add last item.
        var b = a.Item1.OrderByDescending(x => x);
        var po = b.First();
        var pt = b.Take(3).Sum();

        var c = input.AsEnumerable()
            .Select(s => s.Length == 0 ? 0 : int.Parse(s))
            .TakeWhile2(x => x == 0, (sum, current) => sum + current)
            .OrderByDescending(x => x);
        var po2 = c.First();
        var pt2 = c.Take(3).Sum();

        var currentSum = 0;
        var count = new List<int>();
        foreach (var item in input.AsEnumerable())
        {
            if (item?.Length == 0)
            {
                count.Add(currentSum);
                currentSum = 0;
            }
            else
            {
                currentSum += int.Parse(item!);
            }
        }
        count.Add(currentSum); // Add last element.

        var orderedList = count.OrderByDescending(x => x);
        var partOne = orderedList.First();
        var partTwo = orderedList.Take(3).Sum();

        return (partOne, partTwo);
    }
}

public static class ListExtension
{
    public static IEnumerable<TSource> TakeWhile2<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> splitPredicate, Func<TSource, TSource, TSource> func)
    {
        // I want a function,
        // which on an IEnumerable,
        // Iterates over it, until a condition is met,
        // Then forwards that partial iteration
        // To an aggregate function
        // And in the end returns an IEnumerable of those aggregations
        // All while only iterating the original IEnumerable once
        // And getting rid of the need to manually add/handle the end of the list (last aggregate) as in the code above.


        // Based on Enumerable.cs Aggregate source code. https://github.com/microsoft/referencesource/blob/master/System.Core/System/Linq/Enumerable.cs

        //if (source == null) throw Error.ArgumentNull("source");
        //if (func == null) throw Error.ArgumentNull("func");

        using IEnumerator<TSource> e = source.GetEnumerator();
        
        // This is the aggregate function in essense.
        // TODO: Now we need to add a predicate check, and if true, restart the aggregation.
        if (!e.MoveNext()) throw new ArgumentException("Enumrator is empty");

        TSource result = e.Current;
        while (e.MoveNext())
        {
            if (splitPredicate(e.Current))
            {
                yield return result;
                if (!e.MoveNext())
                {
                    // TODO: here we want to exit without the last yield.
                    // But we still want the last yield in case the predicate does not hit on the last element.
                    yield break;
                }
                
                // TODO: In reality we have to loop until predicate is false before setting the current value...
                result = e.Current;

                continue;
            }

            result = func(result, e.Current);
        }

        yield return result;
    }

    public static IEnumerable<TAccumulate> TakeWhile2<TSource, TAccumulate>(
        this IEnumerable<TSource> source,
        Func<TAccumulate> seed,
        Func<TSource, bool> splitPredicate,
        Func<TAccumulate, TSource, TAccumulate> func)
    {
        //if (source == null) throw Error.ArgumentNull("source");
        //if (func == null) throw Error.ArgumentNull("func");

        TAccumulate result = seed();
        foreach (TSource element in source)
        {
            if (splitPredicate(element))
            {
                yield return result;
                result = seed();
                continue;
            }
            result = func(result, element);
        }
        yield return result;
    }
}

public class ListExtensionTests
{
    [Fact]
    public void SplitInTwo()
    {
        var input = new int[] { 2, 3, 4, 0, 5, 6 };

        var result = input.TakeWhile2(x => x == 0, (sum, current) => sum + current);

        result.Should().BeEquivalentTo(new int[] { 9, 11 });
    }

    [Fact]
    public void SplitInThree()
    {
        var input = new int[] { 2, 3, 4, 0, 5, 6, 0, 9, -2 };

        var result = input.TakeWhile2(x => x == 0, (sum, current) => sum + current);

        result.Should().BeEquivalentTo(new int[] { 9, 11, 7 });
    }

    [Fact]
    public void WithoutSplit()
    {
        var input = new int[] { 2, 3, 4 };

        var result = input.TakeWhile2(x => x == 0, (sum, current) => sum + current);

        result.Should().BeEquivalentTo(new int[] { 9 });
    }

    [Fact]
    public void SplitOnLastElement()
    {
        var input = new int[] { 2, 3, 4, 0, 5, 6, 0 };

        var result = input.TakeWhile2(x => x == 0, (sum, current) => sum + current);

        result.Should().BeEquivalentTo(new int[] { 9, 11 });
    }

    [Fact]
    public void SplitOnFirstElement()
    {
        var input = new int[] { 0, 2, 3, 4, 0, 5, 6 };

        var result = input.TakeWhile2(x => x == 0, (sum, current) => sum + current);

        result.Should().BeEquivalentTo(new int[] { 9, 11 });
    }

    [Fact]
    public void PredicateTrueMultipleTimesInARow()
    {
        var input = new int[] { 2, 3, 4, -1, -1, 5, 6 };

        var result = input.TakeWhile2(x => x == -1, (sum, current) => sum + current);

        result.Should().BeEquivalentTo(new int[] { 9, 11 });
    }
}

public class Test2022Day1
{
    [Fact]
    public void FirstProblemExamples()
    {
        Day1
            .Solve2(new MockInputResolver(new string[] {
                "1000",
                "2000",
                "3000",
                "",
                "4000",
                "",
                "5000",
                "6000",
                "",
                "7000",
                "8000",
                "9000",
                "",
                "10000",
            }))
            .Should().Be((24000 ,45000));
    }

    [Fact]
    public void ProblemInput()
    {
        var (partOne, partTwo) = Day1
            .Solve2(new FileInputResolver(1));

        partOne.Should().Be(72240);
        partTwo.Should().Be(210957);
    }
}
