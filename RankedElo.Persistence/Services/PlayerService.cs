using Microsoft.EntityFrameworkCore;
using RankedElo.Core.Entities;
using RankedElo.Core.Interfaces;
using RankedElo.Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RankedElo.Persistence.Services
{
    public class PlayerService : IPlayerService
    {
        private readonly RankedEloDbContext _context;

        public PlayerService(RankedEloDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<Player>> GetAllPlayersAsync()
        {
            return await _context.Players.ToListAsync();
        }

        public async Task<Player> GetPlayerByNameAsync(string name)
        {
            return await _context.Players
                .Include(p => p.EloHistory)
                .FirstOrDefaultAsync(x => x.Name == name);
        }

        public async Task<IEnumerable<Player>> GetTopTenAsync()
        {
            return await _context.Players
                .OrderByDescending(x => x.CurrentElo)
                .Take(10)
                .ToListAsync();
        }
    }
}
