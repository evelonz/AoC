global using AdventOfCode2021.Utility;
global using FluentAssertions;
global using Xunit;

namespace AdventOfCode2020.Puzzles.Day3;

internal static class Day3
{
    internal static string Solve1(IInputResolver input)
    {
        var data = input.AsEnumerable().ToList();
        var mostCommon = new int[data[0].Length];
        
        foreach (var binary in data)
        {
            for (int i = 0; i < binary.Length; i++)
            {
                if (binary[i] == '1')
                {
                    mostCommon[i]++;
                }
                else
                {
                    mostCommon[i]--;
                }
            }
        }
        var gamma = 0;
        var epsilon = 0;
        int k = 0;
        for (int j = mostCommon.Length-1; j >= 0; j--)
        {
            var value = mostCommon[j];
            if(value > 0)
            {
                gamma += (int)Math.Pow(2, k);
                
            }
            else
            {
                epsilon += (int)Math.Pow(2, k);
            }
            k++;
        }

        return (gamma * epsilon).ToString();
    }

    internal static string Solve2(IInputResolver input)
    {
        var data = input.AsEnumerable().ToArray();
        
        var oxygenList = GetLifeSupportRating(data, (mostCommon) => mostCommon >= 0 ? '1' : '0');
        var co2ScrubberList = GetLifeSupportRating(data, (mostCommon) => mostCommon >= 0 ? '0' : '1');

        var oxygen = Convert.ToInt32(oxygenList, 2);
        var co2Scrubber = Convert.ToInt32(co2ScrubberList, 2);

        return (oxygen * co2Scrubber).ToString();
    }

    private static string GetLifeSupportRating(string[] data, Func<int, char> byteToKeep)
    {
        var filteredList = new string[data.Length];
        Array.Copy(data, filteredList, filteredList.Length);

        for (int currentByte = 0; currentByte < data[0].Length; currentByte++)
        {
            var mostCommon = filteredList.Sum(binary => binary[currentByte] == '1' ? 1 : -1);

            var ToKeep = byteToKeep(mostCommon);
            filteredList = filteredList.Where(x => x[currentByte] == ToKeep).ToArray();
            
            if (filteredList.Length == 1)
            {
                break;
            }
        }

        return filteredList[0];
    }
}

public class Test2020Day3
{
    [Fact]
    public void FirstProblemExamples()
    {
        Day3
            .Solve1(new MockInputResolver(new string[] {
                "00100",
                "11110",
                "10110",
                "10111",
                "10101",
                "01111",
                "00111",
                "11100",
                "10000",
                "11001",
                "00010",
                "01010" }))
            .Should().Be("198");
    }

    [Fact]
    public void FirstProblemInput()
    {
        var result = Day3
            .Solve1(new FileInputResolver(3));

        result.Should().Be("2261546");
    }

    [Fact]
    public void SecondProblemExamples()
    {
        Day3
            .Solve2(new MockInputResolver(new string[] {
                "00100",
                "11110",
                "10110",
                "10111",
                "10101",
                "01111",
                "00111",
                "11100",
                "10000",
                "11001",
                "00010",
                "01010" }))
            .Should().Be("230");
    }

    [Fact]
    public void SecondProblemInput()
    {
        var result = Day3
            .Solve2(new FileInputResolver(3));

        result.Should().Be("6775520");
    }
}

