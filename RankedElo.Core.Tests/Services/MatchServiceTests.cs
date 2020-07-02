using System.Collections.Generic;
using System;
using System.Linq;
using System.Threading.Tasks;
using NSubstitute;
using RankedElo.Core.Entities;
using RankedElo.Core.Interfaces;
using RankedElo.Core.Services;
using Xunit;

namespace RankedElo.Core.Tests.Services
{
    class MatchBuilder<T> where T : Match, new() {
        private T match;

        public MatchBuilder()
        {
            match = new T();
        }

        public MatchBuilder<T> WithParticipants(string p1name, double p1elo, string p2name, double p2elo) {
            if (match is TwoPlayerMatch twoPlayerMatch) {
                twoPlayerMatch.Player1 = CreatePlayer(p1name, p1elo);
                twoPlayerMatch.Player2 = CreatePlayer(p2name, p2elo);
            } else if (match is TeamMatch teamMatch) {
                teamMatch.Team1 = CreateTeam(p1name, p1elo);
                teamMatch.Team2 = CreateTeam(p2name, p2elo);
            }else {
                throw new ArgumentException($"Not a {nameof(TwoPlayerMatch)}");
            }
            return this;
        }

        private Player CreatePlayer(string name, double elo) {
            return new Player(name) {
                CurrentElo = elo
            };
        } 

        private Team CreateTeam(string name, double elo) {
            return new Team(name) {
                CurrentElo = elo
            };
        }
        public MatchBuilder<T> WithWinningTeam(TeamSide team) {
            if (team == TeamSide.Home) {
                match.HomeTeamScore = 1;
                match.AwayTeamScore = 0;
            } else {
                match.HomeTeamScore = 0;
                match.AwayTeamScore = 1;
            }
            return this;
        }

        public T Build() {
            return match;
        }
    }
    public class MatchServiceTests
    {
        private readonly IMatchService _sut;
        private List<SoloTeamPlayer> SoloPlayers => new List<SoloTeamPlayer>
        {
            new SoloTeamPlayer
            {
                Player = new Player
                {
                    Name = "Player 1",
                    CurrentElo = 1000
                },
                Team = TeamSide.Home
            },
            new SoloTeamPlayer
            {
                Player = new Player
                {
                    Name = "Player 3",
                    CurrentElo = 900
                },
                Team = TeamSide.Home
            },
            new SoloTeamPlayer
            {
                Player = new Player
                {
                    Name = "Player 2",
                    CurrentElo = 1000
                },
                Team = TeamSide.Away
            },
            new SoloTeamPlayer
            {
                Player = new Player
                {
                    Name = "Player 4",
                    CurrentElo = 1200
                },
                Team = TeamSide.Away
            }
        };
        public MatchServiceTests()
        {
            var repositoryStub = Substitute.For<IMatchRepository>();
            _sut = new MatchService(repositoryStub);
        }

        [Fact]
        public async Task CalculateElo_TwoPlayersTeam1Wins_EloUpdated()
        {
            var match = new MatchBuilder<TwoPlayerMatch>()
                .WithParticipants("Player 1", 1200, "Player 2", 1000)
                .WithWinningTeam(TeamSide.Home)
                .Build();

            var result = await _sut.AddMatchAsync<TwoPlayerMatch>(match);

            var player1 = result.Player1;
            var player2 = result.Player2;

            Assert.Equal("Player 1", player1.Name);
            Assert.Equal(1207.2, player1.CurrentElo, 1);
            Assert.Equal("Player 2", player2.Name);
            Assert.Equal(992.8, player2.CurrentElo, 1);
        }

        [Fact]
        public async Task CalculateElo_TwoPlayersTeam2Wins_EloUpdated()
        {
            var match = new MatchBuilder<TwoPlayerMatch>()
                .WithParticipants("Player 1", 1200, "Player 2", 1000)
                .WithWinningTeam(TeamSide.Away)
                .Build();

            var result = await _sut.AddMatchAsync<TwoPlayerMatch>(match);

            var player1 = result.Player1;
            var player2 = result.Player2;

            Assert.Equal("Player 1", player1.Name);
            Assert.Equal(1177.2, player1.CurrentElo, 1);
            Assert.Equal("Player 2", player2.Name);
            Assert.Equal(1022.8, player2.CurrentElo, 1);
        }

