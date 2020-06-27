using System;

namespace RankedElo.Core.Entities
{
    public class Elo
    {
        public static double DefaultPoints = 1000d;
        public int Id { get; set; }
        public DateTime Timestamp { get; set; }
        public double Points { get; set; } = DefaultPoints;
        public int? PlayerId { get; set; }
        public int? TeamId { get; set; }

        public Elo() {}

        public Elo(double elo) {
            Points = elo;
            Timestamp = DateTime.UtcNow;
        }

        public static (double team1elo, double team2elo) CalculateElo(double team1Elo, double team2Elo, int team1score, int team2score)
        {
            var team1Probability = CalculateProbability(team2Elo, team1Elo);
            var team2Probability = CalculateProbability(team1Elo, team2Elo);

            var team1ActualScore = CalculateActualScore(team1score, team2score);
            var team2ActualScore = CalculateActualScore(team2score, team1score);
            var team1Result = CalculateEloForSingleParticipant(team1Elo, team1Probability, team1ActualScore);
            var team2Result = CalculateEloForSingleParticipant(team2Elo, team2Probability, team2ActualScore);
            return (team1Result, team2Result);
        }

        public static double CalculateElo(double team1Elo, double team2Elo, int team1Score, int team2Score, double playerElo)
        {
            var team1Probability = CalculateProbability(team2Elo, team1Elo);
            var team1ActualScore = CalculateActualScore(team1Score, team2Score);
            return CalculateEloForSingleParticipant(playerElo, team1Probability, team1ActualScore);
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

        private static double CalculateEloForSingleParticipant(double currentElo, double probability, float actualScore, int k = 30)
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
