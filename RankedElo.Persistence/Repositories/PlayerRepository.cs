using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RankedElo.Core.Entities;
using RankedElo.Core.Interfaces;

namespace RankedElo.Persistence.Repositories
{
    public class PlayerRepository : IPlayerRepository
    {
        private readonly RankedEloDbContext _context;

        public PlayerRepository(RankedEloDbContext context)
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
                .FirstOrDefaultAsync(x => x.Name == name);
        }

        public async Task<IEnumerable<Player>> GetTopTenAsync()
        {
            // TODO: Loads all players to sort on client side, degrades performance
            var allPlayers = await _context.Players
                .AsNoTracking()
                .ToListAsync();

            return allPlayers
                .OrderByDescending(x => x.CurrentElo)
                .Take(10);
        }
    }
}
