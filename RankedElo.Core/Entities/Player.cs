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
        public IList<Elo> EloHistory { get; set; } = new List<Elo>();

        public double CurrentElo
        {
            get => EloHistory?
                .OrderByDescending(elo => elo.Timestamp)
                .FirstOrDefault()?.Points ?? Elo.DefaultPoints;
            set => EloHistory.Add(new Elo(value));
        }

        public Player() { }

        public Player(string name)
        {
            Name = name;
        }
    }
}
