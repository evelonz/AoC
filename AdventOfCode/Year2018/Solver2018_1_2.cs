using AdventOfCode.Utility;
using System.Collections.Generic;
using System.Linq;
namespace AdventOfCode.Year2018
{
    class Solver2018_1_2
    {
        public string Solve(IInputResolver input)
        {
            int result = 0;
            var lines = input.AsEnumerable().Select(s => int.Parse(s)).ToList();

            var end = false;
            var h = new HashSet<int>(1000);
            h.Add(result);
            while(!end)
            {
                foreach (var item in lines)
                {
                    var next = item;
                    result += next;
                    if(h.Contains(result))
                    {
                        return result.ToString();
                    }
                    h.Add(result);
                }
            }

            return "Should not end here";
        }

    }
}
