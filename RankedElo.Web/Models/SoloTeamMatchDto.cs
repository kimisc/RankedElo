﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using RankedElo.Core.Entities;

namespace RankedElo.Web.Models
{
    public class SoloTeamPlayerDto
    {
        public string Name { get; set; }
        public TeamSide Team { get; set; }
    }
    public class SoloTeamMatchDto : BaseMatchDto
    {
        public List<SoloTeamPlayerDto> Players { get; set; }
    }

    public class SoloTeamPlayerDtoValidator : AbstractValidator<SoloTeamPlayerDto>
    {
        public SoloTeamPlayerDtoValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(10);
            RuleFor(x => (int)x.Team).InclusiveBetween(1, 2);
        }
    }
    public class SoloTeamMatchDtoValidator : BaseMatchDtoValidator<SoloTeamMatchDto>
    {
        public SoloTeamMatchDtoValidator()
        {
            RuleFor(x => x.Players).NotEmpty().Must(x => x.Count < 10).WithMessage("Maximum 10 players allowed.");
            RuleForEach(x => x.Players).SetValidator(x => new SoloTeamPlayerDtoValidator());
        }
    }
}
