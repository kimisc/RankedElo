using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RankedElo.Core.Interfaces;

namespace RankedElo.Core.Entities
{
    public abstract class Match
    {
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
            var teamEloPoints = Elo.CalculateElo(Team1.CurrentElo, Team2.CurrentElo, Team1Score, Team2Score);
            Team1.CurrentElo = teamEloPoints.team1elo;
            Team2.CurrentElo = teamEloPoints.team2elo;
        }
    }

    public class TwoPlayerMatch : Match, IRankedMatch
    {
        public Player Player1 { get; set; }
        public Player Player2 { get; set; }

        public void CalculateElo()
        {
            var teamEloPoints = Elo.CalculateElo(Player1.CurrentElo, Player2.CurrentElo, Team1Score, Team2Score);
            Player1.CurrentElo = teamEloPoints.team1elo;
            Player2.CurrentElo = teamEloPoints.team2elo;
        }
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
    }
}
