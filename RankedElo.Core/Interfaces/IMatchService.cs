using RankedElo.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RankedElo.Core.Interfaces
{
    public interface IMatchService
    {
        Task<TeamMatch> AddMatchAsync(TeamMatch match);
        Task<SoloTeamMatch> AddMatchAsync(SoloTeamMatch match);
        Task<TwoPlayerMatch> AddMatchAsync(TwoPlayerMatch match);
    }
}
