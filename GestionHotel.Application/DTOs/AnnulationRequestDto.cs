namespace GestionHotel.Application.DTOs
{
    public class AnnulationRequestDto
    {
        public int ReservationId { get; set; }
        public bool DemandeParClient { get; set; }
        public bool ForcerRemboursement { get; set; }
    }
}
