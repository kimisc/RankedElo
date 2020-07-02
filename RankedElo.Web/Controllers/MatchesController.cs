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
        private readonly ITeamRepository _teamRepository;

        public MatchesController(IMatchService matchService, IPlayerRepository playerRepository, ITeamRepository teamRepository)
        {
            _matchService = matchService ?? throw new ArgumentNullException(nameof(matchService));
            _playerRepository = playerRepository ?? throw new ArgumentNullException(nameof(playerRepository));
            _teamRepository = teamRepository ?? throw new ArgumentNullException(nameof(teamRepository));
        }

        [HttpPost("team")]
        public async Task<IActionResult> AddTeamMatch(TwoTeamMatchDto matchDto)
        {
            var match = new TeamMatch
            {
                StartTime = matchDto.StartTime,
                EndTime = matchDto.EndTime,
                HomeTeamScore = matchDto.Team1Score,
                AwayTeamScore = matchDto.Team2Score,
                Team1 = await GetOrCreateTeam(matchDto.HomeTeamName),
                Team2 = await GetOrCreateTeam(matchDto.AwayTeamName)
            };
            var result = await _matchService.AddMatchAsync<TeamMatch>(match);
            return Ok(result);
        }

        [HttpPost("soloteam")]
        public async Task<IActionResult> AddMatch(SoloTeamMatchDto matchDto)
        {
            var soloTeamPlayers = new List<SoloTeamPlayer>();
            foreach(var playerDto in matchDto.Players) {
                var player = await GetOrCreatePlayer(playerDto.Name);
                soloTeamPlayers.Add(new SoloTeamPlayer {
                    Team = playerDto.Team,
                    Player = player
                });
            }

            var match = new SoloTeamMatch()
            {
                EndTime = matchDto.EndTime,
                StartTime = matchDto.StartTime,
                HomeTeamScore = matchDto.Team1Score,
                AwayTeamScore = matchDto.Team2Score,
                Players = soloTeamPlayers
            };
            var result = await _matchService.AddMatchAsync<SoloTeamMatch>(match);
            return Ok(result);
        }

        [HttpPost("twoplayer")]
        public async Task<IActionResult> AddTwoPlayerMatch(TwoTeamMatchDto matchDto)
        {
            var player1 = await GetOrCreatePlayer(matchDto.HomeTeamName);
            var player2 = await GetOrCreatePlayer(matchDto.AwayTeamName);

            var match = new TwoPlayerMatch 
            {
                Player1 = player1,
                Player2 = player2,
                StartTime = matchDto.StartTime,
                EndTime = matchDto.EndTime,
                HomeTeamScore = matchDto.Team1Score,
                AwayTeamScore = matchDto.Team2Score
            };

            var result = await _matchService.AddMatchAsync<TwoPlayerMatch>(match);
            return Ok(result);
        }

        private async Task<Player> GetOrCreatePlayer(string name)
        {
            return await _playerRepository.GetPlayerByNameAsync(name) ?? new Player(name);
        }

        private async Task<Team> GetOrCreateTeam(string name)
        {
            return await _teamRepository.GetTeamByNameAsync(name) ?? new Team(name);
        }

    }
}
