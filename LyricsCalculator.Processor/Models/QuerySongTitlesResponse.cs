using System.Collections.Generic;

namespace LyricsCalculator.Processor.Models
{
    public class QuerySongTitlesResponse
    {
        public IEnumerable<string> SongTitles { get; set; }
    }
}