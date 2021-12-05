global using AdventOfCode2021.Utility;
global using FluentAssertions;
global using Xunit;

namespace AdventOfCode2020.Puzzles.Day4;

internal static class Day4
{
    internal static string Solve1(IInputResolver input) => Solve(input, false);

    internal static string Solve2(IInputResolver input) => Solve(input, true);

    private static string Solve(IInputResolver input, bool part2)
    {
        var data = input.AsEnumerable().ToArray();

        var drawnNumbers = data[0].Split(',').Select(x => int.Parse(x));
        var subData = new string[Board.BoardSize];
        var boards = new List<Board>(data.Length / (Board.BoardSize+1));
        var datas = data.AsSpan();
        for (int i = 2; i < data.Length; i += (Board.BoardSize + 1))
        {

            boards.Add(new Board(datas.Slice(i, Board.BoardSize)));

            if (i + 5 == data.Length)
                break;
        }

        foreach (var number in drawnNumbers)
        {
            foreach (var board in boards)
            {
                var (done, result) = board.SetNumber(number);
                if (done)
                {
                    if(!part2)
                        return result.ToString();

                    boards = boards.Where(x => x != board).ToList();
                    if (boards.Count == 0)
                        return result.ToString();
                }
            }
        }

        return "";
    }

    private class Board
    {
        private (int value, bool isSet)[,] Numbers { get; set; }
        public const int BoardSize = 5;

        public Board(ReadOnlySpan<string> data)
        {
            Numbers = new (int, bool)[BoardSize, BoardSize];
            for (int rows = 0; rows < BoardSize; rows++)
            {
                var item = data[rows];
                var row = item.Split(' ').Where(x => x != "").ToArray();
                for (int col = 0; col < row.Length; col++)
                {
                    Numbers[rows, col] = (int.Parse(row[col]), false);
                }
            }
        }

        public (bool, int) SetNumber(int drawnNumber)
        {
            for (int rows = 0; rows < BoardSize; rows++)
            {
                for (int col = 0; col < BoardSize; col++)
                {
                    if(Numbers[rows, col].value == drawnNumber)
                    {
                        Numbers[rows, col].isSet = true;
                        if((Numbers[rows, 0].isSet && Numbers[rows, 1].isSet && Numbers[rows, 2].isSet &&
                            Numbers[rows, 3].isSet && Numbers[rows, 4].isSet)
                            || 
                            (Numbers[0, col].isSet && Numbers[1, col].isSet && Numbers[2, col].isSet &&
                            Numbers[3, col].isSet && Numbers[4, col].isSet))
                        {
                            var sum = 0;
                            foreach (var (value, isSet) in Numbers)
                            {
                                sum += isSet ? 0 : value;
                            }

                            return (true, sum * drawnNumber);
                        }

                        break;
                    }
                }
            }
            return (false, 0);
        } 
    }

}

public class Test2020Day4
{
    [Fact]
    public void FirstProblemExamples()
    {
        Day4
            .Solve1(new MockInputResolver(new string[] {
                "7,4,9,5,11,17,23,2,0,14,21,24,10,16,13,6,15,25,12,22,18,20,8,19,3,26,1",
                "",
                "22 13 17 11  0",
                 "8  2 23  4 24",
                "21  9 14 16  7",
                 "6 10  3 18  5",
                 "1 12 20 15 19",
                              "",
                 "3 15  0  2 22",
                 "9 18 13 17  5",
                "19  8  7 25 23",
                "20 11 10 24  4",
                "14 21 16 12  6",
                              "",
                "14 21 17 24  4",
                "10 16 15  9 19",
                "18  8 23 26 20",
                "22 11 13  6  5",
                 "2  0 12  3  7" }))
            .Should().Be("4512");
    }

    [Fact]
    public void FirstProblemInput()
    {
        var result = Day4
            .Solve1(new FileInputResolver(4));

        result.Should().Be("44088");
    }

    [Fact]
    public void SecondProblemExamples()
    {
        Day4
            .Solve2(new MockInputResolver(new string[] {
                "7,4,9,5,11,17,23,2,0,14,21,24,10,16,13,6,15,25,12,22,18,20,8,19,3,26,1",
                "",
                "22 13 17 11  0",
                 "8  2 23  4 24",
                "21  9 14 16  7",
                 "6 10  3 18  5",
                 "1 12 20 15 19",
                              "",
                 "3 15  0  2 22",
                 "9 18 13 17  5",
                "19  8  7 25 23",
                "20 11 10 24  4",
                "14 21 16 12  6",
                              "",
                "14 21 17 24  4",
                "10 16 15  9 19",
                "18  8 23 26 20",
                "22 11 13  6  5",
                 "2  0 12  3  7" }))
            .Should().Be("1924");
    }

    [Fact]
    public void SecondProblemInput()
    {
        var result = Day4
            .Solve2(new FileInputResolver(4));

        result.Should().Be("23670");
    }
}

