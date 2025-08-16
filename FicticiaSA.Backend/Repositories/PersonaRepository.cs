using FicticiaBackend.Data;
using FicticiaBackend.Models;
using Microsoft.EntityFrameworkCore;


namespace FicticiaBackend.Repositories
{
    public class PersonaRepository : IPersonaRepository
    {
        private readonly FicticiaContext _context;

        public PersonaRepository(FicticiaContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Persona>> GetAllAsync()
        {
            return await _context.Personas.AsNoTracking().ToListAsync();
        }

        public async Task<Persona> GetByIdAsync(int id)
        {
            var persona = await _context.Personas.FindAsync(id);
            if (persona == null)
                throw new KeyNotFoundException($"No se encontró persona con Id {id}");
            return persona;
        }

        public async Task AddAsync(Persona persona)
        {
            _context.Personas.Add(persona);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Persona persona)
        {
            var existente = await _context.Personas.FindAsync(persona.Id);
            if (existente == null)
            {
                throw new KeyNotFoundException($"No se encontró una persona con ID {persona.Id}");
            }

            existente.NombreCompleto = persona.NombreCompleto;
            existente.Identificacion = persona.Identificacion;
            existente.Edad = persona.Edad;
            existente.Genero = persona.Genero;
            existente.EstadoActivo = persona.EstadoActivo;
            existente.Maneja = persona.Maneja;
            existente.UsaLentes = persona.UsaLentes;
            existente.Diabetico = persona.Diabetico;
            existente.EnfermedadOtra = persona.EnfermedadOtra;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var persona = await _context.Personas.FindAsync(id);
            if (persona != null)
            {
                _context.Personas.Remove(persona);
                await _context.SaveChangesAsync();
            }
        }
    }
}
