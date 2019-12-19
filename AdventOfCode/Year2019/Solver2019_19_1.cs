using AdventOfCode.Utility;
using System;
using System.Linq;

namespace AdventOfCode.Year2019
{
    static class Solver2019_19_1
    {
        public static string Solve(IInputResolver input)
        {
            var instructions = new long[10_000];
            var inData = input.AsEnumerable()
                .First().Split(',')
                .Select(s => long.Parse(s)).ToArray();
            inData.CopyTo(instructions, 0);

            long response = 0; // 1 in field, 0 outside of field.
            ProgramState state = ProgramState.None;
            int size = 50;
            int y = 0;
            int x = 0;
            // Offsets can be used to draw other parts of the map.
            int yOffset = 0;
            int xOffset = 0;

            var map = new int[size][];
            var row = new int[size];
            var sut = new IntcodeComputer(instructions);

            while (state != ProgramState.Halted)
            {
                state = sut.RunProgram(x+xOffset);
                state = sut.RunProgram(y+yOffset);

                if (state == ProgramState.ProvidedOutput)
                {
                    response = sut.LastOutput;
                    row[x++] = (int)response;
                    if(x == size)
                    {
                        map[y++] = row;
                        row = new int[size];
                        x = 0;
                    }
                    if (y == size)
                        break;
                }

                sut.Reset();
            }

            int sum = 0;
            for (int yy = 0; yy < map.Length; yy++)
            {
                var roww = map[yy];
                for (int xx = 0; xx < roww.Length; xx++)
                {
                    var v = roww[xx];
                    Console.Write(v);
                    sum += v;
                }
                Console.WriteLine("");
            }

            return sum.ToString();
        }

    }
}
