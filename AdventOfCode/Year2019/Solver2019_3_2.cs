using AdventOfCode.Utility;
using System.Linq;
using System.Collections.Generic;
using System;

namespace AdventOfCode.Year2019
{
    class Solver2019_3_2
    {
        private bool IsNegDir(char d) => d == 'L' || d == 'D';
        private bool IsVertical(char d) => d == 'U' || d == 'D';

        private (char dir, int dist, int dx, int dy) GetDelta(string wire)
        {
            char dir = wire[0];
            int dist = int.Parse(wire.Substring(1));
            int dx = dir == 'L' ? -dist : dir == 'R' ? dist : 0;
            int dy = dir == 'D' ? -dist : dir == 'U' ? dist : 0;
            return (dir, dist, dx, dy);
        }

        public string Solve(IInputResolver input)
        {
            var wires = input.AsEnumerable();
            var wireOne = wires.First();
            var wireTwo = wires.Last();
            var vLines = new List<
                ((int x, int y) start, (int x, int y) end, int dist, (int x, int y) distStart)>(1000);
            var hLines = new List<
                ((int x, int y) start, (int x, int y) end, int dist, (int x, int y) distStart)>(1000);
            var intersections = new List<(int x, int y, int distW1, int distW2)>(100);

            // Map wire 1
            var pos = (x: 0, y: 0);
            int stepsToFirst = 0;
            foreach (var wire in wireOne.Split(','))
            {
                var (dir, dist, dx, dy) = GetDelta(wire);
                var npos = (pos.x + dx, pos.y + dy);
                var lineCollection = IsVertical(dir) ? vLines : hLines;
                var newLine = IsNegDir(dir) ? (npos, pos, stepsToFirst, pos) 
                    : (pos, npos, stepsToFirst, pos);
                lineCollection.Add(newLine);

                var stepsToAdd = IsVertical(dir) ? Math.Abs(dy) : Math.Abs(dx);
                stepsToFirst += stepsToAdd;
                pos = npos;
            }

            // Map wire 2
            pos = (x: 0, y: 0);
            stepsToFirst = 0;
            foreach (var wire in wireTwo.Split(','))
            {
                var (dir, dist, dx, dy) = GetDelta(wire);
                var npos = (x: pos.x + dx, y: pos.y + dy);
                var sections = Enumerable.Empty<(int x, int y, int distW1, int distW2)>();

                switch (wire[0])
                {
                    case 'L':
                        // Check for intersect.
                        // If V line x is between new line x0 and x1
                        // And new line y is between H line y0 and y1.
                        sections = vLines.Where(c =>
                            c.start.x > npos.x &&
                            c.start.x < pos.x
                            &&
                            pos.y > c.start.y &&
                            pos.y < c.end.y
                        ).Select(s => 
                            (s.start.x, pos.y, 
                            s.dist + Math.Abs(s.distStart.y - pos.y),
                            stepsToFirst + Math.Abs(pos.x - s.start.x)));
                        break;
                    case 'R':
                        sections = vLines.Where(c =>
                            c.start.x > pos.x &&
                            c.start.x < npos.x
                            &&
                            pos.y > c.start.y &&
                            pos.y < c.end.y
                        ).Select(s => 
                            (s.start.x, pos.y,
                            s.dist + Math.Abs(s.distStart.y - pos.y),
                            stepsToFirst + Math.Abs(pos.x - s.start.x)));
                        break;
                    case 'U':
                        // Check for intersect.
                        // If H line y is between new line y0 and y1
                        // And new line x is between H line x0 and x1.
                        sections = hLines.Where(c =>
                            c.start.y > pos.y &&
                            c.start.y < npos.y
                            &&
                            pos.x > c.start.x &&
                            pos.x < c.end.x
                        ).Select(s => 
                            (pos.x, s.start.y,
                            s.dist + Math.Abs(s.distStart.x - pos.x),
                            stepsToFirst + Math.Abs(pos.y - s.start.y)));
                        break;
                    case 'D':
                        sections = hLines.Where(c =>
                            c.start.y > npos.y &&
                            c.start.y < pos.y
                            &&
                            pos.x > c.start.x &&
                            pos.x < c.end.x
                        ).Select(s => 
                            (pos.x, s.start.y,
                            s.dist + Math.Abs(s.distStart.x - pos.x),
                            stepsToFirst + Math.Abs(pos.y - s.start.y)));
                        break;
                }

                intersections.AddRange(sections);
                pos = npos;
                var stepsToAdd = IsVertical(dir) ? Math.Abs(dy) : Math.Abs(dx);
                stepsToFirst += stepsToAdd;
            }

            var minTotalManhattanDistance = intersections
                .Select(s => s.distW1 + s.distW2)
                .Min();

            return minTotalManhattanDistance.ToString();
        }
    }
}
