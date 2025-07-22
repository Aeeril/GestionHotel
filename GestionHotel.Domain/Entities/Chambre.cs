using System.ComponentModel.DataAnnotations;

namespace GestionHotel.Domain.Entities
{
    public enum TypeChambre
    {
        Simple,
        Double,
        Suite
    }

    public enum EtatChambre
    {
        Neuf,
        Refaite,
        ARefaire,
        RienASignaler,
        GrosDegats
    }

    public class Chambre
    {
        public int Id { get; set; }

        [Required]
        public string Numero { get; set; } = string.Empty;

        [Required]
        public int Capacite { get; set; }

        [Required]
        public TypeChambre Type { get; set; }

        [Required]
        public decimal Tarif { get; set; }

        [Required]
        public EtatChambre Etat { get; set; }
    }
}
