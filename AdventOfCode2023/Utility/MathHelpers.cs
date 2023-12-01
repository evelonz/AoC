namespace AdventOfCode2023.Utility
{
    internal static class MathHelpers
    {
        /// <summary>
        /// Get the least common multiple of two numbers.
        /// </summary>
        public static long Lcm(long a, long b)
        {
            long num1 = a;
            long num2 = b;
            if (a < b)
            {
                num1 = b;
                num2 = a;
            }

            for (long i = 1; i < num2; i++)
            {
                if ((num1 * i) % num2 == 0)
                {
                    return i * num1;
                }
            }
            return num1 * num2;
        }
    }
}
