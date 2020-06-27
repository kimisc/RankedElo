using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RankedElo.Core.Entities;

namespace RankedElo.Persistence
{
    public class RankedEloDbContext : DbContext
    {
        public RankedEloDbContext()
        {
        }
        public RankedEloDbContext(DbContextOptions<RankedEloDbContext> options) : base(options) { }

        public DbSet<Match> Matches { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<Team> Teams { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Match>().HasDiscriminator<int>("Type")
                .HasValue<Match>(-1)
                .HasValue<TwoPlayerMatch>(1)
                .HasValue<TeamMatch>(2)
                .HasValue<SoloTeamMatch>(3);

            modelBuilder.Entity<SoloTeamPlayer>().HasKey(sp => new {sp.PlayerId, sp.MatchId});
        }
    }
}
