using AdventOfCode.Utility;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Year2019
{
    static class Solver2019_19_2
    {

        public static string Solve(IInputResolver input, bool print = false)
        {
            var res = GetMap(input);
            return res.ToString();
        }

        private static string GetMap(IInputResolver input)
        {
            var instructions = new long[10_000];
            var inData = input.AsEnumerable()
                .First().Split(',')
                .Select(s => long.Parse(s)).ToArray();
            inData.CopyTo(instructions, 0);


            long response = 0;
            ProgramState state = ProgramState.None;

            int y = 100;
            int x = 0;

            // We simply want to find a y row, where first x = 1 at pos n,
            // has a n + 99 where x = 1 as well.
            // Then the same check for y, for which n may be larger or smaller.

            int yTested = y;
            int xFirst = x;
            int xTail = x;
            int xLast = x;
            int fy = 0;
            int fx = 0;
            while (state != ProgramState.Halted)
            {
                response = CheckCooredinate(x, y, instructions, inData);
                xFirst = (int)response;
                if (xFirst == 1)
                {
                    xLast = x;
                    response = CheckCooredinate(x + 100, y, instructions, inData);

                    if (response == 1)
                    {
                        // We found a place that fits 100 x
                        // Now we want to backtrack to the smallest y?
                        // But we also want to check y as well, as y may be to 
                        // small already, in which case we still have to 
                        // increase y.
                        while(response == 1)
                        {
                            xTail = x;
                            var topLeft = CheckCooredinate(x, y, instructions, inData);
                            var bottomLeft = CheckCooredinate(x, y + 100, instructions, inData);
                            var bottomRight = CheckCooredinate(x + 100, y + 100, instructions, inData);
                            if (topLeft == 1 && bottomLeft == 1 && bottomRight == 1)
                            {
                                fx = x;
                                fy = y;
                                //y--;
                                //x = 0; ; // have to back x a bit as well... Should check backwards actually.
                            }
                            x++;
                            response = CheckCooredinate(x + 100, y, instructions, inData);
                        }
                        // End should not be x + 100, and x is still 1.
                        x = xTail;
                        // Top right already checked in loop.
                        //var topLeft = CheckCooredinate(x, y, instructions, inData);
                        //var bottomLeft = CheckCooredinate(x, y + 100, instructions, inData);
                        //var bottomRight = CheckCooredinate(x + 100, y + 100, instructions, inData);
                        
                        if (fy != 0 && fx != 0) {
                            // This should be it
                            y++;
                            x = 0;
                        }
                        else
                        {
                            y++;
                            x = 0;
                        }
                        // Right, I should find the last x = 1 on row y.
                        // Then check last x = 1 (m), for y + 100,
                        // and x = m - 100 with y + 100,
                        // since the rectangle should fit in the back end.
                    }
                    else if (fy != 0 && fx != 0)
                    {
                        // This should be it?
                    }
                    else
                    {
                        y++;
                        // We should also store x, to get a better start point
                        // for the next loop.
                        //x = xLast;
                        x = 0;
                        // We deal with y later, and also backtrack.
                    }
                }
                else
                {
                    x++;
                }

            }


            return "";
        }

        private static int CheckCooredinate(int x, int y, long[] instructions, long[] inData)
        {
            inData.CopyTo(instructions, 0);
            var sut = new IntcodeComputer(instructions);
            sut.RunProgram(x);
            sut.RunProgram(y);
            return (int)sut.LastOutput;
        }
    }
}
