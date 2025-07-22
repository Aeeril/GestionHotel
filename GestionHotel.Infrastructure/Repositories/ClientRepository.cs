using GestionHotel.Domain.Entities;
using GestionHotel.Domain.Interfaces;
using GestionHotel.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GestionHotel.Infrastructure.Repositories
{
    public class ClientRepository : IClientRepository
    {
        private readonly HotelDbContext _context;

        public ClientRepository(HotelDbContext context)
        {
            _context = context;
        }

        public async Task<Client?> GetByIdAsync(int id) =>
            await _context.Clients.FindAsync(id);

        public async Task<Client?> GetByEmailAsync(string email) =>
            await _context.Clients.FirstOrDefaultAsync(c => c.Email == email);

        public async Task AddAsync(Client client)
        {
            _context.Clients.Add(client);
            await _context.SaveChangesAsync();
        }
    }
}