using AdventOfCode.Utility;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Year2019
{
    static class Solver2019_11_2
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
            var currentColour = 1;
            var colours = new Dictionary<(int x, int y), int>(100);
            colours.Add((0, 0), 1);

            var outp = new List<long>();
            int paintCount = 0;
            var currentOrientation = Orientation.Up;

            ProgramState state = ProgramState.None;
            while (state != ProgramState.Halted)
            {
                // Run and get next colour to paint.
                state = sut.RunProgram(currentColour);
                if (state == ProgramState.ProvidedOutput)
                {
                    var toPaint = sut.LastOutput;
                    if (!colours.TryGetValue(position, out var nextColour))
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

            // Draw the output.
            var minX = colours.Keys.Min(m => m.x);
            var maxX = colours.Keys.Max(m => m.x);
            var minY = colours.Keys.Min(m => m.y);
            var maxY = colours.Keys.Max(m => m.y);
            var xRange = maxX - minX + 1;
            var yRange = maxY - minY + 1;
            var map = new int[yRange, xRange];

            // Move min pos to 0,0 ?
            var adjX = -1 * minX;
            var adjY = -1 * minY;

            foreach (var item in colours)
            {
                var x = item.Key.x + adjX;
                var y = item.Key.y + adjY;
                map[y, x] = item.Value;
            }

            var sb = new System.Text.StringBuilder(yRange * xRange 
                + yRange * System.Environment.NewLine.Length);
            for (int yy = 0; yy < yRange; yy++)
            {
                for (int xx = 0; xx < xRange; xx++)
                {
                    var inn = map[yy, xx];
                    sb.Append(inn == 0 ? "." : "#");
                }
                sb.Append(System.Environment.NewLine);
            }

            return sb.ToString();
        }
    }


}
