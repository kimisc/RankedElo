using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RankedElo.Core.Entities
{
    public abstract class MatchBase
    {
        public int Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int Team1Score { get; set; }
        public int Team2Score { get; set; }
    }

    public class TeamMatch : MatchBase, IEloCalculable
    {
        public RankedTeam Team1 { get; set; }
        public RankedTeam Team2 { get; set; }
        public double Team1Elo
        {
            get => Team1.CurrentElo;
            set => Team1.EloHistory.Add(new Elo
            {
                Team = Team1,
                Points = value,
                Timestamp = DateTime.UtcNow
            });
        }
        public double Team2Elo
        {
            get => Team2.CurrentElo;
            set => Team2.EloHistory.Add(new Elo
            {
                Team = Team2,
                Points = value,
                Timestamp = DateTime.UtcNow
            });
        }
    }

    public class TwoPlayerMatch : MatchBase, IEloCalculable
    {
        public Player Player1 { get; set; }
        public Player Player2 { get; set; }
        public double Team1Elo
        {
            get => Player1.CurrentElo;
            set => Player1.EloHistory.Add(new Elo
            {
                Player = Player1,
                Points = value,
                Timestamp = DateTime.UtcNow
            });
        }
        public double Team2Elo
        {
            get => Player2.CurrentElo;
            set => Player2.EloHistory.Add(new Elo
            {
                Player = Player2,
                Points = value,
                Timestamp = DateTime.UtcNow
            });
        }
    }

    public class SoloTeamMatch : MatchBase
    {
        public IList<Player> Team1Players { get; set; }
        public IList<Player> Team2Players { get; set; }
        public double Team1Elo => Team1Players.Average(x => x.CurrentElo);
        public double Team2Elo => Team2Players.Average(x => x.CurrentElo);
    }
}
