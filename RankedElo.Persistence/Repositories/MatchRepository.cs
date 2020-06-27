using System;
using System.Threading.Tasks;
using RankedElo.Core.Interfaces;

namespace RankedElo.Persistence.Repositories
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
