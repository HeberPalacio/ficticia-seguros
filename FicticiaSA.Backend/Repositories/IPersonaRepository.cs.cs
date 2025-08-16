using FicticiaBackend.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FicticiaBackend.Repositories
{
    public interface IPersonaRepository
    {
        Task<IEnumerable<Persona>> GetAllAsync();
        Task<Persona> GetByIdAsync(int id);
        Task AddAsync(Persona persona);
        Task UpdateAsync(Persona persona);
        Task DeleteAsync(int id);
    }
}
