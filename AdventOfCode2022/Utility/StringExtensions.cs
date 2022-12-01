namespace AdventOfCode2022.Utility;

internal static class StringExtensions
{
    public static string Reverse(this string s)
    {
        char[] charArray = s.ToCharArray();
        Array.Reverse(charArray);
        return new string(charArray);
    }
}
