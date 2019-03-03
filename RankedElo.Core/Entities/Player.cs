using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RankedElo.Core.Entities
{
    public class Player : BaseEntity
    {
        public string Name { get; set; }
        public IList<Elo> EloHistory { get; set; }
        public RankedTeam RankedTeam { get; set; }
        public double CurrentElo
        {
            get
            {
                return EloHistory.OrderBy(x => x.Timestamp).LastOrDefault()?.Points ?? 1000d;
            }
            set
            {
                EloHistory.Add(new Elo
                {
                    Points = value,
                    Timestamp = DateTime.UtcNow
                });
            }
        }
    }
}
