using FicticiaBackend.Models;
using FicticiaBackend.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FicticiaBackend.Queries
{
    public class GetAllPersonasQuery
    {
        private readonly IPersonaRepository _repository;

        public GetAllPersonasQuery(IPersonaRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Persona>> ExecuteAsync()
        {
            return await _repository.GetAllAsync();
        }
    }
}
