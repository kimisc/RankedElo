using RankedElo.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace RankedElo.Core.Tests
{
    public class EloTests
    {

        [Fact]
        public void CalculateElo_TwoPlayersTeam1Wins_EloUpdated()
        {
            IEloCalculable match = CreateTwoPlayerMatch(1200, 1000);
            match.Team1Score = 1;
            match.Team2Score = 0;

            Elo.CalculateElo(ref match);

            var player1 = (match as TwoPlayerMatch).Player1;
            var player2 = (match as TwoPlayerMatch).Player2;

            Assert.Equal("Player 1", player1.Name);
            Assert.Equal(1207.2, player1.CurrentElo, 1);
            Assert.Equal("Player 2", player2.Name);
            Assert.Equal(992.8, player2.CurrentElo, 1);
        }

        [Fact]
        public void CalculateElo_TwoPlayersTeam2Wins_EloUpdated()
        {
            IEloCalculable match = CreateTwoPlayerMatch(1200, 1000);
            match.Team1Score = 0;
            match.Team2Score = 1;

            Elo.CalculateElo(ref match);

            var player1 = (match as TwoPlayerMatch).Player1;
            var player2 = (match as TwoPlayerMatch).Player2;

            Assert.Equal("Player 1", player1.Name);
            Assert.Equal(1177.2, player1.CurrentElo, 1);
            Assert.Equal("Player 2", player2.Name);
            Assert.Equal(1022.8, player2.CurrentElo, 1);
        }

        [Fact]
        public void CalculateElo_RankedTeamTeam1Wins_EloUpdated()
        {
            IEloCalculable match = CreateTeamMatch(1200, 1000);
            match.Team1Score = 1;
            match.Team2Score = 0;

            Elo.CalculateElo(ref match);

            var team1 = (match as TeamMatch).Team1;
            var team2 = (match as TeamMatch).Team2;

            Assert.Equal("Team 1", team1.Name);
            Assert.Equal(1207.2, team1.CurrentElo, 1);
            Assert.Equal("Team 2", team2.Name);
            Assert.Equal(992.8, team2.CurrentElo, 1);
        }

        [Fact]
        public void CalculateElo_RankedTeamTeam2Wins_EloUpdated()
        {
            IEloCalculable match = CreateTeamMatch(1200, 1000);
            match.Team1Score = 0;
            match.Team2Score = 1;

            Elo.CalculateElo(ref match);

            var team1 = (match as TeamMatch).Team1;
            var team2 = (match as TeamMatch).Team2;

            Assert.Equal("Team 1", team1.Name);
            Assert.Equal(1177.2, team1.CurrentElo, 1);
            Assert.Equal("Team 2", team2.Name);
            Assert.Equal(1022.8, team2.CurrentElo, 1);
        }

        [Fact]
        public void CalculateElo_MultiplePlayersTeam1Wins_TeamAverageUsed()
        {
            var match = CreateSoloTeamMatch(1000, 1000);
            match.Team1Players.Add(new Player
            {
                Name = "Player 3",
                CurrentElo = 900
            });
            match.Team2Players.Add(new Player
            {
                Name = "Player 4",
                CurrentElo = 1200
            });
            match.Team1Score = 1;
            match.Team2Score = 0;


            Elo.CalculateElo(ref match);

            var t1_player1 = match.Team1Players.First();
            var t1_player2 = match.Team1Players.Last();
            var t2_player1 = match.Team2Players.First();
            var t2_player2 = match.Team2Players.Last();

            Assert.Equal("Player 1", t1_player1.Name);
            Assert.Equal(1021.1, t1_player1.CurrentElo, 1);
            Assert.Equal("Player 3", t1_player2.Name);
            Assert.Equal(921.1, t1_player2.CurrentElo, 1);
            Assert.Equal("Player 2", t2_player1.Name);
            Assert.Equal(978.9, t2_player1.CurrentElo, 1);
            Assert.Equal("Player 4", t2_player2.Name);
            Assert.Equal(1178.9, t2_player2.CurrentElo, 1);
        }

        [Fact]
        public void CalculateElo_MultiplePlayersTeam2Wins_TeamAverageUsed()
        {
            var match = CreateSoloTeamMatch(1000, 1000);
            match.Team1Players.Add(new Player
            {
                Name = "Player 3",
                CurrentElo = 900
            });
            match.Team2Players.Add(new Player
            {
                Name = "Player 4",
                CurrentElo = 1200
            });
            match.Team1Score = 0;
            match.Team2Score = 1;


            Elo.CalculateElo(ref match);

            var t1_player1 = match.Team1Players.First();
            var t1_player2 = match.Team1Players.Last();
            var t2_player1 = match.Team2Players.First();
            var t2_player2 = match.Team2Players.Last();

            Assert.Equal("Player 1", t1_player1.Name);
            Assert.Equal(991.1, t1_player1.CurrentElo, 1);
            Assert.Equal("Player 3", t1_player2.Name);
            Assert.Equal(891.1, t1_player2.CurrentElo, 1);
            Assert.Equal("Player 2", t2_player1.Name);
            Assert.Equal(1008.9, t2_player1.CurrentElo, 1);
            Assert.Equal("Player 4", t2_player2.Name);
            Assert.Equal(1208.9, t2_player2.CurrentElo, 1);
        }

        [Fact]
        public void CalculateElo_EloChangeBelowZero_ReturnsZero()
        {
            IEloCalculable match = CreateTwoPlayerMatch(0, 1000);
            match.Team1Score = 0;
            match.Team2Score = 1;

            Elo.CalculateElo(ref match);

            var player1 = (match as TwoPlayerMatch).Player1;
            var player2 = (match as TwoPlayerMatch).Player2;

            Assert.Equal("Player 1", player1.Name);
            Assert.Equal(0, player1.CurrentElo);
        }

        [Fact]
        public void CalculateElo_TieGameWithSameElo_EloNotChanged()
        {
            IEloCalculable match = CreateTwoPlayerMatch(1000, 1000);
            match.Team1Score = 0;
            match.Team2Score = 0;

            Elo.CalculateElo(ref match);

            var player1 = (match as TwoPlayerMatch).Player1;
            var player2 = (match as TwoPlayerMatch).Player2;

            Assert.Equal(1000, player1.CurrentElo);
            Assert.Equal(1000, player2.CurrentElo);
        }

        [Fact]
        public void CalculateElo_TieGameWithDifferentElo_EloChanged()
        {
            IEloCalculable match = CreateTwoPlayerMatch(1000, 1200);
            match.Team1Score = 0;
            match.Team2Score = 0;

            Elo.CalculateElo(ref match);

            var player1 = (match as TwoPlayerMatch).Player1;
            var player2 = (match as TwoPlayerMatch).Player2;

            Assert.Equal(1007.8, player1.CurrentElo, 1);
            Assert.Equal(1192.2, player2.CurrentElo, 1);
        }

        private TwoPlayerMatch CreateTwoPlayerMatch(double player1Elo = 1000, double player2Elo = 1000)
        {
            return new TwoPlayerMatch()
            {
                Player1 = new Player()
                {
                    Name = "Player 1",
                    CurrentElo = player1Elo
                },
                Player2 = new Player()
                {
                    Name = "Player 2",
                    CurrentElo = player2Elo
                }
            };
        }

        private TeamMatch CreateTeamMatch(double team1Elo = 1000, double team2Elo = 1000)
        {
            return new TeamMatch
            {
                Team1 = new Team
                {
                    Name = "Team 1",
                    CurrentElo = team1Elo
                },
                Team2 = new Team
                {
                    Name = "Team 2",
                    CurrentElo = team2Elo
                }
            };
        }

        private SoloTeamMatch CreateSoloTeamMatch(double player1Elo = 1000, double player2Elo = 1000)
        {
            return new SoloTeamMatch()
            {
                Team1Players = new List<Player>
                {
                    new Player
                    {
                        Name = "Player 1",
                        CurrentElo = player1Elo
                    }
                },
                Team2Players = new List<Player>
                {
                    new Player
                    {
                        Name = "Player 2",
                        CurrentElo = player2Elo
                    }
                },
            };
        }
    }
}
