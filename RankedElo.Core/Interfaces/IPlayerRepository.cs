﻿using RankedElo.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RankedElo.Core.Interfaces
{
    public interface IPlayerRepository
    {
        Task<Player> GetPlayerByNameAsync(string name);
        Task<IEnumerable<Player>> GetAllPlayersAsync();
        Task<IEnumerable<Player>> GetTopTenAsync();
    }
}
