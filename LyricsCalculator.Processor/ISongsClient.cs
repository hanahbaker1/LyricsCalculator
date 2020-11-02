using System.Collections.Generic;
using LyricsCalculator.Processor.Models;

namespace LyricsCalculator.Processor
{
    public interface ISongsClient
    {
        (Artist artist, IReadOnlyList<string> songs) GetSongsByArtistAsync(string artistName);
    }
}