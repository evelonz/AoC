using AdventOfCode.Utility;
using System.Linq;
using System.Collections.Generic;

namespace AdventOfCode.Year2019
{
    class Solver2019_3_1
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
            var vLines = new List<((int x, int y) start, (int x, int y) end)>(1000);
            var hLines = new List<((int x, int y) start, (int x, int y) end)>(1000);
            var intersections = new List<(int x, int y)>(100);

            // Map wire 1
            var pos = (x: 0, y: 0);
            foreach (var wire in wireOne.Split(','))
            {
                var (dir, dist, dx, dy) = GetDelta(wire);
                var npos = (pos.x + dx, pos.y + dy);
                var newLine = IsNegDir(dir) ? (npos, pos) : (pos, npos);
                var lineCollection = IsVertical(dir) ? vLines : hLines;
                lineCollection.Add(newLine);
                pos = npos;
            }

            // Map wire 2
            pos = (0, 0); // x, y
            foreach (var wire in wireTwo.Split(','))
            {
                var (dir, dist, dx, dy) = GetDelta(wire);
                var npos = (x: pos.x + dx, y: pos.y + dy);
                var sections = Enumerable.Empty<(int, int)>();

                switch (wire[0])
                {
                    case 'L':
                        // Check for intersect.
                        // If V line x is between new line x0 and x1
                        // And new line y is between V line y0 and y1.
                        sections = vLines.Where(c =>
                            c.start.x > npos.x &&
                            c.start.x < pos.x
                            &&
                            pos.y > c.start.y &&
                            pos.y < c.end.y
                        ).Select(s => (s.start.x, pos.y));
                        break;
                    case 'R':
                        sections = vLines.Where(c =>
                            c.start.x > pos.x &&
                            c.start.x < npos.x
                            &&
                            pos.y > c.start.y &&
                            pos.y < c.end.y
                        ).Select(s => (s.start.x, pos.y));
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
                        ).Select(s => (pos.x, s.start.y));
                        break;
                    case 'D':
                        sections = hLines.Where(c =>
                            c.start.y > npos.y &&
                            c.start.y < pos.y
                            &&
                            pos.x > c.start.x &&
                            pos.x < c.end.x
                        ).Select(s => (pos.x, s.start.y));
                        break;
                }
                intersections.AddRange(sections);
                pos = npos;
            }

            var minManhattanIntersection = intersections
                .Select(i => System.Math.Abs(i.x) + System.Math.Abs(i.y))
                .Min();

            return minManhattanIntersection.ToString();
        }
    }
}
