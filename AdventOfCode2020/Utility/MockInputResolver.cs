using System.Collections.Generic;
using System.IO;

namespace AdventOfCode2020.Utility
{
    internal class MockInputResolver : IInputResolver
    {
        private readonly IEnumerable<string> _input;

        public MockInputResolver(IEnumerable<string> input)
        {
            _input = input;
        }

        public IEnumerable<string> AsEnumerable()
        {
            return _input;
        }
    }
}
