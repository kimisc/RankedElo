using RankedElo.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace RankedElo.Core.Tests
{
    public class TeamTests
    {
        [Fact]
        public void GetCurrentElo_NoEloHistory_ReturnsDefault()
        {
            var team = new Team();
            var result = team.CurrentElo;
            Assert.Equal(1000d, result);
        }

        [Fact]
        public void GetCurrentElo_EloHistoryFound_ReturnsLatestResult()
        {
            var team = new Team
            {
                EloHistory = new List<Elo> {
                       new Elo {
                           Timestamp = DateTime.Now,
                           Points = 1200
                       },
                     new Elo {
                           Timestamp = DateTime.Now.AddSeconds(5),
                           Points = 1230
                       },
                }
            };
            var result = team.CurrentElo;
            Assert.Equal(1230, result);
        }
    }
}
