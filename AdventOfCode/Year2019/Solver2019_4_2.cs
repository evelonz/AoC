using AdventOfCode.Utility;

namespace AdventOfCode.Year2019
{
    class Solver2019_4_2
    {

        public string Solve(IInputResolver input)
        {
            var min = 153517;
            var max = 630395;
            int sum = 0;
            for (int i = min; i <= max; i++)
            {
                var twoEqual = false;
                var everIncrease = true;
                int previous = 0;
                int twoEarlier = 0;
                int currentDuplicate = -1;
                for (int j = 100_000; j > 0; j /= 10)
                {
                    var t = i / j;
                    int current = t % 10;

                    if (!twoEqual && current == previous && previous != twoEarlier)
                    {
                        twoEqual = true;
                        currentDuplicate = current;
                    }
                    if (current == previous && previous == twoEarlier 
                        && currentDuplicate == current)
                        twoEqual = false;
                    if (current < previous)
                        everIncrease = false;

                    twoEarlier = previous;
                    previous = current;
                }

                if (everIncrease && twoEqual)
                {
                    sum++;
                    System.Console.WriteLine(i);
                }

                //if (i % 10_000 == 0)
                //    System.Console.WriteLine(i);
            }

            return sum.ToString();
        }
    }
}
