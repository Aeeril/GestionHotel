using GestionHotel.Application.DTOs;
using GestionHotel.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using GestionHotel.Domain.Entities;


namespace GestionHotel.Apis.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChambresController : ControllerBase
    {
        private readonly IChambreRepository _chambreRepo;

        public ChambresController(IChambreRepository chambreRepo)
        {
            _chambreRepo = chambreRepo;
        }
        
        [HttpGet("disponibles")]
        public async Task<IActionResult> GetChambresDisponibles([FromQuery] DateTime dateDebut, [FromQuery] DateTime dateFin)
        {
            if (dateFin <= dateDebut)
                return BadRequest("La date de fin doit être après la date de début.");

            var chambres = await _chambreRepo.GetChambresDisponibles(dateDebut, dateFin);

            var chambresDto = chambres.Select(c => new ChambreAvecEtatDto
            {
                Id = c.Id,
                Numero = c.Numero,
                Type = c.Type.ToString(),
                Tarif = c.Tarif,
                Capacite = c.Capacite,
                Etat = c.Etat.ToString() 
            });

            return Ok(chambresDto);
        }

        [HttpGet("disponibles/reception")]
        public async Task<IActionResult> GetChambresAvecEtatReception([FromQuery] DateTime dateDebut, [FromQuery] DateTime dateFin)
        {
            if (dateFin <= dateDebut)
                return BadRequest("La date de fin doit être après la date de début.");

            var chambres = await _chambreRepo.GetChambresDisponiblesAvecEtat(dateDebut, dateFin);

            var chambresDto = chambres.Select(c => new ChambreAvecEtatDto
            {
                Id = c.Id,
                Numero = c.Numero,
                Type = c.Type.ToString(),
                Tarif = c.Tarif,
                Capacite = c.Capacite,
                Etat = c.Etat.ToString()
            });

            return Ok(chambresDto);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var chambres = await _chambreRepo.GetAllAsync();
            return Ok(chambres);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Chambre chambre)
        {
            await _chambreRepo.AddAsync(chambre);
            return CreatedAtAction(nameof(GetAll), new { id = chambre.Id }, chambre);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Chambre updatedChambre)
        {
            var existing = await _chambreRepo.GetByIdAsync(id);
            if (existing == null)
                return NotFound($"Chambre avec l'id {id} non trouvée.");

            existing.Numero = updatedChambre.Numero;
            existing.Capacite = updatedChambre.Capacite;
            existing.Type = updatedChambre.Type;
            existing.Tarif = updatedChambre.Tarif;
            existing.Etat = updatedChambre.Etat;

            await _chambreRepo.UpdateAsync(existing);
            return NoContent(); 
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var chambre = await _chambreRepo.GetByIdAsync(id);
            if (chambre == null)
                return NotFound($"Chambre avec l'id {id} non trouvée.");

            await _chambreRepo.DeleteAsync(id);
            return NoContent(); 
        }
    }
}
