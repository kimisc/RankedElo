using System;
using System.Collections.Generic;
using System.Text;

namespace RankedElo.Core.Entities
{
    // Many to many mapping for soloteam matches and players
    public class PlayerSoloTeamMatches
    {
        public int PlayerId { get; set; }
        public Player Player { get; set; }
        public int SoloTeamMatchId { get; set; }
        public SoloTeamMatch SoloTeamMatch { get; set; }
    }
}
