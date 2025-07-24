
using GestionHotel.Domain.Entities;
using System.Linq;

namespace GestionHotel.Infrastructure.Data
{
    public static class DataSeeder
    {
        public static void Initialize(HotelDbContext context)
        {
            context.Database.EnsureCreated();

            if (context.Utilisateurs.Any())
            {
                return;   // DB has been seeded
            }

            var utilisateurs = new Utilisateur[]
            {
                new Utilisateur
                {
                    Nom = "receptionniste",
                    Email = "receptionniste@test.com",
                    MotDePasse = "test", // Note: In a real app, passwords should be hashed
                    Role = RoleUtilisateur.Receptionniste
                },
                new Utilisateur
                {
                    Nom = "client",
                    Email = "client@test.com",
                    MotDePasse = "test",
                    Role = RoleUtilisateur.Client
                },
                new Utilisateur
                {
                    Nom = "menage",
                    Email = "menage@test.com",
                    MotDePasse = "test",
                    Role = RoleUtilisateur.PersonnelMenage
                }
            };

            foreach (var u in utilisateurs)
            {
                context.Utilisateurs.Add(u);
            }
            context.SaveChanges();
        }
    }
}
