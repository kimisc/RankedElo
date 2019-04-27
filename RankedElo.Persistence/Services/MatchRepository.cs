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
    public class MatchRepository : IMatchRepository
    {
        private readonly RankedEloDbContext _context;

        public MatchRepository(RankedEloDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task AddMatchAsync(IRankedMatch match)
        {
            await _context.AddAsync(match);
            await _context.SaveChangesAsync();
        }
    }
}
