using AdventOfCode.Utility;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Year2019
{
    static class Solver2019_20_1
    {
        private class DNode
        {
            public (int x, int y) Pos { get; }
            public string Name { get; }
            public bool Visited { get; set; }
            public List<(DNode node, int distance)> Neighbours { get; set; }
            public int TentativeDistance { get; set; }

            public DNode((int x, int y) pos, string name)
            {
                Pos = pos;
                Name = name;
            }

            public override string ToString() => $"{Name}, ({Pos.x},{Pos.y})";
        }

        public static string Solve(IInputResolver input)
        {
            var inData = input.AsEnumerable();

            // Let travers every node, map paths and portals.
            // Save portals as nodes.
            // Traverse all paths from all portals, making a graph between them.
            // Traveling Salesman between nodes. Cut of to long paths.
            // Start at AA, end at ZZ.

            int y = 0;
            int x = 0;
            int xBound = 0;
            int yBound = 0;
            var map = new Dictionary<(int x, int y), char>(100);
            var portals = new List<(int x, int y)>();

            foreach (var item in inData)
            {
                foreach (var pos in item)
                {
                    if (char.IsUpper(pos))
                    {
                        // We found a portal.
                        portals.Add((x, y));
                    }                    
                    map.Add((x, y), pos);
                    x++;
                }
                y++;
                if (x > xBound) xBound = x;
                x = 0;
            }
            yBound = y;

            for (int yy = 0; yy < yBound; yy++)
            {
                for (int xx = 0; xx < xBound; xx++)
                {
                    Console.Write(map[(xx, yy)]);
                    if(char.IsUpper(map[(xx, yy)]))
                        Console.Write($"{xx},{yy}");

                }
                Console.WriteLine("");
            }

            ((int x, int y) pos, string name)[] ports = new[]
            // Ex 1
            //{
            //    (( 9,  2), "AA"),
            //    (( 9,  6), "BC"),
            //    (( 2,  8), "BC"),
            //    (( 6, 10), "DE"),
            //    ((11, 12), "FG"),
            //    (( 2, 13), "DE"),
            //    (( 2, 15), "FG"),
            //    ((13, 16), "ZZ"),
            //};
            // Ex 2.
            //{
            //    ((19,  2), "AA"),
            //    ((17,  8), "AS"),
            //    ((21,  8), "CP"),
            //    ((32, 11), "VT"),
            //    ((26, 13), "YN"),
            //    (( 2, 15), "DI"),
            //    (( 2, 17), "ZZ"),
            //    ((26, 17), "QG"),

            //    ((32, 17), "AS"),
            //    (( 2, 19), "JO"),
            //    (( 8, 21), "DI"),
            //    ((26, 21), "BU"),

            //    ((32, 21), "LF"),
            //    (( 2, 23), "YN"),
            //    ((26, 23), "VT"),
            //    ((32, 23), "QG"),

            //    ((13, 28), "JO"),
            //    ((15, 28), "LF"),
            //    ((21, 28), "JP"),

            //    ((11, 34), "BU"),
            //    ((15, 34), "JP"),
            //    ((19, 34), "CP")
            //};
            {
                //Q           I     B   S     J     P   J
                //I           P     B   L     M     Q   H
                ((37,  2), "QI"),
                ((49,  2), "IP"),
                ((55,  2), "BB"),
                ((59,  2), "SL"),
                ((65,  2), "JM"),
                ((71,  2), "PQ"),
                ((75,  2), "JH"),
                //J       Q E       P     W   J         M
                //M       I A       Q     C   K         Q
                ((41, 32), "JM"),
                ((49,  32), "QI"),
                ((51,  32), "EA"),
                ((59, 32), "PQ"),
                ((65,  32), "WC"),
                ((69,  32), "JK"),
                ((79, 32), "MQ"),
                // Left outer
                (( 2, 37), "SF"),
                (( 2, 45), "YY"),
                (( 2, 55), "YL"),
                (( 2, 59), "ZZ"),
                (( 2, 65), "MQ"),
                (( 2, 69), "OB"),
                (( 2, 77), "DU"),
                (( 2, 83), "GJ"),
                // Left inner
                ((32, 41), "SL"),
                ((32, 45), "OB"),
                ((32, 53), "HI"),
                ((32, 63), "PD"),
                ((32, 71), "GJ"),
                ((32, 79), "LH"),
                ((32, 87), "BB"),
                // inner right
                ((84, 37), "YL"),
                ((84, 45), "VN"),
                ((84, 51), "SF"),
                ((84, 61), "EJ"),
                ((84, 69), "PT"),
                ((84, 79), "YY"),
                ((84, 93), "HV"),
                // outer right
                ((114, 37), "WC"),
                ((114, 47), "HV"),
                ((114, 53), "EA"),
                ((114, 63), "PD"),
                ((114, 69), "LH"),
                ((114, 77), "BC"),
                ((114, 85), "EJ"),
                // Down inner
                //J     B         I   D       P       F
                //H     C         P   U       A       K
                ((41, 92), "JH"),
                ((47, 92), "BC"),
                ((57, 92), "IP"),
                ((61, 92), "DU"),
                ((69, 92), "PA"),
                ((77, 92), "FK"),
                // Down outer
                //P       V P A     J           F       H
                //A       N T A     K           K       I
                ((41, 122), "PA"),
                ((49, 122), "VN"),
                ((51, 122), "PT"),
                ((53, 122), "AA"),
                ((59, 122), "JK"),
                ((71, 122), "FK"),
                ((79, 122), "HI"),

            };

            var unvisited = new List<DNode>(ports.Length);
            foreach (var (pos, name) in ports)
            {
                unvisited.Add(new DNode(pos, name));
            }
            var listForLater = new List<DNode>(unvisited);
            // Travers the map and bind the portals by walkable path, and other end of portal.
            foreach (var item in unvisited)
            {
                var otherPortals = unvisited.Where(w => w.Pos != item.Pos).ToList();
                var res = Bfs(map, item.Pos, otherPortals);
                item.Neighbours = res;
                var endOfPortal = unvisited
                    .FirstOrDefault(f => f.Name == item.Name && f.Pos != item.Pos);
                if (endOfPortal != null) item.Neighbours.Add((endOfPortal, 1));
                item.TentativeDistance = int.MaxValue;
                if (item.Name == "AA") item.TentativeDistance = 0;
            }

            // Now to implement Dijkstra's. First time in 8 years...
            while(unvisited.Count > 0)
            {
                var current = unvisited
                        .OrderBy(o => o.TentativeDistance)
                        .FirstOrDefault(f => f.TentativeDistance < int.MaxValue);
                if(current == null)
                {
                    // Something is wrong. Or we finished.
                    break;
                }
                unvisited.Remove(current);

                foreach (var n in current.Neighbours)
                {
                    var newDistance = current.TentativeDistance + n.distance;
                    if(newDistance < n.node.TentativeDistance)
                    {
                        n.node.TentativeDistance = newDistance;
                    }
                }
            }

            return listForLater.First(f => f.Name == "ZZ").TentativeDistance.ToString();
        }

        private static List<(DNode, int)> Bfs(
            Dictionary<(int x, int y), char> map, 
            (int x, int y) startPos, 
            List<DNode> portals)
        {
            Queue<(int x, int y, int steps)> queue = new Queue<(int x, int y, int steps)>();
            var visited = new HashSet<(int x, int y)>(1000);

            queue.Enqueue((startPos.x, startPos.y, 0));
            visited.Add((startPos.x, startPos.y));

            var retMap = new List<(DNode node, int distnace)>();
            while (queue.Count > 0)
            {
                var current = queue.Dequeue();
                var cPos = (current.x, current.y);
                var onMap = map[cPos];
                // We only care about paths.
                if (onMap != '.')
                {
                    continue;
                }
                
                // If we found another portal, add it to nodes.
                var p = portals.FirstOrDefault(f => f.Pos == cPos);
                if(p != null)
                {
                    retMap.Add((p, current.steps));
                }

                // Find neighbors.
                var a = map.Where(x => x.Key == (current.x, current.y + 1)
                    && !visited.Contains(x.Key)
                    && (x.Value == '.')).Any();
                if (a)
                {
                    queue.Enqueue((current.x, current.y + 1, current.steps + 1));
                    visited.Add((current.x, current.y + 1));
                }
                a = map.Where(x => x.Key == (current.x, current.y - 1)
                    && !visited.Contains(x.Key)
                    && (x.Value == '.')).Any();
                if (a)
                {
                    queue.Enqueue((current.x, current.y - 1, current.steps + 1));
                    visited.Add((current.x, current.y - 1));
                }
                a = map.Where(x => x.Key == (current.x + 1, current.y)
                    && !visited.Contains(x.Key)
                    && (x.Value == '.')).Any();
                if (a)
                {
                    queue.Enqueue((current.x + 1, current.y, current.steps + 1));
                    visited.Add((current.x + 1, current.y));
                }
                a = map.Where(x => x.Key == (current.x - 1, current.y)
                    && !visited.Contains(x.Key)
                    && (x.Value == '.')).Any();
                if (a)
                {
                    queue.Enqueue((current.x - 1, current.y, current.steps + 1));
                    visited.Add((current.x - 1, current.y));
                }
            }

            return retMap;
        }
    }
}
