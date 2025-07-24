using Microsoft.AspNetCore.Mvc;
using GestionHotel.Domain.Entities;
using GestionHotel.Domain.Interfaces;
using GestionHotel.Application.DTOs;


[ApiController]
[Route("api/[controller]")]
public class SignalementsController : ControllerBase
{
    private readonly ISignalementRepository _repo;

    public SignalementsController(ISignalementRepository repo)
    {
        _repo = repo;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var signalements = await _repo.GetAllAsync();
        return Ok(signalements);
    }

    [HttpPost]
    public async Task<IActionResult> Signaler([FromBody] SignalementDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Description))
            return BadRequest("La description est requise.");

        var signalement = new Signalement
        {
            Description = dto.Description,
            ChambreId = dto.ChambreId,
            ReservationId = dto.ReservationId,
            DateSignalement = DateTime.Now,
            Traite = false
        };

        await _repo.AddAsync(signalement);
        return CreatedAtAction(nameof(GetAll), new { id = signalement.Id }, signalement);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] SignalementDto dto)
    {
        var signalement = await _repo.GetByIdAsync(id);
        if (signalement == null)
            return NotFound();

        signalement.Description = dto.Description;
        signalement.ChambreId = dto.ChambreId;
        signalement.ReservationId = dto.ReservationId;
        signalement.Traite = dto.Traite;

        await _repo.UpdateAsync(signalement);

        return Ok(signalement); 
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var signalement = await _repo.GetByIdAsync(id);
        if (signalement == null)
            return NotFound();

        await _repo.DeleteAsync(signalement);
        return NoContent(); 
    }
}
