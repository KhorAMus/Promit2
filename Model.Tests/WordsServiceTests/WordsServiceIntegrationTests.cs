using Model.WordServices;
using Model.WordsServices;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Tests.WordsServiceTests
{
    class WordsServiceIntegrationTests : TransactionScopeTestsBase
    {
        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public void CreateDictionary()
        {
            // Arrange
            var fileReader = Substitute.For<IFileReader>();
            fileReader.ReadFile("fileName.txt").Returns(new Dictionary<string, int>()
            {
                { "блок", 50 },
                { "блоха", 20 },
                { "блокада", 10 },
                { "катерина", 30 },
                { "катер", 20 },
            });
            var wordsService = new WordsService(GlobalTransactionScopeTestSetUp.Context, fileReader);

            // Act
            wordsService.CreateDictionary("fileName.txt").GetAwaiter().GetResult();
            // Assert
            var addedWords = GlobalTransactionScopeTestSetUp.Context.Words.ToList();
            Assert.That(addedWords.Count == 5);
            Assert.That(addedWords.Any(w => w.Count == 50 && w.Value == "блок"));
            Assert.That(addedWords.Any(w => w.Count == 20 && w.Value == "блоха"));
            Assert.That(addedWords.Any(w => w.Count == 10 && w.Value == "блокада"));
            Assert.That(addedWords.Any(w => w.Count == 30 && w.Value == "катерина"));
            Assert.That(addedWords.Any(w => w.Count == 20 && w.Value == "катер"));
        }
    }
}
