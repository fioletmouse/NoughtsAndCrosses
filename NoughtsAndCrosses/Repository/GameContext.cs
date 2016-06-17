using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using NoughtsAndCrosses.Models;

namespace NoughtsAndCrosses.Repository
{
    public class GameContext : DbContext
    {
        public GameContext() : base("GameHistory")
        {
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<GameContext>());
        }

        public DbSet<GameInfo> GameInfo { get; set; }
        public DbSet<MovesInfo> MovesInfo { get; set; }
    }
}