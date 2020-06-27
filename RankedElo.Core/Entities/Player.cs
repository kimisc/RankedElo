using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RankedElo.Core.Entities
{
    public class Player
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IList<Match> MatchHistory { get; set; } = new List<Match>();
        public double CurrentElo { get; set; } = Elo.DefaultPoints;

        public Player() { }

        public Player(string name)
        {
            Name = name;
        }
    }
}
