using LyricsCalculator.Processor.Extensions;
using NUnit.Framework;

namespace LyricsCalculator.Processor.Tests.Extensions
{
    [TestFixture]
    public class StringExtensionsTests
    {

        [TestCase("Thisisoneword", 1)]
        [TestCase("Two words", 2)]
        [TestCase("", 0)]
        [TestCase("One Two \n Three \r Four", 4)]
        public void WordCount_ShouldReturnCorrectNumberOfWords(string text, int expectedCount)
        {
            var actual = text.GetWordCount();
            Assert.AreEqual(expectedCount, actual.WordCount);
        }

        [TestCase("Thisisoneword", 1)]
        [TestCase("Two words", 2)]
        [TestCase("", 0)]
        [TestCase("One Two \n Three \r Four", 4)]
        [TestCase("Hello hello", 1)]
        [TestCase("BA Ba ba", 1)]
        public void WordCount_ShouldReturnCorrectNumberOfDistinctWords(string text, int expectedCount)
        {
            var actual = text.GetWordCount();
            Assert.AreEqual(expectedCount, actual.DistinctWordCount);
        }

    }
}