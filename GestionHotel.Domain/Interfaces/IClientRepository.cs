using GestionHotel.Domain.Entities;

namespace GestionHotel.Domain.Interfaces
{
    public interface IClientRepository
    {
        Task<Client?> GetByIdAsync(int id);
        Task<Client?> GetByEmailAsync(string email);
        Task AddAsync(Client client);
    }
}