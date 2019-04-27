using RankedElo.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace RankedElo.Core.Tests
{
    public class MatchesTests
    {

        [Fact]
        public void CalculateElo_TwoPlayersTeam1Wins_EloUpdated()
        {
            var match = new TwoPlayerMatch()
            {
                Player1 = new Player()
                {
                    Name = "Player 1",
                    CurrentElo = 1200
                },
                Player2 = new Player()
                {
                    Name = "Player 2",
                    CurrentElo = 1000
                },
                Team1Score = 1,
                Team2Score = 0
            };

            match.CalculateElo();

            var player1 = match.Player1;
            var player2 = match.Player2;

            Assert.Equal("Player 1", player1.Name);
            Assert.Equal(1207.2, player1.CurrentElo, 1);
            Assert.Equal("Player 2", player2.Name);
            Assert.Equal(992.8, player2.CurrentElo, 1);
        }

        [Fact]
        public void CalculateElo_TwoPlayersTeam2Wins_EloUpdated()
        {
            var match = new TwoPlayerMatch()
            {
                Player1 = new Player()
                {
                    Name = "Player 1",
                    CurrentElo = 1200
                },
                Player2 = new Player()
                {
                    Name = "Player 2",
                    CurrentElo = 1000
                },
                Team1Score = 0,
                Team2Score = 1
            };

            match.CalculateElo();

            var player1 = match.Player1;
            var player2 = match.Player2;

            Assert.Equal("Player 1", player1.Name);
            Assert.Equal(1177.2, player1.CurrentElo, 1);
            Assert.Equal("Player 2", player2.Name);
            Assert.Equal(1022.8, player2.CurrentElo, 1);
        }

        [Fact]
        public void CalculateElo_RankedTeamTeam1Wins_EloUpdated()
        {
            var match = new TeamMatch
            {
                Team1 = new Team
                {
                    Name = "Team 1",
                    CurrentElo = 1200
                },
                Team2 = new Team
                {
                    Name = "Team 2",
                    CurrentElo = 1000
                },
                Team1Score = 1,
                Team2Score = 0
            };
            match.CalculateElo();

            var team1 = match.Team1;
            var team2 = match.Team2;

            Assert.Equal("Team 1", team1.Name);
            Assert.Equal(1207.2, team1.CurrentElo, 1);
            Assert.Equal("Team 2", team2.Name);
            Assert.Equal(992.8, team2.CurrentElo, 1);
        }

        [Fact]
        public void CalculateElo_RankedTeamTeam2Wins_EloUpdated()
        {
            var match = new TeamMatch
            {
                Team1 = new Team
                {
                    Name = "Team 1",
                    CurrentElo = 1200
                },
                Team2 = new Team
                {
                    Name = "Team 2",
                    CurrentElo = 1000
                },
                Team1Score = 0,
                Team2Score = 1
            };
            match.CalculateElo();
            var team1 = match.Team1;
            var team2 = match.Team2;

            Assert.Equal("Team 1", team1.Name);
            Assert.Equal(1177.2, team1.CurrentElo, 1);
            Assert.Equal("Team 2", team2.Name);
            Assert.Equal(1022.8, team2.CurrentElo, 1);
        }

        [Fact]
        public void CalculateElo_MultiplePlayersTeam1Wins_TeamAverageUsed()
        {
            var match = new SoloTeamMatch
            {
                Team1Players = new List<Player> 
                {
                    new Player()
                    {
                        Name = "Player 1",
                        CurrentElo = 1000
                    },
                    new Player()
                    {
                        Name = "Player 3",
                        CurrentElo = 900
                    } 
                },
                Team2Players = new List<Player> 
                {
                    new Player()
                    {
                        Name = "Player 2",
                        CurrentElo = 1000
                    },
                    new Player()
                    {
                        Name = "Player 4",
                        CurrentElo = 1200
                    } 
                },
                Team1Score = 1,
                Team2Score = 0
            };

            match.CalculateElo();

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
            var match = new SoloTeamMatch
            {
                Team1Players = new List<Player> 
                {
                    new Player()
                    {
                        Name = "Player 1",
                        CurrentElo = 1000
                    },
                    new Player()
                    {
                        Name = "Player 3",
                        CurrentElo = 900
                    } 
                },
                Team2Players = new List<Player> 
                {
                    new Player()
                    {
                        Name = "Player 2",
                        CurrentElo = 1000
                    },
                    new Player()
                    {
                        Name = "Player 4",
                        CurrentElo = 1200
                    } 
                },
                Team1Score = 0,
                Team2Score = 1
            };

            match.CalculateElo();

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
            var match = new TwoPlayerMatch()
            {
                Player1 = new Player()
                {
                    Name = "Player 1",
                    CurrentElo = 0
                },
                Player2 = new Player()
                {
                    Name = "Player 2",
                    CurrentElo = 1000
                },
                Team1Score = 0,
                Team2Score = 1
            };

            match.CalculateElo();

            var player1 = match.Player1;
            var player2 = match.Player2;


            Assert.Equal("Player 1", player1.Name);
            Assert.Equal(0, player1.CurrentElo);
        }

        [Fact]
        public void CalculateElo_TieGameWithSameElo_EloNotChanged()
        {
            var match = new TwoPlayerMatch()
            {
                Player1 = new Player()
                {
                    Name = "Player 1",
                    CurrentElo = 1000
                },
                Player2 = new Player()
                {
                    Name = "Player 2",
                    CurrentElo = 1000
                },
                Team1Score = 1,
                Team2Score = 1
            };

            match.CalculateElo();

            var player1 = match.Player1;
            var player2 = match.Player2;

            Assert.Equal(1000, player1.CurrentElo);
            Assert.Equal(1000, player2.CurrentElo);
        }

        [Fact]
        public void CalculateElo_TieGameWithDifferentElo_EloChanged()
        {
            var match = new TwoPlayerMatch()
            {
                Player1 = new Player()
                {
                    Name = "Player 1",
                    CurrentElo = 1000
                },
                Player2 = new Player()
                {
                    Name = "Player 2",
                    CurrentElo = 1200
                },
                Team1Score = 1,
                Team2Score = 1
            };

            match.CalculateElo();

            var player1 = match.Player1;
            var player2 = match.Player2;

            Assert.Equal(1007.8, player1.CurrentElo, 1);
            Assert.Equal(1192.2, player2.CurrentElo, 1);
        }
    }
}
