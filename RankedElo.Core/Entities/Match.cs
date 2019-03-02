using System;
using System.Collections.Generic;
using System.Text;

namespace RankedElo.Core.Entities
{
    public class Match : BaseEntity
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public virtual IEnumerable<Team> Teams { get; set; }
    }
}
