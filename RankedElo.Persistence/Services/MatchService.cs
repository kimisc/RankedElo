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

        public async Task<Match> GetMatchByIdAsync(int id)
        {
            return await _context.Matches
                .Include(x => x.Team1)
                .Include(x => x.Team2)
                .ThenInclude(y => y.Players)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<Match>> GetLatestMatchesAsync(int count)
        {
            return await _context.Matches
                .Include(x => x.Team1)
                .Include(x => x.Team2)
                .ThenInclude(y => y.Players)
                .OrderByDescending(x => x.EndTime)
                .Take(count)
                .ToListAsync();
        }

        public async Task<Match> AddMatchAsync(Match match)
        {
            var team1 = match.Team1;
            var team2 = match.Team2;

            // Add new or track existing players
            var existingTeam1Players = await _playerService.GetExistingPlayersAsync(team1.Players);
            var existingTeam2Players = await _playerService.GetExistingPlayersAsync(team2.Players);

            team1.Players = existingTeam1Players.Any() ? existingTeam1Players : team1.Players;
            team2.Players = existingTeam2Players.Any() ? existingTeam2Players : team2.Players;

            Elo.CalculateElo(ref team1, ref team2);

            match.Team1 = team1;
            match.Team2 = team2;

            await _context.Matches.AddAsync(match);
            await _context.SaveChangesAsync();

            return match;
        }
    }
}
