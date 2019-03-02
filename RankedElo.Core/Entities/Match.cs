using System;
using System.Collections.Generic;
using System.Text;

namespace RankedElo.Core.Entities
{
    public class Match : BaseEntity
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public Team Team1 { get; set; }
        public Team Team2 { get; set; }
    }
}
