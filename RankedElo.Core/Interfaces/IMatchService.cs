using RankedElo.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RankedElo.Core.Interfaces
{
    public interface IMatchService
    {
        Task AddMatchAsync(IEloCalculable match);                                                                                                                                                                                                                  
    }
}
