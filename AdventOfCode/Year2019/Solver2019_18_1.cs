using AdventOfCode.Utility;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Year2019
{
    static class Solver2019_18_1
    {
        public static string Solve(IInputResolver input)
        {
            var inData = input.AsEnumerable();

            int y = 0;
            int x = 0;
            var map = new Dictionary<(int x, int y), char>(100);
            var sPos = (x: -1, y: -1);
            var allKeys = new List<char>();
            foreach (var item in inData)
            {
                foreach (var pos in item)
                {
                    if (char.IsLower(pos))
                        allKeys.Add(pos);
                    if(pos == '@')
                    {
                        sPos = (x, y);
                        map.Add((x, y), '.');
                    }
                    else
                        map.Add((x, y), pos);
                    x++;
                }
                y++;
                x = 0;
            }

            var keys = new List<char>();
            var minStep = new List<int>();

            var travel = new Queue<((int x, int y) pos, List<char> keys, int steps)>();
            travel.Enqueue((sPos, keys, 0));
            var hset = new HashSet<((int, int), string, int)>();

            while (travel.Count > 0)
            {
                var current = travel.Dequeue();
                var res = Bfs(map, current.pos, current.keys, current.steps);

                foreach (var item in res)
                {
                    //var k = string.Join(",", item.Value.keys);
                    var done = item.Value.keys.All(allKeys.Contains)
                            && item.Value.keys.Count == allKeys.Count;

                    if (done)
                    {
                        Console.WriteLine($"({item.Key.x},{item.Key.y}), {item.Value.keys.Count}, {item.Value.steps}, {done}");
                        minStep.Add(item.Value.steps);
                    }
                    var ok = string.Join("", item.Value.keys);
                    if(!hset.Contains((item.Key, ok, item.Value.steps)))
                    {
                        hset.Add((item.Key, ok, item.Value.steps));
                        travel.Enqueue((item.Key, item.Value.keys, item.Value.steps));
                    }
                    else
                    {

                    }
                }
            }

            return minStep.Min().ToString();
        }

        private static Dictionary<(int x, int y), (List<char> keys, int steps)> Bfs(Dictionary<(int x, int y), char> map, (int x, int y) startPos, List<char> keys, int steps)
        {
            Queue<(int x, int y, int steps)> queue = new Queue<(int x, int y, int steps)>();
            var visited = new HashSet<(int x, int y)>(1000);

            queue.Enqueue((startPos.x, startPos.y, steps));
            visited.Add((startPos.x, startPos.y));

            var retMap = new Dictionary<(int x, int y), (List<char> keys, int steps)>();
            while (queue.Count > 0)
            {
                var current = queue.Dequeue();
                var cPos = (current.x, current.y);
                var onMap = map[cPos];
                if (onMap != '.')
                {
                    // We'd like every key position and door position.
                    // From there, we want to alter the map (open door, take key)
                    // and do a new traversal from that point.

                    // We may already ignore doors we cannot open?
                    if (char.IsUpper(onMap) && !keys.Contains(char.ToLower(onMap)))
                    {
                        continue;
                    }

                    if (char.IsLower(onMap) && !keys.Contains(onMap))
                    {
                        var nList = keys.Concat(new[] { onMap }).OrderBy(o => o).ToList();
                        retMap.Add(cPos, (nList, current.steps));
                        continue;
                    }
                }

                // Find neighbors.
                var a = map.Where(x => x.Key == (current.x, current.y + 1)
                    && !visited.Contains(x.Key)
                    && (x.Value == '.' || char.IsLower(x.Value) || char.IsUpper(x.Value))).Any();
                if (a)
                {
                    queue.Enqueue((current.x, current.y + 1, current.steps + 1));
                    visited.Add((current.x, current.y + 1));
                }
                a = map.Where(x => x.Key == (current.x, current.y - 1)
                    && !visited.Contains(x.Key)
                    && (x.Value == '.' || char.IsLower(x.Value) || char.IsUpper(x.Value))).Any();
                if (a)
                {
                    queue.Enqueue((current.x, current.y - 1, current.steps + 1));
                    visited.Add((current.x, current.y - 1));
                }
                a = map.Where(x => x.Key == (current.x + 1, current.y)
                    && !visited.Contains(x.Key)
                    && (x.Value == '.' || char.IsLower(x.Value) || char.IsUpper(x.Value))).Any();
                if (a)
                {
                    queue.Enqueue((current.x + 1, current.y, current.steps + 1));
                    visited.Add((current.x + 1, current.y));
                }
                a = map.Where(x => x.Key == (current.x - 1, current.y)
                    && !visited.Contains(x.Key)
                    && (x.Value == '.' || char.IsLower(x.Value) || char.IsUpper(x.Value))).Any();
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
