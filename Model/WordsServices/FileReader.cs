using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.WordsServices
{
    public class FileReader : IFileReader
    {
        private readonly IFileSystem _fileSystem;

        private static readonly char[] _delimitersInFile = new[] { ' ', '\t', '\n', '\r' };

        public FileReader(IFileSystem fileSystem)
        {
            _fileSystem = fileSystem;
        }

        public async Task<Dictionary<string, int>> ReadFile(string fileName)
        {
            var fileText = await _fileSystem.File.ReadAllTextAsync(fileName);
            var words = fileText.Split(_delimitersInFile, StringSplitOptions.RemoveEmptyEntries);
            var wordToCount = new Dictionary<string, int>();
            foreach (var word in words)
            {
                wordToCount.TryAdd(word, 0);
                wordToCount[word]++;
            }

            return wordToCount;
        }

    }
}
