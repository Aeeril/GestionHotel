namespace GestionHotel.Application.DTOs
{
    public class ReservationResultDto
    {
        public int ReservationId { get; set; }
        public bool EstValide { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}