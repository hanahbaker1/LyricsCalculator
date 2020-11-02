using System.Threading.Tasks;
using LyricsCalculator.Processor.Models;

namespace LyricsCalculator.Processor
{
    public interface ILyricsClient
    {
        Task<SongLyrics> GetLyricsByArtistAndTitle(string artist, string songTitle);
    }
}