using RankedElo.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace RankedElo.Core.Tests
{
    public class EloTests
    {
        private List<Player> _team1;
        private List<Player> _team2;


        [Fact]
        public void CalculateElo_TwoPlayersTeam1Wins_PlayerEloUsed()
        {
            InitDefaultTeams(1200, 1000);

            Elo.CalculateElo(ref _team1, ref _team2, true);
            var player1 = _team1.First();
            var player2 = _team2.First();

            Assert.Equal("Player 1", player1.Name);
            Assert.Equal(1207.2, player1.Elo, 1);
            Assert.Equal("Player 2", player2.Name);
            Assert.Equal(992.8, player2.Elo, 1);
        }

        [Fact]
        public void CalculateElo_TwoPlayersTeam2Wins_PlayerEloUsed()
        {
            InitDefaultTeams(1200, 1000);

            Elo.CalculateElo(ref _team1, ref _team2, false);
            var player1 = _team1.First();
            var player2 = _team2.First();

            Assert.Equal("Player 1", player1.Name);
            Assert.Equal(1177.2, player1.Elo, 1);
            Assert.Equal("Player 2", player2.Name);
            Assert.Equal(1022.8, player2.Elo, 1);
        }

        [Fact]
        public void CalculateElo_MultiplePlayersTeam1Wins_TeamAverageUsed()
        {
            InitDefaultTeams();
            _team1.Add(new Player() { Name = "Player 3", Elo = 900 });
            _team2.Add(new Player() { Name = "Player 4", Elo = 1200 });

            Elo.CalculateElo(ref _team1, ref _team2, true);
            var t1_player1 = _team1[0];
            var t1_player2 = _team1[1];
            var t2_player1 = _team2[0];
            var t2_player2 = _team2[1];

            Assert.Equal("Player 1", t1_player1.Name);
            Assert.Equal(1021.1, t1_player1.Elo, 1);
            Assert.Equal("Player 3", t1_player2.Name);
            Assert.Equal(921.1, t1_player2.Elo, 1);
            Assert.Equal("Player 2", t2_player1.Name);
            Assert.Equal(978.9, t2_player1.Elo, 1);
            Assert.Equal("Player 4", t2_player2.Name);
            Assert.Equal(1178.9, t2_player2.Elo, 1);
        }

        [Fact]
        public void CalculateElo_MultiplePlayersTeam2Wins_TeamAverageUsed()
        {
            InitDefaultTeams();
            _team1.Add(new Player() { Name = "Player 3", Elo = 900 });
            _team2.Add(new Player() { Name = "Player 4", Elo = 1200 });

            Elo.CalculateElo(ref _team1, ref _team2, false);
            var t1_player1 = _team1[0];
            var t1_player2 = _team1[1];
            var t2_player1 = _team2[0];
            var t2_player2 = _team2[1];

            Assert.Equal("Player 1", t1_player1.Name);
            Assert.Equal(991.1, t1_player1.Elo, 1);
            Assert.Equal("Player 3", t1_player2.Name);
            Assert.Equal(891.1, t1_player2.Elo, 1);
            Assert.Equal("Player 2", t2_player1.Name);
            Assert.Equal(1008.9, t2_player1.Elo, 1);
            Assert.Equal("Player 4", t2_player2.Name);
            Assert.Equal(1208.9, t2_player2.Elo, 1);
        }

        [Fact]
        public void CalculateElo_EloChangeBelowZero_ReturnsZero()
        {
            InitDefaultTeams(0, 1000);
            Elo.CalculateElo(ref _team1, ref _team2, false);
            var player1 = _team1.First();
            var player2 = _team2.First();

            Assert.Equal("Player 1", player1.Name);
            Assert.Equal(0, player1.Elo);
        }

        [Fact(Skip = "Todo: Tie game")]
        public void CalculateElo_TieGameWithSameElo_EloNotChanged()
        {
            InitDefaultTeams();
            Elo.CalculateElo(ref _team1, ref _team2, false);
            var player1 = _team1.First();
            var player2 = _team2.First();

            Assert.Equal(1000, player1.Elo);
            Assert.Equal(1000, player2.Elo);
        }

        [Fact(Skip = "Todo: Tie game")]
        public void CalculateElo_TieGameWithDifferentElo_EloChanged()
        {
            InitDefaultTeams(1000, 1200);
            Elo.CalculateElo(ref _team1, ref _team2, true);
            var player1 = _team1.First();
            var player2 = _team2.First();

            Assert.Equal(998, player1.Elo);
            Assert.Equal(1202, player2.Elo);
        }

        private void InitDefaultTeams(double p1Elo = 1000, double p2Elo = 1000)
        {
            _team1 = new List<Player>()
                {
                    new Player()
                    {
                        Name = "Player 1",
                        Elo = p1Elo
                    }
                };

            _team2 = new List<Player>()
                {
                    new Player()
                    {
                        Name = "Player 2",
                        Elo = p2Elo
                    }
                };
        }

    }
}
