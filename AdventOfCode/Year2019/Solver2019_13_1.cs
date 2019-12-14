using AdventOfCode.Utility;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Year2019
{
    static class Solver2019_13_1
    {
        public static string Solve(IInputResolver input)
        {
            var instructions = new long[10_000];
            var inData = input.AsEnumerable()
                .First().Split(',')
                .Select(s => long.Parse(s)).ToArray();
            inData.CopyTo(instructions, 0);

            var sut = new IntcodeComputer(instructions);

            long pInput = 0;

            int typeOfObject = 0; // 0 = x, 1 = y, 3 = type, then back to 0.
            long x = 0; long y = 0;
            long blockCount = 0;
            ProgramState state = ProgramState.None;
            var map = new HashSet<(long x, long y)>(600);

            while (state != ProgramState.Halted)
            {
                // Input promt is in start. Might have to check when halt is given as well.
                state = sut.RunProgram(pInput);

                // only handle output for part one.
                if (state == ProgramState.ProvidedOutput)
                {
                    var val = sut.LastOutput;
                    if (typeOfObject == 2 && val == 2) // 2 = block
                    {
                        map.Add((x, y));
                    }
                    if (typeOfObject == 0)
                        x = val;
                    if (typeOfObject == 1)
                        y = val;

                    typeOfObject++;
                    typeOfObject %= 3;
                }

            }

            blockCount = map.Count;
            return blockCount.ToString();
        }
    }
}
