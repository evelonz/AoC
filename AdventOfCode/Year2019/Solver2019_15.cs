using AdventOfCode.Utility;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Year2019
{
    static class Solver2019_15
    {
        private enum Map15Type
        {
            None = -1,
            Wall = 0,
            Path = 1,
            O2 = 2,
        }

        public static string SolveFirst(IInputResolver input, bool print = false)
        {
            var (map, start, end) = GetMap(input);
            var steps = Bfs(map, start, end);
            return steps.ToString();
        }

        public static string SolveSecond(IInputResolver input, bool print = false)
        {
            var (map, _, end) = GetMap(input, print);
            var steps = Bfs(map, end);
            return steps.ToString();
        }

        private static (Dictionary<(long x, long y), Map15Type> map, (int x, int y) start, (int x, int y) end) 
            GetMap(IInputResolver input, bool print = false)
        {
            var instructions = new long[10_000];
            var inData = input.AsEnumerable()
                .First().Split(',')
                .Select(s => long.Parse(s)).ToArray();
            inData.CopyTo(instructions, 0);

            var sut = new IntcodeComputer(instructions);

            long? pInput = null;
            long response = 0; // 0 = wall, 1 moved, 2 found oxygen system.
            int size = 600; // Simply guessed on a max size. Think this can be removed as we don't use any array or matrix.
            long x = size/2; long y = size/2;
            ProgramState state = ProgramState.None;

            var map = new Dictionary<(long x, long y), Map15Type>(size);
            map.Add((x, y), Map15Type.Path);
            bool hitWall = false;
            (int x, int y) O2Pos = (-1, -1);
            int tick = 0;

            // Traversing the map following the left hand side.
            // This does not work if there are any open spaces.
            var orientation = 1L; // Start north.
            var attempt = 1; // 1-3, left, straight, right. Relative orientation.

            while (state != ProgramState.Halted)
            {
                state = sut.RunProgram(pInput);

                // Save the display values based on output.
                if (state == ProgramState.ProvidedOutput)
                {
                    response = sut.LastOutput;

                    // If we could move, mark it and update bots position.
                    if(response == 1 || response == 2)
                    {
                        if (map.ContainsKey((x, y)))
                        {
                            map[(x, y)] = Map15Type.Path;
                        }
                        else
                        {
                            map.Add((x, y), Map15Type.Path);
                        }

                        // Update the position.
                        if (pInput == 1)
                            y--;
                        if (pInput == 2)
                            y++;
                        if (pInput == 3)
                            x--;
                        if (pInput == 4)
                            x++;

                        // Mark o2 system if found.
                        if(response == 2)
                        {
                            O2Pos = ((int)x, (int)y);
                        }
                    }
                    // If wall, mark the wall in the position we tried to move to.
                    else if (response == 0)
                    {
                        hitWall = true;

                        var xx = (pInput == 3) ? x - 1
                            : (pInput == 4) ? x + 1 : x;
                        var yy = (pInput == 1) ? y - 1
                            : (pInput == 2) ? y + 1 : y;

                        if (map.ContainsKey((xx, yy)))
                        {
                            map[(xx, yy)] = Map15Type.Wall;
                        }
                        else
                            map.Add((xx, yy), Map15Type.Wall);
                    }
                    else
                    {
                        throw new Exception($"Unexpected response: {response}");
                    }

                }

                state = sut.RunProgram(null);
                if (state == ProgramState.NeedInput)
                {
                    
                    // If back at start, return the map and key positions.
                    if (x == size / 2 && y == size / 2 && tick > 4)
                    {
                        return (map, (size / 2, size / 2), O2Pos);
                    }

                    // Draw the map. Added some throttling.
                    tick++;
                    if (print && tick % 100 == 0)
                    {
                        DrawMap(size, x, y, map, O2Pos);
                    }

                    // Get moves automatically.
                    pInput = pInput ?? 1; // Check first turn null

                    if (hitWall)
                    {
                        // last attempt done, turn around.
                        if (attempt == 3)
                        {
                            var n = GetLeftFromOrientation(orientation, 1);
                            n = GetLeftFromOrientation(n, 1);
                            orientation = n;
                            attempt = 1;
                        }
                    }
                    else
                    {
                        orientation = (int)pInput;
                        attempt = 1;
                    }
                    pInput = GetLeftFromOrientation(orientation, attempt);
                    attempt++;
                    hitWall = false;

                    // Manually play the game
                    //var ch = Console.ReadKey(false).Key;
                    //switch (ch)
                    //{
                    //    case ConsoleKey.UpArrow:
                    //        pInput = 1;
                    //        break;
                    //    case ConsoleKey.DownArrow:
                    //        pInput = 2;
                    //        break;
                    //    case ConsoleKey.LeftArrow:
                    //        pInput = 3;
                    //        break;
                    //    case ConsoleKey.RightArrow:
                    //        pInput = 4;
                    //        break;
                    //}

                }

            }

            throw new Exception("Unable to resolve map!");
        }

        private static object Bfs(Dictionary<(long x, long y), Map15Type> map, (int x, int y) startPos, (long x, long y)? endPoint = null)
        {
            int maxsteps = 0;
            Queue<(long x, long y, int steps)> queue = new Queue<(long x, long y, int steps)>();
            var visited = new HashSet<(long x, long y)>(1000);

            queue.Enqueue((startPos.x, startPos.y, 0));
            visited.Add((startPos.x, startPos.y));

            while (queue.Count > 0)
            {
                var current = queue.Dequeue();
                if(endPoint != null)
                {
                    if(current.x == endPoint.Value.x && current.y == endPoint.Value.y)
                    {
                        return current.steps;
                    }
                }

                if (current.steps > maxsteps)
                    maxsteps = current.steps;

                // Find neighbors.
                var a = map.Where(x => x.Key == (current.x, current.y + 1)
                    && !visited.Contains(x.Key)
                    && x.Value == Map15Type.Path).Any();
                if(a)
                {
                    queue.Enqueue((current.x, current.y + 1, current.steps + 1));
                    visited.Add((current.x, current.y + 1));
                }
                a = map.Where(x => x.Key == (current.x, current.y - 1)
                    && !visited.Contains(x.Key)
                    && x.Value == Map15Type.Path).Any();
                if (a)
                {
                    queue.Enqueue((current.x, current.y - 1, current.steps + 1));
                    visited.Add((current.x, current.y - 1));
                }
                a = map.Where(x => x.Key == (current.x + 1, current.y)
                    && !visited.Contains(x.Key)
                    && x.Value == Map15Type.Path).Any();
                if (a)
                {
                    queue.Enqueue((current.x + 1, current.y, current.steps + 1));
                    visited.Add((current.x + 1, current.y));
                }
                a = map.Where(x => x.Key == (current.x - 1, current.y)
                    && !visited.Contains(x.Key)
                    && x.Value == Map15Type.Path).Any();
                if (a)
                {
                    queue.Enqueue((current.x - 1, current.y, current.steps + 1));
                    visited.Add((current.x - 1, current.y));
                }
            }

            return maxsteps;
        }

        private static void DrawMap(int size, long x, long y, Dictionary<(long x, long y), Map15Type> map, (int x, int y) O2Pos)
        {
            var xmax = map.Max(m => m.Key.x);
            var xmin = map.Min(m => m.Key.x);
            var ymax = map.Max(m => m.Key.y);
            var ymin = map.Min(m => m.Key.y);
            xmax = Math.Max(xmax, x);
            xmin = Math.Min(xmin, x);
            ymax = Math.Max(ymax, y);
            ymin = Math.Min(ymin, y);

            Console.Clear();

            for (int yy = (int)ymin; yy < ymax + 1; yy++)
            {
                for (int xx = (int)xmin; xx < xmax + 1; xx++)
                {
                    if (map.TryGetValue((xx, yy), out var vall)
                        || (xx == (int)x && yy == (int)y))
                    {
                        var t = (vall == Map15Type.Wall) ? "#"
                            : vall == Map15Type.Path ? "."
                            : "?";
                        t = xx == size / 2 && yy == size / 2 ? "S" : t;
                        t = xx == O2Pos.x && yy == O2Pos.y ? "O" : t;
                        t = xx == (int)x && yy == (int)y ? "D" : t;
                        Console.Write(t);
                    }
                    else
                    {
                        Console.Write(" ");
                    }
                }
                Console.WriteLine("");
            }
        }

        private static long GetLeftFromOrientation(long orientation, int attempt)
        {
            if (attempt < 1 || attempt > 3) throw new IndexOutOfRangeException("Attempt must be in range 1-3");
            switch (orientation)
            {
                // N
                case 1:
                    return attempt == 1 ? 3
                        : attempt == 2 ? 1
                        : 4;
                // E
                case 4:
                    return attempt == 1 ? 1
                        : attempt == 2 ? 4
                        : 2;
                // S
                case 2:
                    return attempt == 1 ? 4
                        : attempt == 2 ? 2
                        : 3;
                // W
                case 3:
                    return attempt == 1 ? 2
                        : attempt == 2 ? 3
                        : 1;
            }
            throw new ArgumentException("Invalid orientation", nameof(orientation));
        }
    }
}
