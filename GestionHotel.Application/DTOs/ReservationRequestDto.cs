using System;

namespace GestionHotel.Application.DTOs
{
    public class ReservationRequestDto
    {
        public int ClientId { get; set; }
        public List<int> ChambreIds { get; set; } = new();
        public DateTime DateDebut { get; set; }
        public DateTime DateFin { get; set; }
        public string NumeroCarte { get; set; } = string.Empty;
    }
}