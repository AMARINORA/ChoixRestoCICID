using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Protocols;
using Microsoft.Extensions.Configuration;

namespace ChoixResto.Models
{
	public class BddContext : DbContext
	{

        public BddContext(): base()
        {

        }

        //public IConfiguration Configuration { get; }

        public DbSet<Utilisateur> Utilisateurs { get; set; }
		public DbSet<Resto> Restos { get; set; }
		public DbSet<Vote> Votes { get; set; }
		public DbSet<Sondage> Sondages { get; set; }

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{

            IConfiguration configuration = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json")
            .Build();
            optionsBuilder.UseMySql(configuration.GetConnectionString("DefaultConnection"));
		}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // specification on configuration

            //Declare non nullable columns
            modelBuilder.Entity<Utilisateur>().Property(u => u.Prenom).IsRequired();
            //Add uniqueness constraint
            modelBuilder.Entity<Utilisateur>().HasIndex(u => u.Prenom).IsUnique();
        }

        public void InitializeDb()
        {
            this.Database.EnsureDeleted();
            this.Database.EnsureCreated();
            this.Restos.AddRange(
                new Resto
                {
                    Id = 1,
                    Nom = "Quick",
                    Telephone = "0000000000"
                },
                new Resto
                {
                    Id = 2,
                    Nom = "McDo",
                    Telephone = "1111111111"
                },
                new Resto
                {
                    Id = 3,
                    Nom = "Cellier",
                    Telephone = "2222222222"
                }
            );
            this.Utilisateurs.Add(new Utilisateur { Id = 1, Prenom = "Pierre", Password = "BC-C2-8A-15-B2-66-C8-3C-D4-E2-31-7D-17-16-58-A8" });
            this.Utilisateurs.Add(new Utilisateur { Id = 2, Prenom = "Louis", Password = "FB-32-9E-B0-0E-A1-D6-76-5D-D1-3B-8E-C0-26-3C-CB" });
            this.SaveChanges();
        }
    }
}
