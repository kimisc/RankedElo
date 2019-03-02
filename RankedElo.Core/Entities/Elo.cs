using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RankedElo.Core.Entities
{
    public static class Elo
    {
        public static void CalculateElo(ref Match match)
        {
            var team1 = match.Teams.First();
            var team2 = match.Teams.Last();
            CalculateElo(ref team1, ref team2);
        }

        public static void CalculateElo(ref Team team1, ref Team team2, int k = 30)
        {
            var team1Probability = CalculateProbability(team2.Elo, team1.Elo);
            var team2Probability = CalculateProbability(team1.Elo, team2.Elo);

            var team1ActualScore = CalculateActualScore(team1.Score, team2.Score);
            var team2ActualScore = CalculateActualScore(team2.Score, team1.Score);

            team1.Players.ToList()
                .ForEach(player => player.Elo = CalculateEloForPlayer(player.Elo, team1Probability, team1ActualScore, k));
            team2.Players.ToList()
                .ForEach(player => player.Elo = CalculateEloForPlayer(player.Elo, team2Probability, team2ActualScore, k));
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
