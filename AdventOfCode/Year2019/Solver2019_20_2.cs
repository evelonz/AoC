using AdventOfCode.Utility;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Year2019
{
    static class Solver2019_20_2
    {
        private class DNode
        {
            public (int x, int y) Pos { get; }
            public string Name { get; }
            public bool Visited { get; set; }
            public List<(DNode node, int distance)> Neighbours { get; set; }
            public int TentativeDistance { get; set; }
            public bool OuterWall { get; set; }
            public bool InnerWall => !OuterWall;
            public int Level { get; set; }

            public DNode((int x, int y) pos, string name, bool outer, int level)
            {
                Pos = pos;
                Name = name;
                OuterWall = outer;
                Level = level;
                TentativeDistance = int.MaxValue;
            }

            public override string ToString() => $"{Name}, L:{Level}, In:{InnerWall}, ({Pos.x},{Pos.y}), D: {TentativeDistance}";
        }

        public static string Solve(IInputResolver input)
        {
            var inData = input.AsEnumerable();

            // Let travers every node, map paths and portals.
            // Save portals as nodes.
            // Traverse all paths from all portals, making a graph between them.
            // Traveling Salesman between nodes. Cut of to long paths.
            // Start at AA, end at ZZ.

            // Part two
            // We now can go up or down in multiple levels.
            // At level 0, only AA and ZZ work at the outer wall.
            // At lower levels, AA and ZZ does not work.
            // Portals along the inner wall leads down 1 level. (+1).
            // Portals at the outer wall leads up one level (-1).
            // We don't know the depth, so perhaps we can create 
            // each level on the fly. We also have to add handling of portals
            // now being counted as walls at given levels.

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

            ((int x, int y) pos, string name, bool outer)[] ports = new[]
             //Ex 1
            //{
            //    (( 9,  2), "AA", true),
            //    (( 9,  6), "BC", false),
            //    (( 2,  8), "BC", true),
            //    (( 6, 10), "DE", false),
            //    ((11, 12), "FG", false),
            //    (( 2, 13), "DE", true),
            //    (( 2, 15), "FG", true),
            //    ((13, 16), "ZZ", true),
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
            // Ex 3:
            //{
            //    // Top outer
            //    //Z L X W       C
            //    //Z P Q B       K
            //    ((13, 2), "ZZ", true),
            //    ((15, 2), "LP", true),
            //    ((17, 2), "XQ", true),
            //    ((19, 2), "WB", true),
            //    ((27, 2), "CK", true),
            //    // Top inner
            //    //F       R I       Z
            //    //D       E C       H
            //    ((13, 8), "FD", false),
            //    ((21, 8), "RE", false),
            //    ((23, 8), "IC", false),
            //    ((31, 8), "ZH", false),
            //    // Left outer CJ, XF, RE
            //    ((2, 15), "CJ", true),
            //    ((2, 21), "XF", true),
            //    ((2, 25), "RE", true),
            //    // Left inner OA, CK, CJ
            //    ((8, 13), "OA", false),
            //    ((8, 17), "CK", false),
            //    ((8, 23), "CJ", false),
            //    // Right inner WB, RF, NM
            //    ((36, 13), "WB", false),
            //    ((36, 21), "RF", false),
            //    ((36, 23), "NM", false),
            //    // Right outer ZH, IC, RF
            //    ((42, 13), "ZH", true),
            //    ((42, 17), "IC", true),
            //    ((42, 25), "RF", true),
            //    // Bot inner 
            //    //X   X       L
            //    //F   Q       P
            //    ((17, 28), "XF", false),
            //    ((21, 28), "XQ", false),
            //    ((29, 28), "LP", false),
            //    // Bot outer
            //    //A O F   N
            //    //A A D   M
            //    ((15, 34), "AA", true),
            //    ((17, 34), "OA", true),
            //    ((19, 34), "FD", true),
            //    ((23, 34), "NM", true),
            //};
            // Input:
            {
                //Q           I     B   S     J     P   J
                //I           P     B   L     M     Q   H
                ((37, 2), "QI", true),
                ((49, 2), "IP", true),
                ((55, 2), "BB", true),
                ((59, 2), "SL", true),
                ((65, 2), "JM", true),
                ((71, 2), "PQ", true),
                ((75, 2), "JH", true),
                //J       Q E       P     W   J         M
                //M       I A       Q     C   K         Q
                ((41, 32), "JM", false),
                ((49, 32), "QI", false),
                ((51, 32), "EA", false),
                ((59, 32), "PQ", false),
                ((65, 32), "WC", false),
                ((69, 32), "JK", false),
                ((79, 32), "MQ", false),
                // Left outer
                ((2, 37), "SF", true),
                ((2, 45), "YY", true),
                ((2, 55), "YL", true),
                ((2, 59), "ZZ", true),
                ((2, 65), "MQ", true),
                ((2, 69), "OB", true),
                ((2, 77), "DU", true),
                ((2, 83), "GJ", true),
                // Left inner
                ((32, 41), "SL", false),
                ((32, 45), "OB", false),
                ((32, 53), "HI", false),
                ((32, 63), "PD", false),
                ((32, 71), "GJ", false),
                ((32, 79), "LH", false),
                ((32, 87), "BB", false),
                // inner right
                ((84, 37), "YL", false),
                ((84, 45), "VN", false),
                ((84, 51), "SF", false),
                ((84, 61), "EJ", false),
                ((84, 69), "PT", false),
                ((84, 79), "YY", false),
                ((84, 83), "HV", false), // Corrected from 93.
                // outer right
                ((114, 37), "WC", true),
                ((114, 47), "HV", true),
                ((114, 53), "EA", true),
                ((114, 63), "PD", true),
                ((114, 69), "LH", true),
                ((114, 77), "BC", true),
                ((114, 85), "EJ", true),
                // Down inner
                //J     B         I   D       P       F
                //H     C         P   U       A       K
                ((41, 92), "JH", false),
                ((47, 92), "BC", false),
                ((57, 92), "IP", false),
                ((61, 92), "DU", false),
                ((69, 92), "PA", false),
                ((77, 92), "FK", false),
                // Down outer
                //P       V P A     J           F       H
                //A       N T A     K           K       I
                ((41, 122), "PA", true),
                ((49, 122), "VN", true),
                ((51, 122), "PT", true),
                ((53, 122), "AA", true),
                ((59, 122), "JK", true),
                ((71, 122), "FK", true),
                ((79, 122), "HI", true),
            };

            var unvisited = new List<DNode>(ports.Length);
            foreach (var (pos, name, outer) in ports)
            {
                var level = outer && name != "AA" && name != "ZZ" ? 1 : 0;
                unvisited.Add(new DNode(pos, name, outer, level));
            }
            var listForLater = new List<DNode>(unvisited);
            // Travers the map and bind the portals by walkable path, and other end of portal.
            foreach (var item in unvisited)
            {
                var otherPortals = unvisited
                    .Where(w => w.Pos != item.Pos && w.Level == 0)
                    .ToList();
                var res = Bfs(map, item.Pos, otherPortals, 0);
                item.Neighbours = res;
                var endOfPortal = unvisited
                    .FirstOrDefault(f => f.Name == item.Name && f.Pos != item.Pos);
                if (endOfPortal != null) item.Neighbours.Add((endOfPortal, 1));
                item.TentativeDistance = int.MaxValue;
                if (item.Name == "AA") item.TentativeDistance = 0;
            }

            // We generate levels on the fly.
            // If current level is below this, then we have traverse it's nodes.
            int lowestLevel = 0;
            // It's teh BFS that takes time. Since all connections are the same,
            // just repeated for lower levels, we should save them and then just
            // create new nodes with updated levels and connections.
            // First iteration is with AA and ZZ, and no other outer walls though,
            // So we need at least one more BFS. Or do one and update logic for edge cases.

            // We do a second traversel to get the correct neigbours for all lower levels.
            // Now we have to generate this level!
            foreach (var (pos, name, outer) in ports)
            {
                var level = outer ? 2 : 1;
                if (name != "AA" && name != "ZZ")
                    unvisited.Add(new DNode(pos, name, outer, level));
            }
            // Travers the map and bind the portals by walkable path, and other end of portal.
            // Save the connections so we can reuse them without BFS.
            var nodeConnections = 
                new Dictionary<((int x, int y) pos, string name),
                    List<((int x, int y) pos, string name, int distance, int levelDiff)>>();
            foreach (var item in unvisited.Where(w => w.Level == 1))
            {
                var otherPortals = unvisited
                    .Where(w => w.Pos != item.Pos && w.Level == 1)
                    .ToList();
                var res = Bfs(map, item.Pos, otherPortals, 1);
                item.Neighbours = res;

                var endOfPortal = unvisited
                    .FirstOrDefault(f => f.Name == item.Name && f.Pos != item.Pos
                    && ((item.OuterWall && item.Level - 1 == f.Level)
                    || (item.InnerWall && item.Level + 1 == f.Level)));
                if (endOfPortal != null) item.Neighbours.Add((endOfPortal, 1));
                // Save connections
                var connection = item.Neighbours
                    // Note on level, we are currently on 1, so Level-1 gives 0 if on same level.
                    .Select(s => (s.node.Pos, s.node.Name, s.distance, s.node.Level - 1))
                    .ToList();
                nodeConnections.Add((item.Pos, item.Name), connection);
            }
            lowestLevel = 1;

            foreach (var item in nodeConnections)
            {
                // Check that all nodes connect up and down from levels.
                var (pos, name, outer) = ports.Single(s => s.pos == item.Key.pos && s.name == item.Key.name);
                if (outer)
                {
                    var upLink = item.Value.Single(s => s.name == name && s.levelDiff == -1);
                }
                else
                {
                    var downLink = item.Value.Single(s => s.name == name && s.levelDiff == 1);
                }

            }

            // Now to implement Dijkstra's. First time in 8 years...
            while (unvisited.Count > 0)
            {
                var current = unvisited
                        .OrderBy(o => o.TentativeDistance)
                        .FirstOrDefault(f => f.TentativeDistance < int.MaxValue);
                if(current == null)
                {
                    // Something is wrong. Or we finished.
                    break;
                }
                
                // Add exit condition here, we cannot traverse all nodes here.
                if (current.Name == "ZZ")
                    break;

                if(current.Level > lowestLevel)
                {
                    // Now we have to generate this level!
                    foreach (var (pos, name, outer) in ports)
                    {
                        var level = outer ? current.Level + 1 : current.Level;
                        if(name != "AA" && name != "ZZ")
                            unvisited.Add(new DNode(pos, name, outer, level));
                    }
                    // Travers the map and bind the portals by walkable path, and other end of portal.
                    foreach (var item in unvisited.Where(w => w.Level == current.Level))
                    {
                        var c = nodeConnections[(item.Pos, item.Name)];
                        var n = new List<(DNode node, int distance)>(c.Count);
                        foreach (var (pos, name, distance, levelDiff) in c)
                        {
                            var thisLevelNode = unvisited
                                .FirstOrDefault(w => w.Pos == pos && w.Name == name
                                    && w.Level == current.Level + levelDiff);

                            // This should only happen for the node we stepped down one level from.
                            // So lets check it explicitly as well.
                            var isStepdownNode = name == item.Name
                                && item.Pos != pos && levelDiff == -1;
                            if (thisLevelNode == null && !isStepdownNode)
                            {
                                // Something is off.
                                throw new Exception("Missing node connection: " + current);
                            }
                            if(thisLevelNode != null)
                                n.Add((thisLevelNode, distance));
                        }
                        item.Neighbours = n;
                    }
                    lowestLevel = current.Level;
                    //Console.WriteLine(current);
                    //Console.WriteLine($"------------- {lowestLevel}-------------");
                    //PrintTopNodes(unvisited);
                }
                // Has to be done after generating the new level.
                unvisited.Remove(current);

                foreach (var (node, distance) in current.Neighbours)
                {
                    var newDistance = current.TentativeDistance + distance;
                    if(newDistance < node.TentativeDistance)
                    {
                        node.TentativeDistance = newDistance;
                    }
                }
            }

            return listForLater.First(f => f.Name == "ZZ").TentativeDistance.ToString();
        }

        private static void PrintTopNodes(List<DNode> nodes)
        {
            foreach (var item in nodes.OrderBy(o => o.TentativeDistance).Where(x => x.TentativeDistance < int.MaxValue))
            {
                Console.WriteLine(item);
            }
        }

        private static List<(DNode, int)> Bfs(
            Dictionary<(int x, int y), char> map, 
            (int x, int y) startPos, 
            List<DNode> portals,
            int level)
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
                    // Extra check for part two: AA and ZZ only work on level 0.
                    // Other outer wall portals does not work on level 0.
                    if(level > 0 && (p.Name == "AA" || p.Name == "ZZ"))
                    { }
                    else if (level == 0 && p.OuterWall && p.Name != "AA" && p.Name != "ZZ")
                    { }
                    else
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
