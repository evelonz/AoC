using AdventOfCode.Utility;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Year2019
{
    static class Solver2019_10
    {
        
        public static string SolveFirst(IInputResolver input)
        {
            var inData = input.AsEnumerable();

            Dictionary<(int x, int y), int> astroids = GetAstroids(inData);

            var astroidSightCount = new Dictionary<(int x, int y), int>(astroids.Count);

            foreach (var astroid in astroids)
            {
                var angels = new HashSet<double>(100);
                foreach (var otherAstroid in astroids)
                {
                    //System.Console.WriteLine($"({astroid.Key.x}, {astroid.Key.y})");
                    if (otherAstroid.Key == astroid.Key)
                        continue;

                    var angle = System.Math
                        .Atan2(otherAstroid.Key.y - astroid.Key.y, otherAstroid.Key.x - astroid.Key.x)
                        * 180 / System.Math.PI;
                    angels.Add(angle);
                }
                astroidSightCount.Add(astroid.Key, angels.Count());
            }

            var max = astroidSightCount.Max(x => x.Value);
            var pos = astroidSightCount.First(f => f.Value == max);

            return max.ToString();
        }

        public static string SolveSecond(IInputResolver input, (int x, int y) center, int targetCount = 200)
        {
            var inData = input.AsEnumerable();

            var astroids = GetAstroids(inData);

            var newList = new List<(double angle, int dist, (int x, int y))>(astroids.Count);
            //var center = (x: 25, y: 31);
            //var center = (x: 11, y: 13);
            //var center = (x: 8, y: 3);

            foreach (var astroid in astroids)
            {
                if (astroid.Key == center)
                    continue;

                var angle = System.Math
                    .Atan2(astroid.Key.y - center.y, astroid.Key.x - center.x)
                    * 180 / System.Math.PI;
                var distance = System.Math.Abs(astroid.Key.y - center.y)
                    + System.Math.Abs(astroid.Key.x - center.x);

                newList.Add((angle, distance, astroid.Key));
            }

            int shots = 0;
            double angel = -90.00000001;
            var shotList = new List<(int x, int y)>(220);
            (int x, int y) last = (0, 0);
            while (shots < targetCount)
            {
                var t =
                    (angel >= 0) 
                    ? newList
                        .Where(x => x.angle > angel && angel >= 0
                            && !shotList.Contains(x.Item3))
                        .OrderBy(o => o.angle)
                        .ThenBy(o => o.dist)
                        .FirstOrDefault()
                    : newList
                        .Where(x => x.angle > angel && angel < 0
                            && !shotList.Contains(x.Item3))
                        .OrderBy(o => o.angle)
                        .ThenBy(o => o.dist)
                        .FirstOrDefault();
                if (t.dist != 0)
                {
                    shotList.Add(t.Item3);
                    shots++;
                    //System.Console.WriteLine($"{shots}: ({t.Item3.x}, {t.Item3.y})");
                    last = t.Item3;
                    angel = t.angle;
                }
                else
                {
                    // Try and progress over the fold.
                    angel += 1;
                    if (angel > 180)
                        angel = -180;
                }

            }

            return $"{last.x * 100 + last.y}";
        }

        private static Dictionary<(int x, int y), int> GetAstroids(IEnumerable<string> inData)
        {
            var astroids = new Dictionary<(int x, int y), int>(100);

            int y = 0;
            foreach (var line in inData)
            {
                int x = 0;
                foreach (var spot in line.ToArray())
                {
                    if (spot == '#')
                        astroids.Add((x, y), 0);
                    x++;
                }
                y++;
            }

            return astroids;
        }
    }

}
