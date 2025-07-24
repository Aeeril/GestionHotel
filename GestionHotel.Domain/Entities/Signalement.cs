namespace GestionHotel.Domain.Entities
{
    public class Signalement
    {
        public int Id { get; set; }
        public int ChambreId { get; set; }
        public Chambre Chambre { get; set; } = null!;
        
        public int? ReservationId { get; set; }
        public Reservation? Reservation { get; set; }

        public string Description { get; set; } = string.Empty;
        public DateTime DateSignalement { get; set; } = DateTime.UtcNow;
        public bool Traite { get; set; } = false;
    }
}
