using Microsoft.EntityFrameworkCore;
using RankedElo.Core.Entities;
using RankedElo.Core.Interfaces;
using RankedElo.Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RankedElo.Persistence.Services
{
    public class MatchRepository : IRepository
    {
        private readonly RankedEloDbContext _context;

        public MatchRepository(RankedEloDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<T> GetById<T>(int id) where T : BaseEntity
        {
            return await _context.Set<T>().SingleOrDefaultAsync(e => e.Id == id);
        }

        public async Task<List<T>> List<T>() where T : BaseEntity
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<T> Add<T>(T entity) where T : BaseEntity
        {
            _context.Set<T>().Add(entity);
            await _context.SaveChangesAsync();

            return entity;
        }

        public async Task Delete<T>(T entity) where T : BaseEntity
        {
            _context.Set<T>().Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task Update<T>(T entity) where T : BaseEntity
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}
