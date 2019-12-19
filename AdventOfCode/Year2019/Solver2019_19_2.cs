using AdventOfCode.Utility;
using System.Linq;

namespace AdventOfCode.Year2019
{
    static class Solver2019_19_2
    {
        public static string Solve(IInputResolver input, int startX = 0, int startY = 100)
        {
            var instructions = new long[10_000];
            var inData = input.AsEnumerable()
                .First().Split(',')
                .Select(s => long.Parse(s)).ToArray();
            inData.CopyTo(instructions, 0);
            var sut = new IntcodeComputer(instructions);

            // We have a couple of rows with no traction, which this code does
            // not handle. So we offset y to get past these rows.
            if (startY < 10)
                throw new System.ArgumentException("Value must be at least 10.", nameof(startY));
            int y = startY; 
            int x = startX;
            long response = 0;
            int rectLength = 99; // One less, since the start position is included in the length.

            while (true)
            {
                response = CheckCooredinate(x, y, sut);
                if(response == 0)
                {
                    x++;
                    continue;
                }
                
                response = CheckCooredinate(x + rectLength, y, sut);
                if (response == 0)
                {
                    y++;
                    x = 0;
                    continue;
                }

                while(response == 1)
                {
                    var topLeft = CheckCooredinate(x, y, sut);
                    var bottomLeft = CheckCooredinate(x, y + rectLength, sut);
                    var bottomRight = CheckCooredinate(x + rectLength, y + rectLength, sut);
                    if (topLeft == 1 && bottomLeft == 1 && bottomRight == 1)
                    {
                        return (x * 10_000 + y).ToString();
                    }

                    x++;
                    response = CheckCooredinate(x + rectLength, y, sut);
                }

                y++;
                x = 0;
            }
        }

        private static int CheckCooredinate(int x, int y, IntcodeComputer sut)
        {
            sut.Reset();
            sut.RunProgram(x);
            sut.RunProgram(y);
            return (int)sut.LastOutput;
        }
    }
}
