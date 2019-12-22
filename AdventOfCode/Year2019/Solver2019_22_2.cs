using AdventOfCode.Utility;
using System.Linq;
using System;
namespace AdventOfCode.Year2019
{
    public class LargeArray
    {
        public long Length;
        public int _arrSize;
        public long[][] _array;

        public long this[long index]    // Indexer declaration  
        {
            get
            {
                var row = index / int.MaxValue;
                var col = index % int.MaxValue;
                return _array[row][col];
            }
            set
            {
                var row = index / int.MaxValue;
                var col = index % int.MaxValue;
                _array[row][col] = value;
            }
        }

        public LargeArray(long size)
        {
            Length = size;
            if (size <= int.MaxValue)
            {
                _array = new long[1][];
                var arr = new long[size];
                for (long i = 0; i < size; i++)
                {
                    arr[i] = i;
                }
                _array[0] = arr;
            }
            else
            {
                var arrCount = size / int.MaxValue;
                if (size % int.MaxValue > 0) arrCount += 1;
                _array = new long[arrCount][];
                _arrSize = (int)arrCount;

                int pos = 0;
                int uberIndex = 0;
                while(pos < size)
                {
                    var arr = _array[uberIndex];
                    var end = uberIndex < _arrSize 
                        ? int.MaxValue 
                        : (size % int.MaxValue);
                    for (long i = 0; i < end; i++)
                    {
                        arr[i] = pos;
                        pos++;
                    }
                }
            }
            
        }
    }
    static class Solver2019_22_2
    {
        public static string Solve(IInputResolver input, long size = 10007, int? cardToFind = null)
        {
            var inData = input.AsEnumerable();

            var deck = new LargeArray(size);
            var tempdeck = new LargeArray(size);
            var tempdeck2 = new LargeArray(size);
            for (long i = 0; i < size; i++)
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
                for (long i = 0; i < size; i++)
                {
                    if (deck[i] == 2019)
                        return i.ToString();
                }
                return (-1).ToString();
            }

            var sb = new System.Text.StringBuilder((int)size * 2);
            for (int i = 0; i < size; i++)
            {
                sb.Append(deck[i] + " ");
            }
            return sb.ToString();
        }

        private static void DealIntoNewDeck(ref LargeArray deck, ref LargeArray targetDeck)
        {
            for (long i = 0; i < deck.Length; i++)
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

        private static void CutDeck(ref LargeArray deck, ref LargeArray targetDeck, int cut, LargeArray cutted)
        {
            for (long i = 0; i < cut; i++)
            {
                cutted[i] = deck[i];
            }

            for (long i = cut; i < deck.Length; i++)
            {
                targetDeck[i - cut] = deck[i];
            }

            for (long i = 0; i < cut; i++)
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

        private static void CutDeckNegative(ref LargeArray deck, ref LargeArray targetDeck, int cut, LargeArray cutted)
        {
            for (int i = 0; i < cut; i++)
            {
                var targetIndex = deck.Length - cut + i;
                cutted[i] = deck[targetIndex];
            }

            for (long i = 0; i < deck.Length - cut; i++)
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

        private static void DealWithIncN(ref LargeArray deck, ref LargeArray targetDeck, int increment)
        {
            long set = 0;
            long index = 0;
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
