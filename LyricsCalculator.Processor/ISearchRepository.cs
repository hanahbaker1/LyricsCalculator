using System.Threading.Tasks;
using LyricsCalculator.Processor.Models;

namespace LyricsCalculator.Processor
{
    public interface ISearchRepository
    {
        Task<Result> GetLyricsStatisticsAsync(string artistName);
    }
}