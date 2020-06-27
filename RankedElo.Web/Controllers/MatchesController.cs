using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RankedElo.Core.Entities;
using RankedElo.Core.Interfaces;
using RankedElo.Web.Models;

namespace RankedElo.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MatchesController : Controller
    {
        private readonly IMatchService _matchService;
        private readonly IPlayerRepository _playerRepository;

        public MatchesController(IMatchService matchService, IPlayerRepository playerRepository)
        {
            _matchService = matchService ?? throw new ArgumentNullException(nameof(matchService));
            _playerRepository = playerRepository ?? throw new ArgumentNullException(nameof(playerRepository));
        }

        [HttpPost("team")]
        public async Task<IActionResult> AddMatch(TeamMatch match)
        {
            await _matchService.AddMatchAsync<TeamMatch>(match);
            return Ok();
        }

        [HttpPost("soloteam")]
        public async Task<IActionResult> AddMatch(SoloTeamMatch match)
        {
            await _matchService.AddMatchAsync<SoloTeamMatch>(match);
            return Ok();
        }

        [HttpPost("twoplayer")]
        public async Task<IActionResult> AddMatch(TwoPlayerMatchDTO matchDto)
        {
            if(!ModelState.IsValid) 
            {
                return BadRequest();
            }
            var player1 = await _playerRepository.GetPlayerByNameAsync(matchDto.Player1Name) ?? new Player(matchDto.Player1Name);
            var player2 = await _playerRepository.GetPlayerByNameAsync(matchDto.Player2Name) ?? new Player(matchDto.Player2Name);

            var match = new TwoPlayerMatch 
            {
                Player1 = player1,
                Player2 = player2,
                StartTime = matchDto.StartTime,
                EndTime = matchDto.EndTime,
                Team1Score = matchDto.Team1Score,
                Team2Score = matchDto.Team2Score
            };

            var result = await _matchService.AddMatchAsync<TwoPlayerMatch>(match);
            return Ok(result);
        }

    }
}
