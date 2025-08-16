using FicticiaBackend.Models;
using FicticiaBackend.Repositories;
using System.Threading.Tasks;

namespace FicticiaBackend.Commands
{
    public class CreatePersonaCommand
    {
        private readonly IPersonaRepository _repository;

        public CreatePersonaCommand(IPersonaRepository repository)
        {
            _repository = repository;
        }

        public async Task ExecuteAsync(Persona persona)
        {
            await _repository.AddAsync(persona);
        }
    }
}
