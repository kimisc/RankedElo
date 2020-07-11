using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore.Internal;
using RankedElo.Core.Entities;
using RankedElo.Web.Models;
using Xunit;

namespace RankedElo.Web.Tests
{
    public class DtoValidatorTests
    {
        private static readonly DateTime _validStartDate = new DateTime(2020, 1, 1, 1, 0, 0);
        private static readonly DateTime _validEndDate = _validStartDate.AddMinutes(5);
        public static readonly object[][] BaseValidatorTestData =
        {
            // Valid entry
            new object[] { _validStartDate, _validEndDate, 1, 0, true},
            // Score invalid
            new object[] { _validStartDate, _validEndDate, -1, 0, false},
            new object[] { _validStartDate, _validEndDate, 1, 100, false},
            // Dates invalid
            new object[] { _validEndDate, _validStartDate, 1, 0, false},
            new object[] { null, _validStartDate, 1, 0, false},
            new object[] { _validEndDate, null, 1, 0, false},
        };

        [Theory]
        [MemberData(nameof(BaseValidatorTestData))]
        public void BaseMatchDtoValidator_IsValid(DateTime startTime, DateTime endTime,
            int team1Score, int team2Score, bool expected)
        {
            var input = new BaseMatchDto()
            {
                StartTime = startTime,
                EndTime = endTime,
                Team1Score = team1Score,
                Team2Score = team2Score
            };
            var sut = new BaseMatchDtoValidator<BaseMatchDto>();
            var actual = sut.Validate(input);
            Assert.Equal(expected, actual.IsValid);
        }

        [Fact]
        public void SoloTeamMatchDtoValidator_PlayerNameTooLong_IsNotValid()
        {
            var input = new SoloTeamMatchDto()
            {
                StartTime = _validStartDate,
                EndTime = _validEndDate,
                Team1Score = 0,
                Team2Score = 1,
                Players = new List<SoloTeamPlayerDto>()
                {
                    new SoloTeamPlayerDto
                    {
                        Name = "Valid",
                        Team = TeamSide.Home
                    },
                    new SoloTeamPlayerDto
                    {
                        Name = "Valid2",
                        Team = TeamSide.Home
                    },
                    new SoloTeamPlayerDto
                    {
                        Name = "Toolongplayername",
                        Team = TeamSide.Away
                    }
                }
            };
            var sut = new SoloTeamMatchDtoValidator();
            var actual = sut.Validate(input);
            Assert.False(actual.IsValid);
        }

        [Fact]
        public void SoloTeamMatchDtoValidator_TooManyPlayers_IsNotValid()
        {
            var input = new SoloTeamMatchDto()
            {
                StartTime = _validStartDate,
                EndTime = _validEndDate,
                Team1Score = 0,
                Team2Score = 1,
                Players = Enumerable.Range(0, 20).Select(x => new SoloTeamPlayerDto()
                {
                    Name = x.ToString(),
                    Team = TeamSide.Away
                }).ToList()
            };
            var sut = new SoloTeamMatchDtoValidator();
            var actual = sut.Validate(input);
            Assert.False(actual.IsValid);
        }

        [Fact]
        public void SoloTeamMatchDtoValidator_NoOpposingTeam_IsNotValid()
        {
            var input = new SoloTeamMatchDto()
            {
                StartTime = _validStartDate,
                EndTime = _validEndDate,
                Team1Score = 0,
                Team2Score = 1,
                Players = new List<SoloTeamPlayerDto>() {
                    new SoloTeamPlayerDto {
                        Name = "Player",
                        Team = TeamSide.Away
                    }
                }
            };
            var sut = new SoloTeamMatchDtoValidator();
            var actual = sut.Validate(input);
            Assert.False(actual.IsValid);
        }

        [Fact]
        public void SoloTeamMatchDtoValidator_PlayerNamesMustBeUnique()
        {
            var input = new SoloTeamMatchDto()
            {
                StartTime = _validStartDate,
                EndTime = _validEndDate,
                Team1Score = 0,
                Team2Score = 1,
                Players = new List<SoloTeamPlayerDto>() {
                    new SoloTeamPlayerDto() {
                        Name = "same",
                        Team = TeamSide.Away
                    },
                    new SoloTeamPlayerDto {
                        Name = "same",
                        Team = TeamSide.Home
                    }
                }
            };
            var sut = new SoloTeamMatchDtoValidator();
            var actual = sut.Validate(input);
            Assert.False(actual.IsValid);
        }
    }
}
