using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RankedElo.Core.Entities
{
    public interface IRankedMatch
    {
        void CalculateElo();
    }

    public class Elo
    {
        public int Id { get; set; }
        public DateTime Timestamp { get; set; }
        public double Points { get; set; } = 1000d;
        public int? PlayerId { get; set; }
        public int? TeamId { get; set; }

        public Elo() {}

        public Elo(double elo) {
            Points = elo;
            Timestamp = DateTime.UtcNow;
        }
        public static double CalculateProbability(double rating1, double rating2)
        {
            return 1.0d * 1.0d / (1 + 1.0d * Math.Pow(10, 1.0d * (rating1 - rating2) / 400));
        }

        public static float CalculateActualScore(int team1Score, int team2Score)
        {
            if(team1Score == team2Score)
            {
                return 0.5f;
            }

            return team1Score > team2Score ? 1 : 0;
        }

        public static double CalculateEloForSingleParticipant(double currentElo, double probability, float actualScore, int k = 30)
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
