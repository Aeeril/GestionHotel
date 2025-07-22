namespace GestionHotel.Domain.Entities
{
    public enum RoleUtilisateur { Client, Receptionniste, PersonnelMenage }

    public class Utilisateur
    {
        public int Id { get; set; }
        public string Nom { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string MotDePasse { get; set; } = string.Empty;
        public RoleUtilisateur Role { get; set; }
    }
}