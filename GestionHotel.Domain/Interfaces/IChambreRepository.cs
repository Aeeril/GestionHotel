using GestionHotel.Domain.Entities;

namespace GestionHotel.Domain.Interfaces
{
    public interface IChambreRepository
    {
        Task<Chambre?> GetByIdAsync(int id);
        Task<List<Chambre>> GetAllAsync();
        Task<List<Chambre>> GetChambresDisponibles(DateTime dateDebut, DateTime dateFin);
        Task AddAsync(Chambre chambre);
        Task UpdateAsync(Chambre chambre);
        Task DeleteAsync(int id);
    }
}
