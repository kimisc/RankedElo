using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RankedElo.Core.Entities
{
    public class Team
    {
        public int Score { get; set; }
        public IList<Player> Players { get; set; }
        public double Elo => Players.Average(x => x.Elo);
    }
}
