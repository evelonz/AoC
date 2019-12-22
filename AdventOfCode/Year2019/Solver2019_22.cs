using AdventOfCode.Utility;
using System.Linq;
using System;
namespace AdventOfCode.Year2019
{
    static class Solver2019_22
    {
        public static string SolveFirst(IInputResolver input, int size = 10007, int? cardToFind = null)
        {
            var inData = input.AsEnumerable();

            var deck = new int[size];
            var tempdeck = new int[size];
            var tempdeck2 = new int[size];
            for (int i = 0; i < size; i++)
            {
                deck[i] = i;
            }

            foreach (var command in inData)
            {
                if(command == "deal into new stack")
                    DealIntoNewDeck(ref deck, ref tempdeck);
                else if (command.StartsWith("cut"))
                {
                    var num = command.Substring(4);
                    var anum = int.Parse(num);
                    if(anum > 0)
                        CutDeck(ref deck, ref tempdeck, anum, tempdeck2);
                    else
                        CutDeckNegative(ref deck, ref tempdeck, Math.Abs(anum), tempdeck2);
                }
                else if (command.StartsWith("deal with increment"))
                {
                    var num = command.Substring(20);
                    var anum = int.Parse(num);
                    DealWithIncN(ref deck, ref tempdeck, anum);
                }
            }

            if(cardToFind.HasValue)
            {
                for (int i = 0; i < size; i++)
                {
                    if (deck[i] == 2019)
                        return i.ToString();
                }
                return (-1).ToString();
            }

            var sb = new System.Text.StringBuilder(size * 2);
            for (int i = 0; i < size; i++)
            {
                sb.Append(deck[i] + " ");
            }
            return sb.ToString();
        }

        public static string SolveSecond(IInputResolver input)
        {
            return "";
        }

        private static void DealIntoNewDeck(ref int[] deck, ref int[] targetDeck)
        {
            for (int i = 0; i < deck.Length; i++)
            {
                var take = deck[i];
                var targetIndex = deck.Length - i - 1;
                targetDeck[targetIndex] = take;
            }
            // Swap deck refs.
            var retDeck = targetDeck;
            targetDeck = deck;
            deck = retDeck;
        }

        private static void CutDeck(ref int[] deck, ref int[] targetDeck, int cut, int[] cutted)
        {
            for (int i = 0; i < cut; i++)
            {
                cutted[i] = deck[i];
            }

            for (int i = cut; i < deck.Length; i++)
            {
                targetDeck[i - cut] = deck[i];
            }

            for (int i = 0; i < cut; i++)
            {
                var take = cutted[i];
                var targetIndex = deck.Length - cut + i;
                targetDeck[targetIndex] = take;
            }
            // Swap deck refs.
            var retDeck = targetDeck;
            targetDeck = deck;
            deck = retDeck;
        }

        private static void CutDeckNegative(ref int[] deck, ref int[] targetDeck, int cut, int[] cutted)
        {
            for (int i = 0; i < cut; i++)
            {
                var targetIndex = deck.Length - cut + i;
                cutted[i] = deck[targetIndex];
            }

            for (int i = 0; i < deck.Length - cut; i++)
            {
                targetDeck[i + cut] = deck[i];
            }

            for (int i = 0; i < cut; i++)
            {
                targetDeck[i] = cutted[i];
            }
            // Swap deck refs.
            var retDeck = targetDeck;
            targetDeck = deck;
            deck = retDeck;
        }

        private static void DealWithIncN(ref int[] deck, ref int[] targetDeck, int increment)
        {
            int set = 0;
            int index = 0;
            while(set < deck.Length)
            {
                targetDeck[index] = deck[set];
                index = (index + increment) % deck.Length;

                set++;
            }
            // Swap deck refs.
            var retDeck = targetDeck;
            targetDeck = deck;
            deck = retDeck;
        }

    }
}
