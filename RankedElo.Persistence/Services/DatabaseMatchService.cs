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
    public class DatabaseMatchService : IMatchService
    {
        private readonly RankedEloDbContext _context;

        public DatabaseMatchService(RankedEloDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task AddMatchAsync(IRankedMatch match)
        {
            // Could be moved above and make this a repo
            match.CalculateElo();

            await _context.AddAsync(match);
            await _context.SaveChangesAsync();
        }
    }
}
