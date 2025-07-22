using GestionHotel.Domain.Entities;

namespace GestionHotel.Domain.Interfaces
{
    public interface IReservationRepository
    {
        Task AddAsync(Reservation reservation);
        Task<Reservation?> GetByIdAsync(int id);
        Task UpdateAsync(Reservation reservation);
        Task<List<Reservation>> GetReservationsParChambreEtPeriode(int chambreId, DateTime dateDebut, DateTime dateFin);
    }
}
