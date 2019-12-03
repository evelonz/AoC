using AdventOfCode.Utility;
using System.Linq;
using System.Collections.Generic;

namespace AdventOfCode.Year2019
{
    class Solver2019_3_1
    {
        public string Solve(IInputResolver input)
        {
            var wires = input.AsEnumerable();
            var wireOne = wires.First();
            var wireTwo = wires.Last();
            var vLines = new List<((int, int), (int, int))>(1000);
            var hLines = new List<((int, int), (int, int))>(1000);
            var intersections = new List<(int, int)>(100);

            // Map wire 1
            var pos = (0, 0); // x, y
            foreach (var wire in wireOne.Split(','))
            {
                char dir = wire[0];
                int x = 0;
                int y = 0;
                (int, int) npos;
                switch(wire[0])
                {
                    case 'L':
                        x = -1 * int.Parse(wire.Substring(1));
                        npos = (pos.Item1 + x, pos.Item2);
                        hLines.Add((npos, pos));
                        pos = npos;
                        break;
                    case 'R':
                        x = int.Parse(wire.Substring(1));
                        npos = (pos.Item1 + x, pos.Item2);
                        hLines.Add((pos, npos));
                        pos = npos;
                        break;
                    case 'U':
                        y = int.Parse(wire.Substring(1));
                        npos = (pos.Item1, pos.Item2 + y);
                        vLines.Add((pos, npos));
                        pos = npos;
                        break;
                    case 'D':
                        y = -1 * int.Parse(wire.Substring(1));
                        npos = (pos.Item1, pos.Item2 + y);
                        vLines.Add((npos, pos));
                        pos = npos;
                        break;
                }

            }

            // Map wire 2
            pos = (0, 0); // x, y
            foreach (var wire in wireTwo.Split(','))
            {
                int x = 0;
                int y = 0;
                (int, int) npos;
                IEnumerable<(int, int)> sections;
                switch (wire[0])
                {
                    case 'L':
                        x = -1 * int.Parse(wire.Substring(1));
                        npos = (pos.Item1 + x, pos.Item2);

                        // Check for intersect.
                        // If V line x is between new line x0 and x1
                        // And new line y is between H line y0 and y1.
                        sections = vLines.Where(c =>
                            c.Item1.Item1 > npos.Item1 &&
                            c.Item1.Item1 < pos.Item1
                            &&
                            pos.Item2 > c.Item1.Item2 &&
                            pos.Item2 < c.Item2.Item2
                        ).Select(s => (s.Item1.Item1, pos.Item2));

                        intersections.AddRange(sections);
                        pos = npos;
                        break;
                    case 'R':
                        x = int.Parse(wire.Substring(1));
                        npos = (pos.Item1 + x, pos.Item2);

                        sections = vLines.Where(c =>
                            c.Item1.Item1 > pos.Item1 &&
                            c.Item1.Item1 < npos.Item1
                            &&
                            pos.Item2 > c.Item1.Item2 &&
                            pos.Item2 < c.Item2.Item2
                        ).Select(s => (s.Item1.Item1, pos.Item2));

                        intersections.AddRange(sections);
                        pos = npos;
                        break;
                    case 'U':
                        y = int.Parse(wire.Substring(1));
                        npos = (pos.Item1, pos.Item2 + y);

                        // Check for intersect.
                        // If H line y is between new line y0 and y1
                        // And new line x is between H line x0 and x1.
                        sections = hLines.Where(c =>
                            c.Item1.Item2 > pos.Item2 &&
                            c.Item1.Item2 < npos.Item2
                            &&
                            pos.Item1 > c.Item1.Item1 &&
                            pos.Item1 < c.Item2.Item1
                        ).Select(s => (pos.Item1, s.Item1.Item2));

                        intersections.AddRange(sections);
                        pos = npos;
                        break;
                    case 'D':
                        y = -1 * int.Parse(wire.Substring(1));
                        npos = (pos.Item1, pos.Item2 + y);

                        sections = hLines.Where(c =>
                            c.Item1.Item2 > npos.Item2 &&
                            c.Item1.Item2 < pos.Item2
                            &&
                            pos.Item1 > c.Item1.Item1 &&
                            pos.Item1 < c.Item2.Item1
                        ).Select(s => (pos.Item1, s.Item1.Item2));

                        intersections.AddRange(sections);
                        pos = npos;
                        break;
                }

            }

            int min = int.MaxValue;
            foreach (var item in intersections)
            {
                System.Console.WriteLine($"{item.Item1}, {item.Item2}");
                var dist = System.Math.Abs(item.Item1) 
                    + System.Math.Abs(item.Item2);
                if (dist < min)
                    min = dist;
            }

            return min.ToString();
        }
    }
}
