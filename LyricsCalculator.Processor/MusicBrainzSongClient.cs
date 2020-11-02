using System.Linq;
using LyricsCalculator.Common;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using LyricsCalculator.Processor.Exceptions;
using LyricsCalculator.Processor.Models;

namespace LyricsCalculator.Processor
{
    public class MusicBrainzSongsClient : ISongsClient
    {
        private readonly HttpClient _client;

        public MusicBrainzSongsClient(HttpClient client)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public (Artist artist, IReadOnlyList<string> songs) GetSongsByArtistAsync(string artistName)
        {
            var artist = GetArtist(artistName).Result;
            if (artist == null)
                throw new ArtistNotFoundException();

            var songTitles = GetSongsByArtistId(artist.Id).Result;

            return (artist, songTitles);
        }

        public async Task<Artist> GetArtist(string artistName)
        {
            using var response = await _client.GetAsync(BuildRequestUri("artist?query", artistName));

            if (!response.IsSuccessStatusCode)
                return null;

            var artistsResponse = await response.Content.ReadAsAsync<ArtistContainer>();
            return artistsResponse.Artists.FirstOrDefault();
        }

        public async Task<IReadOnlyList<string>> GetSongsByArtistId(string artistId)
        {
            using var response = await _client.GetAsync(BuildRequestUri("work?artist", artistId));

            if (!response.IsSuccessStatusCode)
                return null;

            var worksResponse = await response.Content.ReadAsAsync<WorksResponse>().ConfigureAwait(false);

            return worksResponse.Works.Select(work => work.Title).ToList();
        }

        private static Uri BuildRequestUri(string address, string parameter)
        {
            return new Uri($"{address}={parameter}", UriKind.Relative);
        }
    }
}
