using RankedElo.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace RankedElo.Core.Tests
{
    public class PlayerTests
    {
        [Fact]
        public void GetCurrentElo_NoEloHistory_ReturnsDefault()
        {
            var Player = new Player();
            var result = Player.CurrentElo;
            Assert.Equal(1000d, result);
        }

        [Fact]
        public void GetCurrentElo_HasCurrentElo_ReturnsLatestResult()
        {
            var Player = new Player
            {
                CurrentElo = 1230
            };
            Assert.Equal(1230, Player.CurrentElo);
        }
    }
}
