using System.Threading.Tasks;
using RankedElo.Core.Entities;

namespace RankedElo.Core.Interfaces
{
    public interface ITeamRepository
    {
        Task<Team> GetTeamByNameAsync(string name);
    }
}