using System;
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
        private readonly IEnumerable<SoloTeamPlayerDto> _players;
        public SoloTeamPlayerDtoValidator(IEnumerable<SoloTeamPlayerDto> allPlayers)
        {
            _players = allPlayers;
            RuleFor(x => x.Name).NotEmpty().MaximumLength(10);
            RuleFor(x => x.Name).Must(IsUniqueName).WithMessage("Names must be unique.");
            RuleFor(x => (int)x.Team).InclusiveBetween(1, 2);
        }
        public bool IsUniqueName(SoloTeamPlayerDto editedPlayer, string newValue)
        {
            return _players.All(player => player.Equals(editedPlayer) || player.Name != newValue);
        }
    }
    public class SoloTeamMatchDtoValidator : BaseMatchDtoValidator<SoloTeamMatchDto>
    {
        public SoloTeamMatchDtoValidator()
        {
            RuleFor(x => x.Players).NotEmpty().Must(x => x.Count < 10).WithMessage("Maximum 10 players allowed.");
            RuleForEach(x => x.Players).SetValidator(x => new SoloTeamPlayerDtoValidator(x.Players));
        }

    }
}
