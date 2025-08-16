using FicticiaBackend.Models;
using FicticiaBackend.Repositories;

namespace FicticiaBackend.Queries
{
    public class GetPersonaByIdQuery
    {
        private readonly IPersonaRepository _repository;

        public GetPersonaByIdQuery(IPersonaRepository repository)
        {
            _repository = repository;
        }

        public async Task<Persona> ExecuteAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }
    }
}
