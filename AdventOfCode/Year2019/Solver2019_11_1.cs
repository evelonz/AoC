using AdventOfCode.Utility;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Year2019
{
    static class Solver2019_11_1
    {
        public static string Solve(IInputResolver input)
        {
            var instructions = new long[10_000];
            var inData = input.AsEnumerable()
                .First().Split(',')
                .Select(s => long.Parse(s)).ToArray();
            inData.CopyTo(instructions, 0);

            var sut = new IntcodeComputer(instructions);

            // 0 = black
            // 1 = white
            var position = (x: 0, y: 0);
            var currentColour = 0;
            var colours = new Dictionary<(int x, int y), int>(100);
            var outp = new List<long>();
            int paintCount = 0;
            var currentOrientation = Orientation.Up;

            ProgramState state = ProgramState.None;
            while (state != ProgramState.Halted)
            {
                // Input promt is in start. Might have to check when halt is given as well.
                state = sut.RunProgram(currentColour);
                if (state == ProgramState.NeedInput) { } // Just keep going.

                // Run and get next colour to paint.
                state = sut.RunProgram(currentColour);
                if (state == ProgramState.ProvidedOutput)
                {
                    var toPaint = sut.LastOutput;
                    if(!colours.TryGetValue(position, out var nextColour))
                    {
                        paintCount++;
                        colours.Add(position, (int)toPaint);
                    }
                    else
                    {
                        colours[position] = (int)toPaint;
                    }
                }
                // Run and get next orientation
                state = sut.RunProgram(currentColour);
                if (state == ProgramState.ProvidedOutput)
                {
                    var orientation = sut.LastOutput;
                    // Left
                    if (orientation == 0)
                    {
                        currentOrientation = (currentOrientation == Orientation.Up) ? Orientation.Left :
                            (currentOrientation == Orientation.Left) ? Orientation.Down :
                            (currentOrientation == Orientation.Down) ? Orientation.Right :
                            //(currentOrientation == Orientation.Right) ? 
                            Orientation.Up;
                    }
                    else
                    {
                        currentOrientation = (currentOrientation == Orientation.Up) ? Orientation.Right :
                            (currentOrientation == Orientation.Left) ? Orientation.Up :
                            (currentOrientation == Orientation.Down) ? Orientation.Left :
                            //(currentOrientation == Orientation.Right) ? 
                            Orientation.Down;
                    }
                }

                // Move
                if (currentOrientation == Orientation.Up)
                    position = (position.x, position.y - 1);
                if (currentOrientation == Orientation.Down)
                    position = (position.x, position.y + 1);
                if (currentOrientation == Orientation.Left)
                    position = (position.x - 1, position.y);
                if (currentOrientation == Orientation.Right)
                    position = (position.x + 1, position.y);

                // Next input
                if (colours.TryGetValue(position, out var nextColour2))
                {
                    currentColour = nextColour2;
                }
                else
                    currentColour = 0;

            }

            return paintCount.ToString();
        }
    }

    public enum Orientation
    {
        Up = 0,
        Left = 1,
        Right = 2,
        Down = 3,
    }

}
