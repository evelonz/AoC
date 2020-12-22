using AdventOfCode2020.Utility;
using FluentAssertions;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using System.Collections;

namespace AdventOfCode2020.Day22
{
    internal static class Day22
    {
        internal static (long partOne, long partTwo) Solve(IInputResolver input)
        {
            var data = input.AsEnumerable();

            var player1 = new Queue<int>();
            var player2 = new Queue<int>();
            LoadDecks(data, player1, player2);

            PlayCombat(player1, player2);
            int ans1 = CalculateScore(player1, player2);

            player1 = new Queue<int>();
            player2 = new Queue<int>();
            LoadDecks(data, player1, player2);
            PlayRecursiveCombat(player1, player2);
            int ans2 = CalculateScore(player1, player2);

            return (ans1, ans2);
        }

        private static void PlayRecursiveCombat(Queue<int> player1, Queue<int> player2)
        {
            // We need some way to communicate back up who won the games.
            // We also need the decks, to count the score of the top game winner.
            // Perhaps we just use the decks themselves, and we check again on the higher level who won.
            var earlierP1Decks = new HashSet<string>();
            var earlierP2Decks = new HashSet<string>();
            for (int rounds = 1; player1.Count > 0 && player2.Count > 0; rounds++)
            {
                // 1. Check if the the set of decks have existed before, to hinder endless looping.
                var deckValue1 = player1.Select(s => s.ToString()).Aggregate((a, s) => a + "-" + s);
                var deckValue2 = player2.Select(s => s.ToString()).Aggregate((a, s) => a + "-" + s);
                if(earlierP1Decks.Contains(deckValue1) || earlierP2Decks.Contains(deckValue2))
                {
                    player2.Clear();
                    break;
                }
                earlierP1Decks.Add(deckValue1);
                earlierP2Decks.Add(deckValue2);

                var c1 = player1.Dequeue();
                var c2 = player2.Dequeue();
                // 2. Check if both players have enough cards in the decks for an sub game.
                if (c1 <= player1.Count && c2 <= player2.Count)
                {
                    // I doubt this is very performant. Should be a better way to do this.
                    var newP1Deck = CopySubDock(player1, c1);
                    var newP2Deck = CopySubDock(player2, c2);

                    PlayRecursiveCombat(newP1Deck, newP2Deck);
                    QueueWinnersCards(player1, player2, newP1Deck.Count > 0, c1, c2);
                }
                else
                {
                    // 3. If not enough for a subgame, determine the winner like normal.
                    QueueWinnersCards(player1, player2, c1 > c2, c1, c2);
                }
            }
        }

        private static Queue<int> CopySubDock(Queue<int> originalDeck, int count)
        {
            var copyOfDeck = new Queue<int>(originalDeck);
            var newDeck = new Queue<int>(count);
            for (int i = 0; i < count; i++)
            {
                newDeck.Enqueue(copyOfDeck.Dequeue());
            }

            return newDeck;
        }

        private static void PlayCombat(Queue<int> player1, Queue<int> player2)
        {
            for (int rounds = 1; player1.Count > 0 && player2.Count > 0; rounds++)
            {
                var c1 = player1.Dequeue();
                var c2 = player2.Dequeue();
                QueueWinnersCards(player1, player2, c1 > c2, c1, c2);
            }
        }

        private static void QueueWinnersCards(Queue<int> player1, Queue<int> player2, bool player1Won, int c1, int c2)
        {
            if (player1Won)
            {
                player1.Enqueue(c1);
                player1.Enqueue(c2);
            }
            else
            {
                player2.Enqueue(c2);
                player2.Enqueue(c1);
            }
        }

        private static int CalculateScore(Queue<int> player1, Queue<int> player2)
        {
            var answer = 0;
            var winningPlayer = player1.Count > 0 ? player1 : player2;
            var multiplayer = winningPlayer.Count;
            foreach (var card in winningPlayer)
            {
                answer += (card * multiplayer--);
            }

            return answer;
        }

        private static void LoadDecks(IEnumerable<string> data, Queue<int> player1, Queue<int> player2)
        {
            int skips = 1;
            foreach (var row in data.Skip(skips))
            {
                if (string.IsNullOrWhiteSpace(row))
                    break;
                player1.Enqueue(int.Parse(row));
                skips++;
            }
            skips += 2;
            foreach (var row in data.Skip(skips))
            {
                if (string.IsNullOrWhiteSpace(row))
                    break;
                player2.Enqueue(int.Parse(row));
            }
        }
    }


    public class Test2020Day22
    {
        [Theory]
        [MemberData(nameof(ExampleData))]
        public void SolveProblemExamples(string[] example, int expectedFirst, int expectedSecond)
        {
            var (partOne, partTwo) = Day22
                .Solve(new MockInputResolver(example));
            partOne.Should().Be(expectedFirst);
            partTwo.Should().Be(expectedSecond);
        }

        [Fact]
        public void SolveProblemInput()
        {
            var (partOne, partTwo) = Day22
                .Solve(new FileInputResolver(22));
            partOne.Should().Be(32495);
            partTwo.Should().Be(32665);
        }

        public readonly static List<object[]> ExampleData = new List<object[]>
        {
            new object[] {
                new [] {
                    "Player 1:",
                    "9",
                    "2",
                    "6",
                    "3",
                    "1",
                    "",
                    "Player 2:",
                    "5",
                    "8",
                    "4",
                    "7",
                    "10",
                }, 306, 291
            }
        };
    }
}
