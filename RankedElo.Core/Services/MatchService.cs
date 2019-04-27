using RankedElo.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using RankedElo.Core.Interfaces;

namespace RankedElo.Core.Services
{
    public class MatchService : IMatchService
    {
        private readonly IMatchRepository _repository;
        public MatchService(IMatchRepository repository) {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<T> AddMatchAsync<T>(IRankedMatch match) where T : IRankedMatch
        {
            match.CalculateElo();
            await _repository.AddMatchAsync(match);
            return (T)match;
        }
    }
}
