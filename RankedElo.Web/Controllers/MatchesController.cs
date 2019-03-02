using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RankedElo.Core.Entities;
using RankedElo.Core.Interfaces;

namespace RankedElo.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MatchesController : ControllerBase
    {
        private readonly IMatchService _matchService;

        public MatchesController(IMatchService matchService)
        {
            _matchService = matchService ?? throw new ArgumentNullException(nameof(matchService));
        }

        [HttpGet("latest/{count}")]
        public async Task<IActionResult> GetLatest(int count)
        {
            return Ok(await _matchService.GetLatestMatchesAsync(count));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetMatch(int id)
        {
            var match = await _matchService.GetMatchByIdAsync(id);

            if(match is null)
            {
                return NotFound();
            }

            return Ok(match);
        }

        [HttpPost]
        public async Task<IActionResult> AddMatch(Match match)
        {
            return Ok(await _matchService.AddMatchAsync(match));
        }

    }
}
