using AdventOfCode2020.Utility;
using FluentAssertions;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace AdventOfCode2020.Day23
{
#pragma warning disable CS8602 // Dereference of a possibly null reference.
    internal static class Day23
    {
        internal static (long partOne, long partTwo) Solve(IInputResolver input, int rounds, bool skipPartTwo = false)
        {
            var (list, min, max) = CreateList(input.AsEnumerable());
            PlayCrabCups(rounds, list, min, max);

            // ans 1
            string ans1 = "";
            var start = list.Find(1).Next ?? list.First;
            while (start.Value != 1)
            {
                ans1 += start.Value;
                start = start.Next ?? list.First;
            }
            if(skipPartTwo)
                return (long.Parse(ans1), 0);

            var (list2, min2, max2) = CreateList(input.AsEnumerable());
            max2 = PadList(list2, 1_000_000, max2);
            PlayCrabCups(10_000_000, list2, min2, max2);

            var first = list2.Find(1).Next ?? list2.First;
            var second = first.Next ?? list2.First;
            long ans2 = ((long)first.Value * (long)second.Value);

            return (long.Parse(ans1), ans2);
        }

        private static int PadList(LinkedList<int> list, int newMax, int max)
        {
            for (int i = max+1; i <= newMax; i++)
            {
                list.AddLast(i);
            }
            return newMax;
        }

        private static (LinkedList<int> list, int min, int max) CreateList(IEnumerable<string> input)
        {
            var data = input.First().ToCharArray().Select(s => int.Parse(s.ToString()));
            var list = new LinkedList<int>();
            int min = int.MaxValue;
            int max = int.MinValue;
            foreach (var cup in data)
            {
                if (cup < min)
                    min = cup;
                if (cup > max)
                    max = cup;
                list.AddLast(cup);
            }
            return (list, min, max);
        }

        private static void PlayCrabCups(int rounds, LinkedList<int> list, int min, int max)
        {
            var currentCup = list.First;
            // The lookup array takes the speed from ~400 rounds/second to over 1 000 000.
            var lookup = new LinkedListNode<int>[max+1];
            for (var node = list.First; node != null; node = node.Next)
            {
                lookup[node.Value] = node;
            }
            for (int i = 1; i <= rounds; i++)
            {
                // 1. Take 3 cups.
                var taken = new LinkedListNode<int>[3];
                for (int j = 0; j < 3; j++)
                {
                    var n = currentCup.Next ?? list.First; // Make the take circular.
                    list.Remove(n!);
                    taken[j] = n!;
                }

                // 2. Destination cup.
                LinkedListNode<int>? destination = null;
                int nxtValue = currentCup.Value;
                while (destination is null)
                {
                    nxtValue--;
                    if (nxtValue < min)
                        nxtValue = max;
                    //destination = list.Find(nxtValue);
                    destination = lookup[nxtValue];
                    if (taken.Contains(destination))
                        destination = null;
                }

                // 3. Insert taken cups.
                for (int j = 0; j < 3; j++)
                {
                    var toInsert = taken[j];
                    list.AddAfter(destination!, toInsert);
                    destination = destination.Next;
                }

                // 4. New current cup.
                currentCup = currentCup.Next ?? list.First;
            }
        }
#pragma warning restore CS8602 // Dereference of a possibly null reference.
    }

    public class Test2020Day23
    {
        [Fact]
        public void SolveProblemExamples()
        {
            var (partOne, partTwo) = Day23
                .Solve(new MockInputResolver(new string[] { "389125467" }), 100);
            partOne.Should().Be(67384529);
            partTwo.Should().Be(149245887792);
        }

        [Fact]
        public void SolveExampleOneFor10Rounds()
        {
            var (partOne, _) = Day23
                .Solve(new MockInputResolver(new string[] { "389125467" }), 10, true);
            partOne.Should().Be(92658374);
        }

        [Fact]
        public void SolveProblemInput()
        {
            var (partOne, partTwo) = Day23
                .Solve(new MockInputResolver(new string[] { "523764819" }), 100);
            partOne.Should().Be(49576328);
            partTwo.Should().Be(511780369955);
        }
    }
}
