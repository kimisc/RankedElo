using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RankedElo.Core.Entities
{
    
    public class Team
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double CurrentElo { get; set; } = Elo.DefaultPoints;
    }

}
