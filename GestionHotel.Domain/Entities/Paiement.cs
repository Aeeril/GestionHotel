namespace GestionHotel.Domain.Entities
{
    public class Paiement
    {
        public int Id { get; set; }
        public string NumeroCarte { get; set; } = string.Empty;
        public decimal Montant { get; set; }
        public bool EstEffectue { get; set; }
        public DateTime DatePaiement { get; set; }
    }
}