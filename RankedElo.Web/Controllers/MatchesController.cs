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

        [HttpPost("team")]
        public async Task<IActionResult> AddMatch(TeamMatch match)
        {
            return Ok(await _matchService.AddMatchAsync(match));
        }

        [HttpPost("soloteam")]
        public async Task<IActionResult> AddMatch(SoloTeamMatch match)
        {
            return Ok(await _matchService.AddMatchAsync(match));
        }

        [HttpPost("twoplayer")]
        public async Task<IActionResult> AddMatch(TwoPlayerMatch match)
        {
            return Ok(await _matchService.AddMatchAsync(match));
        }

    }
}
