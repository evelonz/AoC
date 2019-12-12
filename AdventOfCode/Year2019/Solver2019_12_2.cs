using AdventOfCode.Utility;

namespace AdventOfCode.Year2019
{
    static class Solver2019_12_2
    {
        public static string Solve((int x, int y, int z, int vx, int vy, int vz)[] moons)
        {
            // If the state loops, then the first state will be the first to return again,
            // since the only effect on the state is part of said state.

            // We should be able to calculate changes in each axis individually.
            // We then only have to find the the step when all of the axis are back in their start state.

            // Example: x axis for all bodies are back in the start state after 10 iterations.
            // y is back after 50, and z is back after 30.
            // Then we have to find the smallest common multiplier, to know how many steps
            // it takes until all axis align.
            // x: 2 * 5. y: 2 * 5 * 5. z: 2 * 5 * 3.
            // LCM = 2 * 5 * 5 * 3 = 150

            var tempMoons = new (int pos, int vel)[4];
            tempMoons[0] = (moons[0].x, moons[0].vx);
            tempMoons[1] = (moons[1].x, moons[1].vx);
            tempMoons[2] = (moons[2].x, moons[2].vx);
            tempMoons[3] = (moons[3].x, moons[3].vx);
            var stepX = GetStepsForAxis(tempMoons);

            tempMoons[0] = (moons[0].y, moons[0].vy);
            tempMoons[1] = (moons[1].y, moons[1].vy);
            tempMoons[2] = (moons[2].y, moons[2].vy);
            tempMoons[3] = (moons[3].y, moons[3].vy);
            var stepY = GetStepsForAxis(tempMoons);

            tempMoons[0] = (moons[0].z, moons[0].vz);
            tempMoons[1] = (moons[1].z, moons[1].vz);
            tempMoons[2] = (moons[2].z, moons[2].vz);
            tempMoons[3] = (moons[3].z, moons[3].vz);
            var stepZ = GetStepsForAxis(tempMoons);

            // Apply LCM twice to get the LCM for all three numbers.
            var steps = MathHelpers.Lcm(stepX, stepY);
            steps = MathHelpers.Lcm(steps, stepZ);
            return steps.ToString();
        }

        private static long GetStepsForAxis((int pos, int vel)[] moons)
        {
            var firstState = (moons[0], moons[1], moons[2], moons[3]);
            var step = 0;

            while (true)
            {
                // Apply gravity to get new velocity.
                for (int moon1Index = 0; moon1Index < moons.Length - 1; moon1Index++)
                {
                    var moon1 = moons[moon1Index];
                    for (int moon2Index = moon1Index + 1; moon2Index < moons.Length; moon2Index++)
                    {
                        var moon2 = moons[moon2Index];
                        var pull = (moon1.pos < moon2.pos) ? 1 : (moon1.pos > moon2.pos) ? -1 : 0;

                        moon1.vel += pull;
                        moon2.vel -= pull;

                        moons[moon2Index] = moon2;
                    }
                    moons[moon1Index] = moon1;
                }
                // Update positions using velocity.
                for (int i = 0; i < moons.Length; i++)
                {
                    var mon = moons[i];
                    mon.pos += mon.vel;
                    moons[i] = mon;
                }

                step++;
                var thisState = (moons[0], moons[1], moons[2], moons[3]);
                if (firstState == thisState)
                    return step;
            }
        }
    }
}
