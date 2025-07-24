using GestionHotel.Domain.Entities;

namespace GestionHotel.Domain.Interfaces
{
    public interface ISignalementRepository
    {
        Task<List<Signalement>> GetAllAsync();
        Task<Signalement?> GetByIdAsync(int id);
        Task AddAsync(Signalement signalement);
        Task UpdateAsync(Signalement signalement);
        Task DeleteAsync(Signalement signalement);

    }
}
