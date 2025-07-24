using GestionHotel.Domain.Entities;
using GestionHotel.Domain.Interfaces;
using GestionHotel.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GestionHotel.Infrastructure.Repositories
{
    public class SignalementRepository : ISignalementRepository
    {
        private readonly HotelDbContext _context;

        public SignalementRepository(HotelDbContext context)
        {
            _context = context;
        }

        public async Task<List<Signalement>> GetAllAsync() =>
            await _context.Signalements.Include(s => s.Chambre).Include(s => s.Reservation).ToListAsync();

        public async Task<Signalement?> GetByIdAsync(int id) =>
            await _context.Signalements.FindAsync(id);

        public async Task AddAsync(Signalement signalement)
        {
            _context.Signalements.Add(signalement);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Signalement signalement)
        {
            _context.Signalements.Update(signalement);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var s = await _context.Signalements.FindAsync(id);
            if (s != null)
            {
                _context.Signalements.Remove(s);
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(Signalement signalement)
        {
            _context.Signalements.Remove(signalement);
            await _context.SaveChangesAsync();
        }
    }
}