        [Fact]
        public async Task CalculateElo_RankedTeamTeam1Wins_EloUpdated()
        {
            var match = new MatchBuilder<TeamMatch>()
                .WithParticipants("Team 1", 1200, "Team 2", 1000)
                .WithWinningTeam(TeamSide.Home)
                .Build();

            var result = await _sut.AddMatchAsync<TeamMatch>(match);

            var team1 = result.Team1;
            var team2 = result.Team2;

            Assert.Equal("Team 1", team1.Name);
            Assert.Equal(1207.2, team1.CurrentElo, 1);
            Assert.Equal("Team 2", team2.Name);
            Assert.Equal(992.8, team2.CurrentElo, 1);
        }

        [Fact]
        public async Task CalculateElo_RankedTeamTeam2Wins_EloUpdated()
        {
            var match = new MatchBuilder<TeamMatch>()
                .WithParticipants("Team 1", 1200, "Team 2", 1000)
                .WithWinningTeam(TeamSide.Away)
                .Build();
            var result = await _sut.AddMatchAsync<TeamMatch>(match);

            var team1 = result.Team1;
            var team2 = result.Team2;

            Assert.Equal("Team 1", team1.Name);
            Assert.Equal(1177.2, team1.CurrentElo, 1);
            Assert.Equal("Team 2", team2.Name);
            Assert.Equal(1022.8, team2.CurrentElo, 1);
        }

        [Theory]
        [InlineData(1, 0, "Player 1", 1021.1, "Player 3", 921.1, "Player 2", 978.9, "Player 4", 1178.9)]
        [InlineData(0, 1, "Player 1", 991.1, "Player 3", 891.1, "Player 2", 1008.9, "Player 4", 1208.9)]
        public async Task CalculateElo_MultiplePlayers_TeamAverageUsed(int team1score, int team2score,
            string p1name, double p1elo, string p2name, double p2elo, string p3name, double p3elo, string p4name, double p4elo)
        {
            IRankedMatch match = new SoloTeamMatch
            {
                Players = SoloPlayers,
                HomeTeamScore = team1score,
                AwayTeamScore = team2score
            };

            var result = await _sut.AddMatchAsync<SoloTeamMatch>(match);

            var team1 = result.Players.Where(x => x.Team == TeamSide.Home);
            var team2 = result.Players.Where(x => x.Team == TeamSide.Away);
            var t1_player1 = team1.First().Player;
            var t1_player2 = team1.Last().Player;
            var t2_player1 = team2.First().Player;
            var t2_player2 = team2.Last().Player;

            Assert.Equal(p1name, t1_player1.Name);
            Assert.Equal(p1elo, t1_player1.CurrentElo, 1);
            Assert.Equal(p2name, t1_player2.Name);
            Assert.Equal(p2elo, t1_player2.CurrentElo, 1);
            Assert.Equal(p3name, t2_player1.Name);
            Assert.Equal(p3elo, t2_player1.CurrentElo, 1);
            Assert.Equal(p4name, t2_player2.Name);
            Assert.Equal(p4elo, t2_player2.CurrentElo, 1);
        }

        [Fact]
        public async Task CalculateElo_EloChangeBelowZero_ReturnsZero()
        {
            var match = new MatchBuilder<TwoPlayerMatch>()
                .WithParticipants("Player 1", 0, "Player 2", 1000)
                .WithWinningTeam(TeamSide.Away)
                .Build();

            var result = await _sut.AddMatchAsync<TwoPlayerMatch>(match);

            var player1 = result.Player1;

            Assert.Equal("Player 1", player1.Name);
            Assert.Equal(0, player1.CurrentElo);
        }

        [Fact]
        public async Task CalculateElo_TieGameWithSameElo_EloNotChanged()
        {
            var match = new MatchBuilder<TwoPlayerMatch>()
                .WithParticipants("Player 1", 1000, "Player 2", 1000)
                .Build();

            var result = await _sut.AddMatchAsync<TwoPlayerMatch>(match);

            var player1 = result.Player1;
            var player2 = result.Player2;

            Assert.Equal(1000, player1.CurrentElo);
            Assert.Equal(1000, player2.CurrentElo);
        }

        [Fact]
        public async Task CalculateElo_TieGameWithDifferentElo_EloChanged()
        {
            var match = new MatchBuilder<TwoPlayerMatch>()
                .WithParticipants("Player 1", 1000, "Player 2", 1200)
                .Build();
            var result = await _sut.AddMatchAsync<TwoPlayerMatch>(match);

            var player1 = result.Player1;
            var player2 = result.Player2;

            Assert.Equal(1007.8, player1.CurrentElo, 1);
            Assert.Equal(1192.2, player2.CurrentElo, 1);
        }
    }
}
