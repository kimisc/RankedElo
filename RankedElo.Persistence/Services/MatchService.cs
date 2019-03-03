using Microsoft.EntityFrameworkCore;
using RankedElo.Core.Entities;
using RankedElo.Core.Interfaces;
using RankedElo.Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RankedElo.Persistence.Services
{
    public class MatchService : IMatchService
    {
        private readonly RankedEloDbContext _context;
        private readonly IPlayerService _playerService;

        public MatchService(RankedEloDbContext context, IPlayerService playerService)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _playerService = playerService ?? throw new ArgumentNullException(nameof(playerService));
        }

        public async Task<SoloTeamMatch> AddMatchAsync(SoloTeamMatch match)
        {
            var team1 = await GetExistingPlayersIfFound(match.Team1Players);
            var team2 = await GetExistingPlayersIfFound(match.Team2Players);

            Elo.CalculateElo(ref match);

            match.Team1Players = team1;
            match.Team2Players = team2;

            foreach (var player in match.Team1Players)
            {
                await AddEloHistoryForPlayers(player);
                await _context.AddAsync(new PlayerSoloTeamMatches
                {
                    Player = player,
                    SoloTeamMatch = match
                });
            }

            foreach (var player in match.Team2Players)
            {
                await AddEloHistoryForPlayers(player);
                await _context.AddAsync(new PlayerSoloTeamMatches
                {
                    Player = player,
                    SoloTeamMatch = match
                });
            }

            await _context.SoloTeamMatches.AddAsync(match);
            await _context.SaveChangesAsync();

            return match;
        }

        public async Task<TeamMatch> AddMatchAsync(TeamMatch match)
        {
            match.Team1 = _context.Teams.FirstOrDefault(x => x.Name == match.Team1.Name) ?? match.Team1;
            match.Team2 = _context.Teams.FirstOrDefault(x => x.Name == match.Team2.Name) ?? match.Team2;

            var castedMatch = (IEloCalculable)match;
            Elo.CalculateElo(ref castedMatch);
            match = (TeamMatch)castedMatch;

            await AddEloHistoryForTeam(match.Team1, match.Team2);

            await _context.AddAsync(match);
            await _context.SaveChangesAsync();

            return match;
        }

        public async Task<TwoPlayerMatch> AddMatchAsync(TwoPlayerMatch match)
        {
            match.Player1 = await _playerService.GetPlayerByNameAsync(match.Player1.Name) ?? match.Player1;
            match.Player2 = await _playerService.GetPlayerByNameAsync(match.Player2.Name) ?? match.Player2;

            var castedMatch = (IEloCalculable)match;
            Elo.CalculateElo(ref castedMatch);
            match = (TwoPlayerMatch)castedMatch;

            await _context.AddAsync(match);
            await AddEloHistoryForPlayers(match.Player1, match.Player2);
            await _context.SaveChangesAsync();

            return match;
        }

        private async Task<IList<Player>> GetExistingPlayersIfFound(IList<Player> players)
        {
            for (var i = 0; i < players.Count; i++)
            {
                var currentPlayer = players[i];
                currentPlayer = await _playerService.GetPlayerByNameAsync(currentPlayer.Name) ?? currentPlayer;
                players[i] = currentPlayer;
            }

            return players;
        }

        private async Task AddEloHistoryForTeam(params Team[] teams)
        {
            foreach (var team in teams)
            {
                if (team.Id != 0)
                {
                    await _context.EloHistory.AddAsync(new Elo
                    {
                        TeamId = team.Id,
                        Points = team.CurrentElo,
                        Timestamp = DateTime.UtcNow
                    });
                }
            }
        }

        private async Task AddEloHistoryForPlayers(params Player[] players)
        {
            foreach (var player in players)
            {
                if (player.Id != 0)
                {
                    await _context.EloHistory.AddAsync(new Elo
                    {
                        PlayerId = player.Id,
                        Points = player.CurrentElo,
                        Timestamp = DateTime.UtcNow
                    });
                }
            }
        }
    }
}
