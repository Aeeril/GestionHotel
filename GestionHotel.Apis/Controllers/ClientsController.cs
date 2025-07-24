using GestionHotel.Domain.Entities;
using GestionHotel.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GestionHotel.Apis.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Receptionniste")]
    public class ClientsController : ControllerBase
    {
        private readonly IClientRepository _repo;

        public ClientsController(IClientRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var clients = await _repo.GetAllAsync();
            return Ok(clients);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Client client)
        {
            await _repo.AddAsync(client);
            return CreatedAtAction(nameof(GetAll), new { id = client.Id }, client);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Client updatedClient)
        {
            var existing = await _repo.GetByIdAsync(id);
            if (existing == null)
                return NotFound($"Client avec l'id {id} non trouvé.");

            existing.Nom = updatedClient.Nom;
            existing.Email = updatedClient.Email;
            existing.Telephone = updatedClient.Telephone;

            await _repo.UpdateAsync(existing);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var client = await _repo.GetByIdAsync(id);
            if (client == null)
                return NotFound($"Client avec l'id {id} non trouvé.");

            await _repo.DeleteAsync(id);
            return NoContent();
        }
    }
}
