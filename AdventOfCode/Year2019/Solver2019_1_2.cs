using AdventOfCode.Utility;

namespace AdventOfCode.Year2019
{
    class Solver2019_1_2
    {
        public string Solve(IInputResolver input)
        {
            var data = input.AsEnumerable();
            int sum = 0;
            foreach (var item in data)
            {
                var moduleMass = int.Parse(item);
                var fuelMass = moduleMass / 3 - 2;
                
                // Calculate fuel for fuel.
                do
                {
                    sum += fuelMass;
                    fuelMass = fuelMass / 3 - 2;
                } while (fuelMass > 0);
            }

            return sum.ToString();
        }

    }
}
