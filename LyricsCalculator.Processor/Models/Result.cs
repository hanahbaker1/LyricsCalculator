namespace LyricsCalculator.Processor.Models
{
    public class Result
    {
        public string Artist { get; set; }

        public int SongsAnalysed { get; set; }

        public double AverageWords { get; set; }

        public double AverageDistinctWords { get; set; }

        public SongLyrics SongWithMostWords { get; set; }

        public SongLyrics SongWithFewestWords { get; set; }
    }
}