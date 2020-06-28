using System;
using System.ComponentModel.DataAnnotations;
using System.Data;
using FluentValidation;

namespace RankedElo.Web.Models
{
    public class BaseMatchDto
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int Team1Score { get; set; }
        public int Team2Score { get; set; }
    }

    public class TwoTeamMatchDto : BaseMatchDto
    {
        public string HomeTeamName { get; set; }
        public string AwayTeamName { get; set; }
    }

    public class BaseMatchDtoValidator<T> : AbstractValidator<T> where T : BaseMatchDto
    {
        public BaseMatchDtoValidator()
        {
            RuleFor(x => x.Team1Score).InclusiveBetween(0, 99);
            RuleFor(x => x.Team2Score).InclusiveBetween(0, 99);
            RuleFor(m => m.StartTime)
                .NotEmpty();

            RuleFor(m => m.EndTime)
                .NotEmpty()
                .GreaterThan(m => m.StartTime)
                .When(m => m.StartTime != default);
        }
    }

    public class TwoTeamMatchDtoValidator : BaseMatchDtoValidator<TwoTeamMatchDto>
    {
        public TwoTeamMatchDtoValidator()
        {
            RuleFor(x => x.AwayTeamName).NotEmpty().MaximumLength(10);
            RuleFor(x => x.HomeTeamName).NotEmpty().MaximumLength(10);
        }
    }
}