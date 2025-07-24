using GestionHotel.Application.DTOs;
using GestionHotel.Application.Interfaces;
using GestionHotel.Domain.Interfaces;

namespace GestionHotel.Application.Services
{
    public class MenageService : IMenageService
    {
        private readonly IChambreRepository _chambreRepository;

        public MenageService(IChambreRepository chambreRepository)
        {
            _chambreRepository = chambreRepository;
        }

        public async Task<IEnumerable<ChambreAvecEtatDto>> GetChambresANettoyerAsync()
        {
            var chambres = await _chambreRepository.GetChambresNonPropresAsync();
            return chambres.Select(c => new ChambreAvecEtatDto
            {
                Id = c.Id,
                Numero = c.Numero,
                Type = c.Type.ToString(),
                Tarif = c.Tarif,
                Etat = c.Etat.ToString(),
                EstPropre = c.EstPropre
            });
        }

        public async Task MarquerChambreCommeNettoyeeAsync(int chambreId)
        {
            var chambre = await _chambreRepository.GetByIdAsync(chambreId);
            if (chambre != null)
            {
                chambre.EstPropre = true;
                chambre.DommagesSignales = null;
                await _chambreRepository.UpdateAsync(chambre);
            }
        }

        public async Task SignalerDommageAsync(int chambreId, string description)
        {
            var chambre = await _chambreRepository.GetByIdAsync(chambreId);
            if (chambre != null)
            {
                chambre.DommagesSignales = description;
                await _chambreRepository.UpdateAsync(chambre);
            }
        }
    }
}
