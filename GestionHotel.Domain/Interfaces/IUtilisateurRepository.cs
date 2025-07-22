using GestionHotel.Domain.Entities;

namespace GestionHotel.Domain.Interfaces
{
    public interface IUtilisateurRepository
    {
        Task<Utilisateur?> GetByEmailAsync(string email);
        Task<Utilisateur?> GetByIdAsync(int id);
        Task AddAsync(Utilisateur utilisateur);
    }
}