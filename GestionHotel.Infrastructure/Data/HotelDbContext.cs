using GestionHotel.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GestionHotel.Infrastructure.Data
{
    public class HotelDbContext : DbContext
    {
        public HotelDbContext(DbContextOptions<HotelDbContext> options) : base(options) { }

        public DbSet<Chambre> Chambres => Set<Chambre>();
        public DbSet<Client> Clients => Set<Client>();
        public DbSet<Reservation> Reservations => Set<Reservation>();
        public DbSet<Utilisateur> Utilisateurs => Set<Utilisateur>();
        public DbSet<Paiement> Paiements => Set<Paiement>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Reservation>()
                .HasMany(r => r.Chambres)
                .WithMany(); // simplifié, sinon ajouter une table de jointure
        }
    }
}