using System;
using System.Linq;
using LyricsCalculator.Processor.Models;

namespace LyricsCalculator.Processor.Extensions
{
    public static class StringExtensions
    {
        public static WordCountInfo GetWordCount(this string input)
        {
            char[] delimiters = { ' ', '\r', '\n' };

            var words = input.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
            var distinctWords = words.Distinct(StringComparer.CurrentCultureIgnoreCase).Count();

            return new WordCountInfo
            {
                WordCount = words.Length,
                DistinctWordCount = distinctWords
            };
        }
    }
}