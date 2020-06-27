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
        public async Task<IActionResult> AddMatch(SoloTeamBaseMatchDto matchDto)
        {
            var playerTasks = matchDto.Players.Select(async x => new SoloTeamPlayer {
               Player = await GetOrCreatePlayer(x.Name),
               Team = x.Team
            });
            var soloTeamPlayers = await Task.WhenAll(playerTasks);

            var match = new SoloTeamMatch()
            {
                EndTime = matchDto.EndTime,
                StartTime = matchDto.StartTime,
                Team1Score = matchDto.Team1Score,
                Team2Score = matchDto.Team2Score,
                Players = soloTeamPlayers
            };
            var result = await _matchService.AddMatchAsync<SoloTeamMatch>(match);
            return Ok(result);
        }

        [HttpPost("twoplayer")]
        public async Task<IActionResult> AddMatch(TwoPlayerMatchDto matchDto)
        {
            if(!ModelState.IsValid) 
            {
                return BadRequest();
            }
            var player1 = await GetOrCreatePlayer(matchDto.Player1Name);
            var player2 = await GetOrCreatePlayer(matchDto.Player2Name);

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

        private async Task<Player> GetOrCreatePlayer(string name)
        {
            return await _playerRepository.GetPlayerByNameAsync(name) ?? new Player(name);
        }

    }
}
