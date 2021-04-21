using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Microsoft.EntityFrameworkCore;

namespace Model
{
    public class WordsService
    {
        private static readonly char[] _delimitersInFile = new[] { ' ', '\t', '\n', '\r' };

        private readonly string _wordsTableName;

        private WordsContext _context;

        public WordsService(WordsContext wordsContext)
        {
            _context = wordsContext;
            var entityType = _context.Model.FindEntityType(typeof(Word));
            _wordsTableName = entityType.GetTableName();
        }

        public async Task CreateDictionary(string fileName)
        {
            Dictionary<string, int> wordToCount = await ReadFile(fileName);
            await ClearDictionary();
            var wordEntities = wordToCount.Select((wordAndCount) => new Word() { Count = wordAndCount.Value, Value = wordAndCount.Key })
                                          .ToList();
            await _context.Words.AddRangeAsync(wordEntities);
        }

        private static async Task<Dictionary<string, int>> ReadFile(string fileName)
        {
            var fileText = await File.ReadAllTextAsync(fileName);
            var words = fileText.Split(_delimitersInFile, StringSplitOptions.RemoveEmptyEntries);
            var wordToCount = new Dictionary<string, int>();
            foreach (var word in words)
            {
                wordToCount.TryAdd(word, 0);
                wordToCount[word]++;
            }

            return wordToCount;
        }

        public async Task ClearDictionary()
        {
            await _context.Database.ExecuteSqlRawAsync($"TRUNCATE TABLE [{_wordsTableName}]");
        }
    }
}
