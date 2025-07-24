using System.ComponentModel.DataAnnotations;

namespace GestionHotel.Application.DTOs
{
    public class SignalementDto
    {
        [Required]
        public string Description { get; set; } = string.Empty;

        [Required]
        public int ChambreId { get; set; }

        public int? ReservationId { get; set; }

        public bool Traite { get; set; } = false;
    }
}
