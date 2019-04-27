using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RankedElo.Core.Entities
{
    public class Match
    {
        public int Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int Team1Score { get; set; }
        public int Team2Score { get; set; }
    }

    public class TeamMatch : Match, IEloCalculable
    {
        public Team Team1 { get; set; }
        public Team Team2 { get; set; }

        public void CalculateElo()
        {
            var team1Probability = Elo.CalculateProbability(Team2.CurrentElo, Team1.CurrentElo);
            var team2Probability = Elo.CalculateProbability(Team1.CurrentElo, Team2.CurrentElo);

            var team1ActualScore = Elo.CalculateActualScore(Team1Score, Team2Score);
            var team2ActualScore = Elo.CalculateActualScore(Team2Score, Team1Score);

            Team1.CurrentElo = Elo.CalculateEloForSingleParticipant(Team1.CurrentElo, team1Probability, team1ActualScore);
            Team2.CurrentElo = Elo.CalculateEloForSingleParticipant(Team2.CurrentElo, team2Probability, team2ActualScore);
        }
    }

    public class TwoPlayerMatch : Match, IEloCalculable
    {
        public Player Player1 { get; set; }
        public Player Player2 { get; set; }

        public void CalculateElo()
        {
            var team1Probability = Elo.CalculateProbability(Player2.CurrentElo, Player1.CurrentElo);
            var team2Probability = Elo.CalculateProbability(Player1.CurrentElo, Player2.CurrentElo);

            var team1ActualScore = Elo.CalculateActualScore(Team1Score, Team2Score);
            var team2ActualScore = Elo.CalculateActualScore(Team2Score, Team1Score);

            Player1.CurrentElo = Elo.CalculateEloForSingleParticipant(Player1.CurrentElo, team1Probability, team1ActualScore);
            Player2.CurrentElo = Elo.CalculateEloForSingleParticipant(Player2.CurrentElo, team2Probability, team2ActualScore);
        }
    }

    public class SoloTeamMatch : Match, IEloCalculable
    {
        public IList<Player> Team1Players { get; set; }
        public IList<Player> Team2Players { get; set; }
        public double Team1Elo => Team1Players.Average(x => x.CurrentElo);
        public double Team2Elo => Team2Players.Average(x => x.CurrentElo);

        public void CalculateElo()
        {
            var team1Probability = Elo.CalculateProbability(Team2Elo, Team1Elo);
            var team2Probability = Elo.CalculateProbability(Team1Elo, Team2Elo);

            var team1ActualScore = Elo.CalculateActualScore(Team1Score, Team2Score);
            var team2ActualScore = Elo.CalculateActualScore(Team2Score, Team1Score);

            Team1Players.ToList()
                .ForEach(player => player.CurrentElo = Elo.CalculateEloForSingleParticipant(player.CurrentElo, team1Probability, team1ActualScore));
            Team2Players.ToList()
                .ForEach(player => player.CurrentElo = Elo.CalculateEloForSingleParticipant(player.CurrentElo, team2Probability, team2ActualScore));
        }
    }
}
