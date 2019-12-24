using AdventOfCode.Utility;
using System.Linq;
using System;
using System.Collections.Generic;

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
            var l = new List<string>(100);
            int itt = 0;
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

                for (int i = 0; i < size; i++)
                {
                    if (deck[i] == 2019)
                        l.Add($"{itt++}: {i}");
                }
            }
            l.Reverse();
            foreach (var item in l)
            {
                Console.WriteLine(item);
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

        public static string SolveFirstIndex(IInputResolver input, long length, long index)
        {
            var inData = input.AsEnumerable();

            foreach (var command in inData)
            {
                if (command == "deal into new stack")
                    index = DealIntoNewDeckIndex(index, length);
                else if (command.StartsWith("cut"))
                {
                    var num = command.Substring(4);
                    var anum = int.Parse(num);
                    if (anum > 0)
                        index = CutDeckIndex(index, anum, length);
                    else
                        index = CutDeckNegativeIndex(index, Math.Abs(anum), length);
                }
                else if (command.StartsWith("deal with increment"))
                {
                    var num = command.Substring(20);
                    var anum = int.Parse(num);
                    index = DealWithIncNIndex(index, anum, length);
                }
            }

            return index.ToString();
        }

        public static string SolveReverse(IInputResolver input, long length, long index)
        {
            var inData = input.AsEnumerable();

            inData = inData.Reverse().ToList();

            Console.WriteLine(index);
            //var itt = 0;
            //var sw = new System.Diagnostics.Stopwatch();
            //sw.Start();
            var hits = new HashSet<long>(1_000_000);
            for (long i = 0; i < 101_741_582_076_661; i++)
            {
                index = SolveReverse(length, index, inData);
                if (hits.Contains(index))
                {

                }
                hits.Add(index);
                //if (i % 100_000 == 0)
                //    Console.WriteLine(sw.ElapsedMilliseconds);
            }

            return index.ToString();
        }

        private static long SolveReverse(long length, long index, IEnumerable<string> inData)
        {
            foreach (var command in inData)
            {
                if (command == "deal into new stack")
                    index = DealIntoNewDeckIndexRev(index, length);
                else if (command.StartsWith("cut"))
                {
                    var num = command.Substring(4);
                    var anum = int.Parse(num);
                    if (anum > 0)
                        index = CutDeckIndexRev(index, anum, length);
                    else
                        index = CutDeckNegativeIndexRev(index, Math.Abs(anum), length);
                }
                else if (command.StartsWith("deal with increment"))
                {
                    var num = command.Substring(20);
                    var anum = int.Parse(num);
                    index = DealWithIncNIndexRev2(index, anum, length);
                }

                //Console.WriteLine($"{itt++}: {index} - C: {command}");
            }

            return index;
        }

        public static long DealIntoNewDeckIndex(long index, long length)
        {
            // New position of the index n is (length - 1) - n?
            // Examples: L = 10. 9 - 0 = 9. 9 - 2 = 7. 9 - 0 = 9.
            return length - 1 - index;
        }

        public static long CutDeckIndex(long index, int cut, long length)
        {
            // A cut is in essense a shift of the deck.
            // Damn, can all of these be done using bit operators? :)
            var first = index - cut;
            if(first < 0)
            {
                first += length;
            }
            return first;
        }

        public static long CutDeckNegativeIndex(long index, int cut, long length)
        {
            // Negative cut is just a shift in the other direction.
            var first = index + cut;
            if (first > length - 1)
            {
                first %= length;
            }
            return first;
        }

        public static long DealWithIncNIndex(long index, int increment, long length)
        {
            // This is a bit more tricky.
            // Each index is ofset by an amount times the inc.
            // For inc = 3: 0 => 0, 1 => 3, 2 => 6.
            // It should however start to wrap after length - 1.
            // For L = 10: n = 4 wrappes to pos 2.
            // 4 * 3 = 12. 12 % 10 = 2.
            // 7 should wrap to pos 1.
            // 7 * 3 = 21. 21 % 10 = 1.
            return (index * increment) % length;
        }

        public static long DealIntoNewDeckIndexRev(long index, long length)
        {
            // New position of the index n is (length - 1) - n?
            // Examples: L = 10. 9 - 0 = 9. 9 - 2 = 7. 9 - 0 = 9.
            // Reversed, it should be the same I guess.
            return length - 1 - index;
        }

        public static long CutDeckIndexRev(long index, int cut, long length)
            => CutDeckNegativeIndex(index, cut, length);

        public static long CutDeckNegativeIndexRev(long index, int cut, long length)
            => CutDeckIndex(index, cut, length);

        public static long DealWithIncNIndexRev(long index, int increment, long length)
        {
            // This is a bit more tricky.
            // Each index is ofset by an amount times the inc.
            // For inc = 3: 0 => 0, 1 => 3, 2 => 6.
            // It should however start to wrap after length - 1.
            // For L = 10: n = 4 wrappes to pos 2.
            // 4 * 3 = 12. 12 % 10 = 2.
            // 7 should wrap to pos 1.
            // 7 * 3 = 21. 21 % 10 = 1.
            //return (index * increment) % length;
            if (index == 0) return 0;
            // + increment, since we count from 1. -1 to offset 0 index.
            var positionInGroup = (index + increment - 1) / increment;

            var offset = index % increment;
            // Have to reverse offset somehow. 0 = 0
            // but 2 = 1, and 1 = 2, and so on.
            var offsetCorrected = offset == 0 ? 0 : increment - offset;
            var res = offsetCorrected * increment + positionInGroup;
            return res;

        }

        public static long DealWithIncNIndexRev2(long index, int increment, long length)
        {
            // Unable to find a good solution to this...
            // Can only think of a solution which need "increament" number of calculations.
            // For my input, that is at most 71 calculations.
            if (index == 0) return 0;
            var lastIndexPos = DealWithIncNIndex(length - 1, increment, length);
            double x;
            for (int m = 0; m <= increment; m++)
            {
                x = (length * m + lastIndexPos - index) / (increment * 1.0);
                if (x > length - 1)
                    return -1; // X will only grow and we already past a valid res.
                var whole = Math.Abs(x % 1) <= (Double.Epsilon * 100);
                if (whole && x >= 0)
                {
                    return length - 1 - (long)x;
                }
            }
            return -1;
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
