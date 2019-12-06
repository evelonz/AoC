using AdventOfCode.Utility;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Year2019
{
    static class Solver2019_6_1
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

                // Assume any object only orbits one other object.
                orbs.Add(obj2, obj1);
            }

            foreach (var start in startPoints)
            {
                string next = start;
                while (orbs.TryGetValue(next, out var nxt))
                {
                    next = nxt;
                    orbits++;
                }
            }

            return orbits.ToString();
        }



    }
}
