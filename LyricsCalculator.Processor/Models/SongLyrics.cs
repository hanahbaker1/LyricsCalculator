using System;
using LyricsCalculator.Processor.Extensions;

namespace LyricsCalculator.Processor.Models
{
    public class SongLyrics : IComparable<SongLyrics>
    {
        public SongLyrics(string title, string lyrics)
        {
            Title = title;
            Lyrics = lyrics;
            WordCountGrouping = Lyrics.GetWordCount();
        }

        public string Title { get; }

        public string Lyrics { get; }

        public WordCountInfo WordCountGrouping { get; }
        public int CompareTo(SongLyrics other)
        {
            if (ReferenceEquals(this, other)) return 0;
            if (ReferenceEquals(null, other)) return 1;
            var titleComparison = string.Compare(Title, other.Title, StringComparison.Ordinal);
            if (titleComparison != 0) return titleComparison;
            return WordCountGrouping.WordCount.CompareTo(other.WordCountGrouping.WordCount);
        }
    }
}