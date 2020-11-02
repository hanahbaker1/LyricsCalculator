using System;
using System.Net.Http;
using System.Threading.Tasks;
using LyricsCalculator.Common;
using LyricsCalculator.Processor.Models;

namespace LyricsCalculator.Processor
{
    public class LyricsOvhClient : ILyricsClient
    {
        private readonly HttpClient _client;

        public LyricsOvhClient(HttpClient client)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public async Task<SongLyrics> GetLyricsByArtistAndTitle(string artist, string songTitle)
        {
            using var response = await _client.GetAsync(BuildRequestUri(artist, songTitle));

            if (!response.IsSuccessStatusCode)
                return null;

            var lyricsContainer = await response.Content.ReadAsAsync<LyricsContainer>();
            return new SongLyrics(songTitle, lyricsContainer.Lyrics);
        }

        private static Uri BuildRequestUri(string artistName, string songTitle)
        {
            var buildRequestUri = new Uri($"{artistName}/{songTitle}", UriKind.Relative);
            return buildRequestUri;
        }
    }
}