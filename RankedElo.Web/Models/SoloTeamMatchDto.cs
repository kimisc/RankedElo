using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RankedElo.Web.Models
{
    public class SoloTeamPlayerDto
    {
        public string Name { get; set; }
        public int Team { get; set; }
    }
    public class SoloTeamBaseMatchDto : BaseMatchDto
    {
        public List<SoloTeamPlayerDto> Players { get; set; }
    }
}
