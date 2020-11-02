using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace LyricsCalculator.Common
{
    public static class HttpContentExtension
    {
        public static async Task<T> ReadAsAsync<T>(this HttpContent content)
        {
            var response = await content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<T>(response, new JsonSerializerOptions(){ PropertyNameCaseInsensitive = true});
        }
    }
}
