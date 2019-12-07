using System.Collections.Generic;
using System.IO;

namespace AdventOfCode.Utility
{
    class FileInputResolver : IInputResolver
    {
        private int _year;
        private int _day;

        public FileInputResolver(int year, int day)
        {
            _year = year;
            _day = day;
        }

        public IEnumerable<string> AsEnumerable()
        {
            using (StreamReader reader = AsStream())
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    yield return line;
                }
            }
        }

        private StreamReader AsStream()
        {
            var path = Path.Combine(
                Directory.GetCurrentDirectory(),
                $"Year{_year}/AoCSource/{_day}.txt");
            FileStream fileStream = new FileStream(path, FileMode.Open);
            return new StreamReader(fileStream);
        }
    }
}
