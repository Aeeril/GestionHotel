using GestionHotel.Application.DTOs;
using System.Threading.Tasks;

namespace GestionHotel.Application.Interfaces
{
    public interface IReservationService
    {
        Task<ReservationResultDto> ReserverAsync(ReservationRequestDto dto);
        Task<bool> AnnulerReservationAsync(int reservationId);
    }
}
