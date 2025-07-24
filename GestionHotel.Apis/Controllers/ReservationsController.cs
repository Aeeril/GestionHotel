using GestionHotel.Application.DTOs;
using GestionHotel.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GestionHotel.Apis.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ReservationsController : ControllerBase
    {
        private readonly ReservationService _service;

        public ReservationsController(ReservationService service)
        {
            _service = service;
        }

        [HttpPost]
        [Authorize(Roles = "Client, Receptionniste")]
        public async Task<IActionResult> Reserver(ReservationRequestDto dto)
        {
            var result = await _service.ReserverAsync(dto);
            return Ok(result);
        }

        [HttpPost("annuler/{id:int}")]
        [Authorize(Roles = "Client, Receptionniste")]
        public async Task<IActionResult> Annuler(int id)
        {
            await _service.AnnulerReservationAsync(id, demandeParClient: true, forcerRemboursement: false);
            return Ok("Réservation annulée.");
        }

        [HttpPost("annuler-par-reception")]
        [Authorize(Roles = "Receptionniste")]
        public async Task<IActionResult> AnnulerParReception(int reservationId, bool rembourser)
        {
            await _service.AnnulerReservationAsync(reservationId, demandeParClient: false, forcerRemboursement: rembourser);
            return Ok("Réservation annulée par la réception.");
        }

        [HttpPost("checkin")]
        [Authorize(Roles = "Receptionniste")]
        public async Task<IActionResult> CheckIn(CheckInRequestDto dto)
        {
            await _service.CheckInAsync(dto);
            return Ok("Check-in effectué avec succès.");
        }

        [HttpPost("checkout")]
        [Authorize(Roles = "Receptionniste")]
        public async Task<IActionResult> CheckOut(CheckOutRequestDto dto)
        {
            await _service.CheckOutAsync(dto.ReservationId);
            return Ok("Check-out effectué avec succès.");
        }

        [HttpGet("actives")]
        [Authorize(Roles = "Receptionniste")]
        public async Task<IActionResult> GetActives()
        {
            var actives = await _service.GetReservationsActivesAsync();
            return Ok(actives);
        }
    }
}
