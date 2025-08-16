using FicticiaBackend.Models;
using FicticiaBackend.Repositories;
using System.Threading.Tasks;

namespace FicticiaBackend.Commands
{
    public class UpdatePersonaCommand
    {
        private readonly IPersonaRepository _repository;

        public UpdatePersonaCommand(IPersonaRepository repository)
        {
            _repository = repository;
        }

        public async Task ExecuteAsync(Persona persona)
        {
            await _repository.UpdateAsync(persona);
        }
    }
}
