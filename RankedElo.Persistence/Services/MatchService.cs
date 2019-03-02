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

        public MatchService(RankedEloDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<Match> GetMatchByIdAsync(int id)
        {
            return await _context.Matches
                .Include(x => x.Teams)
                .ThenInclude(y => y.Players)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<Match>> GetLatestMatchesAsync(int count)
        {
            return await _context.Matches
                .Include(x => x.Teams)
                .ThenInclude(y => y.Players)
                .OrderByDescending(x => x.EndTime)
                .Take(count)
                .ToListAsync();
        }

        public async Task<Match> AddMatchAsync(Match match)
        {
            Elo.CalculateElo(ref match);
            _context.Add(match);
            await _context.SaveChangesAsync();

            return match;
        }
    }
}
