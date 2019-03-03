using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RankedElo.Core.Entities
{
    public interface IEloCalculable
    {
        double Team1Elo { get; set; }
        double Team2Elo { get; set; }
        int Team1Score { get; set; }
        int Team2Score { get; set; }
    }

    public class Elo
    {
        public int Id { get; set; }
        public DateTime Timestamp { get; set; }
        public double Points { get; set; } = 1000d;
        public int? PlayerId { get; set; }
        public int? TeamId { get; set; }


        public static void CalculateElo(ref IEloCalculable match, int k = 30)
        {
            var team1Probability = CalculateProbability(match.Team2Elo, match.Team1Elo);
            var team2Probability = CalculateProbability(match.Team1Elo, match.Team2Elo);

            var team1ActualScore = CalculateActualScore(match.Team1Score, match.Team2Score);
            var team2ActualScore = CalculateActualScore(match.Team2Score, match.Team1Score);

            match.Team1Elo = CalculateEloForPlayer(match.Team1Elo, team1Probability, team1ActualScore, k);
            match.Team2Elo = CalculateEloForPlayer(match.Team2Elo, team2Probability, team2ActualScore, k);
        }

        public static void CalculateElo(ref SoloTeamMatch match, int k = 30)
        {
            var team1Probability = CalculateProbability(match.Team2Elo, match.Team1Elo);
            var team2Probability = CalculateProbability(match.Team1Elo, match.Team2Elo);

            var team1ActualScore = CalculateActualScore(match.Team1Score, match.Team2Score);
            var team2ActualScore = CalculateActualScore(match.Team2Score, match.Team1Score);

            match.Team1Players.ToList()
                .ForEach(player => player.CurrentElo = CalculateEloForPlayer(player.CurrentElo, team1Probability, team1ActualScore, k));
            match.Team2Players.ToList()
                .ForEach(player => player.CurrentElo = CalculateEloForPlayer(player.CurrentElo, team2Probability, team2ActualScore, k));
        }

        private static double CalculateProbability(double rating1, double rating2)
        {
            return 1.0d * 1.0d / (1 + 1.0d * Math.Pow(10, 1.0d * (rating1 - rating2) / 400));
        }

        private static float CalculateActualScore(int team1Score, int team2Score)
        {
            if(team1Score == team2Score)
            {
                return 0.5f;
            }

            return team1Score > team2Score ? 1 : 0;
        }

        private static double CalculateEloForPlayer(double currentElo, double probability, float actualScore, int k)
        {
            var elo = currentElo + k * (actualScore - probability);

            if(elo < 0)
            {
                return 0;
            }

            return elo;
        }
    }
}
