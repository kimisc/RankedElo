using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RankedElo.Core.Entities;
using RankedElo.Core.Interfaces;

namespace RankedElo.Persistence.Repositories
{
    public class TeamRepository : ITeamRepository
    {
        private readonly RankedEloDbContext _context;

        public TeamRepository(RankedEloDbContext context)
        {
            _context = context;
        }
        public async Task<Team> GetTeamByNameAsync(string name)
        {
            return await _context.Teams.FirstOrDefaultAsync(x => x.Name == name);
        }
    }
}