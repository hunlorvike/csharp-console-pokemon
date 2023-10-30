using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PokemonConsoleApp.Model;

namespace PokemonConsoleApp.Config
{
    internal class AppDbContext : DbContext
    {
        public DbSet<PokemonModel> pokemons { get; set; }
        public DbSet<UserModel> users { get; set; }
        public DbSet<UserPokemon> userpokemons { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            string connectionString = "server=localhost;port=3306;database=csharp_pokemons;user=root;password=";
            optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserPokemon>()
                .HasKey(up => new { up.UserID, up.PokemonID });

        }

    }
}
