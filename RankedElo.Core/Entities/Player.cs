using System;
using System.Collections.Generic;
using System.Text;

namespace RankedElo.Core.Entities
{
    public class Player : BaseEntity
    {
        public string Name { get; set; }
        public double Elo { get; set; } = 1000d;
    }
}
