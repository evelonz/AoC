using System.Collections.Generic;
using System.IO;

namespace AdventOfCode.Utility
{
    class FileInputResolver : IInputResolver
    {
        private int _year;
        private int _day;
        private int _question;

        public FileInputResolver(int year, int day, int question)
        {
            _year = year;
            _day = day;
            _question = question;
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

        public StreamReader AsStream()
        {
            var path = Path.Combine(
                Directory.GetCurrentDirectory(),
                $"Year{_year}/AoCSource/{_day}-{_question}.txt");
            FileStream fileStream = new FileStream(path, FileMode.Open);
            return new StreamReader(fileStream);
        }
    }
}
