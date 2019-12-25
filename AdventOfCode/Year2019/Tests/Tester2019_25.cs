using AdventOfCode.Utility;
using FluentAssertions;
using Xunit;

namespace AdventOfCode.Year2019.Tests
{
    public class Tester2019_25
    {
        [Fact]
        public void FirstProblemMyInput()
        {
            Solver2019_25
                .SolveFirst(new FileInputResolver(2019, 25))
                .Should().Be(ResultPartOne);
        }

        private static readonly string ResultPartOne = "\r\n\r\n\r\n== Pressure-Sensitive Floor ==\r\nAnalyzing...\r\n\r\nDoors here lead:\r\n- east\r\n\r\nA loud, robotic voice says \"Analysis complete! You may proceed.\" and you enter the cockpit.\r\nSanta notices your small droid, looks puzzled for a moment, realizes what has happened, and radios your ship directly.\r\n\"Oh, hello! You should be able to get in by typing 328960 on the keypad at the main airlock.\"\r\n";
    }
}
