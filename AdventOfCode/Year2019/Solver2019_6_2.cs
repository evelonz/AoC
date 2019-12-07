using AdventOfCode.Utility;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Year2019
{
    static class Solver2019_6_2
    {
        public static string Solve(IInputResolver input)
        {
            var orbitobjects = input.AsEnumerable();
            int orbits = 0;

            var startPoints = new HashSet<string>(1000);
            var orbs = new Dictionary<string, string>(1000);

            foreach (var item in orbitobjects)
            {
                var a = item.Split(')');
                var obj1 = a[0];
                var obj2 = a[1];

                startPoints.Add(obj1);
                startPoints.Add(obj2);

                // assume any object only orbits one other object.
                orbs.Add(obj2, obj1);
            }

            // Go from me to base.
            // Then startfrom san towards base.
            // Find first intersect.
            // Go back from there (or store the steps to that object)
            var start = orbs["YOU"];
            string next = start;
            var meSteps = new Dictionary<string, int>(100);
            while (orbs.TryGetValue(next, out var nxt))
            {
                next = nxt;
                orbits++;
                meSteps.Add(next, orbits);
            }

            start = orbs["SAN"];
            orbits = 0;
            next = start;
            while (orbs.TryGetValue(next, out var nxt))
            {
                next = nxt;
                orbits++;
                if(meSteps.TryGetValue(next, out var steps))
                {
                    orbits += steps;
                    break;
                }
            }

            return orbits.ToString();
        }
    }
}
