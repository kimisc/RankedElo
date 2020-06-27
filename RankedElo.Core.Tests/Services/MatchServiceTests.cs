using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NSubstitute;
using RankedElo.Core.Entities;
using RankedElo.Core.Interfaces;
using RankedElo.Core.Services;
using Xunit;

namespace RankedElo.Core.Tests.Services
{
    public class MatchServiceTests
    {
        private readonly IMatchService _sut;
        public MatchServiceTests()
        {
            var repositoryStub = Substitute.For<IMatchRepository>();
            _sut = new MatchService(repositoryStub);
        }

        [Fact]
        public async Task CalculateElo_TwoPlayersTeam1Wins_EloUpdated()
        {
            IRankedMatch match = new TwoPlayerMatch()
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
            IRankedMatch match = new TwoPlayerMatch()
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
            IRankedMatch match = new TeamMatch
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
            IRankedMatch match = new TeamMatch
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
            var result = await _sut.AddMatchAsync<TeamMatch>(match);

            var team1 = result.Team1;
            var team2 = result.Team2;

            Assert.Equal("Team 1", team1.Name);
            Assert.Equal(1177.2, team1.CurrentElo, 1);
            Assert.Equal("Team 2", team2.Name);
            Assert.Equal(1022.8, team2.CurrentElo, 1);
        }

        [Fact]
        public async Task CalculateElo_MultiplePlayersTeam1Wins_TeamAverageUsed()
        {
            IRankedMatch match = new SoloTeamMatch
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

            var result = await _sut.AddMatchAsync<SoloTeamMatch>(match);

            var t1_player1 = result.Team1Players.First();
            var t1_player2 = result.Team1Players.Last();
            var t2_player1 = result.Team2Players.First();
            var t2_player2 = result.Team2Players.Last();

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
        public async Task CalculateElo_MultiplePlayersTeam2Wins_TeamAverageUsed()
        {
            IRankedMatch match = new SoloTeamMatch
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

            var result = await _sut.AddMatchAsync<SoloTeamMatch>(match);

            var t1_player1 = result.Team1Players.First();
            var t1_player2 = result.Team1Players.Last();
            var t2_player1 = result.Team2Players.First();
            var t2_player2 = result.Team2Players.Last();

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
        public async Task CalculateElo_EloChangeBelowZero_ReturnsZero()
        {
            IRankedMatch match = new TwoPlayerMatch()
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

            var result = await _sut.AddMatchAsync<TwoPlayerMatch>(match);

            var player1 = result.Player1;
            var player2 = result.Player2;


            Assert.Equal("Player 1", player1.Name);
            Assert.Equal(0, player1.CurrentElo);
        }

        [Fact]
        public async Task CalculateElo_TieGameWithSameElo_EloNotChanged()
        {
            IRankedMatch match = new TwoPlayerMatch()
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

            var result = await _sut.AddMatchAsync<TwoPlayerMatch>(match);

            var player1 = result.Player1;
            var player2 = result.Player2;

            Assert.Equal(1000, player1.CurrentElo);
            Assert.Equal(1000, player2.CurrentElo);
        }

        [Fact]
        public async Task CalculateElo_TieGameWithDifferentElo_EloChanged()
        {
            IRankedMatch match = new TwoPlayerMatch()
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

            var result = await _sut.AddMatchAsync<TwoPlayerMatch>(match);

            var player1 = result.Player1;
            var player2 = result.Player2;

            Assert.Equal(1007.8, player1.CurrentElo, 1);
            Assert.Equal(1192.2, player2.CurrentElo, 1);
        }
    }
}
