using FicticiaBackend.Models;
using FicticiaBackend.Commands;
using FicticiaBackend.Queries;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace FicticiaBackend.Controllers
{
    [Authorize] 
    [Route("api/[controller]")]
    [ApiController]
    public class PersonasController : ControllerBase
    {
        private readonly GetAllPersonasQuery _getAllPersonasQuery;
        private readonly GetPersonaByIdQuery _getPersonaByIdQuery;
        private readonly CreatePersonaCommand _createPersonaCommand;
        private readonly UpdatePersonaCommand _updatePersonaCommand;
        private readonly DeletePersonaCommand _deletePersonaCommand;

        public PersonasController(
            GetAllPersonasQuery getAllPersonasQuery,
            GetPersonaByIdQuery getPersonaByIdQuery,
            CreatePersonaCommand createPersonaCommand,
            UpdatePersonaCommand updatePersonaCommand,
            DeletePersonaCommand deletePersonaCommand)
        {
            _getAllPersonasQuery = getAllPersonasQuery;
            _getPersonaByIdQuery = getPersonaByIdQuery;
            _createPersonaCommand = createPersonaCommand;
            _updatePersonaCommand = updatePersonaCommand;
            _deletePersonaCommand = deletePersonaCommand;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Persona>>> Get()
        {
            var personas = await _getAllPersonasQuery.ExecuteAsync();
            return Ok(personas);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Persona>> Get(int id)
        {
            var persona = await _getPersonaByIdQuery.ExecuteAsync(id);
            if (persona == null)
                return NotFound(new { mensaje = $"No se encontró una persona con ID {id}" });

            return Ok(persona);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] Persona persona)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _createPersonaCommand.ExecuteAsync(persona);
            return CreatedAtAction(nameof(Get), new { id = persona.Id }, persona);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] Persona persona)
        {
            if (id != persona.Id)
                return BadRequest(new { mensaje = "El ID de la URL no coincide con el ID del cuerpo de la solicitud." });

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existente = await _getPersonaByIdQuery.ExecuteAsync(id);
            if (existente == null)
                return NotFound(new { mensaje = $"No se puede actualizar porque no existe la persona con ID {id}" });

            await _updatePersonaCommand.ExecuteAsync(persona);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")] 
        public async Task<ActionResult> Delete(int id)
        {
            var existente = await _getPersonaByIdQuery.ExecuteAsync(id);
            if (existente == null)
                return NotFound(new { mensaje = $"No se puede eliminar porque no existe la persona con ID {id}" });

            await _deletePersonaCommand.ExecuteAsync(id);
            return NoContent();
        }
    }
}
