using AdventOfCode.Utility;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Year2019
{
    static class Solver2019_19_1
    {

        public static string Solve(IInputResolver input, bool print = false)
        {
            var res = GetMap(input);
            return res.ToString();
        }

        //public static string SolveSecond(IInputResolver input, bool print = false)
        //{
        //    var (map, _, end) = GetMap(input, print);
        //    var steps = Bfs(map, end);
        //    return steps.ToString();
        //}

        private static string GetMap(IInputResolver input)
        {
            var instructions = new long[10_000];
            var inData = input.AsEnumerable()
                .First().Split(',')
                .Select(s => long.Parse(s)).ToArray();
            inData.CopyTo(instructions, 0);

            

            long response = 0; // 0 = wall, 1 moved, 2 found oxygen system.
            ProgramState state = ProgramState.None;
            int size = 130;
            int yOffset = 1100;
            int y = 0;
            int xOffset = 650;
            int x = 0;

            var map = new int[size][];
            var row = new int[size];
            while (state != ProgramState.Halted)
            {
                inData.CopyTo(instructions, 0);
                var sut = new IntcodeComputer(instructions);
                state = sut.RunProgram(null);
                state = sut.RunProgram(x+xOffset);
                state = sut.RunProgram(y+yOffset);

                // Save the display values based on output.
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


            }

            int sum = 0;
            for (int yy = 0; yy < map.Length; yy++)
            {
                var roww = map[yy];
                for (int xx = 0; xx < roww.Length; xx++)
                {
                    var v = roww[xx];
                    //if(yy+yOffset >= 673 && yy+yOffset < 673 + 100
                    //    && xx+xOffset >= 1107 && xx+xOffset < 1107+100)
                    //    Console.Write("#");
                    //else
                    Console.Write(v);
                    sum += v;
                }
                Console.WriteLine("");
            }

            return sum.ToString();
        }

    }
}
