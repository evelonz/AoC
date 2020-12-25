using AdventOfCode2020.Utility;
using FluentAssertions;
using System.Linq;
using Xunit;

namespace AdventOfCode2020.Day25
{
    internal static class Day25
    {
        internal static long Solve(IInputResolver input)
        {
            var data = input.AsEnumerable().Select(s => int.Parse(s)).ToList();
            var pubKey1 = data[0];
            var pubKey2 = data[1];
            var loop1 = FindLoopSize(pubKey1);
            var loop2 = FindLoopSize(pubKey2);

            return TransformSubjectNumber(pubKey1, loop2);
        }

        internal static long FindLoopSize(long pubKey1)
        {
            long value = 1;
            int loopSize;
            for (loopSize = 0; value != pubKey1; loopSize++)
            {
                value *= 7L; // Default subject number.
                value %= 20201227;
            }
            return loopSize;
        }

        internal static long TransformSubjectNumber(long subjectNumber, long loopSize)
        {
            long value = 1;
            for (var i = 0; i < loopSize; i++)
            {
                value *= subjectNumber;
                value %= 20201227;
            }
            return value;
        }
    }

    public class Test2020Day25
    {
        [Fact]
        public void SolveProblemExamples()
        {
            var partOne = Day25
                .Solve(new MockInputResolver(new string[] { "5764801", "17807724" }));
            partOne.Should().Be(14897079);
        }

        [Fact]
        public void SolveProblemInput()
        {
            var partOne = Day25
                .Solve(new MockInputResolver(new string[] { "7573546", "17786549" }));
            partOne.Should().Be(7032853);
        }

        [Theory]
        [InlineData(5764801, 8)]
        [InlineData(17807724, 11)]
        public void FindTheCoorectLoopSize(long publicKey, long expectedLoopSize)
        {
            var sut = Day25.FindLoopSize(publicKey);
            sut.Should().Be(expectedLoopSize);
        }

        [Fact]
        public void FindTheCorrectEncryptionKey()
        {
            var sut1 = Day25.TransformSubjectNumber(17807724, 8);
            var sut2 = Day25.TransformSubjectNumber(5764801, 11);
            sut1.Should().Be(14897079);
            sut2.Should().Be(sut1);
        }
    }
}
