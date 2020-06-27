using System;
using System.Collections.Generic;
using System.Text;

namespace RankedElo.Core.Entities
{
    public class SoloTeamPlayer
    {
        public int PlayerId { get; set; }
        public int MatchId { get; set; }
        public Match Match { get; set; }
        public Player Player { get; set; }
        public TeamSide Team { get; set; }
    }

    public enum TeamSide
    {
        Home = 1,
        Away
    }
}
