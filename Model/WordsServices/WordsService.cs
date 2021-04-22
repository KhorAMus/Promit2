using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Microsoft.EntityFrameworkCore;
using System.IO.Abstractions;
using Model.WordsServices;

namespace Model.WordServices
{
    public class WordsService
    {
        private readonly string _wordsTableName;

        private readonly WordsContext _context;

        private readonly IFileReader _fileReader;

        public WordsService(WordsContext wordsContext, IFileReader fileReader)
        {
            _fileReader = fileReader;
            _context = wordsContext;
            var entityType = _context.Model.FindEntityType(typeof(Word));
            _wordsTableName = entityType.GetTableName();
        }

        public async Task CreateDictionary(string fileName)
        {
            Dictionary<string, int> wordToCount = await _fileReader.ReadFile(fileName);
            await ClearDictionary();
            var wordEntities = wordToCount.Select((wordAndCount) => new Word() { Count = wordAndCount.Value, Value = wordAndCount.Key })
                                          .ToList();
            await _context.Words.AddRangeAsync(wordEntities);
            _context.SaveChanges();
        }



        public async Task ClearDictionary()
        {
            await _context.Database.ExecuteSqlRawAsync($"TRUNCATE TABLE [{_wordsTableName}]");
            _context.SaveChanges();
        }
    }
}
