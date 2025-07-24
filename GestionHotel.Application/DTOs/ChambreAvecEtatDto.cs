using System.ComponentModel.DataAnnotations;

namespace GestionHotel.Application.DTOs
{
    public class ChambreAvecEtatDto
    {
        public int Id { get; set; }

        [Required]
        public required string Numero { get; set; }

        public required string Type { get; set; }

        public decimal Tarif { get; set; }

        public int Capacite { get; set; }

        public bool EstPropre { get; set; }

        public required string Etat { get; set; }
    }
}
