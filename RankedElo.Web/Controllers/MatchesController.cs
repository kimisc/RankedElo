﻿using System;
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
            await _matchService.AddMatchAsync(match);
            return Ok();
        }

        [HttpPost("soloteam")]
        public async Task<IActionResult> AddMatch(SoloTeamMatch match)
        {
            await _matchService.AddMatchAsync(match);
            return Ok();
        }

        [HttpPost("twoplayer")]
        public async Task<IActionResult> AddMatch(TwoPlayerMatch match)
        {
            await _matchService.AddMatchAsync(match);
            return Ok();
        }

    }
}
