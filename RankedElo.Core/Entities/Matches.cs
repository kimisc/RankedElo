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
        public int HomeTeamScore { get; set; }
        public int AwayTeamScore { get; set; }
        // TODO: Calculated elo change for match. + for winning team, - for losing team.
        // public double EloChange { get; set; }
    }

    public class TeamMatch : Match, IRankedMatch
    {
        public Team Team1 { get; set; }
        public Team Team2 { get; set; }

        public void CalculateElo()
        {
            var (team1Elo, team2Elo) = Elo.CalculateElo(Team1.CurrentElo, Team2.CurrentElo, HomeTeamScore, AwayTeamScore);
            Team1.CurrentElo = team1Elo;
            Team2.CurrentElo = team2Elo;
        }
    }

    public class TwoPlayerMatch : Match, IRankedMatch
    {
        public Player Player1 { get; set; }
        public Player Player2 { get; set; }

        public void CalculateElo()
        {
            var (team1Elo, team2Elo) = Elo.CalculateElo(Player1.CurrentElo, Player2.CurrentElo, HomeTeamScore, AwayTeamScore);
            Player1.CurrentElo = team1Elo;
            Player2.CurrentElo = team2Elo;
        }
    }

    public class SoloTeamMatch : Match, IRankedMatch
    {
        public IList<SoloTeamPlayer> Players { get; set; }

        public void CalculateElo()
        {
            var Team1Players = Players.Where(x => x.Team == TeamSide.Home);
            var Team2Players = Players.Where(x => x.Team == TeamSide.Away);
            var team1Elo = Team1Players.Average(x => x.Player.CurrentElo);
            var team2Elo = Team2Players.Average(x => x.Player.CurrentElo);
            Team1Players.ToList()
                .ForEach(soloTeamPlayer =>
                    soloTeamPlayer.Player.CurrentElo = Elo.CalculateElo(team1Elo, team2Elo, HomeTeamScore, AwayTeamScore, soloTeamPlayer.Player.CurrentElo));
            Team2Players.ToList()
                .ForEach(player =>
                    player.Player.CurrentElo = Elo.CalculateElo(team2Elo, team1Elo, AwayTeamScore, HomeTeamScore, player.Player.CurrentElo));
            Players.ToList().ForEach(x => x.Match = this);
        }
    }
}
