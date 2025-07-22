using GestionHotel.Domain.Entities;
using GestionHotel.Domain.Interfaces;
using GestionHotel.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GestionHotel.Infrastructure.Repositories
{
    public class UtilisateurRepository : IUtilisateurRepository
    {
        private readonly HotelDbContext _context;

        public UtilisateurRepository(HotelDbContext context)
        {
            _context = context;
        }

        public async Task<Utilisateur?> GetByEmailAsync(string email) =>
            await _context.Utilisateurs.FirstOrDefaultAsync(u => u.Email == email);

        public async Task<Utilisateur?> GetByIdAsync(int id) =>
            await _context.Utilisateurs.FindAsync(id);

        public async Task AddAsync(Utilisateur utilisateur)
        {
            _context.Utilisateurs.Add(utilisateur);
            await _context.SaveChangesAsync();
        }
    }
}