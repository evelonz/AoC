namespace AdventOfCode2021.Utility;

internal class FileInputResolver : IInputResolver
{
    private readonly int _day;

    public FileInputResolver(int day)
    {
        _day = day;
    }

#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
    public IEnumerable<string> AsEnumerable()
    {
        using StreamReader reader = AsStream();
        string line;

        while ((line = reader.ReadLine()) != null)
        {
            yield return line;
        }
    }

    private StreamReader AsStream()
    {
        var path = Path.Combine(
            Directory.GetCurrentDirectory(),
            $"Puzzles\\Day{_day}\\{_day}.txt");
        var fileStream = new FileStream(path, FileMode.Open);
        return new StreamReader(fileStream);
    }
}
