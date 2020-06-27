using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RankedElo.Core.Interfaces;

namespace RankedElo.Core.Entities
{
    public abstract class Match
    {
        public abstract long Discriminator { get; }
        public int Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int Team1Score { get; set; }
        public int Team2Score { get; set; }
    }

    public class TeamMatch : Match, IRankedMatch
    {
        public Team Team1 { get; set; }
        public Team Team2 { get; set; }

        public void CalculateElo()
        {
            var (team1Elo, team2Elo) = Elo.CalculateElo(Team1.CurrentElo, Team2.CurrentElo, Team1Score, Team2Score);
            Team1.CurrentElo = team1Elo;
            Team2.CurrentElo = team2Elo;
        }

        public override long Discriminator => 2;
    }

    public class TwoPlayerMatch : Match, IRankedMatch
    {
        public Player Player1 { get; set; }
        public Player Player2 { get; set; }

        public void CalculateElo()
        {
            var (team1Elo, team2Elo) = Elo.CalculateElo(Player1.CurrentElo, Player2.CurrentElo, Team1Score, Team2Score);
            Player1.CurrentElo = team1Elo;
            Player2.CurrentElo = team2Elo;
        }

        public override long Discriminator => 1;
    }

    public class SoloTeamMatch : Match, IRankedMatch
    {
        public IList<Player> Team1Players { get; set; }
        public IList<Player> Team2Players { get; set; }

        public void CalculateElo()
        {
            var team1Elo = Team1Players.Average(x => x.CurrentElo); ;
            var team2Elo = Team2Players.Average(x => x.CurrentElo); ;
            Team1Players.ToList()
                .ForEach(player =>
                    player.CurrentElo = Elo.CalculateElo(team1Elo, team2Elo, Team1Score, Team2Score, player.CurrentElo));
            Team2Players.ToList()
                .ForEach(player =>
                    player.CurrentElo = Elo.CalculateElo(team2Elo, team1Elo, Team2Score, Team1Score, player.CurrentElo));
        }

        public override long Discriminator => 3;
    }
}
