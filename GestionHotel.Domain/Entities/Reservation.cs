using System;

namespace GestionHotel.Domain.Entities
{
    public enum StatutReservation { EnCours, Annulee, Confirmee, Terminee }

    public class Reservation
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public Client? Client { get; set; }

        public List<Chambre> Chambres { get; set; } = new();
        public DateTime DateDebut { get; set; }
        public DateTime DateFin { get; set; }
        public StatutReservation Statut { get; set; } = StatutReservation.EnCours;

        public Paiement? Paiement { get; set; }
    }
}