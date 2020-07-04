using System;
using System.Collections.Generic;
using System.Linq;
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
        public double EloChange { get; set; }

        public void SetEloChange(double oldElo, double newElo)
        {
            EloChange = Math.Abs(oldElo - newElo);
        }
    }

    public class TeamMatch : Match, IRankedMatch
    {
        public Team Team1 { get; set; }
        public Team Team2 { get; set; }

        public void CalculateElo()
        {
            var (team1Elo, team2Elo) = Elo.CalculateElo(Team1.CurrentElo, Team2.CurrentElo, HomeTeamScore, AwayTeamScore);
            SetEloChange(Team1.CurrentElo, team1Elo);
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
            SetEloChange(Player1.CurrentElo, team1Elo);
            Player1.CurrentElo = team1Elo;
            Player2.CurrentElo = team2Elo;
        }
    }

    public class SoloTeamMatch : Match, IRankedMatch
    {
        public IList<SoloTeamPlayer> Players { get; set; }

        public void CalculateElo()
        {
            var allPlayers = Players.ToList();
            var homeElo = allPlayers.Where(x => x.Team == TeamSide.Home).Average(x => x.Player.CurrentElo);
            var awayElo = allPlayers.Where(x => x.Team == TeamSide.Away).Average(x => x.Player.CurrentElo);
            allPlayers.ForEach(player => UpdatePlayer(player, homeElo, awayElo));
        }

        private void UpdatePlayer(SoloTeamPlayer soloTeamPlayer, double homeTeamElo, double awayTeamElo)
        {
            soloTeamPlayer.Match = this;
            var newElo = soloTeamPlayer.Team == TeamSide.Home ?
                Elo.CalculateElo(homeTeamElo, awayTeamElo, HomeTeamScore, AwayTeamScore, soloTeamPlayer.Player.CurrentElo) :
                Elo.CalculateElo(awayTeamElo, homeTeamElo, AwayTeamScore, HomeTeamScore, soloTeamPlayer.Player.CurrentElo);

            SetEloChange(soloTeamPlayer.Player.CurrentElo, newElo);
            soloTeamPlayer.Player.CurrentElo = newElo;
        }
    }
}
