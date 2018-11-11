using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RankedElo.Core.Entities
{
    public static class Elo
    {

        public static void CalculateElo(ref List<Player> team1, ref List<Player> team2, bool team1Wins, int k = 30)
        {
            var team1Elo = team1.Average(x => x.Elo);
            var team2Elo = team2.Average(x => x.Elo);

            var team1Probability = CalculateProbability(team2Elo, team1Elo);
            var team2Probability = CalculateProbability(team1Elo, team2Elo);

            var team1ActualScore = team1Wins ? 1 : 0;
            var team2ActualScore = team1Wins ? 0 : 1;

            team1.ForEach(player => player.Elo = CalculateEloForPlayer(player.Elo, team1Probability, team1ActualScore, k));
            team2.ForEach(player => player.Elo = CalculateEloForPlayer(player.Elo, team2Probability, team2ActualScore, k));
        }

        private static double CalculateEloForPlayer(double currentElo, double probability, int actualScore, int k)
        {
            var elo = currentElo + k * (actualScore - probability);

            if(elo < 0)
            {
                return 0;
            }

            return elo;
        }

        private static double CalculateProbability(double rating1, double rating2)
        {
            return 1.0d * 1.0d / (1 + 1.0d * Math.Pow(10, 1.0d * (rating1 - rating2) / 400));
        }
    }
}
