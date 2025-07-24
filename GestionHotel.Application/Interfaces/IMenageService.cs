using GestionHotel.Application.DTOs;

namespace GestionHotel.Application.Interfaces
{
    public interface IMenageService
    {
        Task<IEnumerable<ChambreAvecEtatDto>> GetChambresANettoyerAsync();
        Task MarquerChambreCommeNettoyeeAsync(int chambreId);
        Task SignalerDommageAsync(int chambreId, string description);
    }
}
