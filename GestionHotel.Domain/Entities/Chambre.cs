namespace GestionHotel.Domain.Entities
{
    public enum ChambreType { Simple, Double, Suite }
    public enum EtatChambre { Neuf, Refaite, ARevoir, RienASignaler, GrosDegats }

    public class Chambre
    {
        public int Id { get; set; }
        public string Numero { get; set; } = string.Empty;
        public int Capacite { get; set; }
        public ChambreType Type { get; set; }
        public decimal Tarif { get; set; }
        public EtatChambre Etat { get; set; }
    }
}