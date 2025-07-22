using GestionHotel.Domain.Entities;
using GestionHotel.Domain.Interfaces;
using GestionHotel.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GestionHotel.Infrastructure.Repositories
{
    public class ChambreRepository : IChambreRepository
    {
        private readonly HotelDbContext _context;

        public ChambreRepository(HotelDbContext context)
        {
            _context = context;
        }

        public async Task<List<Chambre>> GetChambresDisponibles(DateTime dateDebut, DateTime dateFin)
        {
            var chambresReservees = await _context.Reservations
                .Where(r =>
                    (dateDebut < r.DateFin && dateFin > r.DateDebut)) // chevauchement
                .SelectMany(r => r.Chambres)
                .Select(c => c.Id)
                .ToListAsync();

            return await _context.Chambres
                .Where(c => !chambresReservees.Contains(c.Id))
                .ToListAsync();
        }

        public async Task<List<Chambre>> GetAllAsync()
        {
            return await _context.Chambres.ToListAsync();
        }

        public async Task<Chambre?> GetByIdAsync(int id)
        {
            return await _context.Chambres.FindAsync(id);
        }

        public async Task AddAsync(Chambre chambre)
        {
            _context.Chambres.Add(chambre);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Chambre chambre)
        {
            _context.Chambres.Update(chambre);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var chambre = await _context.Chambres.FindAsync(id);
            if (chambre != null)
            {
                _context.Chambres.Remove(chambre);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<List<Chambre>> GetChambresDisponiblesAvecEtat(DateTime dateDebut, DateTime dateFin)
        {
            return await GetChambresDisponibles(dateDebut, dateFin);
        }

        public async Task<IEnumerable<Chambre>> GetChambresNonPropresAsync()
        {
            return await _context.Chambres.Where(c => !c.EstPropre).ToListAsync();
        }
    }
}
