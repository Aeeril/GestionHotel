using GestionHotel.Application.DTOs;
using System.Threading.Tasks;

namespace GestionHotel.Application.Interfaces
{
    public interface IReservationService
    {
        Task<ReservationResultDto> ReserverAsync(ReservationRequestDto dto);
        Task AnnulerReservationAsync(int reservationId, bool demandeParClient, bool forcerRemboursement);
    }
}
