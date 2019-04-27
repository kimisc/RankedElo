using System.Threading.Tasks;
using RankedElo.Core.Entities;

namespace RankedElo.Core.Interfaces
{
    public interface IMatchRepository
    {
        Task AddMatchAsync(IRankedMatch match);
    }
}