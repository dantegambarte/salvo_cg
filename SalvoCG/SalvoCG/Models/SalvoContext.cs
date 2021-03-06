using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalvoCG.Models
{
    public class SalvoContext : DbContext
    {
        public SalvoContext(DbContextOptions<SalvoContext> options) : base(options)
        {
        }
        public DbSet<Player> Players { get; set; } 
        public DbSet<Game> Games { get; set; }
        public DbSet<GamePlayer> GamePlayers { get; set; }
        public DbSet<Ship> Ships { get; set; }
        public DbSet<ShipLocation> ShipLocations { get; set; }
        public DbSet<Salvo> Salvo { get; set; }
        public DbSet<SalvoLocation> SalvoLocations { get; set; }
        public DbSet<Score> Scores { get; set; }

    }
}
