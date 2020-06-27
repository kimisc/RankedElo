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
    }
}
