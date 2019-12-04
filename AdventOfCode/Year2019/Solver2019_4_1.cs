using AdventOfCode.Utility;

namespace AdventOfCode.Year2019
{
    class Solver2019_4_1
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
                for (int j = 100_000; j > 0; j /= 10)
                {
                    var t = i / j;
                    int current = t % 10;

                    if (!twoEqual && current == previous)
                        twoEqual = true;
                    if (current < previous)
                        everIncrease = false;

                    previous = current;
                }

                if (everIncrease && twoEqual)
                {
                    sum++;
                    //System.Console.WriteLine(i);
                }

                if (i % 10_000 == 0)
                    System.Console.WriteLine(i);
            }

            return sum.ToString();
        }
    }
}
