namespace AdventOfCode2023.Day1;

internal static class Day1
{
    internal static int Solve(IInputResolver input)
    {
        var sum = 0;
        foreach (var item in input.AsEnumerable())
        {
            int first = 0;
            int last = 0;
            bool firstDigit = true;

            var (lowdigit, highdigit, lowIndex, highIndex) = FindDigitsAsText(item);
            int index = 0;
            int highIndex2 = -1;
            int lowIndex2 = item.Length + 1;
            foreach (var item2 in item)
            {
                if (int.TryParse(item2.ToString().AsSpan(), out var res))
                {
                    last = res;
                    highIndex2 = index;
                    if (firstDigit)
                    {
                        first = last;
                        firstDigit = false;
                        lowIndex2 = index;
                    }
                }
                index++;
            }
            if (lowIndex < lowIndex2)
                first = lowdigit;
            if (highIndex > highIndex2)
                last = highdigit;
            var digit = (first * 10) + last;
            sum += digit;
            System.Diagnostics.Debug.WriteLine($"{first},{last}, {digit}, {sum}");
        }
        return sum;
    }

    internal static (int lowdigit, int highdigit, int lowIndex, int highIndex)  FindDigitsAsText(string input)
    {
        var digits = new string[] {
                "one",
                "two",
                "three",
                "four",
                "five",
                "six",
                "seven",
                "eight",
                "nine"
            };
        var lowIndex = input.Length + 1;
        var lowdigit = -1;
        var highIndex = -1;
        var highdigit = -1;
        int value = 1;
        foreach (var digit in digits)
        {
            var low = input.IndexOf(digit);
            if(low != -1)
            {
                if (low < lowIndex)
                {
                    lowIndex = low;
                    lowdigit = value;

                }
                if (low > highIndex)
                {
                    highIndex = low;
                    highdigit = value;
                }
            }
            low = input.LastIndexOf(digit);
            if (low != -1)
            {
                if (low < lowIndex)
                {
                    lowIndex = low;
                    lowdigit = value;

                }
                if (low > highIndex)
                {
                    highIndex = low;
                    highdigit = value;
                }
            }
            value++;
        }
        return (lowdigit,  highdigit, lowIndex, highIndex);
    }
}

public class Test2022Day231
{
    [Fact]
    public void FirstProblemExamples()
    {
        Day1
            .Solve(new MockInputResolver(new string[] {
                "1abc2",
                "pqr3stu8vwx",
                "a1b2c3d4e5f",
                "treb7uchet"
            }))
            .Should().Be(142);
    }

    [Fact]
    public void FirstProblemExamples2()
    {
        Day1
            .Solve(new MockInputResolver(new string[] {
                "two1nine",
                "eightwothree",
                "abcone2threexyz",
                "xtwone3four",
                "4nineeightseven2",
                "zoneight234",
                "7pqrstsixteen",
            }))
            .Should().Be(281);
    }

    [Fact]
    public void FindDigitFromString()
    {
        Day1
            .FindDigitsAsText("two1nine")
            .Should().Be((2, 9, 0, 4));
    }

    [Fact]
    public void ProblemInput()
    {
        var res = Day1
            .Solve(new FileInputResolver(1));

        //res.Should().Be(53080);
        res.Should().Be(53268);
    }
}
