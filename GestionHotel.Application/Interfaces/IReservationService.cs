using GestionHotel.Domain.Entities;
using GestionHotel.Application.DTOs;
using System.Threading.Tasks;
using GestionHotel.Domain.Entities;


namespace GestionHotel.Application.Interfaces
{
    public interface IReservationService
    {
        Task<ReservationResultDto> ReserverAsync(ReservationRequestDto dto);
        Task AnnulerReservationAsync(int reservationId, bool demandeParClient, bool forcerRemboursement);
        Task CheckInAsync(CheckInRequestDto dto);
        Task<List<Reservation>> GetReservationsActivesAsync();
    }
}
