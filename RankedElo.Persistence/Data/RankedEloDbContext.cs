﻿using Microsoft.EntityFrameworkCore;
using RankedElo.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace RankedElo.Persistence.Contexts
{
    public class RankedEloDbContext : DbContext
    {

        public RankedEloDbContext(DbContextOptions<RankedEloDbContext> options): base(options) { }

        public DbSet<TwoPlayerMatch> TwoPlayerMatches { get; set; }
        public DbSet<TeamMatch> TeamMatches { get; set; }
        public DbSet<SoloTeamMatch> SoloTeamMatches { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<Elo> EloHistory { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Elo>()
                .HasOne<Player>()
                .WithMany()
                .HasForeignKey(x => x.PlayerId);

            modelBuilder.Entity<Elo>()
                .HasOne<Team>()
                .WithMany()
                .HasForeignKey(x => x.TeamId);

            modelBuilder.Entity<TeamMatch>()
                .Ignore(x => x.Team1Elo)
                .Ignore(x => x.Team2Elo);

            modelBuilder.Entity<TwoPlayerMatch>()
                .Ignore(x => x.Team1Elo)
                .Ignore(x => x.Team2Elo);

            modelBuilder.Entity<SoloTeamMatch>()
                .Ignore(x => x.Team1Players)
                .Ignore(x => x.Team2Players);

            modelBuilder.Entity<PlayerSoloTeamMatches>()
            .HasKey(bc => new { bc.PlayerId, bc.SoloTeamMatchId });

            modelBuilder.Entity<PlayerSoloTeamMatches>()
                .HasOne(bc => bc.Player)
                .WithMany()
                .HasForeignKey(bc => bc.PlayerId);

            modelBuilder.Entity<PlayerSoloTeamMatches>()
                .HasOne(bc => bc.SoloTeamMatch)
                .WithMany()
                .HasForeignKey(bc => bc.SoloTeamMatchId);
        }
    }
}
