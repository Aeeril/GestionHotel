using GestionHotel.Domain.Entities;

namespace GestionHotel.Domain.Interfaces
{
    public interface IChambreRepository
    {
        Task<List<Chambre>> GetChambresDisponibles(DateTime dateDebut, DateTime dateFin);
        Task<List<Chambre>> GetAllAsync();
        Task<Chambre?> GetByIdAsync(int id);
        Task AddAsync(Chambre chambre);
        Task UpdateAsync(Chambre chambre);
        Task DeleteAsync(int id);
        Task<List<Chambre>> GetChambresDisponiblesAvecEtat(DateTime dateDebut, DateTime dateFin);
        Task<IEnumerable<Chambre>> GetChambresNonPropresAsync();

    }

}
