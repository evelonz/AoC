﻿using AdventOfCode.Utility;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Year2019
{
    static class Solver2019_14
    {
        public static string SolveFirst(IInputResolver input)
        {
            var recipes = GetRecipes(input);

            var storage = recipes.Keys.ToDictionary(d => d, d => 0);
            var res = MakeOneFuel(recipes, storage);

            return res.ToString();
        }

        public static string SolveSecond(IInputResolver input, long oreStock = 1_000_000_000_000)
        {
            var recipes = GetRecipes(input);

            var storage = recipes.Keys.ToDictionary(d => d, d => 0);
            long fuel = 0;

            while (oreStock > 0)
            {
                int oresUsed = MakeOneFuel(recipes, storage);
                oreStock -= oresUsed;
                if (oreStock >= 0)
                    fuel++;
            }

            return fuel.ToString();
        }



        private static Dictionary<string, (int quantity, List<(string type, int num)> ingredience)> GetRecipes(IInputResolver input)
        {
            var inData = input.AsEnumerable()
                .Select(s => s.Split("=>"));

            var c = inData.Select(s => (ingr: s[0].Split(",").Select(ss => ss.Trim()), res: s[1].Trim().Split(" ")));

            var b = c.Select(s =>
                (
                    ingr: s.ingr.Select(ss => ss.Split(" "))
                        .Select(a => (type: a[1].Trim(), num: int.Parse(a[0]))).ToList(),
                    res: (type: s.res[1].Trim(), num: int.Parse(s.res[0]))
                )
            ).ToList();

            var recepeis = new Dictionary<string,
                (int quantity, List<(string type, int num)> ingredience)>(b.Count);
            foreach (var item in b)
            {
                recepeis.Add(item.res.type,
                    (item.res.num, item.ingr));
            }

            return recepeis;
        }

        private static int MakeOneFuel(
            Dictionary<string, (int quantity, List<(string type, int num)> ingredience)> recipes,
            Dictionary<string, int> storage)
        {
            var needed = recipes.Keys.ToDictionary(d => d, d => 0);
            needed["FUEL"] = 1;
            needed.Add("ORE", 0);
            var target = "FUEL";

            while (true)
            {
                var needQuantity = needed[target];

                // First chack how much we have in store.
                var inStore = storage[target];
                if (inStore > 0)
                {
                    var delta = needQuantity - inStore;
                    // If fully covered
                    if (delta <= 0)
                    {
                        storage[target] = (inStore - needQuantity);
                        needed[target] = 0;

                        if (!TryGetNextTarget(needed, out target))
                            break;
                        else
                            continue;
                    }
                    else
                    {
                        storage[target] = 0;
                        needQuantity -= inStore;
                    }
                }

                // If not enough stored, then create more using the recepie.
                var (madeQuantity, ingredience) = recipes[target];
                // We can only make complete recepies. To get all we need, we have to round up.
                // We store any excess products in storage.
                var ratio = (int)System.Math.Ceiling(needQuantity / (double)madeQuantity);
                madeQuantity *= ratio;
                foreach (var (type, num) in ingredience)
                {
                    needed[type] += num * ratio;
                }

                if (needQuantity < madeQuantity)
                {
                    storage[target] += madeQuantity - needQuantity;
                }

                needed[target] = 0;

                // Get next target. If only ore left, then we are done.
                if (!TryGetNextTarget(needed, out target))
                    break;
            }

            return needed["ORE"];
        }

        private static bool TryGetNextTarget(Dictionary<string, int> needed, out string nextTarget)
        {
            nextTarget = needed.FirstOrDefault(f => f.Key != "ORE" && f.Value > 0).Key;
            return nextTarget != null;
        }
    }
}
