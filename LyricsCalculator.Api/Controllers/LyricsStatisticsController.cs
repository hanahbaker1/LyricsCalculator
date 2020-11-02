using System;
using System.Threading.Tasks;
using LyricsCalculator.Processor;
using LyricsCalculator.Processor.Models;
using Microsoft.AspNetCore.Mvc;

namespace LyricsCalculator.Api.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class LyricsStatisticsController : ControllerBase
    {
        private readonly ISearchRepository _searchRepository;

        public LyricsStatisticsController(ISearchRepository searchRepository)
        {
            _searchRepository = searchRepository ?? throw new ArgumentNullException(nameof(searchRepository));
        }

        [HttpGet]
        [Route(nameof(Search))]
        public async Task<ActionResult<Result>> Search(string artistName)
        {
            if (artistName is null || artistName.Length < 2)
            {
                return BadRequest();
            }

            var results = await _searchRepository.GetLyricsStatisticsAsync(artistName);

            if (results is null)
            {
                return NoContent();
            }

            return Ok(results);
        }
    }
}