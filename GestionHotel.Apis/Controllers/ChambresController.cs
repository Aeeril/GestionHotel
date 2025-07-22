using GestionHotel.Application.DTOs;
using GestionHotel.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

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
    }
}
