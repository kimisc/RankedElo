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

        public DbSet<Match> Matches { get; set; }
    }
}
