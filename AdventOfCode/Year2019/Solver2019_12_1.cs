using System;
namespace AdventOfCode.Year2019
{
    static class Solver2019_12_1
    {
        public static string Solve((int x, int y, int z, int vx, int vy, int vz)[] moons, int steps)
        {
            for (int step = 0; step < steps; step++)
            {
                // Apply gravity to get new velocities.
                for (int moon1Index = 0; moon1Index < moons.Length - 1; moon1Index++)
                {
                    var moon1 = moons[moon1Index];
                    for (int moon2Index = moon1Index+1; moon2Index < moons.Length; moon2Index++)
                    {
                        var moon2 = moons[moon2Index];
                        var xPull = (moon1.x < moon2.x) ? 1 : (moon1.x > moon2.x) ? -1 : 0;
                        var yPull = (moon1.y < moon2.y) ? 1 : (moon1.y > moon2.y) ? -1 : 0;
                        var zPull = (moon1.z < moon2.z) ? 1 : (moon1.z > moon2.z) ? -1 : 0;

                        moon1.vx += xPull;
                        moon1.vy += yPull;
                        moon1.vz += zPull;

                        moon2.vx -= xPull;
                        moon2.vy -= yPull;
                        moon2.vz -= zPull;

                        moons[moon2Index] = moon2;
                    }
                    moons[moon1Index] = moon1;
                }

                // Update positions using velocity.
                for (int i = 0; i < moons.Length; i++)
                {
                    var moon = moons[i];
                    moon.x += moon.vx;
                    moon.y += moon.vy;
                    moon.z += moon.vz;
                    moons[i] = moon;
                }
            }

            // Calculate energy
            int totalEnergy = 0;
            for (int i = 0; i < moons.Length; i++)
            {
                var (x, y, z, vx, vy, vz) = moons[i];
                // Potential and kinetic energy.
                var potentialEnergy = Math.Abs(x)+  Math.Abs(y)+  Math.Abs(z);
                var kineticEnergy = Math.Abs(vx) + Math.Abs(vy) + Math.Abs(vz);

                totalEnergy += potentialEnergy * kineticEnergy;
            }

            return totalEnergy.ToString();
        }
    }
}
