using Model.WordsServices;
using NUnit.Framework;
using System.IO.Abstractions.TestingHelpers;
using System.Linq;

namespace Model.Tests.WordsServiceTests
{

    public class FileReaderUnitTests
    {
        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public void ReadFile_SimpleTestWithOneWord()
        {
            //Arrange
            var mockFileSystem = new MockFileSystem();
            var mockInputFile = new MockFileData("line1");
            string path = @"C:\temp\in.txt";
            mockFileSystem.AddFile(path, mockInputFile);
            var fileReader = new FileReader(mockFileSystem);

            //Act
            var dictionary = fileReader.ReadFile(path).GetAwaiter().GetResult();
            //Assert
            Assert.That(dictionary.ContainsKey("line1"));
            Assert.That(dictionary.Count == 1);
            Assert.That(dictionary["line1"] == 1);
        }

        [Test]
        public void ReadFile_TestWithDuplicatesWord()
        {
            //Arrange
            var mockFileSystem = new MockFileSystem();
            var mockInputFile = new MockFileData("line1\nline1 line1 \n\rline1");
            string path = @"C:\temp\in.txt";
            mockFileSystem.AddFile(path, mockInputFile);
            var fileReader = new FileReader(mockFileSystem);

            //Act
            var dictionary = fileReader.ReadFile(path).GetAwaiter().GetResult();
            //Assert
            Assert.That(dictionary.ContainsKey("line1"));
            Assert.That(dictionary.Count == 1);
            Assert.That(dictionary["line1"] == 4);
        }

        [Test]
        public void ReadFile_TestWithDifferentWords()
        {
            //Arrange
            var mockFileSystem = new MockFileSystem();
            var mockInputFile = new MockFileData("word1\nword2 load \nкатерина");
            string path = @"C:\temp\in.txt";
            mockFileSystem.AddFile(path, mockInputFile);
            var fileReader = new FileReader(mockFileSystem);

            //Act
            var dictionary = fileReader.ReadFile(path).GetAwaiter().GetResult();
            //Assert
            Assert.That(dictionary.ContainsKey("word1"));
            Assert.That(dictionary.ContainsKey("word2"));
            Assert.That(dictionary.ContainsKey("load"));
            Assert.That(dictionary.ContainsKey("катерина"));

            Assert.That(dictionary.Count == 4);
            Assert.That(dictionary["word1"] == 1);
            Assert.That(dictionary["word2"] == 1);
            Assert.That(dictionary["load"] == 1);
            Assert.That(dictionary["катерина"] == 1);

        }

        [Test]
        public void ReadFile_TestWithDifferentDuplicatesWords()
        {
            //Arrange
            var mockFileSystem = new MockFileSystem();
            var mockInputFile = new MockFileData("word1\n\n\n\nword1 load \nload");
            string path = @"C:\temp\in.txt";
            mockFileSystem.AddFile(path, mockInputFile);
            var fileReader = new FileReader(mockFileSystem);

            //Act
            var dictionary = fileReader.ReadFile(path).GetAwaiter().GetResult();
            //Assert
            Assert.That(dictionary.ContainsKey("word1"));
            Assert.That(dictionary.ContainsKey("load"));

            Assert.That(dictionary.Count == 2);
            Assert.That(dictionary["word1"] == 2);
            Assert.That(dictionary["load"] == 2);
        }

        [Test]
        public void ReadFile_TestWithManyDifferentDuplicatesWords()
        {
            //Arrange
            var mockFileSystem = new MockFileSystem();
            var mockInputFile = new MockFileData(string.Join('\t', Enumerable.Repeat("слово1\n\n\nслово\n", 100000)));
            string path = @"C:\temp\in.txt";
            mockFileSystem.AddFile(path, mockInputFile);
            var fileReader = new FileReader(mockFileSystem);

            //Act
            var dictionary = fileReader.ReadFile(path).GetAwaiter().GetResult();
            //Assert
            Assert.That(dictionary.ContainsKey("слово1"));
            Assert.That(dictionary.ContainsKey("слово"));

            Assert.That(dictionary.Count == 2);
            Assert.That(dictionary["слово1"] == 100000);
            Assert.That(dictionary["слово"] == 100000);
        }
    }
}