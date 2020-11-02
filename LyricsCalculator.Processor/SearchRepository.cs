using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LyricsCalculator.Processor.Models;

namespace LyricsCalculator.Processor
{
    public class SearchRepository : ISearchRepository
    {
        private readonly ISongsClient _songClient;
        private readonly ILyricsClient _lyricsClient;

        public SearchRepository(ISongsClient songClient, ILyricsClient lyricsClient)
        {
            _songClient = songClient ?? throw new ArgumentNullException(nameof(songClient));
            _lyricsClient = lyricsClient ?? throw new ArgumentNullException(nameof(lyricsClient));
        }

        public async Task<Result> GetLyricsStatisticsAsync(string artistName)
        {
            var songLyrics = new List<SongLyrics>();
            var lyricsResult = await GetLyricsAsync(artistName);
            for (var index = 0; index < lyricsResult.Count; index++)
            {
                var item = lyricsResult[index];
                if (item != null) songLyrics.Add(item);
            }

            if (songLyrics.Count > 0)
            {
                return new Result
                {
                    Artist = artistName,
                    SongsAnalysed = lyricsResult.Count,
                    AverageWords = Math.Round(songLyrics.Average(x => x.WordCountGrouping.WordCount), 2),
                    AverageDistinctWords = Math.Round(songLyrics.Average(x => x.WordCountGrouping.DistinctWordCount), 2),
                    SongWithMostWords = songLyrics.Max(),
                    SongWithFewestWords = songLyrics.Min()
                };
            }

            return null;
        }

        private async Task<List<SongLyrics>> GetLyricsAsync(string artistName)
        {
            var songsByArtistAsync = _songClient.GetSongsByArtistAsync(artistName);

            var lyricsList = new List<SongLyrics>();

            foreach (var song in songsByArtistAsync.songs)
            {
                var lyricsByArtistAndTitle = await _lyricsClient.GetLyricsByArtistAndTitle(artistName, song);
                lyricsList.Add(lyricsByArtistAndTitle);
            }

            return lyricsList;
        }
    }
}
